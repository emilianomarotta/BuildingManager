import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SessionStorageService } from '../services/session-storage.service';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private _sessionStorageService: SessionStorageService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const token = this._sessionStorageService.getToken();
    let newRequest = request;

    if(request.url.includes('login')){
      return next.handle(request);
    }
    if(request.url.includes('invitation-response')){
      return next.handle(request);
    }

    if (!!token) {
      const headers = request.headers.set('Authorization', token);
      newRequest = request.clone({ headers });
    }
    return next.handle(newRequest);
  }
}
