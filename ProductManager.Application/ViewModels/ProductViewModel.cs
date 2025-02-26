using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManager.Application.Models;
using ProductManager.Application.Models.DBEntities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ProductManager.Application.ViewModels;

public class ProductViewModel
{
    public Product Product { get; set; }
    [DisplayName("Цена продукта")]
    [Required(ErrorMessage = "Price of product is required.")]
    [RegularExpression(@"\d{1,6}(\.|\,)\d{1,2}", ErrorMessage = "Invalid product price format.")]
    public string ProductPriceString { get; set; } = string.Empty;//строковое представление цены продукта
    public string ProductCategoryString{ get; set; } = string.Empty;//строковое представление категории продукта
    public ProductViewModel()
    {
        Product = new();
        ProductCategoryString = SystemValues.GetCategoryUncategorized().CategoryName;
        ProductPriceString = "0,00"; 
    }
    public ProductViewModel(Product product, SelectList categories) 
    {
        Product = product;
        ProductCategoryString = SystemValues.GetCategoryUncategorized().CategoryName;
        foreach (SelectListItem c in categories)
        {
            if (c.Value == product.CategoryID.ToString())
            {
                ProductCategoryString = c.Text; 
                break;
            }
        }
        ProductPriceString = product.ProductPrice.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
    }
}
