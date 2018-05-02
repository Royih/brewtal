import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OutputComponent } from './output/output.component';

@NgModule({
    imports: [CommonModule],
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
