import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgbModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { PidComponent } from './pid/pid.component';
import { CustomInterceptor } from '../infrastructure/customInterceptor';

import { LogsComponent } from './logs/logs.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { PidsComponent } from './pids/pids.component';
import { ConfirmDialogComponent, ConfirmService } from './confirm';

import { YesNoPipe, DateTimePipe, DateOnlyPipe, DateTimeSecPipe, NumberPipe } from './pipes';
import { DatePipe } from '@angular/common';
import { LogDetailsComponent } from './log-details/log-details.component';
import { LogDetailsRenameDialogComponent } from './log-details/renameDialog.component';


import { ChartsModule } from 'ng2-charts';



const routes: Routes = [
  { 'path': '', 'redirectTo': '/home', 'pathMatch': 'full' },
  { 'path': 'home', 'component': PidsComponent },
  { 'path': 'logs', 'component': LogsComponent },
  { 'path': 'logs/:sessionId', 'component': LogDetailsComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    ConfirmDialogComponent,
    PidComponent,
    LogsComponent,
    HeaderComponent,
    FooterComponent,
    PidsComponent,
    LogDetailsComponent,
    LogDetailsRenameDialogComponent,
    YesNoPipe, DateTimePipe, DateOnlyPipe, DateTimeSecPipe, NumberPipe
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    ChartsModule,
    NgbModule.forRoot(), NgbModalModule.forRoot()
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: CustomInterceptor,
    multi: true
  }, DatePipe, DateTimeSecPipe, ConfirmService],
  bootstrap: [AppComponent],
  entryComponents: [ConfirmDialogComponent, LogDetailsRenameDialogComponent]
})
export class AppModule { }
