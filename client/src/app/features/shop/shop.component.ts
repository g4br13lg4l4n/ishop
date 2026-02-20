import { ChangeDetectionStrategy, Component, computed, DestroyRef, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Product } from '../../shared/models/product';
import { ShopService } from '../../core/services/shop.service';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';
import { Filters } from '../../shared/models/filters';
import { AsyncPipe } from '@angular/common';
import { debounceTime, map, Observable } from 'rxjs';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { Sort } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-shop',
  imports: [
    ProductItemComponent,
    MatButton,
    MatIcon,
    AsyncPipe,
    MatSelectionList,
    MatListOption,
    MatMenu,
    MatMenuTrigger,
    MatPaginator,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ShopComponent {

  shopService = inject(ShopService);
  dialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

  searchForm = new FormGroup({
    search: new FormControl('', [Validators.required, Validators.minLength(3)])
  });

  products$!: Observable<Pagination<Product>>;
  filters$!: Observable<Filters>;
  totalProducts = computed(() => this.products$.pipe(map((products) => products.count)));
  listProducts = computed(() => this.products$.pipe(map((products) => products.data)));

  sortOptions = signal<{name: string, value: string}[]>([
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'},
  ]);

  @ViewChild('sortMenu') sortMenu!: ElementRef<MatMenu>;
  shopParams = this.shopService.shopParams;

  constructor() {
    this.shopService.getProducts(this.shopParams());
    this.products$ = this.shopService.products$;

    this.shopService.loadFilters();
    this.filters$ = this.shopService.brandsAndTypes$;
    this.onSearchChange();
  }

  onPageChange($event: PageEvent) {
    $event.pageIndex > ($event.previousPageIndex ?? 0) ? $event.pageIndex = $event.pageIndex + 1 : $event.pageIndex = $event.previousPageIndex ?? 1;
    if($event.pageSize !== this.shopParams().pageSize) {
      $event.pageIndex = 1;
    }

    this.shopParams.update(prev => ({ ...prev, pageIndex: $event.pageIndex, pageSize: $event.pageSize }));
    this.shopService.getProducts(this.shopParams());
  }

  onPageChangeNew($event: Event) {
    console.log($event);
  }

  sortProducts($event: MatSelectionListChange) {
    this.shopParams.update(prev => ({ ...prev, sort: $event.options[0].value as Sort }));
    this.shopService.getProducts({...this.shopParams(), pageIndex: 1});
  }

  openFiltersDialog() {
    const params = this.shopParams();
    const dialogRef = this.dialog.open(FiltersDialogComponent, {
      width: '500px',
      data: {
        filters$: this.filters$,
        initialSelection: {
          brands: params.brands ?? [],
          types: params.types ?? [],
        },
      },
    });
    dialogRef.afterClosed().pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe((result: Filters) => {
      this.shopParams.update(prev => ({ ...prev, brands: result?.brands ?? [], types: result?.types ?? [] }));
      this.shopService.getProducts({ ...this.shopParams(), pageIndex: 1 });
    });
  }

  onSearchChange(): void {
    this.searchForm.get('search')?.valueChanges.pipe(
      debounceTime(1000),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe((value) => {
      this.shopParams.update(prev => ({ ...prev, search: value ?? '' }));
      this.shopService.getProducts({ ...this.shopParams(), pageIndex: 1 });
    });
  }
}
