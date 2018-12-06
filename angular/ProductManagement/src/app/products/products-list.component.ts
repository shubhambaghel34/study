import { Component, OnInit, ReflectiveInjector } from '@angular/core';
import {IProduct} from '../models/product';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.css']
})
export class ProductsListComponent implements OnInit {
  pageTitle: String = 'Product List';
  showImage: Boolean = true;
  // tslint:disable-next-line:no-inferrable-types
  _filterBy: string = '';
  _errorMessage;
  get ErrorMessage():string{
    return this._errorMessage;
  }
  set ErrorMessage(value: string){
    this._errorMessage = value;
  }

  get filterBy(): string {
    return this._filterBy;
  }
  set filterBy(value: string) {
    this._filterBy = value;
    this.filteredProducts = this.filterBy ? this.performFilter(this._filterBy) : this.products;
  }

  products: IProduct[] = null;
  filteredProducts: IProduct[] = null;

  constructor(private _productService: ProductService) { }

  ngOnInit() {
    this._productService.getAllProducts().subscribe(
      products => {this.products = products;
                  this.filteredProducts = this.products},
      error => {this._errorMessage = <any>error;},
      ()=> {this.filteredProducts = this.products;}
    );
  }

  toggleImage(): void {
    this.showImage = !this.showImage;
  }

  performFilter(filterBy: string): IProduct[] {
    return this.products.filter((product: IProduct) =>
    product.name.toLowerCase().indexOf(filterBy) !== -1);
  }

  onRatingClicked(message: string): void{
    //this.pageTitle = `ProductList : ${message}`;
  }
}
