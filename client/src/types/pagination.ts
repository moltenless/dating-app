export type Pagination = {
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

export type PaginatedResult<T> = {
    items: T[];
    metadata: Pagination;
}