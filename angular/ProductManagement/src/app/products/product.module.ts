import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ProductDetailsComponent } from './product-details.component';
import { ProductsListComponent } from './products-list.component';
import { SharedModule } from "../shared/shared.module";
import { ProductRoutingModule } from './product-routing.module';
import { ProductEditComponent } from './product-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
@NgModule({
  declarations: [
    ProductDetailsComponent,
    ProductsListComponent,
    ProductEditComponent],

  imports: [
    SharedModule,
    ProductRoutingModule, 
    ReactiveFormsModule]
})
export class ProductModule { }
