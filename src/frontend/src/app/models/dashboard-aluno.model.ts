export interface ProgressoGeralModel {
  cursosMatriculados: number;
  cursosConcluidos: number;
  certificadosEmitidos: number;
  percentualConcluidoGeral: number;
  horasEstudadas: number;
}

import type { MatriculaModel } from './matricula.model';
import type { CertificadoModel } from './certificado.model';

export interface DashboardAlunoModel {
  matriculas: MatriculaModel[];
  certificados: CertificadoModel[];
  progressoGeral: ProgressoGeralModel;
}


