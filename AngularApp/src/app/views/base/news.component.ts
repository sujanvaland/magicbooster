import { Component, ViewChild } from '@angular/core';
import { ReportService } from '../../services/report.service';
import { CommonService } from '../../services/common.service';
import { ModalDirective } from 'ngx-bootstrap';

@Component({
  templateUrl: 'news.component.html'
})
export class NewsComponent {

  constructor(private commonservice:CommonService) { }
  CustomerId:string = localStorage.getItem("CustomerId");
  NewsData = [];
  NewsLetter = { Body:'' };
  @ViewChild('infoModal') public infoModal: ModalDirective;
  ngOnInit(): void {
    this.commonservice.GetNewsletter()
    .subscribe(
      res => {
        console.log(res.data);
        this.NewsData = JSON.parse(res.data);
      },
      err => console.log(err)
    )
  }

  ViewNewsLetter(NewsLetter){
    this.NewsLetter.Body = NewsLetter.Body;
    this.infoModal.show();
  }
}
