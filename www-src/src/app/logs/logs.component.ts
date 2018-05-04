import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/debounceTime';
import { LogSessionDto } from '../../models';
import { StartLoggingComponent } from './startLogging.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html'
})
export class LogsComponent implements OnInit {

  sessions: LogSessionDto[];

  currentLogSession: LogSessionDto;

  constructor(private http: HttpClient, private modalService: NgbModal) {

  }

  private loadData() {
    this.http.get<LogSessionDto[]>('logging/list').toPromise().then(res => {
      this.sessions = res;
      this.currentLogSession = res.find(x => !x.completed);
    });
  }

  start() {
    const startLoggingComponent = this.modalService.open(StartLoggingComponent);
    return startLoggingComponent.result.then(name => {
      this.http.post('logging/start', { name: name }).toPromise().then(res => {
        console.log('Logging started');
        this.loadData();
      });
    }, (reason) => {
      return false;
    });
  }
  stop() {
    this.http.post('logging/stop', {}).toPromise().then(res => {
      this.loadData();
      console.log('Logging stopped');
    });
  }

  ngOnInit(): void {
    this.loadData();
  }

}
