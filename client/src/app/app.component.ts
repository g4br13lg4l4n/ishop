import { Component, EnvironmentInjector, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './layout/header/header.component';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';

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

  products: Product[] = [];

  ngOnInit() {
    this.http.get<Response>(`${this.baseUrl}/api/products`)
    .pipe(map((res) => {
      return res.data;
    }))
    .subscribe((res: Product[]) => {
      this.products = res;
    })
  }
}


interface Response {
  data: Product[];
  pageIndex: number;
  pageSize: number;
  count: number;
}

interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  pictureUrl: string;
  type: string;
  brand: string;
  quantityInStock: number;
}
