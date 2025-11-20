export interface PagamentoCreateModel {
  alunoId: string;
  matriculaId: string;
  total: number;
  nomeCartao: string;
  numeroCartao: string;
  cvvCartao: string;
  expiracaoCartao: string;
}