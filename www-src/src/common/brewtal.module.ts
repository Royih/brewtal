import { NgModule } from '@angular/core';
import { OutputComponent } from './output/output.component';

@NgModule({
    imports: [],
    declarations: [OutputComponent],
    exports: [OutputComponent]
})

export class BrewtalModule {

    static forRoot() {
        return {
            ngModule: BrewtalModule,
            providers: [],
        };
    }
}
