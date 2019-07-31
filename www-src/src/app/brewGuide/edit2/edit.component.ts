import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastMaster } from '../../../infrastructure/toastMaster';
import { Subscription, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ConfirmService } from '../../confirm';
import { FormGroup } from '@angular/forms';
import { XFullFormInterface, XCanComponentDeactivate } from '../../../infrastructure/xComponents/utils';
import { BrewDto } from '../../../models';

@Component({
    moduleId: module.id,
    selector: 'app-edit2-brew',
    templateUrl: 'edit.component.html',
    styleUrls: ['edit.component.css']
})
export class Edit2BrewlogComponent implements OnInit, OnDestroy, XCanComponentDeactivate, XFullFormInterface {

    private sub: Subscription;
    private id: number;
    initializeResult: any;

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

    private load() {
        this.http.get<BrewDto>('brewguide/setup/' + this.id).toPromise().then(brew => {
            this.brew = brew;
        });
    }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            this.id = +params['id']; // (+) converts string 'id' to a number
            this.load();
        });
    }

    ngOnDestroy(): void {
        if (this.sub) {
            this.sub.unsubscribe();
        }
    }

    save() {
        this.http.post('brewguide/saveSetup', this.brew).toPromise().then((res: any) => {
            this.toaster.displayBriefMessage('Saved', 'Your changes has been saved', false);
            this.router.navigate(['brew', res.id]);

        });
    }

    delete() {
        this.confirm.display('Are you sure you want to delete this brew completly?', 'Are you sure?').then(res => {
            if (res) {
                this.http.post('brewguide/delete', this.brew)
                    // .map(response => response.json())
                    .subscribe(
                        data => {
                            this.router.navigate(['brew']);
                        },
                        () => console.log('brewlog deleted.')
                    );
            }
        });
    }

    cancel() {
        if (this.id === 0) {
            this.router.navigate(['brew']);
        } else {
            this.router.navigate(['brew', this.id]);
        }
    }

}
