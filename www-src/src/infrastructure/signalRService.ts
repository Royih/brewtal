
import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';
import { environment } from '../environments/environment';
import { HardwareStatusDto } from '../models';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class SignalRService {

    hubConnection: HubConnection;
    hwStatus = new Subject<HardwareStatusDto>();
    brewUpdated = new Subject<number>();
    status = 'Pending';
    dataReceivedDate: Date;
    allowReconnect = false;
    heartbeat = false;

    start() {
        this.allowReconnect = false;
        this.heartbeat = false;

        this.hubConnection = new HubConnection(environment.apiUrl + 'brewtal');

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
        });

        this.hubConnection.start()
            .then(() => {
                console.log('Hub connection started');
                this.status = 'Connected';
            })
            .catch(() => {
                this.status = 'Conn.err';
                this.allowReconnect = true;
                console.log('Error while establishing connection');
            });
    }

    constructor() {
        this.start();
    }

    invoke(methodName: string, args: any): Promise<any> {
        console.log('Invoking', methodName, 'with arguments: ', args);
        return this.hubConnection.invoke(methodName, args);
    }

}
