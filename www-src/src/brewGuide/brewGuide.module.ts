import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditBrewlogComponent } from './edit.component';
import { ListBrewlogComponent } from './list.component';
import { BrewGuideComponent } from './brewGuide.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BrewGuideRoutingModule } from './brewGuide.routes';
import { NguiDatetimePickerModule } from '@ngui/datetime-picker';
import { MomentModule } from 'angular2-moment';
import { ResumeComponent } from './resume.component';
import { PipeModule } from '../app/pipes/pipe.module';
import { BrewtalModule } from '../common/brewtal.module';

@NgModule({
  imports: [CommonModule, FormsModule, RouterModule, BrewGuideRoutingModule, NguiDatetimePickerModule, MomentModule,
    PipeModule.forRoot(), BrewtalModule.forRoot()],
  declarations: [EditBrewlogComponent, ListBrewlogComponent, BrewGuideComponent, ResumeComponent],
})
export class BrewGuideModule { }
