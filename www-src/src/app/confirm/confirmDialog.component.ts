import { Component } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    moduleId: module.id,
    selector: 'app-confirm-dialog',
    template: ` <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">{{title}}</h5>
                    <button type="button" class="close" (click)="activeModal.dismiss('cancel')">
                    <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    {{message}}
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss('cancel')">Cancel</button>
                    <button type="button" class="btn btn-primary" (click)="activeModal.close(true)">Yes</button>
                </div>
                `
})
export class ConfirmDialogComponent {

    public title = '';
    public message = 'Are you sure?';

    constructor(public activeModal: NgbActiveModal) {

    }

}
