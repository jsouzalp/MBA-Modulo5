import { Injectable } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class GlobalErrorInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: any) => {
        if (error instanceof HttpErrorResponse) {
          // Pula erros 401 pois já há tratamento que redireciona no BaseService
          if (error.status === 401) {
            return throwError(() => error);
          }
          if (error.status === 403) {
            return throwError(() => error);
          }

          const extracted = this.extractErrors(error);
          if (extracted && extracted.length > 0) {
            this.toastr.error(extracted.join('\n'));
          } else {
            this.toastr.error('Ocorreu um erro inesperado. Tente novamente.');
          }
        } else {
          this.toastr.error('Erro de comunicação. Verifique sua conexão.');
        }

        return throwError(() => error);
      })
    );
  }

  private extractErrors(error: HttpErrorResponse): string[] {
    // 1) errors como array simples
    const errors = (error?.error?.errors ?? []) as any[];
    if (Array.isArray(errors) && errors.length > 0) {
      return errors.map(e => (typeof e === 'string' ? e : (e?.errorMessage ?? e?.message ?? JSON.stringify(e))));
    }

    // 2) novo contrato: error.data.errors é um array de objetos com errorMessage
    const dataErrors = (error?.error?.data?.errors ?? []) as any[];
    if (Array.isArray(dataErrors) && dataErrors.length > 0) {
      return dataErrors.map(e => (typeof e === 'string' ? e : (e?.errorMessage ?? e?.message ?? JSON.stringify(e))));
    }

    // 3) dicionário de erros
    const dict = error?.error?.errors;
    if (dict && typeof dict === 'object' && !Array.isArray(dict)) {
      const list: string[] = [];
      Object.keys(dict).forEach(key => {
        const fieldErrors = (dict as any)[key];
        if (Array.isArray(fieldErrors)) {
          fieldErrors.forEach((msg: any) => list.push(String(msg)));
        } else if (fieldErrors) {
          list.push(String(fieldErrors));
        }
      });
      if (list.length > 0) return list;
    }

    // 4) fallback por título/mensagem
    const title = (error?.error?.title ?? error?.error?.data?.title ?? error?.message ?? '') as string;
    return title ? [title] : [];
  }
}


