export interface IPagination {
  currentPage: number;
  itemPerPage: number;
  totalItems: number;
  totalPages: number;
}

export class PaginatedResult<T> {
  data: T;
  pagination: IPagination;

  constructor(data: T, pagination: IPagination) {
    this.data = data;
    this.pagination = pagination;
  }
}

export class PagingParams {
  pageNumber;
  pageSize;

  constructor(pageNumber = 1, pageSize = 2) {
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
  }
}
