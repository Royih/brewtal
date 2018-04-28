import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import * as _ from 'underscore';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
    moduleId: module.id,
    selector: 'app-resume-brew',
    template: `
    <div>

    </div>
    `

})
export class ResumeComponent implements OnInit {

    brews: any;

    constructor(private http: HttpClient, private router: Router) {

    }

    ngOnInit() {
        this.http.get('brewguide').toPromise().then(res => {
            this.brews = res;
            if (this.brews) {
                // console.log(this.brewlogs[0].id);
                this.router.navigate(['brew/' + this.brews[0].id]);
            }
        });
    }

}
