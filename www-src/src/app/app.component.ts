import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/debounceTime';
import { ToasterConfig, ToasterService } from 'angular2-toaster';

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

  constructor(private toasterService: ToasterService) {

  }

  ngOnInit(): void {

  }

}
