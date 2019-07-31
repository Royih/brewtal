import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastMaster } from '../../infrastructure/toastMaster';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import * as moment from 'moment';
import { PidStatusDto } from '../../models';
import { HardwareStatusDto } from '../../models/HardwareStatusDto';
import { SignalRService } from '../../infrastructure/signalRService';
import { ConfirmService } from '../confirm';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EditNotesComponent } from './editNotes.component';

@Component({
    moduleId: module.id,
    selector: 'app-brew-guide',
    templateUrl: 'brewGuide.component.html',
    styleUrls: ['brewGuide.component.css']
})

export class BrewGuideComponent implements OnInit {

    brew: any;
    startTime: any;
    completeTime: any;
    countDown: any;
    stepTime: any;
    brewHistory: any;
    dataCaptureValues: any;
    definedDataCaptureValues: any;
    changeDCTimeout: any;
    saving: boolean;
    private sub: Subscription;
    private id: number;
    pidStatuses: HardwareStatusDto;
    pid1Status: PidStatusDto;
    pid2Status: PidStatusDto;


    constructor(private route: ActivatedRoute, private http: HttpClient, private confirm: ConfirmService,
        private toaster: ToastMaster, private router: Router, private signalR: SignalRService,
        private modalService: NgbModal) {

        signalR.hwStatus.subscribe(res => {
            this.pidStatuses = res;
            this.pid1Status = res.pids[0];
            this.pid2Status = res.pids[1];
        });

        signalR.brewUpdated.subscribe((res: number) => {
            if (+this.id === res) {
                this.load();
            }
        });

    }


    private load() {
        this.http.get('brewGuide/' + this.id).toPromise().then(res => {
            this.brew = res;
            this.startTime = this.brew.currentStep.startTime;
            this.completeTime = this.brew.currentStep.completeTime;

            this.updateStepTime();
            this.updateCountdown();

            this.http.get('datacapture/' + this.brew.currentStep.id).toPromise().then(dataCaptureValues => {
                this.dataCaptureValues = dataCaptureValues;
            });
            this.getDefinedDataCaptureValues();
        });

        this.http.get('brewGuide/getBrewHistory/' + this.id).toPromise().then(res => {
            this.brewHistory = res;
        });
    }

    ngOnInit() {
        const self = this;
        this.sub = this.route.params.subscribe(params => {
            this.id = params['id']; // (+) converts string 'id' to a number
            this.load();
        });

    }

    private getDefinedDataCaptureValues() {
        this.http.get('datacapture/getDefinedValues/' + this.id).toPromise().then(res => {
            this.definedDataCaptureValues = res;
        });
    }


    private updateStepTime() {
        const self = this;
        setTimeout(function () {
            const startTime = moment.utc(self.startTime);
            self.stepTime = moment.utc(moment().diff(startTime)).format('HH:mm:ss');
            self.updateStepTime();
        }, 1000);
    }

    private updateCountdown() {
        const self = this;
        if (self.completeTime) {
            setTimeout(function () {
                const endTime = moment.utc(self.completeTime);
                self.countDown = moment.utc(endTime.diff(moment())).format('HH:mm:ss');
                self.updateCountdown();
            }, 1000);
        } else {
            self.countDown = null;
        }

    }

    goToNextStep() {
        this.http.post('brewGuide/goToNextStep', { brewId: this.id })
            .subscribe(
                () => this.load()
            );
    }

    goBackOneStep() {
        this.confirm.display('Are you sure you want to abort the current step and return to the previous?', 'Are you sure?').then(res => {
            if (res) {
                this.http.post('brewGuide/goBackOneStep', { brewId: this.id })
                    .subscribe(
                        () => this.load()
                    );
            }
        });
    }

    saveDataCaptureValues() {
        this.saving = true;
        this.saveIfNotChangedAfterNSeconds(1);
    }

    private saveIfNotChangedAfterNSeconds(seconds) {
        const self = this;
        if (this.changeDCTimeout) {
            clearTimeout(this.changeDCTimeout);
        }
        this.changeDCTimeout = setTimeout(function () {
            self.http.post('datacapture', self.dataCaptureValues).toPromise().then(res => {
                self.saving = null;
                self.getDefinedDataCaptureValues();
            });
        }, seconds * 1000); // delay n seconds
    }

    saveDataCaptureValues2() {
        this.saving = true;
        this.saveIfNotChangedAfterNSeconds2(1);
    }

    private saveIfNotChangedAfterNSeconds2(seconds) {
        const self = this;
        if (this.changeDCTimeout) {
            clearTimeout(this.changeDCTimeout);
        }
        this.changeDCTimeout = setTimeout(function () {
            self.http.post('datacapture', self.definedDataCaptureValues).toPromise().then(res => {
                self.saving = null;
                self.getDefinedDataCaptureValues();
            });
        }, seconds * 1000); // delay n seconds
    }

    editNotes() {
        const editNotesComponent = this.modalService.open(EditNotesComponent);
        editNotesComponent.componentInstance.notes = this.brew.setup.notes;
        return editNotesComponent.result.then(newNotes => {
            this.http.post('brewGuide/saveNotes', { brewId: this.id, Notes: newNotes }).toPromise().then(res => {
                this.brew.setup.notes = newNotes;
            });
        }, (reason) => {
            return false;
        });
    }

    editShoppingList() {
        const editNotesComponent = this.modalService.open(EditNotesComponent);
        editNotesComponent.componentInstance.notes = this.brew.setup.shoppingList;
        return editNotesComponent.result.then(shoppingList => {
            this.http.post('brewGuide/saveShoppingList', { brewId: this.id, ShoppingList: shoppingList }).toPromise().then(res => {
                this.brew.setup.shoppingList = shoppingList;
            });
        }, (reason) => {
            return false;
        });
    }

}
