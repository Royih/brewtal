import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    moduleId: module.id,
    selector: 'app-start-logging-dialog',
    template: ` <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Notes</h5>
                    <button type="button" class="close" (click)="activeModal.dismiss('cancel')">
                    <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <input type="text" class="form-control" [(ngModel)]="name" placeholder="Enter log name here">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss('cancel')">Cancel</button>
                    <button type="button" class="btn btn-primary" (click)="activeModal.close(name)">Yes</button>
                </div>
                `
})
export class StartLoggingComponent implements OnInit {

    name: string;

    constructor(public activeModal: NgbActiveModal) {

    }

    ngOnInit(): void {

    }

}
