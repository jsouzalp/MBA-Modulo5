export interface AulaModelDto {
    aulaId: string;
    cursoId: string;
    nomeAula: string;
    ordemAula: number;
    ativo: boolean;
    dataInicio: Date | null;
    dataTermino: Date | null;
    aulaJaIniciadaRealizada: boolean;
    url: string;
}