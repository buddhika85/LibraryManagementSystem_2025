import { AddressDto } from "./address-dto";

export type UserInfoDto = { 
    firstName: string;
    lastName: string;   
    email: string;
    address: AddressDto;
}

export interface LoginResponseDto extends UserInfoDto {
    
}