import { Component,OnInit  } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MatrixService } from '../../services/matrix.service';
import { environment } from '../../../environments/environment';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'dailytask',
  templateUrl: 'dailytask.component.html'
})
export class DailyTaskComponent implements OnInit {

CustomerId: string = "";
showform: boolean = true;
constructor(private formBuilder: FormBuilder,
  private toastr:ToastrService,
  private matrixservice: MatrixService,
  private router: Router) { }
  
  ngOnInit (){
    this.CustomerId = localStorage.getItem("CustomerId");
  }
}
