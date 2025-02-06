using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.ViewModels;

public class ProductViewModel
{
    public ProductViewModel()
    {
        Product = new Product();
        Categories = new List<Category>();
    }
    public Product Product { get; set; }
    public List<Category> Categories { get; set; }
}
