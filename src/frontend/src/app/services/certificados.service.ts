import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';
import { CertificadoModel } from '../models/certificado.model';

@Injectable({ providedIn: 'root' })
export class CertificadosService extends BaseService {
  constructor(private http: HttpClient) { super(); }

  listar(): Observable<CertificadoModel[]> {
    return this.http
      .get(this.UrlServiceV1 + 'alunos/certificados', this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }
}


