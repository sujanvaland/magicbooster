import {Component } from '@angular/core';
import { navItems } from '../../_nav';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html'
})
export class DefaultLayoutComponent {
  constructor(private router:Router){}
  public sidebarMinimized = false;
  public navItems = navItems;
  showSpinner : boolean = false;
  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }

  toggleSpinner(){
    this.showSpinner = !this.showSpinner;
  }

  Logout(){
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
