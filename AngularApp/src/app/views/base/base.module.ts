// Angular
import { CommonModule } from '@angular/common';
import { FormsModule  } from '@angular/forms';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { AddFundComponent} from './addfund.component';
import { TransferFundComponent} from './transferfund.component';
import {MagicCoinComponent} from './magiccoin.component';
import {BuyAdPackComponent } from './Buyadpack.component';
import {BuyMatrixPositionComponent } from './buymatrixposition.component';
import { BoardComponent } from './board.component';
import { MyTeamComponent } from './myteam.component';
import { WithdrawalComponent } from './withdrawal.component';
import { LoginAdComponent } from  './loginad.component';
import { MarketingPageComponent} from './marketingpage.component';
import { MySharesComponent} from './myshares.component';
import { ModalModule } from 'ngx-bootstrap';
// Tabs Component
import { TabsModule } from 'ngx-bootstrap/tabs';

// Carousel Component
import { CarouselModule } from 'ngx-bootstrap/carousel';

// Collapse Component
import { CollapseModule } from 'ngx-bootstrap/collapse';

// Dropdowns Component
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

// Pagination Component
import { PaginationModule } from 'ngx-bootstrap/pagination';

// Popover Component
import { PopoverModule } from 'ngx-bootstrap/popover';
import { PaginationsComponent } from './paginations.component';

// Progress Component
import { ProgressbarModule } from 'ngx-bootstrap/progressbar';

// Tooltip Component
import { TooltipModule } from 'ngx-bootstrap/tooltip';

// navbars
import { NavbarsComponent } from './navbars/navbars.component';

// Components Routing
import { BaseRoutingModule } from './base-routing.module';
import { PromotionComponent,NewSafePipe } from './promotion.component';
import { banner125x125Component } from './banner125x125.component';
import { banner728x90Component } from './banner728x90.component';
import { banner468x60Component } from './banner468x60.component';
import { DepositHistoryComponent } from './deposithistory.component';
import { WithdrawalHistoryComponent } from './withdrawalhistory.component';
import { PurchaseHistoryComponent } from './purchasehistory.component';
import { TreeComponent } from './tree.component';
import { SupportComponent } from './support.component';
import { AccountComponent } from './account.component';
import { ChangePasswordComponent } from './changepassword.component';
import { NewsComponent } from './news.component';
import { SurfAdsComponent, SafePipe } from './surfads.component';
import { TransferHistoryComponent } from './transferhistory.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    BaseRoutingModule,
    BsDropdownModule.forRoot(),
    TabsModule,
    CarouselModule.forRoot(),
    CollapseModule.forRoot(),
    PaginationModule.forRoot(),
    PopoverModule.forRoot(),
    ProgressbarModule.forRoot(),
    TooltipModule.forRoot(),
    ReactiveFormsModule,
    ModalModule
  ],
  declarations: [
    TransferHistoryComponent,
    MagicCoinComponent,
    AddFundComponent,
    TransferFundComponent,
    BuyAdPackComponent,
    BuyMatrixPositionComponent,
    BoardComponent,
    WithdrawalComponent,
    banner125x125Component,
    banner728x90Component,
    banner468x60Component,
    DepositHistoryComponent,
    WithdrawalHistoryComponent,
    PurchaseHistoryComponent,
    MyTeamComponent,
    LoginAdComponent,
    MarketingPageComponent,
    PromotionComponent,
    MySharesComponent,
    PaginationsComponent,
    NavbarsComponent,
    TreeComponent,
    SupportComponent,
    AccountComponent,
    ChangePasswordComponent,
    NewsComponent,
    SurfAdsComponent,
    SafePipe,
    NewSafePipe
  ]
})
export class BaseModule { }
