import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';
@Pipe({ name: 'dateTime' })
export class DateTimePipe implements PipeTransform {
    constructor(private datePipe: DatePipe) { }
    transform(value: Date): string {
        return value ? this.datePipe.transform(value, 'yyyy-MM-dd HH:mm') : '';
    }
}
