import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CustomerDetails, CustomerListItem } from './models';

@Injectable({ providedIn: 'root' })
export class CustomerApi {
  private readonly http = inject(HttpClient);

  search(term: string): Observable<CustomerListItem[]> {
    let params = new HttpParams();

    if (term.trim().length > 0) {
      params = params.set('search', term.trim());
    }

    return this.http.get<CustomerListItem[]>('/api/customers', { params });
  }

  getById(id: number): Observable<CustomerDetails> {
    return this.http.get<CustomerDetails>(`/api/customers/${id}`);
  }
}
