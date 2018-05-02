import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastMaster } from '../infrastructure/toastMaster';
import { Subscription } from 'rxjs/Subscription';
import { HttpClient } from '@angular/common/http';
import { ConfirmService } from '../app/confirm';

@Component({
    moduleId: module.id,
    selector: 'app-edit-brew',
    templateUrl: 'edit.component.html',
    styleUrls: ['edit.component.css']
})
export class EditBrewlogComponent implements OnInit {

    brewlog: any;
    private sub: Subscription;
    private id: number;
    initializeResult: any;

    constructor(private route: ActivatedRoute, private http: HttpClient,
        private toaster: ToastMaster, private router: Router, private confirm: ConfirmService) {

    }

    private load() {
        this.http.get('brewguide/setup/' + this.id).toPromise().then(res => {
            this.brewlog = res;
        });
    }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            this.id = +params['id']; // (+) converts string 'id' to a number
            this.load();
        });
    }

    save() {
        this.http.post('brewguide/saveSetup', this.brewlog).toPromise().then((res: any) => {
            this.toaster.displayBriefMessage('Saved', 'Your changes has been saved', false);
            this.router.navigate(['brew', res.id]);

        });
    }

    delete() {
        this.confirm.display('Are you sure you want to delete this brew completly?', 'Are you sure?').then(res => {
            if (res) {
                this.http.post('brewguide/delete', this.brewlog)
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
            this.router.navigate(['brews']);
        } else {
            this.router.navigate(['brew', this.id]);
        }
    }

}
