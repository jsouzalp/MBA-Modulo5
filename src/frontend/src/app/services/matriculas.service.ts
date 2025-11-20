import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';
import { MatriculaCreateModel, MatriculaModel } from '../models/matricula.model';
import { SolicitarCertificadoRequest } from '../models/certificado.model';
import { AulaModelDto } from '../models/aulacurso.dto';

@Injectable({ providedIn: 'root' })
export class MatriculasService extends BaseService {
  constructor(private http: HttpClient) { super(); }

  obterAulasPorMatricula(matriculaId: string): Observable<AulaModelDto[]> {
    return this.http
      .get(this.UrlServiceV1 + `Alunos/aulas/${matriculaId}`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  criarMatricula(data: MatriculaCreateModel): Observable<string> {
    const alunoId = this.LocalStorage.getUser()?.usuarioToken?.id;
    return this.http
      .post(this.UrlServiceV1 + `Alunos/${alunoId}/matricular-aluno`, data, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  obterMatricula(matriculaId: string) : Observable<MatriculaModel> {
    return this.http
      .get(this.UrlServiceV1 + `Alunos/matricula/${matriculaId}`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  listarMatriculas(): Observable<MatriculaModel[]> {
    return this.http
      .get(this.UrlServiceV1 + `alunos/todas-matriculas`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  concluirCurso(alunoId: string, matriculaId: string) {
    return this.http
      .put(this.UrlServiceV1 + `alunos/${alunoId}/concluir-curso`, { alunoId, MatriculaCursoId: matriculaId }, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  solicitarCertificado(request: SolicitarCertificadoRequest): Observable<string> {
    const alunoId = this.LocalStorage.getUser()?.usuarioToken?.id;
    return this.http
      .post(this.UrlServiceV1 + `alunos/${alunoId}/solicitar-certificado`, request, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  registrarHistoricoAprendizado(matriculaId: string, alunoId: string, aulaId: string, dataTermino?: string) {
    return this.http
      .post(this.UrlServiceV1 + `alunos/${alunoId}/registrar-historico-aprendizado`, { MatriculaCursoId: matriculaId, alunoId, aulaId, dataTermino }, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }
}


