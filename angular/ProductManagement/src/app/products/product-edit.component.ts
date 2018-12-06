import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ValidatorFn } from '../common/validatorFn';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../services/product.service';
import { Product } from '../models/product';
import { Subscription } from 'rxjs';
import { ValidationErrorMessenger } from '../common/ValidationErrorMessenger';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styles: []
})
export class ProductEditComponent implements OnInit, OnDestroy{
  public errorMessage: any;
  public productForm: FormGroup;
  public pageTitle = 'Add new product';
  public product: Product | undefined;
  private sub: Subscription;
  private validationMessages: {[key:string]: {[key:string]: string }};
  public displayValidationMessages: {[key: string]:string} = {};
  public validationErrorMessenger: ValidationErrorMessenger;
  private productId:number = 0;

  
  constructor(private fb: FormBuilder, private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService) {

      this.validationMessages = {
        code:{
          required: 'Code is required.',
          minlength: 'Minimum code length is 5.',
          maxlength: 'Maximun code length is 10.'
        },
        name:{
          required: 'Product name is required.',
          text: 'Enter aphabhates only.'
        },
        releaseDate:{
          required: 'releaseDate name is required.'
        },
        starRating:{
          number: 'Enter numbers only.',
          range: 'Rate the product between 1 (lowest) and 5 (highest).'
        },
        imageUrl:{
          text: 'Enter aphabhates only.'
        },
        price:{
          number: 'Enter numbers only.'
        }
      }

      this.validationErrorMessenger = new ValidationErrorMessenger(this.validationMessages);
     }

  ngOnInit() {
    this.setDefaultValues();
    this.sub = this.route.paramMap.subscribe(
      params => {
        this.productId = +params.get('productId');
        this.getProduct(this.productId);
      }
    );
      
    this.productForm.valueChanges.subscribe( value =>
      this.displayValidationMessages = this.validationErrorMessenger.getValidationErrorMessages(this.productForm)
    )
    // this.productForm.valueChanges.pipe(
    //   debounceTime(800)
    // ).subscribe(value => {
    //   this.displayMessage = this.genericValidator.processMessages(this.productForm);
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  public setDefaultValues()
  {   
    this.productForm = this.fb.group({
      name: ['',[Validators.required]],
      code: ['',[Validators.required, Validators.minLength(5), Validators.maxLength(10)]],
      // releaseDate:[Date.now, [Validators.required]],
      starRating:['', [ValidatorFn.range(1,5)]],
      description:[''],
      price:['', [ValidatorFn.number]]
    });
  }

  getProduct(id: number) {
    this.productService.getProductById(id).subscribe(
      product =>{this.product = product;
                  if (id != 0 &&(this.product == null || this.product.productId == 0 && this.productForm)) {
                    this.errorMessage = `Enable to find product with id =${id}.`;
                  }else if(id!= 0 &&(this.product != null || this.product.productId != 0 && this.productForm)){
                    this.displayProduct();
                    this.pageTitle = `Edit product ${this.product.name}`;
                  }
                  else{
                    this.product = new Product(0,'','',new Date(),'',0.0,0,'');
                    this.pageTitle = `Add new product`;
                  }
      },
      error => this.errorMessage = <any>error);
  }

  displayProduct(){
    this.productForm.patchValue({
      name: this.product.name,
      code: this.product.code,
      description: this.product.description,
      starRating: this.product.starRating,
      price: this.product.price,
      //releaseDate: new Date("2018-12-12")//this.product.releaseDate
    })
  }

  public deleteProduct(){
    if(this.productId != 0 && confirm('Do you realy want to delete product?')){
        this.productService.deleteProduct(this.productId).subscribe(
          response =>{
            alert('Product deleted successfully');
            this.completeAction();
          },
          error=> this.errorMessage == JSON.stringify(error));
    }
  }

  public save(){
    if(this.productForm.dirty){
      if(this.productForm.valid){
        const product = {...this.product, ...this.productForm.value}
        if(this.productId == 0){      
          this.productService.addProduct(product).subscribe(
            response =>{alert('Product addedd successfully');
                        this.completeAction();},
            error=> this.errorMessage == JSON.stringify(error))
        }else{
          this.productService.updateProduct(product).subscribe(
            response =>{alert('Product updated successfully');
                        this.completeAction();},
            error=> this.errorMessage == JSON.stringify(error))
        }      
      }else{
        alert('Resolve all the errors before saving product.'); 
      }

    }else{
      alert('No changes maid to product.')
    }
  }

  private completeAction(){
    this.productForm.reset();
    this.router.navigate(['/products']);
  }

  reset(){
    this.productForm.reset();
  }

  cancel(){
    this.completeAction();
  }
}
