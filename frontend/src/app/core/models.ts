export type OrderStatus = 'Pending' | 'Shipped' | 'Delivered' | 'Cancelled';

export interface CustomerListItem {
  id: number;
  companyName: string;
  vatNumber: string;
  city: string;
  orderCount: number;
  totalRevenue: number;
  lastOrderDate: string | null;
}

export interface CustomerDetails {
  id: number;
  companyName: string;
  vatNumber: string;
  email: string;
  city: string;
  createdAt: string;
  orders: OrderSummary[];
}

export interface OrderSummary {
  id: number;
  orderDate: string;
  status: OrderStatus;
  lineCount: number;
  total: number;
}

export interface OrderLine {
  id: number;
  productLabel: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface OrderDetails {
  id: number;
  orderDate: string;
  status: OrderStatus;
  customerId: number;
  customerName: string;
  lines: OrderLine[];
  total: number;
}
