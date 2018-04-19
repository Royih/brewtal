import { Injectable } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmDialogComponent } from './confirmDialog.component';

@Injectable()
export class ConfirmService {


    constructor(private modalService: NgbModal) { }

    public display(message?: string, title?: string): Promise<boolean> {
        const confirmComponent = this.modalService.open(ConfirmDialogComponent);
        if (title) {
            confirmComponent.componentInstance.title = title;
        }
        if (message) {
            confirmComponent.componentInstance.message = message;
        }

        return confirmComponent.result.then((result) => {
            return result;
        }, (reason) => {
            return false;
        });
    }

}
