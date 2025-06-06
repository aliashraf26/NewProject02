namespace VisualTimeTracking.Common.ResponseModel
{
    public class PaginationServiceResponse<T>
    {
        public T? Data { get; set; }
        public int TotalRecords { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
