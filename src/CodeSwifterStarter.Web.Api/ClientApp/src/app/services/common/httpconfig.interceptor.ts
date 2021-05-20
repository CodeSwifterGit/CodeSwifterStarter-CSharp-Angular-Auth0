import {
  HttpErrorResponse,
  HttpEvent, HttpEventType, HttpHandler, HttpInterceptor,
  HttpRequest,



  HttpXsrfTokenExtractor
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize, mergeMap, tap, timeout } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class HttpConfigInterceptor implements HttpInterceptor {
  constructor(private readonly auth: AuthService,
    private readonly tokenExtractor: HttpXsrfTokenExtractor) { }

  private actions: string[] = ['POST', 'PUT', 'DELETE'];
  private forbiddenActions: string[] = ['HEAD', 'OPTIONS'];

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Implement anti-forgery token
    let token = this.tokenExtractor.getToken();
    let permitted = this.findByActionName(request.method, this.actions);
    let forbidden = this.findByActionName(request.method, this.forbiddenActions);;

    if (permitted !== undefined && forbidden === undefined && token !== null) {
      request = request.clone({ setHeaders: { 'X-XSRF-TOKEN': token } });
    }

    let requestTimeout = environment.waitForPendingRemoteDataRequest;
    if (request.headers.has(environment.requestTimeoutHeaderName)) {
      requestTimeout = +request.headers.get(environment.requestTimeoutHeaderName);
    }

    if (request.headers.get('Authorization') === 'YES') {
      return this.executeAuthorized(request, next, requestTimeout);
    } else {
      return this.executeAnonymously(request, next, requestTimeout);
    }

  }

  private executeAnonymously(request: HttpRequest<any>, next: HttpHandler, requestTimeout: number | Date): Observable<HttpEvent<any>> {
    let lastResponse: HttpEvent<any>;
    let error: HttpErrorResponse;

    const tokenReq = request.clone({
      setHeaders: {
        Accept: 'application/json',
        Content: 'application/json'
      }
    });
    return next.handle(tokenReq).pipe(
      timeout(requestTimeout),
      tap((response: HttpEvent<any>) => {
        lastResponse = response;
      }),
      catchError((error: any) => {
        return throwError(error);
      }),
      finalize(() => {
        if (lastResponse.type === HttpEventType.Sent && !error) {
          return throwError({ error: { type: 'Request cancelled', url: request.url } });
        }
      })
    );
  }

  private executeAuthorized(request: HttpRequest<any>, next: HttpHandler, requestTimeout: number | Date): Observable<HttpEvent<any>> {
    let lastResponse: HttpEvent<any>;
    let error: HttpErrorResponse;

    // Add authorisation token
    return this.auth.getTokenSilently$().pipe(
      mergeMap(bearerToken => {
        const tokenReq = request.clone({
          setHeaders: {
            Accept: 'application/json',
            Authorization: `Bearer ${bearerToken}`,
            Content: 'application/json'
          }
        });
        return next.handle(tokenReq).pipe(
          timeout(requestTimeout),
          tap((response: HttpEvent<any>) => {
            lastResponse = response;
          }),
          catchError((error: any) => {
            return throwError(error);
          }),
          finalize(() => {
            if (lastResponse.type === HttpEventType.Sent && !error) {
              return throwError({ error: { type: 'Request cancelled', url: request.url } });
            }
          })
        );
      }),
      catchError((error: any) => {
        return throwError(error);
      }),
      finalize(() => {
        if (lastResponse.type === HttpEventType.Sent && !error) {
          return throwError({ type: 'Request cancelled', url: request.url, status: 499 });
        }
      })
    );
  }

  private findByActionName(name: string, actions: string[]): string {
    return actions.find(action => action.toLocaleLowerCase() === name.toLocaleLowerCase());
  }
}
