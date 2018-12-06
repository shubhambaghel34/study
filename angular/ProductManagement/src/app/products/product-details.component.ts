import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute} from '@angular/router';
import { ProductService } from '../services/product.service';
import { IProduct } from '../models/product';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  private _product: IProduct;
  get product(): IProduct{
    return this._product;
  }
  set product(value: IProduct){
    this._product = value;
  }
  
  public errorMessage: string = '';
  private sub: Subscription;
  
  constructor(private _productService: ProductService ,private _router: Router, private _activeteRouter: ActivatedRoute) { }

  ngOnInit() {
    // const param = this._activeteRouter.snapshot.paramMap.get('productId');
    // if (param) {
    //   const id = +param;
    //   this.getProductById(id);
    // }

    this.sub = this._activeteRouter.paramMap.subscribe(
      params => {
        const id = +params.get('productId');
        this.getProductById(id);
      }
    );
  }

  getProductById(id: number) {
    this._productService.getProductById(id).subscribe(
      product => this.product = product,
      error => this.errorMessage = <any>error);
  }

  onBack(): void{
    this._router.navigate(['/products']);
  }
}
