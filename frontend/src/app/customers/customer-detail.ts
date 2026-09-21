import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CustomerApi } from '../core/customer-api';
import { CustomerDetails } from '../core/models';
import { OrderStatusLabelPipe } from '../core/order-status-label-pipe';

@Component({
  selector: 'app-customer-detail',
  imports: [CurrencyPipe, DatePipe, RouterLink, OrderStatusLabelPipe],
  templateUrl: './customer-detail.html',
})
export class CustomerDetail implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(CustomerApi);

  protected readonly customer = signal<CustomerDetails | null>(null);
  protected readonly notFound = signal(false);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.api.getById(id).subscribe({
      next: (customer) => this.customer.set(customer),
      error: () => this.notFound.set(true),
    });
  }
}
