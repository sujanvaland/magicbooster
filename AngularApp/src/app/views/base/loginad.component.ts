import { Component ,OnInit} from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
@Component({
  templateUrl: 'loginad.component.html'
})
export class LoginAdComponent implements OnInit{

loginad: FormGroup;
submitted = false;

constructor(private formBuilder: FormBuilder) { }

ngOnInit (){
    this.loginad =this.formBuilder.group({
    name: ['', Validators.required],
    assigncredit:['', Validators.required],
    url:['', Validators.required]
  });
}

get f() { return this.loginad.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.loginad.invalid) {
            return;
        }

        // display form values on success
        alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.loginad.value, null, 4));
    }

    onReset() {
        this.submitted = false;
        this.loginad.reset();
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
