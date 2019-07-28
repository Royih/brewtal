
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '../environments/environment';
import { HardwareStatusDto } from '../models';
import { Subject } from 'rxjs';
import { ToastMaster } from './toastMaster';
import { timeout } from 'q';

@Injectable()
export class SignalRService {

    hubConnection: HubConnection;
    hwStatus = new Subject<HardwareStatusDto>();
    brewUpdated = new Subject<number>();
    status = 'Pending';
    dataReceivedDate: Date;
    allowReconnect = false;
    heartbeat = false;

    start(): Promise<boolean> {
        this.allowReconnect = false;
        this.heartbeat = false;

        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.apiUrl + 'brewtal')
            .build();

        this.hubConnection.on('HarwareStatus', (data: HardwareStatusDto) => {
            this.hwStatus.next(data);
            this.dataReceivedDate = new Date();
            this.status = 'Ok';
            this.heartbeat = !this.heartbeat;
        });

        this.hubConnection.on('BrewUpdated', (brewId: number) => {
            console.log(`Brew with id ${brewId} was updated`);
            this.brewUpdated.next(brewId);
        });

        this.hubConnection.onclose(res => {
            this.status = 'Closed';
            this.allowReconnect = true;
            this.heartbeat = false;
            console.log('Initiating reconnecto');
            this.reconnecto();
        });

        return this.hubConnection.start()
            .then(() => {
                console.log('Hub connection started');
                this.status = 'Connected';
                return true;
            })
            .catch(() => {
                this.status = 'Conn.err';
                this.allowReconnect = true;
                console.log('Error while establishing connection');
                return false;
            });
    }

    constructor(private toastMasta: ToastMaster) {
        this.start();
    }

    reconnecto() {
        this.toastMasta.displayBriefMessage('Reconnecting..', 'Reconnecting SignalR', false);
        this.start().then(res => {
            if (!res) {
                setTimeout(this.reconnecto, 4000);
            }
        });
    }

    invoke(methodName: string, args: any): Promise<any> {
        console.log('Invoking', methodName, 'with arguments: ', args);
        return this.hubConnection.invoke(methodName, args);
    }

}
