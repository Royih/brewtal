import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection } from '@aspnet/signalr-client';
import { HardwareStatusDto, ManualOutputDto } from '../../models';
import { SignalRService } from '../../infrastructure/signalRService';

@Component({
  selector: 'app-common-output',
  templateUrl: './output.component.html',
  styleUrls: ['./output.component.css']
})
export class OutputComponent implements OnInit {

  manualOutputs: ManualOutputDto[];

  constructor(private signalR: SignalRService) {

  }

  ngOnInit(): void {

    this.signalR.hwStatus.subscribe(res => {
      if (!this.manualOutputs) {
        this.manualOutputs = res.manualOutputs;
      } else {
        // Only update own copy if there is a changed value on one of the outputs
        for (let i = 0; i < res.manualOutputs.length; i++) {
          const existingOutput = this.manualOutputs.find(x => x.output === res.manualOutputs[i].output);
          if (existingOutput.value !== res.manualOutputs[i].value) {
            existingOutput.value = res.manualOutputs[i].value;
          }
        }
      }
    });

  }

  toggleOutput(output: ManualOutputDto) {
    output.value = !output.value;
    this.signalR.invoke('SetOutput', { Output: output.output, Value: output.value }).then(res => {
    });
  }

}
