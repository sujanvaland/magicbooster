import { Component ,OnInit} from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
@Component({
  templateUrl: 'banner125x125.component.html'
})
export class banner125x125Component implements OnInit{

banner125x125: FormGroup;
submitted = false;

constructor(private formBuilder: FormBuilder) { }

ngOnInit (){
    this.banner125x125 =this.formBuilder.group({
    name: ['', Validators.required],
    assigncredit:['', Validators.required],
    url:['', Validators.required]
  });
}

get f() { return this.banner125x125.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.banner125x125.invalid) {
            return;
        }

        // display form values on success
        alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.banner125x125.value, null, 4));
    }

    onReset() {
        this.submitted = false;
        this.banner125x125.reset();
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
