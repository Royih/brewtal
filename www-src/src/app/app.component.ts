import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection } from '@aspnet/signalr-client';
import { environment } from '../environments/environment';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/debounceTime';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'app';

  public async: any;
  message = '';
  messages: string[] = [];

  gpio26 = false;
  gpio6 = false;
  gpio22 = false;
  gpio4 = false;

  private _hubConnection: HubConnection;

  pidUpdateStatus: any;

  constructor(private http: HttpClient) {

    this._hubConnection = new HubConnection(environment.apiUrl + 'temp');

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

    this.http.get(environment.apiUrl + 'api/pins/get/26').toPromise().then((res: boolean) => {
      this.gpio4 = res;
    });
    this.http.get(environment.apiUrl + 'api/pins/get/6').toPromise().then((res: boolean) => {
      this.gpio6 = res;
    });
    this.http.get(environment.apiUrl + 'api/pins/get/22').toPromise().then((res: boolean) => {
      this.gpio22 = res;
    });
    this.http.get(environment.apiUrl + 'api/pins/get/4').toPromise().then((res: boolean) => {
      this.gpio4 = res;
    });

  }

  togglePin(pin: number): void {
    let myVal = false;
    if (pin === 26) {
      this.gpio26 = !this.gpio26;
      myVal = this.gpio26;
    } else if (pin === 6) {
      this.gpio6 = !this.gpio6;
      myVal = this.gpio6;
    } else if (pin === 22) {
      this.gpio22 = !this.gpio22;
      myVal = this.gpio22;
    } else if (pin === 4) {
      this.gpio4 = !this.gpio4;
      myVal = this.gpio4;
    }

    this.http.post(environment.apiUrl + 'api/pins/set', { PinId: pin, Status: myVal }).toPromise().then(res => {
      console.log('Did toggle pin ', pin, res);
    });
  }

}
