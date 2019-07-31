import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatButtonModule, MatCheckboxModule, MatTabsModule, MatInputModule, MatSelectModule, MatDialogModule,
  MatToolbarModule
} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { XInputComponent } from './x-input/x-input.component';
import { XSelectComponent } from './x-select/x-select.component';
import { XCheckComponent } from './x-check/x-check.component';
import { XMultiSelectComponent } from './x-multi-select/x-multi-select.component';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { XCanDeactivateGuard } from './utils';
import { XConfirmDialogComponent } from './utils/x-confirm-dialog/x-confirm-dialog.component';
import { XPageHeaderComponent } from './x-page-header/x-page-header-component';
import { XDateComponent } from './x-date/x-date.component';
import { OwlDateTimeModule, OWL_DATE_TIME_FORMATS } from 'ng-pick-datetime';
import { OwlMomentDateTimeModule } from 'ng-pick-datetime-moment';


@NgModule({
  declarations: [XInputComponent, XSelectComponent, XCheckComponent, XMultiSelectComponent, XConfirmDialogComponent, XPageHeaderComponent,
    XDateComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule, MatCheckboxModule, MatTabsModule, MatInputModule, MatSelectModule, MatCheckboxModule, ScrollingModule,
    MatDialogModule, MatToolbarModule, OwlDateTimeModule, OwlMomentDateTimeModule,
  ],
  exports: [
    MatTabsModule, ScrollingModule, XInputComponent, XSelectComponent, XCheckComponent, XMultiSelectComponent, XConfirmDialogComponent,
    MatButtonModule, XPageHeaderComponent, XDateComponent
    /*MatButtonModule, MatCheckboxModule*/
  ],
  providers: [
    XCanDeactivateGuard
  ],
  entryComponents: [XConfirmDialogComponent]
})
export class XComponentsModule { }
