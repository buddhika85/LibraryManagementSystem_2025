import { AddressDto } from "./address-dto";
import { UserRoles } from "./user-roles-enum";

export type UserInfoDto = 
{ 
    firstName: string;
    lastName: string;   
    email: string;
    address: AddressDto;
    role: UserRoles;
}

export interface LoginResponseDto extends UserInfoDto {
    
}