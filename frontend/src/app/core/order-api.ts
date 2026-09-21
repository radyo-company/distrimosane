import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderDetails } from './models';

@Injectable({ providedIn: 'root' })
export class OrderApi {
  private readonly http = inject(HttpClient);

  getById(id: number): Observable<OrderDetails> {
    return this.http.get<OrderDetails>(`/api/orders/${id}`);
  }
}
