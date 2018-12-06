import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class ProductDetailsGuard implements CanActivate {

  constructor(private _router: Router){}

  canActivate(
    next: ActivatedRouteSnapshot,
    //state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    state: RouterStateSnapshot): boolean {
      let id = +next.url[1].path;
      if(isNaN(id) || id < 1)
      {
        alert("Ops! Invalid productId.");
        this._router.navigate(['/products']);
        return false;
      }
      return true;
  }
}
