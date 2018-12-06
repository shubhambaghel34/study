import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AboutusComponent } from './aboutus.component';

@NgModule({
  imports: [ RouterModule.forChild([
    {path: 'aboutus', component: AboutusComponent},])
  ],

  exports: [RouterModule]
})
export class AboutusRoutingModule { }
