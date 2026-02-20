import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { BehaviorSubject, forkJoin, map, Observable, shareReplay, tap } from 'rxjs';
import { Filters } from '../../shared/models/filters';
import { ShopParams } from '../../shared/models/shopParams';


@Injectable({
  providedIn: 'root',
})
export class ShopService {

  shopParams = signal<ShopParams>(new ShopParams());
  
  private http = inject(HttpClient);
  baseUrl = environment.baseUrl;

  private subProducts = new BehaviorSubject<Pagination<Product>>(new Pagination<Product>());
  products$: Observable<Pagination<Product>> = this.subProducts.asObservable();

  private SubBrandsAndTypes = new BehaviorSubject<Filters>({ brands: [], types: [] });
  brandsAndTypes$: Observable<Filters> = this.SubBrandsAndTypes.asObservable();


  getProducts(shopParams: ShopParams): void {
    const filterParams: Record<string, number | string> = {
      ...(shopParams.brands?.length > 0 && { brands: shopParams.brands.join(',') }),
      ...(shopParams.types?.length > 0 && { types: shopParams.types.join(',') }),
      ...(shopParams.sort && { sort: shopParams.sort }),
      ...(shopParams.search && { search: shopParams.search }),
      ...(shopParams.pageIndex && { pageIndex: shopParams.pageIndex }),
      ...(shopParams.pageSize && { pageSize: shopParams.pageSize }),
    };

    this.http
      .get<Pagination<Product>>(`${this.baseUrl}/api/products`, {
        params: filterParams,
      })
      .pipe(
        map((res) => res),
        tap((pagination) => this.subProducts.next(pagination)),
        tap((pagination) => this.shopParams.update(prev => ({ ...prev, pageIndex: pagination.pageIndex, pageSize: pagination.pageSize, count: pagination.count }))),
        shareReplay()
      )
      .subscribe();
  }

  loadFilters() {
    this.brandsAndTypes$ = forkJoin([this.getBrands(), this.getTypes()]).pipe(
      map(
        ([brands, types]) => ({ brands, types }),
      ),
      shareReplay()
    );
  }

  getBrands() {
    return this.http.get<string[]>(`${this.baseUrl}/api/products/brands`)
      .pipe(
        map((res: string[]) => res),
        shareReplay()
      );
  }

  getTypes() {
    return this.http.get<string[]>(`${this.baseUrl}/api/products/types`)
      .pipe(
        map((res: string[]) => res),
        shareReplay()
      );
  }
}
