import { Component, OnInit } from '@angular/core';
import { ConfirmService } from '../confirm';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private confirm: ConfirmService, private http: HttpClient) { }

  ngOnInit() {
  }

  reboot() {
    this.confirm.display('Do you want to reebot the system?', 'Are you sure?').then(res => {
      if (res) {
        this.http.post('system/reboot', {}).toPromise().then(res2 => {
          console.log('System reboot initiated');
        });
      }
    });
  }

  shutdown() {
    this.confirm.display('Do you want to shutdown the system?', 'Are you sure?').then(res => {
      if (res) {
        this.http.post('system/shutdown', {}).toPromise().then(res2 => {
          console.log('System shutdown initiated');
        });
      }
    });
  }

}
