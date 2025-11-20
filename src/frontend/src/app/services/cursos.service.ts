import { FilterCurso, PagedResult } from './../models/paged-result.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';
import { AulaCreateModel, AulaEditModel } from '../pages/conteudo/models/aula.model';
import { CursoCreateModel, CursoModel } from '../pages/conteudo/models/curso.model';

@Injectable({ providedIn: 'root' })
export class CursosService extends BaseService {
  constructor(private http: HttpClient) { super(); }

  listar(filter: FilterCurso): Observable<PagedResult<CursoModel>> {
    const params = this.getParams(filter);
    return this.http
      .get(this.UrlServiceV1 + `Conteudos/cursos?${params}`, this.getAuthHeaderJson())
      .pipe(
        map(r => {
          const data = this.extractData(r);
          // Suporta paginação: { pageSize, pageIndex, totalResults, items: [] }
          if (data && typeof data === 'object' && Array.isArray((data as any).items)) {
            return {
              items: (data as any).items as CursoModel[],
              pageSize: (data as any).pageSize ?? 0,
              pageIndex: (data as any).pageIndex ?? 0,
              totalResults: (data as any).totalResults ?? ((data as any).items.length ?? 0)
            };
          }
          // Suporta retorno direto como array
          if (Array.isArray(data)) {
            return {
              items: data as CursoModel[],
              pageSize: (data as CursoModel[]).length,
              pageIndex: 0,
              totalResults: (data as CursoModel[]).length
            };
          }
          // Suporta retorno de item único
          if (typeof data === 'object' && (data as any).id) {
            return {
              items: [data as CursoModel],
              pageSize: 1,
              pageIndex: 0,
              totalResults: 1
            };
          }
          // Caso não haja dados
          return {
            items: [],
            pageSize: 0,
            pageIndex: 0,
            totalResults: 0
          };
        }),
        catchError(e => this.serviceError(e))
      );
  }

  obter(id: string, includeAulas = true): Observable<CursoModel> {
    return this.http
      .get(this.UrlServiceV1 + `Conteudos/${id}?includeAulas=${includeAulas}`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  create(curso: CursoCreateModel): Observable<CursoModel> {
    return this.http
      .post(this.UrlServiceV1 + 'Conteudos/cursos', curso, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  update(id: string, curso: CursoCreateModel): Observable<CursoModel> {
    return this.http
      .put(this.UrlServiceV1 + `Conteudos/cursos/${id}`, curso, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  getAulasByCurso(cursoId: string): Observable<any>{
    return this.http
      .get(this.UrlServiceV1 + `Conteudos/cursos/${cursoId}/aulas`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  addAula(cursoId: string, aula: AulaCreateModel): Observable<any> {
    return this.http
      .post(this.UrlServiceV1 + `Conteudos/cursos/${cursoId}/aulas`, aula, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  updateAula(cursoId: string, aulaId: string, aula: AulaEditModel): Observable<any> {
    return this.http
      .put(this.UrlServiceV1 + `Conteudos/cursos/${cursoId}/aulas/${aulaId}`, aula, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  getParams(filter: FilterCurso): string {
    return Object.entries(filter).map(([key, value]) => `${key}=${value}`).join('&');
  }
}


