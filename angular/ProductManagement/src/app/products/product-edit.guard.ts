import { Injectable } from '@angular/core';
import { Router, CanDeactivate } from '@angular/router';
import { ProductEditComponent } from './product-edit.component';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductEditGuard implements  CanDeactivate<ProductEditComponent> {
    
    canDeactivate(
      component: ProductEditComponent
      ): Observable<boolean> | Promise<boolean> | boolean {
        if (component.productForm.dirty) {
        return confirm("Do you really wan to discard changes?");
        }
        return true;
    }
  }
