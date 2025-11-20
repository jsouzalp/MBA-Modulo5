export interface AulaModel {
  id: string;
  cursoId: string;
  nome: string;
  descricao: string;
  numero: number;
  duracaoMinutos: number;
  videoUrl: string;
  tipoAula?: string;
  // Campos opcionais que podem vir da API e serão exibidos
  status?: string;
  aulaRealizada?: boolean;
  dataInicio: Date | null;
  dataTermino: Date | null;
}

export interface AulaCreateModel extends Omit<AulaModel, 'id'> {}
export interface AulaEditModel extends Partial<AulaModel> {}
