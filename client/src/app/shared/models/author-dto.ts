import { BaseDto } from './base-dto';

export interface AuthorDto extends BaseDto {
  name: string;
  country: string;
  biography: string;
  dateOfBirth: string;       
  dateOfBirthStr: string;
}
