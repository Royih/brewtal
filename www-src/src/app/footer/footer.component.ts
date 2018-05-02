import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../../infrastructure/signalRService';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {

  constructor(public signalR: SignalRService) {
    this.signalR.hwStatus.subscribe(res => {
    });
  }

  ngOnInit() {

  }

}
