import { CategoryComponent } from './category/category.component';
import { ProductComponent } from './product/product.component';
import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent },
    { path: 'product', component: ProductComponent },
    { path: 'category', component: CategoryComponent },
    { path: '**', redirectTo: 'notes', pathMatch: 'full'}
];
