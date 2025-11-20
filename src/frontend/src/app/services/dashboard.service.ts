import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';
import { DashboardAlunoModel } from '../models/dashboard-aluno.model';
import { DashboardAdminModel } from '../models/dashboard-admin.model';

@Injectable({ providedIn: 'root' })
export class DashboardService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getDashboardAluno(): Observable<DashboardAlunoModel> {
    const url: string = `${this.UrlServiceV1}Dashboard/aluno`;
    const response = this.http
      .get(url, this.getAuthHeaderJson())
      .pipe(
        map(r => this.extractData(r) as DashboardAlunoModel),
        catchError(e => this.serviceError(e))
      );
    return response;
  }

  getDashboardAdmin(): Observable<DashboardAdminModel> {
    const url: string = `${this.UrlServiceV1}Dashboard/admin`;
    const response = this.http
      .get(url, this.getAuthHeaderJson())
      .pipe(
        map(r => this.extractData(r) as DashboardAdminModel),
        catchError(e => this.serviceError(e))
      );
    return response;
  }
}
