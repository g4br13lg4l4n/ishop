import { Component, inject } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/select';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-product-details',
  imports: [
    AsyncPipe,
    CurrencyPipe,
    MatIcon,
    MatFormField,
    MatInput,
    MatLabel,
    MatDivider,
    MatButton
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.css',
})
export class ProductDetailsComponent {
  shopService = inject(ShopService);
  id = inject(ActivatedRoute).snapshot.params['id'];

  product$ = this.shopService.getProduct(this.id);

  constructor() {
    
  }

}
