import { Routes } from '@angular/router';
import { CustomerDetail } from './customers/customer-detail';
import { CustomerList } from './customers/customer-list';
import { OrderDetail } from './orders/order-detail';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'clients' },
  { path: 'clients', component: CustomerList, title: 'Clients' },
  { path: 'clients/:id', component: CustomerDetail, title: 'Fiche client' },
  { path: 'commandes/:id', component: OrderDetail, title: 'Commande' },
  { path: '**', redirectTo: 'clients' },
];
