import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SignupComponent } from './signup.component';

@NgModule({
  imports: [RouterModule.forChild([
    {path: 'signup', component: SignupComponent},])],
  exports: [RouterModule]
})
export class SignupRoutingModule { }
