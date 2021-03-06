import { Pipe, PipeTransform } from '@angular/core';
@Pipe({ name: 'yesNo' })
export class YesNoPipe implements PipeTransform {
    constructor() { }
    transform(value: boolean): string {
        return value ? 'Yes' : 'No';
    }
}
