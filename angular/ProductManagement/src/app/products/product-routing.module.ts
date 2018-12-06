import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductsListComponent } from './products-list.component';
import { ProductDetailsGuard } from './product-details.guard';
import { ProductDetailsComponent } from './product-details.component';
import { ProductEditComponent } from './product-edit.component';
import { ProductEditGuard } from './product-edit.guard';

@NgModule({
  imports: [ RouterModule.forChild([
    {path: 'products', component: ProductsListComponent},
    {path: 'productdetails/:productId', canActivate:[ProductDetailsGuard] ,component: ProductDetailsComponent},
    {path: 'products/:productId/edit', canDeactivate:[ProductEditGuard], component: ProductEditComponent},
    {path: 'productdetails/:productId/edit', canDeactivate:[ProductEditGuard], component: ProductEditComponent},
    {path: 'addproduct', canDeactivate:[ProductEditGuard], component: ProductEditComponent}])
  ],

  exports: [RouterModule]
})

export class ProductRoutingModule { }
