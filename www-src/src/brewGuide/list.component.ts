import { Component, OnInit } from '@angular/core';
import * as _ from 'underscore';
import { HttpClient } from '@angular/common/http';

@Component({
    moduleId: module.id,
    selector: 'app-list-brews',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class ListBrewlogComponent implements OnInit {

    brewlogs: any;

    constructor(private http: HttpClient) {

    }

    ngOnInit() {
        this.http.get('brewguide').toPromise().then(res => {
            this.brewlogs = res;
        });
    }

}
