import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListBrewlogComponent } from './list.component';
import { EditBrewlogComponent } from './edit.component';
import { BrewGuideComponent } from './brewGuide.component';
import { ResumeComponent } from './resume.component';

const myRoutes: Routes = [
  { path: 'brew', component: ListBrewlogComponent },
  { path: 'brew/resume', component: ResumeComponent },
  { path: 'brew/edit/:id', component: EditBrewlogComponent },
  { path: 'brew/:id', component: BrewGuideComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(myRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class BrewGuideRoutingModule { }
