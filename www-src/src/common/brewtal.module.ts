import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OutputComponent } from './output/output.component';
import { OwlDateTimeModule, OWL_DATE_TIME_FORMATS } from 'ng-pick-datetime';
import { OwlMomentDateTimeModule } from 'ng-pick-datetime-moment';

export const MY_MOMENT_FORMATS = {
    parseInput: 'YYYY-MM-DD HH:MM',
    fullPickerInput: 'YYYY-MM-DD HH:mm',
    datePickerInput: 'YYYY-MM-DD',
    timePickerInput: 'HH:MM',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
};

@NgModule({
    imports: [CommonModule, OwlDateTimeModule, OwlMomentDateTimeModule],
    declarations: [OutputComponent],
    exports: [OutputComponent, OwlDateTimeModule, OwlMomentDateTimeModule],
    providers: [{ provide: OWL_DATE_TIME_FORMATS, useValue: MY_MOMENT_FORMATS }]
})

export class BrewtalModule {

    static forRoot() {
        return {
            ngModule: BrewtalModule,
            providers: [],
        };
    }
}
