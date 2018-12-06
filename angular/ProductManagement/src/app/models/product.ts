export interface IProduct {
    productId: number;
    name: string;
    code: string;
    releaseDate: Date;
    description: string;
    price: number;
    starRating: number;
    imageUrl: string;
}

export class Product implements IProduct {
    constructor(public productId: number,
        public name: string,
        public code: string,
        public releaseDate: Date,
        public description: string,
        public price: number,
        public starRating: number,
        public imageUrl: string){}
}
