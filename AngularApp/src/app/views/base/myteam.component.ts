import { Component ,OnInit} from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { MatrixService } from '../../services/matrix.service';

@Component({
  templateUrl: 'myteam.component.html'
})
export class MyTeamComponent implements OnInit {

  myteam: FormGroup;
  submitted = false;
  matrixPlan = [];
  teamMembers = [];
  LevelId:string = "1";
  plan = { Id : 1}
  constructor(private formBuilder: FormBuilder,
    private customerService: CustomerService,
    private matrixService: MatrixService) { }

  CustomerId : string;
  ngOnInit (){
      this.CustomerId = localStorage.getItem("CustomerId");
      this.customerService.MyReferral(this.CustomerId,this.LevelId).subscribe(
        res => {
          this.teamMembers = res.data.Data;
        },
        err => console.log(err)
      );
      this.matrixService.GetMatrixPlan().subscribe(
        res => {
          this.matrixPlan = res.data;
          this.plan = this.matrixPlan[0];
          console.log(this.matrixPlan);
        },
        err => console.log(err)
      );
  }

  GetTeam(){
    this.customerService.MyReferral(this.CustomerId,this.LevelId).subscribe(
      res => {
        this.teamMembers = res.data.Data;
      },
      err => console.log(err)
    );
  }
}
