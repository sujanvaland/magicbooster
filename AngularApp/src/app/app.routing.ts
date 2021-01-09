import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Import Containers
import { DefaultLayoutComponent } from './containers';
import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { LoginComponent } from './views/login/login.component';
import { RegisterComponent } from './views/register/register.component';
import { AuthguardService } from './services/authguard.service';
import { HomeComponent } from './views/home/home.component';
import { ForgotComponent } from './views/forgotpassword/forgot.component';
import { FaqComponent } from './views/faq/faq.component';
import { PasswordRecoveryComponent } from './views/passwordrecovery/passwordrecovery.component';
import { CountryManagerComponent } from './views/countrymanager/countrymanager.component';
import { ContactUsComponent } from './views/contactus/contactus.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'fbshare',
    redirectTo:"fbshare.html"
  },
  {
    path: 'dashboard',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: '404',
    component: P404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    component: P500Component,
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'passwordreset',
    component: PasswordRecoveryComponent,
    data: {
      title: 'Password Reset'
    }
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: {
      title: 'Register Page'
    }
  },
  {
    path: 'countrymanagers',
    component: CountryManagerComponent,
    data: {
      title: 'Country Manager'
    }
  },
  {
    path: 'contactus',
    component: ContactUsComponent,
    data: {
      title: 'Contact Us'
    }
  },
  {
    path: 'forgotpassword',
    component: ForgotComponent,
    data: {
      title: 'Forgot Password'
    }
  },
  {
    path: 'faq',
    component: FaqComponent,
    data: {
      title: 'Frequently Ask Questions ?'
    }
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'base',
        loadChildren: () => import('./views/base/base.module').then(m => m.BaseModule),
        canActivate:[AuthguardService]
      },
      {
        path: 'buttons',
        loadChildren: () => import('./views/buttons/buttons.module').then(m => m.ButtonsModule)
      },
      {
        path: 'charts',
        loadChildren: () => import('./views/chartjs/chartjs.module').then(m => m.ChartJSModule)
      },
      {
        path: 'dashboard',
        loadChildren: () => import('./views/dashboard/dashboard.module').then(m => m.DashboardModule),
        canActivate:[AuthguardService]
      },
      {
        path: 'icons',
        loadChildren: () => import('./views/icons/icons.module').then(m => m.IconsModule)
      },
      {
        path: 'notifications',
        loadChildren: () => import('./views/notifications/notifications.module').then(m => m.NotificationsModule)
      },
      {
        path: 'theme',
        loadChildren: () => import('./views/theme/theme.module').then(m => m.ThemeModule)
      },
      {
        path: 'widgets',
        loadChildren: () => import('./views/widgets/widgets.module').then(m => m.WidgetsModule)
      }
    ]
  },
  { path: '**', component: P404Component }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
