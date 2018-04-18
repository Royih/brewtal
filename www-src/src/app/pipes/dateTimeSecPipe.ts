import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';
@Pipe({ name: 'dateTimeSec' })
export class DateTimeSecPipe implements PipeTransform {
    constructor(private datePipe: DatePipe) { }
    transform(value: Date): string {
        return value ? this.datePipe.transform(value, 'yyyy-MM-dd HH:mm:ss') : '';
    }
}
