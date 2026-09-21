import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderDetails } from '../core/models';
import { OrderApi } from '../core/order-api';
import { OrderStatusLabelPipe } from '../core/order-status-label-pipe';

@Component({
  selector: 'app-order-detail',
  imports: [CurrencyPipe, DatePipe, RouterLink, OrderStatusLabelPipe],
  templateUrl: './order-detail.html',
})
export class OrderDetail implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(OrderApi);

  protected readonly order = signal<OrderDetails | null>(null);
  protected readonly notFound = signal(false);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.api.getById(id).subscribe({
      next: (order) => this.order.set(order),
      error: () => this.notFound.set(true),
    });
  }
}
