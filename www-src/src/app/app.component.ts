import { Component, OnInit } from '@angular/core';
import { ToasterConfig, ToasterService } from 'angular2-toaster';
import { ToastMaster } from '../infrastructure/toastMaster';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'app';

  public toasterconfig: ToasterConfig =
    new ToasterConfig({
      showCloseButton: true,
      animation: 'flyRight'
      /*tapToDismiss: false,
      timeout: 0, */
      // positionClass: 'toast-bottom-full-width'
    });

  constructor() {

  }

  ngOnInit(): void {

  }

}
