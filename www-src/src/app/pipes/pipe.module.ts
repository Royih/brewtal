import { NgModule } from '@angular/core';
import { OnOffPipe } from './onOffPipe';
import { SplitInLinesPipe } from './splitInLinesPipe';
import { YesNoPipe } from './yesNoPipe';
import { DateTimePipe } from './dateTimePipe';
import { DateOnlyPipe } from './dateOnlyPipe';
import { DateTimeSecPipe } from './dateTimeSecPipe';
import { TwoDigitsPipe } from './twoDigitsPipe';

@NgModule({
    imports: [],
    declarations: [OnOffPipe, SplitInLinesPipe, YesNoPipe, DateTimePipe, DateOnlyPipe, DateTimeSecPipe, TwoDigitsPipe],
    exports: [OnOffPipe, SplitInLinesPipe, YesNoPipe, DateTimePipe, DateOnlyPipe, DateTimeSecPipe, TwoDigitsPipe]
})

export class PipeModule {

    static forRoot() {
        return {
            ngModule: PipeModule,
            providers: [],
        };
    }
}
