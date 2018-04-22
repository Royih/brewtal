import { Pipe } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Pipe({
    name: 'twodigits'
})
export class TwoDigitsPipe extends DecimalPipe {
    transform(value: number): any {
        return super.transform((value || 0), '1.2-2');
    }
}
