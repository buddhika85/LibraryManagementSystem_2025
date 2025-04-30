import { AddressDto } from "./address-dto";
import { UserRoles } from "./user-roles-enum";

export type RegisterDto = {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    password: string;
    role: UserRoles;
    address?: AddressDto;
  };
  
// Using Omit<>: MemberRegisterDto inherits from RegisterDto but overrides role to always be 'Member'
export interface MemberRegisterDto extends RegisterDto {
    role: UserRoles.member; // Override the role to always be 'Member'
}
