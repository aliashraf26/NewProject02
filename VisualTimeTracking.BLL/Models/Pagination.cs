namespace VisualTimeTracking.BL.Models;

public class Pagination
{
    public int PageNumber = 1;
    public int PageSize = 10;
    public int Skip = 0;
    public int Take = 0;
    public int TotalCount = 0;

    public Pagination(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber <= 0 ? 1 : pageNumber;
        PageSize = pageSize <= 0 ? 10 : pageSize;
        Skip = (PageNumber - 1) * PageSize;
        Take = pageSize;
    }

}
