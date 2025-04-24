import { BookGenre } from "../../shared/models/book-genre";

export class EnumUtils 
{
    static convertToBookGenre(num : number) : BookGenre
    {
        return BookGenre[num] !== undefined ? num as BookGenre : BookGenre.None;
    }
}