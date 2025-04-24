export interface ResultDto
{
    ErrorMessage: string;
    IsSuccess: boolean;
}

export interface InsertUpdateResultDto extends ResultDto
{
    EntityId: number;
}