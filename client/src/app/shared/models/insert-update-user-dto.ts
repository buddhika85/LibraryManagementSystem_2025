import { AddressDto } from "./address-dto";
import { UserRoles } from "./user-roles-enum";

export interface InsertUpdateUserDto {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    address: AddressDto;
    role: UserRoles;
  }