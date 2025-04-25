export interface ResultDto
{
    ErrorMessage: string;
    IsSuccess: boolean;
}

export interface InsertResultDto extends ResultDto
{
    EntityId: number;
}