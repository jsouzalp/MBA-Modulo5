export interface PagedResult<T> {
  items: T[];
  pageSize: number;
  pageIndex: number;
  totalResults: number;
}
export interface FilterCurso {
  pageSize: number;
  pageIndex: number;
  query: string;
  includeAulas: boolean;
  ativos: boolean;
}