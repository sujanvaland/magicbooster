import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule,  HTTP_INTERCEPTORS } from '@angular/common/http';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { AuthguardService } from './services/authguard.service';
import {LoginserviceService} from './services/loginservice.service';
import { TokenInterceptorService } from './services/token-interceptor.service';
const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

import { AppComponent } from './app.component';

// Import containers
import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { LoginComponent } from './views/login/login.component';
import { RegisterComponent } from './views/register/register.component';
import { HomeComponent } from './views/home/home.component';
import { ForgotComponent } from './views/forgotpassword/forgot.component';
import { FaqComponent } from './views/faq/faq.component';
import { OwlModule } from 'ngx-owl-carousel';
import { CountdownModule } from 'ngx-countdown';
import { FormsModule } from '@angular/forms';

const APP_CONTAINERS = [
  DefaultLayoutComponent
];

import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule
  
} from '@coreui/angular';

// Import routing module
import { AppRoutingModule } from './app.routing';

// Import 3rd party components
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common'; //added this line to your module
import { ToastrModule } from 'ngx-toastr';
import { PasswordRecoveryComponent } from './views/passwordrecovery/passwordrecovery.component';
import { CountryManagerComponent } from './views/countrymanager/countrymanager.component';
import { ContactUsComponent } from './views/contactus/contactus.component';
@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    ReactiveFormsModule,
    TabsModule.forRoot(),
    ChartsModule,
    HttpClientModule,
    CommonModule,
    OwlModule,
    CountdownModule,
    ToastrModule.forRoot(),
    FormsModule
  ],
  declarations: [
    AppComponent,
    ...APP_CONTAINERS,
    P404Component,
    P500Component,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    FaqComponent,
    ForgotComponent,
    PasswordRecoveryComponent,
    CountryManagerComponent,
    ContactUsComponent
  ],
  providers: [{
    provide: LocationStrategy,
    useClass: HashLocationStrategy
  },AuthguardService,LoginserviceService,{
    provide: HTTP_INTERCEPTORS,
    useClass:TokenInterceptorService,
    multi:true
  }],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
