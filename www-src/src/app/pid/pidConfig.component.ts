import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';


@Component({
    moduleId: module.id,
    selector: 'app-pid-config-dialog',
    template: ` <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Configure pid "{{pidName}}"</h5>
                    <button type="button" class="close" (click)="activeModal.dismiss('cancel')">
                    <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <label>Kp:</label>
                    <input type="number" class="form-control" [(ngModel)]="pidConfig.pidKp" >
                    <label>Ki:</label>
                    <input type="number" class="form-control" [(ngModel)]="pidConfig.pidKi" >
                    <label>Kd:</label>
                    <input type="number" class="form-control" [(ngModel)]="pidConfig.pidKd" >
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss('cancel')">Cancel</button>
                    <button type="button" class="btn btn-primary" (click)="activeModal.close(pidConfig)">Save</button>
                </div>
                `
})
export class PidConfigDialogComponent implements OnInit {


    pidId: number;
    pidName = '';
    pidConfig: any;

    constructor(public activeModal: NgbActiveModal, private http: HttpClient) {

    }


    ngOnInit(): void {
        this.http.get('pid/' + this.pidId).toPromise().then(res => {
            this.pidConfig = res;
        });
    }




}
