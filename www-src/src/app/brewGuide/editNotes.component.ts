import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    moduleId: module.id,
    selector: 'app-brewguide-edit-notes-dialog',
    template: ` <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Notes</h5>
                    <button type="button" class="close" (click)="activeModal.dismiss('cancel')">
                    <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <textarea type="text" class="form-control" style="height: 400px;"
                    [(ngModel)]="notes" placeholder="Enter your notes here"></textarea>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss('cancel')">Cancel</button>
                    <button type="button" class="btn btn-primary" (click)="activeModal.close(notes)">Yes</button>
                </div>
                `
})
export class EditNotesComponent implements OnInit {


    notes: string;

    constructor(public activeModal: NgbActiveModal) {

    }

    ngOnInit(): void {

    }

}
