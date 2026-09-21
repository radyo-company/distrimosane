import { Pipe, PipeTransform } from '@angular/core';
import { OrderStatus } from './models';

const LABELS: Record<OrderStatus, string> = {
  Pending: 'En attente',
  Shipped: 'Expédiée',
  Delivered: 'Livrée',
  Cancelled: 'Annulée',
};

@Pipe({ name: 'orderStatusLabel' })
export class OrderStatusLabelPipe implements PipeTransform {
  transform(status: OrderStatus): string {
    return LABELS[status];
  }
}
