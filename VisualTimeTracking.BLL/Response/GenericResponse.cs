using VisualTimeTracking.Common.ResponseModel;

namespace VisualTimeTracking.BL.Response;

public static class GenericResponse<T>
{
    #region Return Error Response
    public static ServiceResponse<T> ReturnError(string message)
    {
        return new ServiceResponse<T>
        {
            Message = $"{message}",
            Success = false
        };
    }
    #endregion

    #region Return Result List Response
    public static ServiceResponse<List<T>> ReturnResultList(string message, List<T> Data)
    {
        return new ServiceResponse<List<T>>
        {
            Data = Data,
            Message = $"{message}"
        };
    }
    #endregion

    #region Return Result List Response
    public static ServiceResponse<T> ReturnResultList(string message, T Data)
    {
        return new ServiceResponse<T>
        {
            Data = Data,
            Message = $"{message}"
        };
    }
    #endregion

    #region Return Error List Response
    public static ServiceResponse<List<T>> ReturnErrorList(string message)
    {
        return new ServiceResponse<List<T>>
        {
            Message = $"{message}",
            Success = false
        };
    }
    #endregion

    #region Return Error List Pagination Response
    public static PaginationServiceResponse<List<T>> ReturnErrorPaginationList(string message)
    {
        return new PaginationServiceResponse<List<T>>
        {
            Message = $"{message}",
            Success = false
        };
    }
    #endregion

    #region Return Result List Pagination Response
    public static PaginationServiceResponse<List<T>> ReturnResultPaginationList(string message, List<T> Data, int TotalRecords, int TotalPages)
    {
        return new PaginationServiceResponse<List<T>>
        {
            Data = Data,
            TotalPages = TotalPages,
            TotalRecords = TotalRecords,
            Message = $"{message}"
        };
    }
    #endregion

    #region Return Result Response
    public static ServiceResponse<T> ReturnResult(string message)
    {
        return new ServiceResponse<T>
        {
            Message = $"{message}"
        };
    }
    #endregion

    #region Return Result Response
    public static ServiceResponse<T> ReturnResult(string message, T Data)
    {
        return new ServiceResponse<T>
        {
            Data = Data,
            Message = $"{message}"
        };
    }
    #endregion
}
