import { Component } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    moduleId: module.id,
    selector: 'app-log-details-rename-dialog',
    template: ` <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">{{title}}</h5>
                    <button type="button" class="close" (click)="activeModal.dismiss('cancel')">
                    <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <label>New name:</label>
                    <input type="text" class="form-control" [(ngModel)]="newName" >
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss('cancel')">Cancel</button>
                    <button type="button" class="btn btn-primary" [disabled]="!newName" (click)="activeModal.close(newName)">Yes</button>
                </div>
                `
})
export class LogDetailsRenameDialogComponent {

    name = '';

    constructor(public activeModal: NgbActiveModal) {

    }


}
