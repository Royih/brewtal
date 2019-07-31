import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditBrewlogComponent } from './edit.component';
import { Edit2BrewlogComponent } from './edit2/edit.component';
import { ListBrewlogComponent } from './list.component';
import { BrewGuideComponent } from './brewGuide.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrewGuideRoutingModule } from './brewGuide.routes';

import { MomentModule } from 'angular2-moment';
import { ResumeComponent } from './resume.component';
import { PipeModule } from '../pipes/pipe.module';
import { BrewtalModule } from '../../common/brewtal.module';
import { EditNotesComponent } from './editNotes.component';
import { XComponentsModule } from '../../infrastructure/xComponents/x-components.module';

@NgModule({
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule, BrewGuideRoutingModule, MomentModule,
    PipeModule.forRoot(), BrewtalModule.forRoot(), XComponentsModule],
  declarations: [EditBrewlogComponent, Edit2BrewlogComponent, ListBrewlogComponent, BrewGuideComponent, ResumeComponent,
    EditNotesComponent],
  entryComponents: [EditNotesComponent]
})
export class BrewGuideModule { }
