export interface CertificadoModel {
  id: string;
  nomeCurso: string;
  codigo: string;
  dataEmissao: string;
  url: string;
}

export interface SolicitarCertificadoRequest {
  alunoId: string;
  matriculaCursoId: string;
}


