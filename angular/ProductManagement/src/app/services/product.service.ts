import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { IProduct, Product } from '../models/product';
import { Observable, throwError } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private productsUrl = `https://localhost:44312/api/Product`;

  // http://xipl0500:8001/api/Product/getallproductsasync

  constructor(private _httpService: HttpClient) { }

  getAllProducts(): Observable<IProduct[]>{
    return this._httpService.get<IProduct[]>(`${this.productsUrl}/getallproductsasync`).pipe(
        // tap(data=> console.log(`Response: ${JSON.stringify(data)}`)),
        catchError(this.errorHandler));
  }

  getProductById(productId: number): Observable<IProduct>{
    return this._httpService.get<IProduct>(`${this.productsUrl}/getproductbyidasync?ProductId=${productId}`).pipe(
      //.tap((response: Response) => <IProduct>response.json()),
      catchError(this.errorHandler));
  }

  addProduct(product: Product): Observable<IProduct>{    
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });    
    return this._httpService.post<IProduct>(`${this.productsUrl}/addproductasync`, product, { headers: headers })
      .pipe(
        tap(data => console.log('createProduct: ' + JSON.stringify(data))),
        catchError(this.errorHandler)
      );
  }

  deleteProduct(productId: number): Observable<IProduct>{
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.productsUrl}/deleteproductbyIdasync?ProductId=${productId}`;
    return this._httpService.delete<IProduct>(url, { headers: headers })
      .pipe(
        tap(data => console.log('deleteProduct: ' + JSON.stringify(data))),
        catchError(this.errorHandler));
  }

  updateProduct(product: IProduct): Observable<IProduct>{
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpService.put<IProduct>(`${this.productsUrl}/updateproductbyidasync`,product, {headers: headers})
    .pipe(
      tap(response => console.log(JSON.stringify(response))),
      catchError(this.errorHandler));
  }
  private errorHandler(response: HttpErrorResponse){
      if(response instanceof ErrorEvent)
      {
      }else{
      } 
      let errorMessage = `Server returned code: ${response.status}, error message is: ${response.message}`;
      return throwError(errorMessage);
      //throw response.error;
  }
  
}
