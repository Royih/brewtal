import { Pipe, PipeTransform } from '@angular/core';
import { DecimalPipe } from '@angular/common';
@Pipe({ name: 'decimal' })
export class NumberPipe implements PipeTransform {
    constructor(private decimalPipe: DecimalPipe) { }
    transform(value: Date): string {
        return value ? this.decimalPipe.transform(value, '1.2-2') : '';
    }
}
