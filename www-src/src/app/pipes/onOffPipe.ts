import { Pipe, PipeTransform } from '@angular/core';
@Pipe({ name: 'onOff' })
export class OnOffPipe implements PipeTransform {
    constructor() { }
    transform(value: boolean): string {
        return value ? 'On' : 'Off';
    }
}
