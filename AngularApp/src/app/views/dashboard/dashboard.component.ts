import { Component, OnInit, ViewChild } from '@angular/core';
import { getStyle, hexToRgba } from '@coreui/coreui/dist/js/coreui-utilities';
import { CustomTooltips } from '@coreui/coreui-plugin-chartjs-custom-tooltips';
import { CustomerService } from '../../services/customer.service';
import { environment } from '../../../environments/environment';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import * as $ from 'jquery';
import { AdvertismentService } from '../../services/advertisment.service';
import { MatrixService } from '../../services/matrix.service';
import { ToastrService } from 'ngx-toastr';
import { CommonService } from '../../services/common.service';
import { timer } from 'rxjs';
@Component({
  selector: 'app-dashboard',
  templateUrl: 'dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  constructor(
    private matrixservice: MatrixService,
    private toastr: ToastrService,
    private customerservice: CustomerService,
    private advertismenetSerive:AdvertismentService,
    private commonservice:CommonService,
    private router: Router) { }
  radioModel: string = 'Month';
  CustomerId :string = localStorage.getItem("CustomerId");
  SiteUrl : string = environment.siteUrl;
  CustomerInfoModel = { Status : '', FullName :'',AvailableBalance :0,NetworkIncome:0,RoyaltyIncome:0,TodaysPair:'',AvailableCoin:0,TotalEarning:0,DirectBonus:0,
  AvailableCoins:0,UnilevelEarning : 0,CyclerIncome:0,CustomerId:0,RegistrationDate:'',ServerTime :'',ReferredBy:'',AffilateId:0,
  NoOfSecondsToSurf:0,NoOfAdsToSurf:0,PlacementId:0,Position:'',Username:'',PlacementUserName:'',AccumulatedPairing:'',PackageName:''}
  CustomerBoard = [];
  Managers = [] = environment.Managers;
  Campaigns = [];
  NewsLetter ={ Body:""};
  MerchantAcc:string = "";
  CurrencyCode:string = "USD";
  FinalAmount:number = 10;
  PaymentMemo:string = "Matrix Position Purchase";
  ipn_url:string = '';
  ShowPhaseMessage = true;
  TransactionId = localStorage.getItem("transactionno");
  @ViewChild('infoModal') public infoModal: ModalDirective;
  @ViewChild('congratsModal') public congratsModal: ModalDirective;
  @ViewChild('newsModal') public newsModal: ModalDirective;
   // lineChart2
   public lineChart2Data: Array<any> = [
    {
      data: [1, 18, 9, 17, 34, 22, 11],
      label: 'Series A'
    }
  ];
  public lineChart2Labels: Array<any> = ['January', 'February', 'March', 'April', 'May', 'June', 'July'];
  public lineChart2Options: any = {
    tooltips: {
      enabled: false,
      custom: CustomTooltips
    },
    maintainAspectRatio: false,
    scales: {
      xAxes: [{
        gridLines: {
          color: 'transparent',
          zeroLineColor: 'transparent'
        },
        ticks: {
          fontSize: 2,
          fontColor: 'transparent',
        }

      }],
      yAxes: [{
        display: false,
        ticks: {
          display: false,
          min: 1 - 5,
          max: 34 + 5,
        }
      }],
    },
    elements: {
      line: {
        tension: 0.00001,
        borderWidth: 1
      },
      point: {
        radius: 4,
        hitRadius: 10,
        hoverRadius: 4,
      },
    },
    legend: {
      display: false
    }
  };
  public lineChart2Colours: Array<any> = [
    { // grey
      backgroundColor: getStyle('--info'),
      borderColor: 'rgba(255,255,255,.55)'
    }
  ];
  public lineChart2Legend = false;
  public lineChart2Type = 'line';

   // barChart1
   public barChart1Data: Array<any> = [
    {
      data: [78, 81, 80, 45, 34, 12, 40, 78, 81, 80, 45, 34, 12, 40, 12, 40],
      label: 'Series A',
      barPercentage: 0.6,
    }
  ];
  public barChart1Labels: Array<any> = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16'];
  public barChart1Options: any = {
    tooltips: {
      enabled: false,
      custom: CustomTooltips
    },
    maintainAspectRatio: false,
    scales: {
      xAxes: [{
        display: false,
      }],
      yAxes: [{
        display: false
      }]
    },
    legend: {
      display: false
    }
  };
  public barChart1Colours: Array<any> = [
    {
      backgroundColor: 'rgba(255,255,255,.3)',
      borderWidth: 0
    }
  ];
  public barChart1Legend = false;
  public barChart1Type = 'bar';
  
  ngOnInit(): void {
        this.CurrencyCode = (localStorage.getItem("CurrencyCode") == null) ? "USD" : localStorage.getItem("CurrencyCode");
        this.ipn_url = environment.siteUrl + '/IPNHandler';
        this.FinalAmount = 10;
        this.MerchantAcc = environment.coinPaymentMerAcc;
        $('.loaderbo').show();
        
        this.customerservice.GetCustomerInfo(this.CustomerId)
        .subscribe(
          res => {
            this.CustomerInfoModel = res.data;
            let timerId = setInterval(countdown, 1000);
            let timeleft = this.CustomerInfoModel.NoOfSecondsToSurf;
            function countdown() {
              var elem = document.getElementById('divTimer');
              if (timeleft == -1) {
                clearInterval();
              } else {
                elem.innerHTML = timeleft + ' more';
                timeleft--;
              }
            }
            if(this.CustomerInfoModel.FullName == null){
              this.router.navigate(['/base/account']);
            }
            
            if(localStorage.getItem("firstvisit") != "true"){
             // this.congratsModal.show();
              localStorage.setItem("firstvisit","true");
            }

            if(this.CustomerInfoModel.Status != "Active" && environment.AllowPurchase){
             // this.infoModal.show();
            }
            $('.loaderbo').hide();
          },
          err => {
            if(err.status == 401){
              localStorage.clear();
              this.router.navigate(['/login']);
            }
          }
        )
    
        let model = {
          CustomerId : this.CustomerId
        }
        this.customerservice.GetCustomerBoard(model)
        .subscribe(
          res => {
            this.CustomerBoard = res.data.Data;
           
            if(this.CustomerBoard.length > 0){
              this.ShowPhaseMessage = false;
              this.CustomerBoard.sort(function (a, b) {
                return a.Id - b.Id;
              });
              this.CustomerBoard.forEach((board,index)=>{
                let per = 0;
                if(board.NoOfPositionFilled > 0){
                  per = (board.NoOfPositionFilled/(board.NoOfPositionRemaining + board.NoOfPositionFilled)) * 100
                }
                board.CompletionPer = per.toFixed(2);
                board.style = board.CompletionPer +"%";
              });
            }
           
          },
          err => {
            if(err.status == 401){
              localStorage.clear();
              this.router.navigate(['/login']);
            }
          }
        )

        this.advertismenetSerive.GetCountryManager().subscribe(
          res => {
            if(res.Message == "success"){
              this.Managers = res.data;
            }
          }
        )
        
       
        this.commonservice.GetNewsletter().subscribe(
          res => {
            if(res.Message == "success"){
              this.NewsLetter = JSON.parse(res.data)[0];
              //this.newsModal.show();
            }
          }
        )
  }

  countshare(){
    alert("shared link");
  }
}
