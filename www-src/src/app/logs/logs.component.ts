import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/debounceTime';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html'
})
export class LogsComponent implements OnInit {

  sessions: any;

  constructor(private http: HttpClient) {

  }

  private loadData() {
    this.http.get('logging/list').toPromise().then(res => {
      this.sessions = res;
    });
  }

  ngOnInit(): void {
    this.loadData();
  }

}
