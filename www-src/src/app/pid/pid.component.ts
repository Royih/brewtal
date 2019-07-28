import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Subject } from 'rxjs';

import { PidConfigDialogComponent } from './pidConfig.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HardwareStatusDto } from '../../models';
import { SignalRService } from '../../infrastructure/signalRService';
import { debounceTime, distinctUntilChanged, tap, switchMap, merge, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-pid',
  templateUrl: './pid.component.html',
  styleUrls: ['./pid.component.css']
})
export class PidComponent implements OnInit {

  @Input() name = '';
  @Input() pidId = 0;

  pidStat: any;

  message = '';
  messages: string[] = [];
  targetTemp = 0;

  freeValuePin = 0;
  freeValue = false;


  gpio26 = false;
  gpio6 = false;
  gpio22 = false;
  gpio4 = false;

  targetChanges = new Subject<number>();
  targetTempChanging = false;

  constructor(private http: HttpClient, private modalService: NgbModal, private signalR: SignalRService) {
    this.signalR.hwStatus.subscribe(res => {
      this.pidStat = res.pids.find(x => x.pidId === this.pidId);
      if (!this.targetTempChanging) {
        this.targetTemp = this.pidStat.targetTemp;
      }
    });
  }

  ngOnInit(): void {

    this.targetChanges.pipe(debounceTime(1000)).subscribe(res => {
      this.changeTargetTemp();
    });
    this.targetChanges.subscribe(res => {
      this.targetTempChanging = true;
    });

  }

  updateTargetTemp(change) {
    let newTemp = this.targetTemp + change;
    if (newTemp < 0) {
      newTemp = 0;
    } else if (newTemp > 100) {
      newTemp = 100;
    }
    this.targetTemp = newTemp;
    this.targetChanges.next(newTemp);
  }

  changeTargetTemp(): void {

    this.signalR.invoke('UpdateTarget', { PidId: this.pidId, NewTargetTemp: this.targetTemp }).then(res => {
      console.log('Target temp changed to: ' + this.targetTemp + ' for pid: ' + this.pidId);
      this.targetTempChanging = false;
    });

    /*this.http.post('pid/updateTarget', { PidId: this.pidId, NewTargetTemp: this.targetTemp }).toPromise().then(res => {

    });*/

  }

  showPidConfig(): void {
    const configDialog = this.modalService.open(PidConfigDialogComponent);
    configDialog.componentInstance.pidName = this.name;
    configDialog.componentInstance.pidId = this.pidId;

    configDialog.result.then((result) => {
      this.http.post('pid/updatePidConfig', {
        PIDId: this.pidId,
        PIDKp: result.pidKp,
        PIDKi: result.pidKi,
        PIDKd: result.pidKd
      }).toPromise().then(res2 => {

      });
    }, (reason) => {

    });
  }

}
