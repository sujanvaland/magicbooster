import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PaginationsComponent } from './paginations.component';
import { NavbarsComponent } from './navbars/navbars.component';
import { AddFundComponent } from './addfund.component';
import { TransferFundComponent } from './transferfund.component';
import {MagicCoinComponent} from './magiccoin.component';
import{ BuyAdPackComponent } from './Buyadpack.component';
import{ BuyMatrixPositionComponent } from './buymatrixposition.component';
import { BoardComponent } from './board.component';
import { MyTeamComponent } from './myteam.component';
import { WithdrawalComponent } from './withdrawal.component'
import { LoginAdComponent } from  './loginad.component';
import { MarketingPageComponent} from './marketingpage.component';
import { MySharesComponent} from './myshares.component';
import { PromotionComponent } from './promotion.component';
import { banner125x125Component } from './banner125x125.component';
import { banner728x90Component } from './banner728x90.component';
import { banner468x60Component } from './banner468x60.component';
import { DepositHistoryComponent } from './deposithistory.component';
import { WithdrawalHistoryComponent } from './withdrawalhistory.component';
import { PurchaseHistoryComponent } from './purchasehistory.component';
import { AuthguardService } from '../../services/authguard.service';
import { TreeComponent } from './tree.component';
import { SupportComponent } from './support.component';
import { AccountComponent } from './account.component';
import { ChangePasswordComponent } from './changepassword.component';
import { NewsComponent } from './news.component';
import { SurfAdsComponent } from './surfads.component';
import { TransferHistoryComponent } from './transferhistory.component';


const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Base'
    },
    children: [
      {
        path: '',
        redirectTo: 'cards'
      },
      {
        path: 'addfund',
        component: AddFundComponent,
        data: {
          title: 'Add Fund'
        }
      },
      {
        path: 'transferfund',
        component: TransferFundComponent,
        data: {
          title: 'Transfer Fund'
        }
      },
      {
        path: 'transferhistory',
        component: TransferHistoryComponent,
        data: {
          title: 'Transfer History'
        }
      },      
      {
        path: 'magiccoin',
        component: MagicCoinComponent,
        data: {
          title: 'Magic Coin'
        }
      },
      {
        path: 'buyadpack',
        component: BuyAdPackComponent,
        data: {
          title: 'Buy Package'
        }
      },
      {
        path: 'buymatrixposition',
        component: BuyMatrixPositionComponent,
        data: {
          title: 'Purchase Matrix Position'
        }
      },
      {
        path: 'board',
        component: BoardComponent,
        data: {
          title: 'View Position'
        }
      },
      {
        path: 'tree',
        component: TreeComponent,
        data: {
          title: 'View tree'
        }
      },
      {
        path: 'myteam',
        component: MyTeamComponent,
        data: {
          title: 'My Referrals'
        }
      },
      {
        path: 'withdrawal',
        component: WithdrawalComponent,
        data: {
          title: 'Withdraw Funds'
        }
      },
      {
        path: 'loginad',
        component: LoginAdComponent,
        data: {
          title: 'Manage Ad Campaign'
        }
      },
      {
        path :'marketing',
        component:MarketingPageComponent,
        data:{
          title : 'Marketing'
        }
      },
      {
        path: 'promotion',
        component: PromotionComponent,
        data: {
          title: 'Marketing Page'
        }
      },
      {
        path: 'news',
        component: NewsComponent,
        data: {
          title: 'Program Updates'
        }
      },
      {
        path: 'surfads',
        component: SurfAdsComponent,
        data: {
          title: 'Surf Ads'
        }
      },
      {
        path: 'banner125x125',
        component: banner125x125Component,
        data: {
          title: 'Banner 125x125'
        }
      },
      {
        path: 'banner728x90',
        component: banner728x90Component,
        data: {
          title: 'Banner 728x90'
        }
      },
      {
        path: 'banner468x60',
        component: banner468x60Component,
        data: {
          title: 'Banner 468x60'
        }
      },
      {
        path: 'deposithistory',
        component: DepositHistoryComponent,
        data: {
          title: 'Deposit History'
        }
      },
      {
        path: 'withdrawalhistory',
        component: WithdrawalHistoryComponent,
        data: {
          title: 'Withdrawal History'
        }
      },
      {
        path: 'purchasehistory',
        component: PurchaseHistoryComponent,
        data: {
          title: 'Purchase History'
        }
      },
      {
        path: 'myshares',
        component: MySharesComponent,
        data: {
          title: 'My Investment Plan'
        }
      },
      {
        path: 'support',
        component: SupportComponent,
        data: {
          title: 'Raise Support Ticket'
        }
      },
      {
        path: 'account',
        component: AccountComponent,
        data: {
          title: 'Account Details'
        }
      },
      {
        path: 'changepassword',
        component: ChangePasswordComponent,
        data: {
          title: 'Change Password'
        }
      },
      {
        path: 'paginations',
        component: PaginationsComponent,
        data: {
          title: 'Pagination'
        }
      },
      {
        path: 'navbars',
        component: NavbarsComponent,
        data: {
          title: 'Navbars'
        }
      }
    ],
    canActivate:[AuthguardService]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BaseRoutingModule {}
