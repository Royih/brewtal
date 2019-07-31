import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { MatDialog } from '@angular/material';
import { XConfirmDialogComponent } from './x-confirm-dialog/x-confirm-dialog.component';

export interface XCanComponentDeactivate {
    canDeactivate: () => Observable<boolean> | Promise<boolean> | boolean;
}

@Injectable()
export class XCanDeactivateGuard implements CanDeactivate<XCanComponentDeactivate> {

    constructor(public dialog: MatDialog) {

    }

    confirmUnsavedChanges(): Promise<boolean> {
        const dialogRef = this.dialog.open(XConfirmDialogComponent, {
            width: '350px',
            data: {
                title: 'Are you sure you want to leave this page?',
                message: `It seems like you have unsaved changes. 
                <br>If you leave, your changes will be lost. <br> <br><p>Are you sure you want to leave?</p>`,
                optionTrueLabelNotDefault: 'Leave',
                optionFalseLabelDefault: 'Stay'
            }
        });

        return dialogRef.afterClosed().toPromise().then(result => {
            return result;
        });

    }

    canDeactivate(component: XCanComponentDeactivate,
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot) {

        // let url: string = state.url;
        // console.log('Url: ' + url, component.canDeactivate);

        const allowDeactivate = component.canDeactivate ? component.canDeactivate() : true;

        if (!allowDeactivate) {
            return this.confirmUnsavedChanges().then(result => {
                return result === true;
            });
        } else {
            return true;
        }

    }
}
