import { AddressDto } from "./address-dto";
import { UserRoles } from "./user-roles-enum";

export type UserDisplayDto = {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    role: UserRoles;
    roleStr: string;
    address: AddressDto;
    addressStr: string;
};

export interface MemberUserDisplayDto extends UserDisplayDto 
{

}

export interface StaffUserDisplayDto extends UserDisplayDto 
{

}

export type UsersListDto = {
    usersList: UserDisplayDto[];
    count: number;
};