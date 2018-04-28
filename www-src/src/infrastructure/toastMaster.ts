
import { Injectable } from '@angular/core';
import { ToasterService, Toast, ToasterConfig } from 'angular2-toaster';

@Injectable()
export class ToastMaster {

    private toasterService: ToasterService;

    constructor(toasterService: ToasterService) {
        this.toasterService = toasterService;
    }

    public displayBriefMessage(title: string, body: string, isError: boolean, timeout = 5000) {

        const toast: Toast = {
            type: isError ? 'error' : 'success',
            title: title,
            body: body,
            timeout: timeout,
            /*toasterConfig: new ToasterConfig(
                {
                    positionClass: "toast-bottom-full-width"
                })*/

        };
        this.toasterService.pop(toast);
    }

    public displayMessage(title: string, message: any, isError: boolean) {
        let body = '';
        if (message.constructor === Array) {
            body = message.join('\n');
            console.log(body);
        } else {
            body = message;
        }
        const toast: Toast = {
            type: isError ? 'error' : 'success',
            title: title,
            body: body,
            timeout: 0
        };
        this.toasterService.pop(toast);
    }
}
