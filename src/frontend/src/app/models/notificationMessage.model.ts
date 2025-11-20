export interface NotificationMessage {
  id: number;
  type: string; // Example: 'Sucesso', 'Atenção', 'Erro', 'Informação'
  message: string;
}