import { AddressDto } from "./address-dto";

export type UserUpdateDto =
{
     firstName: string;
        lastName: string;   
        email: string;
        phoneNumber: string;
        address: AddressDto;
}