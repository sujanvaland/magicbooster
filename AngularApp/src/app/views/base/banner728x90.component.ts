import { Component ,OnInit} from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
@Component({
  templateUrl: 'banner728x90.component.html'
})
export class banner728x90Component  implements OnInit{

banner728x90: FormGroup;
submitted = false;

constructor(private formBuilder: FormBuilder) { }

ngOnInit (){
    this.banner728x90 =this.formBuilder.group({
    name: ['', Validators.required],
    assigncredit:['', Validators.required],
    url:['', Validators.required]
  });
}

get f() { return this.banner728x90.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.banner728x90.invalid) {
            return;
        }

        // display form values on success
        alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.banner728x90.value, null, 4));
    }

    onReset() {
        this.submitted = false;
        this.banner728x90.reset();
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
