import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './layout/header/header.component';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  baseUrl = environment.baseUrl;
  title = 'iShop';

  private http = inject(HttpClient);
  // signal for the products
  products = signal<Product[]>([]);


  ngOnInit() {
    this.http.get<Pagination<Product>>(`${this.baseUrl}/api/products`)
    .pipe(map((res) => {
      return res.data;
    }))
    .subscribe((res: Product[]) => {
      this.products.set(res);
    })
  }
}

