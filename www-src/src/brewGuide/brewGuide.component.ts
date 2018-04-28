import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastMaster } from '../infrastructure/toastMaster';
import { Subscription } from 'rxjs/Subscription';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';
import * as moment from 'moment';
import { HubConnection } from '@aspnet/signalr-client';
import { PidStatusesDto, PidStatusDto } from '../models';

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
    pidStatuses: PidStatusesDto;
    pid1Status: PidStatusDto;
    pid2Status: PidStatusDto;

    private _hubConnection: HubConnection;


    constructor(private route: ActivatedRoute, private http: HttpClient, private toaster: ToastMaster, private router: Router) {
        this._hubConnection = new HubConnection(environment.apiUrl + 'brewtal');
        this._hubConnection.on('PIDUpdate', (data: PidStatusesDto) => {
            this.pidStatuses = data;
            this.pid1Status = data.pids[0];
            this.pid2Status = data.pids[1];
        });

        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started');
            })
            .catch(() => {
                console.log('Error while establishing connection');
            });
    }


    private load() {
        this.http.get('brewGuide/' + this.id).toPromise().then(res => {
            this.brew = res;
            this.startTime = this.brew.currentStep.startTime;

            this.updateStepTime();
            if (this.brew.currentStep.completeTime) {
                this.completeTime = this.brew.currentStep.completeTime;
                this.updateCountdown();
            }
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
        setTimeout(function () {
            const endTime = moment.utc(self.completeTime);
            self.countDown = moment.utc(endTime.diff(moment())).format('HH:mm:ss');
            self.updateCountdown();
        }, 1000);
    }

    goToNextStep() {
        this.http.post('brewGuide/goToNextStep', { brewId: this.id })
            .subscribe(
                () => this.load()
            );
    }

    goBackOneStep() {
        if (confirm('Are you sure you want to abort the current step and return to the previous?')) {
            this.http.post('brewGuide/goBackOneStep', { brewId: this.id })
                .subscribe(
                    () => this.load()
                );
        }
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

}
