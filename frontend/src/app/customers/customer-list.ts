import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { Subject, debounceTime, distinctUntilChanged, startWith, switchMap } from 'rxjs';
import { CustomerApi } from '../core/customer-api';
import { CustomerListItem } from '../core/models';

@Component({
  selector: 'app-customer-list',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './customer-list.html',
})
export class CustomerList {
  private readonly api = inject(CustomerApi);
  private readonly searchTerm = new Subject<string>();

  protected readonly customers = signal<CustomerListItem[]>([]);
  protected readonly loading = signal(true);

  constructor() {
    this.searchTerm
      .pipe(
        startWith(''),
        debounceTime(250),
        distinctUntilChanged(),
        switchMap((term) => {
          this.loading.set(true);
          return this.api.search(term);
        }),
        takeUntilDestroyed(),
      )
      .subscribe((customers) => {
        this.customers.set(customers);
        this.loading.set(false);
      });
  }

  protected onSearch(term: string): void {
    this.searchTerm.next(term);
  }
}
