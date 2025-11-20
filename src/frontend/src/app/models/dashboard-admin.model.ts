export interface DashboardAdminModel {
  title: string;
  status: number;
  data: DashboardAdminData;
  errors: DashboardErrors;
}

export interface DashboardAdminData {
  estatisticasAlunos: EstatisticasAlunos;
  estatisticasCursos: EstatisticasCursos;
  relatorioVendas: RelatorioVendas;
  estatisticasUsuarios: EstatisticasUsuarios;
  cursosPopulares: CursoPopular[];
  vendasRecentes: VendaRecente[];
}

export interface EstatisticasAlunos {
  totalAlunos: number;
  alunosAtivos: number;
  alunosInativos: number;
  novasMatriculasHoje: number;
  novasMatriculasSemana: number;
  novasMatriculasMes: number;
  taxaRetencao: number;
}

export interface EstatisticasCursos {
  totalCursos: number;
  cursosAtivos: number;
  cursosInativos: number;
  mediaAvaliacoes: number;
  totalAulas: number;
  horasConteudo: number;
}

export interface RelatorioVendas {
  vendasHoje: number;
  vendasSemana: number;
  vendasMes: number;
  vendasAno: number;
  ticketMedio: number;
  totalTransacoes: number;
  taxaConversao: number;
}

export interface EstatisticasUsuarios {
  totalUsuarios: number;
  usuariosAtivos: number;
  usuariosOnline: number;
  adminsAtivos: number;
  alunosAtivos: number;
}

export interface CursoPopular {
  id: string;
  nome: string;
  totalMatriculas: number;
  receita: number;
  mediaAvaliacoes: number;
  totalAvaliacoes: number;
}

export interface VendaRecente {
  id: string;
  alunoNome: string;
  cursoNome: string;
  valor: number;
  dataVenda: string;
  status: string;
  formaPagamento: string;
}

export interface DashboardErrors {
  mensagens: string[];
}
