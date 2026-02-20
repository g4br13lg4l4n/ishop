import { Component, inject, signal } from '@angular/core';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButton } from '@angular/material/button';
import { Filters } from '../../../shared/models/filters';
import { map, Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface FiltersDialogData {
  filters$: Observable<Filters>;
  initialSelection: Filters;
}

@Component({
  selector: 'app-filters-dialog',
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton,
    AsyncPipe,
    FormsModule
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.css',
})
export class FiltersDialogComponent {

  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  private dialogData: FiltersDialogData = inject(MAT_DIALOG_DATA);

  brands$ = this.dialogData.filters$.pipe(map((f) => f.brands));
  types$ = this.dialogData.filters$.pipe(map((f) => f.types));

  selectedFilters = signal<Filters>({
    brands: [...(this.dialogData.initialSelection.brands ?? [])],
    types: [...(this.dialogData.initialSelection.types ?? [])],
  });

  applyFilters() {
    const current = this.selectedFilters();
    this.dialogRef.close({
      brands: [...(current.brands ?? [])],
      types: [...(current.types ?? [])],
    });
  }

}
