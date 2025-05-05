import { BookGenre } from "../../shared/models/book-genre";
import { UserRoles } from "../../shared/models/user-roles-enum";

export class EnumUtils 
{
    static convertToBookGenre(num : number) : BookGenre
    {
        return BookGenre[num] !== undefined ? num as BookGenre : BookGenre.None;
    }

    static convertToUserRole(num : number) : UserRoles
    {
        return UserRoles[num] !== undefined ? num as UserRoles : UserRoles.member;
    }
}