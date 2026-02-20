export interface Pagination<T> {
    data: T[];
    pageIndex: number;
    pageSize: number;
    count: number;
}

export class Pagination<T> {
    data: T[] = [];
    pageIndex = 0;
    pageSize = 12;
    count = 0;
}