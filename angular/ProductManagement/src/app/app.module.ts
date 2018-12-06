import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { WelcomeComponent } from './home/welcome.component';
import { PagenotfoundComponent } from './errorpages/pagenotfound.component';
import { AboutusComponent } from './aboutus/aboutus.component';
import { ContactusComponent } from './contactus/contactus.component';
import { ProductModule } from './products/product.module';
import { AboutusModule } from './aboutus/aboutus.module';
import { ContactusModule } from './contactus/contactus.module';
import { SignupModule } from './signup/signup.module';

@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent,
    PagenotfoundComponent,
    AboutusComponent,
    ContactusComponent
  ],
  imports: [
    BrowserModule,
    ProductModule,
    FormsModule,
    HttpClientModule,
    AboutusModule,
    ContactusModule,
    SignupModule,
    AppRoutingModule    
  ],
  exports: [],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
