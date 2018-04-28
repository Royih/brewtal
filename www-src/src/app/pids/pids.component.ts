import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection } from '@aspnet/signalr-client';
import { environment } from '../../environments/environment';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/debounceTime';

@Component({
  selector: 'app-pids',
  templateUrl: './pids.component.html',
  styleUrls: ['./pids.component.css']
})
export class PidsComponent implements OnInit {

  newLogName = '';
  startingLogging = false;
  stoppingLogging = false;

  private _hubConnection: HubConnection;

  pidUpdateStatus: any;

  constructor(private http: HttpClient) {

    this._hubConnection = new HubConnection(environment.apiUrl + 'brewtal');

    this._hubConnection.on('PIDUpdate', (data: any) => {
      this.pidUpdateStatus = data;
    });

    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
      })
      .catch(() => {
        console.log('Error while establishing connection');
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
