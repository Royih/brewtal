import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { LogRecordDto, LogSessionDto } from '../../models';
import { DateTimeSecPipe } from '../pipes';
import { ConfirmService } from '../confirm';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LogDetailsRenameDialogComponent } from './renameDialog.component';

@Component({
  selector: 'app-log-details',
  templateUrl: './log-details.component.html',
  styleUrls: ['./log-details.component.css']
})
export class LogDetailsComponent implements OnInit {

  sessionId: number;
  data: LogRecordDto[];
  session: LogSessionDto;
  pid = 1;
  resolution = 15;

  constructor(private route: ActivatedRoute, private http: HttpClient, private datePipe: DateTimeSecPipe,
    private confirm: ConfirmService, private router: Router, private modalService: NgbModal) { }

  public lineChartData: Array<any>;
  public lineChartLabels: Array<any>;
  public lineChartOptions: any = {
    responsive: true
  };

  public lineChartLegend = true;
  public lineChartType = 'line';

  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }

  refreshData(): void {
    this.lineChartData = null;
    this.http.get<LogSessionDto>('logging/get/' + this.sessionId).toPromise().then(res => {
      this.session = res;
    });

    this.http.get<LogRecordDto[]>('logging/listLogRecords/' + this.sessionId + '/' + this.resolution).toPromise().then(res => {
      this.data = res;
      this.selectPid(this.pid);
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.sessionId = params.sessionId;

      this.refreshData();

    });

  }

  delete(): void {
    this.confirm.display('This log session and everything about it will be lost forever...', 'Are you sure?').then(res => {
      if (res) {
        this.http.post('logging/delete', { logSession: this.session }).toPromise().then(res2 => {
          this.router.navigate(['../logs']);
        });
      }
    });
  }

  rename(): void {
    const renameComponent = this.modalService.open(LogDetailsRenameDialogComponent);
    renameComponent.componentInstance.newName = this.session.name;

    renameComponent.result.then((result) => {
      this.http.post('logging/renameSession', { session: this.session, newName: result }).toPromise().then(res2 => {
        this.refreshData();
      });
    }, (reason) => {

    });
  }


  selectPid(pid: number): void {
    this.pid = pid;
    this.lineChartLabels = this.data.map(x => this.datePipe.transform(x.timeStamp));
    if (pid === 1) {
      this.lineChartData = [
        { data: this.data.map(x => x.actualTemp1), label: 'Temp' },
        { data: this.data.map(x => x.targetTemp1), label: 'Target' },
        { data: this.data.map(x => x.output1 ? 100 : 0), label: 'Output' }
      ];
    } else if (pid === 2) {
      this.lineChartData = [
        { data: this.data.map(x => x.actualTemp2), label: 'Temp' },
        { data: this.data.map(x => x.targetTemp2), label: 'Target' },
        { data: this.data.map(x => x.output2 ? 100 : 0), label: 'Output' }
      ];
    }
  }

  changeResolution(newResolution: number): void {
    this.lineChartData = null;
    this.resolution = newResolution;
    this.refreshData();
  }



}
