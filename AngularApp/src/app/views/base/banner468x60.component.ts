import { Component ,OnInit} from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';

@Component({
  templateUrl: 'banner468x60.component.html'
})
export class banner468x60Component  implements OnInit{

  banner468x60: FormGroup;
  submitted = false;
  
  constructor(private formBuilder: FormBuilder) { }
  
  ngOnInit (){
      this.banner468x60 =this.formBuilder.group({
      name: ['', Validators.required],
      assigncredit:['', Validators.required],
      url:['', Validators.required]
    });
  }
  
  get f() { return this.banner468x60.controls; }
  
      onSubmit() {
          this.submitted = true;
  
          // stop here if form is invalid
          if (this.banner468x60.invalid) {
              return;
          }
  
          // display form values on success
          alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.banner468x60.value, null, 4));
      }
  
      onReset() {
          this.submitted = false;
          this.banner468x60.reset();
      }

  isCollapsed: boolean = false;
  iconCollapse: string = 'icon-arrow-up';
  
  collapsed(event: any): void {
    // console.log(event);
  }

  expanded(event: any): void {
    // console.log(event);
  }

  toggleCollapse(): void {
    this.isCollapsed = !this.isCollapsed;
    this.iconCollapse = this.isCollapsed ? 'icon-arrow-down' : 'icon-arrow-up';
  }

}
