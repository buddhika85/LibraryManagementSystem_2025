export interface ResultDto
{
    errorMessage: string;
    isSuccess: boolean;
}

export interface InsertResultDto extends ResultDto
{
    EntityId: number;
}

export interface ReturnResultDto extends ResultDto
{

}