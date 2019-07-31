import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastMaster } from '../../../infrastructure/toastMaster';
import { Subscription, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { XFullFormInterface, XCanComponentDeactivate } from '../../../infrastructure/xComponents/utils';
import { BrewDto } from '../../../models';
import { ConfirmService } from '../../../app/confirm';

@Component({
    moduleId: module.id,
    selector: 'app-x-component-demo',
    templateUrl: 'demo.component.html',
    styleUrls: ['demo.component.css']
})
export class DemoComponent implements OnInit, XCanComponentDeactivate, XFullFormInterface {

    tech = 'Bootstrap';
    brew: BrewDto;
    myFormGroup: FormGroup = new FormGroup({});

    constructor(private route: ActivatedRoute, private http: HttpClient,
        private toaster: ToastMaster, private router: Router, private confirm: ConfirmService) {

    }

    canDeactivate(): boolean | Observable<boolean> | Promise<boolean> {
        return !this.myFormGroup.dirty;
    }

    saveData(): Promise<boolean> {
        throw new Error('Method not implemented.');
    }

    loadData(): Promise<boolean> {
        throw new Error('Method not implemented.');
    }

    ngOnInit() {
        this.brew = <BrewDto>{
            id: 1,
            name: '',
            batchNumber: 123,
            beginMash: new Date(),
            mashTemp: 67.5,
            strikeTemp: 73.6,
            spargeTemp: 75.6,
            mashOutTemp: 78,
            mashTimeInMinutes: 60,
            boilTimeInMinutes: 60,
            batchSize: 40,
            mashWaterAmount: 30,
            spargeWaterAmount: 20
        };
    }

    save() {
        this.toaster.displayBriefMessage('Saved', 'Your changes has been (fake) saved', false);
    }

    delete() {
        this.confirm.display('Are you sure you want to delete this brew completly?', 'Are you sure?').then(res => {
            if (res) {

            }
        });
    }

}
