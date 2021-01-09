import { Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import { MatrixService } from '../../services/matrix.service';
import { CustomerService } from '../../services/customer.service';
import * as $ from 'jquery';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  templateUrl: 'tree.component.html'
})
export class TreeComponent {
  constructor(private matrixservice:MatrixService,
    private route: ActivatedRoute,
    private router: Router,
    private customerservice:CustomerService){
  }
  PositionId = 10
  CustomerId:string;
  treebalance:any = {};
  CustomerBoard=[];
  matrixPlan = [];
  plan = { Id : 1,Height:5}
  MyTreeView = [];
  PhaseDetail = [];
  TreeViewHTML:string="";
  Level = 1;
  prevId = localStorage.getItem("CustomerId");
  ngOnInit() {
    
    this.route.queryParams
    .subscribe(params => {
      if(params.customerid){
        this.CustomerId = params.customerid;
      }
      else{
        this.CustomerId = localStorage.getItem("CustomerId");
      }
    });
    //this.CustomerId = localStorage.getItem("CustomerId");
    this.getTreeData(this.CustomerId);
    //this.GetTreeBalance(this.CustomerId);
  }

  getTreeData(customerid){
   
    this.GetTeam(customerid);
    this.GetTreeBalance(customerid);
  }
  GetTeam(CustId){
    this.TreeViewHTML = "";
    this.matrixservice.GetTreeView(CustId).subscribe(
      response =>{
          this.MyTreeView = response.data;
          console.log(response.data);
          let level = 1;
          if(this.MyTreeView.length > 0){
            let parentRow = this.MyTreeView.find(element => element.parentid == 0);
            this.TreeViewHTML += '<ul>';
            this.display_with_children(parentRow,level);
            this.TreeViewHTML += '</ul>';
          }
      }
    );
  }

  GetTreeBalance(CustId){
    this.matrixservice.GetTreeBalance(CustId).subscribe(
      response =>{
          let balance = response.data;
          this.treebalance = balance[0];
      }
    );
  }
  myFunc(Id){
    alert(Id)
  };
  display_with_children(parentRow, level) { 
      this.TreeViewHTML +=  '<li><span class="tf-nc"><img src="assets/img/usertree.png"/><br><a target="_blank" href="#/base/tree?customerid='+parentRow.CustomerId+'">'+ 
      parentRow.Name +'</a> <br/>(' + parentRow.BoardName+')<br/>(' + parentRow.PurchaseDate+')</span>';
      let childlist = this.MyTreeView.filter(node =>node.parentid == parentRow.CustomerId)
      let newchildList = [];
      childlist.forEach(childnode =>{
        let obj = childnode;
        obj.Position = childnode.Email.split('_')[1];
        newchildList.push(obj);         
      }) 
      newchildList = newchildList.sort(function(a, b){
          if(a.Position < b.Position) { return -1; }
          if(a.Position > b.Position) { return 1; }
          return 0;
      })
      if (newchildList.length != 0) {
         this.TreeViewHTML +=  '<ul>';
          // use the fetch_assoc to get an associative array
          if(parentRow.HasLeft == 0){
            this.TreeViewHTML +=  '<li><span class="tf-nc"><img width="20" src="assets/img/user-male-icon.png"/><br><a target="_blank" href="#/register?r='+parentRow.Name+'&p=L">+ Add Left</a>';
          }
          newchildList.forEach(childnode =>{
            this.display_with_children(childnode,level+1);            
          }) 
          if(parentRow.HasRight == 0){
            this.TreeViewHTML +=  '<li><span class="tf-nc"><img width="20" src="assets/img/user-male-icon.png"/><br><a target="_blank" href="#/register?r='+parentRow.Name+'&p=R">+ Add Right</a>';
          }
          this.TreeViewHTML += '</ul>';
      }
      
      if(newchildList.length == 0){
        this.TreeViewHTML +=  '<ul>';
        if(parentRow.HasLeft == 0){
          this.TreeViewHTML +=  '<li><span class="tf-nc"><img width="20" src="assets/img/user-male-icon.png"/><br><a target="_blank" href="#/register?r='+parentRow.Name+'&p=L">+ Add Left</a>';
        }
        if(parentRow.HasRight == 0){
          this.TreeViewHTML +=  '<li><span class="tf-nc"><img width="20" src="assets/img/user-male-icon.png"/><br><a target="_blank" href="#/register?r='+parentRow.Name+'&p=R">+ Add Right</a>';
        }
        this.TreeViewHTML += '</ul>';
      }
      
      
      this.TreeViewHTML += '</li>';      
  } 

}
