import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-common-output',
  templateUrl: './output.component.html',
  styleUrls: ['./output.component.css']
})
export class OutputComponent implements OnInit {

  gpio26 = false;
  gpio6 = false;
  gpio22 = false;
  gpio4 = false;

  constructor(private http: HttpClient) {

  }

  ngOnInit(): void {

    this.http.get('pins/get/26').toPromise().then((res: boolean) => {
      this.gpio4 = res;
    });
    this.http.get('pins/get/6').toPromise().then((res: boolean) => {
      this.gpio6 = res;
    });
    this.http.get('pins/get/22').toPromise().then((res: boolean) => {
      this.gpio22 = res;
    });
    this.http.get('pins/get/4').toPromise().then((res: boolean) => {
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

    this.http.post('pins/set', { PinId: pin, Status: myVal }).toPromise().then(res => {
      console.log('Did toggle pin ', pin, res);
    });
  }


}
