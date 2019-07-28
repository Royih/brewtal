import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Subject } from 'rxjs';

import { HardwareStatusDto } from '../../models';
import { SignalRService } from '../../infrastructure/signalRService';

@Component({
  selector: 'app-pids',
  templateUrl: './pids.component.html',
  styleUrls: ['./pids.component.css']
})
export class PidsComponent implements OnInit {

  newLogName = '';
  startingLogging = false;
  stoppingLogging = false;

  hwStatus: HardwareStatusDto;

  constructor(private http: HttpClient, private signalR: SignalRService) {

    this.signalR.hwStatus.subscribe(res => {
      this.hwStatus = res;
    });
  }

  ngOnInit(): void {

  }

}
