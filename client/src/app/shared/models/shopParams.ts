export class ShopParams {
    pageIndex = 0;
    pageSize = 12;
    brands: string[] = [];
    types: string[] = [];
    sort: Sort = 'name';
    search?: string = '';
}

export type Sort = 'name' | 'priceAsc' | 'priceDesc';