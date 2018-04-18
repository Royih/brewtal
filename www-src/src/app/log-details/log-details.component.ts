import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { LogRecordDto } from '../../models/logRecordDto';
import { DateTimeSecPipe } from '../pipes';

@Component({
  selector: 'app-log-details',
  templateUrl: './log-details.component.html',
  styleUrls: ['./log-details.component.css']
})
export class LogDetailsComponent implements OnInit {

  sessionId: number;
  data: LogRecordDto[];
  pid = 1;
  resolution = 15;

  constructor(private route: ActivatedRoute, private http: HttpClient, private datePipe: DateTimeSecPipe) { }

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
    this.http.get<LogRecordDto[]>('logging/get/' + this.sessionId + '/' + this.resolution).toPromise().then(res => {
      this.data = res;
      this.selectPid(this.pid);
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.sessionId = params.sessionId;
    });
    this.refreshData();
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
