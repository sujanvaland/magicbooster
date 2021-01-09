import { Component ,OnInit, ViewChild } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { ToastrService } from 'ngx-toastr';
import { tick } from '@angular/core/testing';
import { ModalDirective } from 'ngx-bootstrap';
import * as $ from 'jquery';
@Component({
  templateUrl: 'support.component.html'
})
export class SupportComponent implements OnInit{

supportRequest: FormGroup;
submitted = false;

constructor(private formBuilder: FormBuilder,
  private customerservice:CustomerService,
  private toastr:ToastrService) { }

CustomerId:string = localStorage.getItem("CustomerId");
SupportModel = { Email:'',Enquiry:'',Subject:'',CustomerId:0,FullName:''};
CurrencyCode:string;
Countries=[];
Tickets = [];
Message = "";
replyTicket = { Id:0,Subject:"",Message:""};
@ViewChild('supportModal') public supportModal: ModalDirective;

ngOnInit(): void {
  this.supportRequest =this.formBuilder.group({
    Subject: ['', Validators.required],
    Enquiry:['', Validators.required]
  });

  $('.loaderbo').show();
  this.customerservice.GetCustomerInfo(this.CustomerId)
  .subscribe(
    res => {
      this.SupportModel = res.data;
      $('.loaderbo').hide();
    },
    err => console.log(err)
  )

  this.getSupportTicket();
}

getSupportTicket(){
  this.customerservice.GetSupportRequest(this.CustomerId)
  .subscribe(
    res => {
      this.Tickets = res.data;
     
    }
  )
}
get f() { return this.supportRequest.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.supportRequest.invalid) {
            return;
        }
        $('.loaderbo').show();
        this.supportRequest.value.CustomerId = this.CustomerId;
        this.supportRequest.value.FullName = this.SupportModel.FullName;
        this.supportRequest.value.Email = this.SupportModel.Email;
        this.customerservice.SubmitTicket(this.supportRequest.value)
        .subscribe(
          res =>{
            if(res.Message === "success"){
              this.toastr.success(res.data,"Success");
              let supportticket = { Subject : this.supportRequest.value.Subject,
                Message : this.supportRequest.value.Enquiry,
                CustomerId: this.CustomerId
                }
                this.customerservice.AddSupportTicket(supportticket).subscribe(res =>{
                  this.getSupportTicket();
                  $('.loaderbo').hide();
                });
                $('.loaderbo').hide();
            }
            else if(res.Message === "Request Allowed Once in Month"){
              this.toastr.error("Request Allowed Once in Month","Error");
              $('.loaderbo').hide();
            }
          },
          err => {
            this.toastr.error("Something went wrong, contact support","Error");
            $('.loaderbo').hide();
          }
        )
    }

    onReset() {
        this.submitted = false;
        this.supportRequest.reset();
    }

    OpenModel(ticket){
      this.replyTicket=ticket;
      this.supportModal.show();
    }
    ReplyBack(){
      $('.loaderbo').show();
        let supportticket = { 
        Id : this.replyTicket.Id,
        Subject : this.replyTicket.Subject,
        Message : this.replyTicket.Message,
        CustomerId: this.CustomerId,
        LastReplied: this.Message }
        this.customerservice.AddSupportTicket(supportticket).subscribe(res =>{
          $('.loaderbo').hide();
        });
    } 
}
