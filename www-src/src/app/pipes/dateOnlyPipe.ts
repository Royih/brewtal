import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';
@Pipe({ name: 'dateOnly' })
export class DateOnlyPipe implements PipeTransform {
    constructor(private datePipe: DatePipe) { }
    transform(value: Date): string {
        return value ? this.datePipe.transform(value, 'yyyy-MM-dd') : '';
    }
}
