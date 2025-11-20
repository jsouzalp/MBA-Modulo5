import { Injectable } from '@angular/core';
import { BaseService } from './BaseService';
import { PagamentoCreateModel } from '../models/pagamento.model';
import { catchError, map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PagamentosService extends BaseService {

  constructor(private http: HttpClient) { super(); }

  realizarPagamento(data: PagamentoCreateModel): Observable<any> {
    return this.http
      .post(this.UrlServiceV1 + `Pagamentos/registrar-pagamento`, data, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }
}
