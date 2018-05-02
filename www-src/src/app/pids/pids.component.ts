import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection } from '@aspnet/signalr-client';
import { environment } from '../../environments/environment';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/debounceTime';
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

  startLogging(): void {
    this.startingLogging = true;
    this.http.post('logging/start', { name: this.newLogName }).toPromise().then(res => {
      this.stoppingLogging = false;
      console.log('Logging started');
    });
  }

  stopLogging(): void {
    this.stoppingLogging = true;
    this.http.post('logging/stop', {}).toPromise().then(res => {
      this.startingLogging = false;
      console.log('Logging stopped');
    });
  }
}
