import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgbModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { PidComponent } from './pid/pid.component';
import { BrewtalModule } from '../common/brewtal.module';
import { CustomInterceptor } from '../infrastructure/customInterceptor';

import { LogsComponent } from './logs/logs.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { PidsComponent } from './pids/pids.component';
import { ConfirmDialogComponent, ConfirmService } from './confirm';

import { LogDetailsComponent } from './log-details/log-details.component';
import { LogDetailsRenameDialogComponent } from './log-details/renameDialog.component';
import { PidConfigDialogComponent } from './pid/pidConfig.component';

import { ToasterModule } from 'angular2-toaster';
import { ToastMaster } from '../infrastructure/toastMaster';

import { ChartsModule } from 'ng2-charts';

import { BrewGuideModule } from '../brewGuide/brewGuide.module';
import { PipeModule } from './pipes/pipe.module';
import { DateTimePipe } from './pipes/dateTimePipe';
import { DatePipe } from '@angular/common';
import { DateTimeSecPipe } from './pipes/dateTimeSecPipe';
import { SignalRService } from '../infrastructure/signalRService';


const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: PidsComponent },
  { path: 'logs', component: LogsComponent },
  { path: 'logs/:sessionId', component: LogDetailsComponent },
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
    PidConfigDialogComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(routes),
    ToasterModule.forRoot(),
    ChartsModule,
    NgbModule.forRoot(), NgbModalModule.forRoot(),
    BrewGuideModule,
    BrewtalModule.forRoot(),
    PipeModule.forRoot()

  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: CustomInterceptor,
    multi: true
  }, ConfirmService, ToastMaster, DatePipe, DateTimePipe, DateTimeSecPipe, SignalRService],
  bootstrap: [AppComponent],
  entryComponents: [ConfirmDialogComponent, LogDetailsRenameDialogComponent, PidConfigDialogComponent]
})
export class AppModule { }
