
namespace ProductManager.Application.ViewModels;

public class PageViewModel
{
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    public PageViewModel()
    {
        TotalItems = 1;
        PageSize = 1;
        CurrentPage = 1;
    }
}
