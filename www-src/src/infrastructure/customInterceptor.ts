import { HttpInterceptor, HttpHandler, HttpEvent, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable()
export class CustomInterceptor implements HttpInterceptor {

    constructor() {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        console.log(`%c ${request.method}: ${request.url} `, 'background: #222; color: #bada55');
        request = request.clone({
            url: environment.apiUrl + 'api/' + request.url,
            withCredentials: true
        });

        return next.handle(request);
    }
}
