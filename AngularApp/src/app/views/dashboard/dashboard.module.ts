import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { ModalModule } from 'ngx-bootstrap';
import { JwSocialButtonsModule } from 'jw-angular-social-buttons';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    DashboardRoutingModule,
    ChartsModule,
    BsDropdownModule,
    JwSocialButtonsModule,
    ButtonsModule.forRoot(),
    ModalModule.forRoot()
  ],
  declarations: [ DashboardComponent ]
})
export class DashboardModule { }
