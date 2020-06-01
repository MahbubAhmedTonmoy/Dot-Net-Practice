import { Category } from './category';
export class Product implements  Category {
    id: number;
    name: string;
    category: Category;
    createDate: number;
}
