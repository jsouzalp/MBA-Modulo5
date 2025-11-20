import { AulaModel } from './aula.model';

export interface CursoModel {
  id: string;
  nome: string;
  descricao: string;
  categoriaId: string;
  valor: number;
  createdAt: string;
  updatedAt: string;
  resumo?: string;
  nivel?: string;
  instrutor?: string;
  duracaoHoras?: number;
  imagemUrl?: string;
  nomeCategoria?: string;
  vagasMaximas?: number;
  vagasOcupadas?: number;
  vagasDisponiveis?: number;
  podeSerMatriculado?: boolean;
  validoAte?: string;
  objetivos?: string;
  preRequisitos?: string;
  publicoAlvo?: string;
  metodologia?: string;
  recursos?: string;
  avaliacao?: string;
  bibliografia?: string;
  aulas?: AulaModel[];
}

export interface CursoCreateModel {
  nome: string;
  valor: number;
  duracaoHoras: number;
  nivel: string;
  instrutor: string;
  vagasMaximas: number;
  imagemUrl?: string;
  validoAte?: string; // ISO string
  categoriaId: string;
  resumo?: string;
  descricao?: string;
  objetivos?: string;
  preRequisitos?: string;
  publicoAlvo?: string;
  metodologia?: string;
  recursos?: string;
  avaliacao?: string;
  bibliografia?: string;
}
