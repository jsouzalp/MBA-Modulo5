export interface MatriculaCreateModel {
  alunoId: string;
  cursoId: string;
  observacao?: string;
}

export interface MatriculaModel {
  id: string;
  cursoId: string;
  alunoId: string;
  pagamentoPodeSerRealizado: boolean;
  nomeCurso: string;
  valor: number;
  dataMatricula: string;
  dataConclusao?: string;
  estadoMatricula: string;
  certificado?: {
    id: string;
    matriculaCursoId: string;
    nomeCurso: string;
    dataSolicitacao: string;
    cargaHoraria: number;
    notaFinal: number;
    pathCertificado: string;
    nomeInstrutor: string;
  };
}

