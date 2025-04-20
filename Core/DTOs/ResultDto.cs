namespace Core.DTOs
{
    public class ResultDto
    {
        public string? ErrorMessage { get; set; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
    }

    public class InsertUpdateResultDto : ResultDto
    {
        public int EntityId { get; set; }
    }
}
