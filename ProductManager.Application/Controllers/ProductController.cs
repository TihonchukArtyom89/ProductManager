using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Models;
using ProductManager.Application.ViewModels;
using System.Web;

namespace ProductManager.Application.Controllers;

public class ProductController : Controller
{
    private IProductRepository productRepository;
    public ProductController(IProductRepository _productRepository)
    {
        productRepository = _productRepository;
    }
    public ViewResult ProductList(string? category, string? searchString, SortOrder sortOrder = SortOrder.Neutral, int productPage = 1, int pageSize = 1)
    {
        ViewBag.SelectedPageSize = pageSize;
        ViewBag.SelectedCategory = category;
        ViewBag.PriceSortOrder = sortOrder == SortOrder.PriceDesc ? SortOrder.PriceAsc : SortOrder.PriceDesc;
        ViewBag.NameSortOrder = sortOrder == SortOrder.NameDesc ? SortOrder.NameAsc : SortOrder.NameDesc;
        ViewBag.PriceSortingText = sortOrder != SortOrder.PriceDesc ? "От дорогих к дешёвым" : "От дешёвых к дорогим";
        ViewBag.NameSortingText = sortOrder != SortOrder.NameDesc ? "От Я до А" : "От А до Я";
        Category? CurrentCategory = category == null ? null : productRepository.Categories.Where(e => e.CategoryName == category).FirstOrDefault();
        IEnumerable<Product> products = productRepository.Products;
        string namePlaceholder = "Нет в наличии!";
        string descriptionPlaceholder = "Продуктов категории " + (CurrentCategory ?? new Category() { CategoryName = "Категория не указана" }).CategoryName + " не имеется!";
        int totalItems = category == null ? productRepository.Products.Count() : productRepository.Products.Where(e => e.CategoryID == CurrentCategory!.CategoryID).Count();
        if (CurrentCategory == null && category != null)//check if category not right transferred to product controller 
        {
            category = null;
            CurrentCategory = null;
        }
        if (!String.IsNullOrEmpty(searchString))
        {
            ViewBag.SearchString = searchString;
            namePlaceholder = "Не найдено!";
            descriptionPlaceholder = "Продуктов с запросом '" + ViewBag.SearchString + "' нет!";
            products = products.Where(e => e.ProductName.ToLower().Contains(searchString.ToLower()) || e.ProductDescription.ToLower().Contains(searchString.ToLower()));
            totalItems = category == null ? products.Count() : products.Where(e => e.CategoryID == CurrentCategory!.CategoryID).Count();
        }
        products = products.Where(p => CurrentCategory == null || p.CategoryID == CurrentCategory.CategoryID).OrderBy(p => p.ProductID);
        if (products.Count() == 0 && productPage != 1)
        {
            productPage = 1;
            products = productRepository.Products.Where(p => CurrentCategory == null || p.CategoryID == CurrentCategory.CategoryID).OrderBy(p => p.ProductID);
        }
        if (productPage > (int)totalItems / pageSize)
        {//check if current page is more than pages 
            productPage = 1;
        }
        //decimal? productPrice = 0;
        //if(totalItems == 0)
        //{
        //    productPrice = null;
        //}
        products = products.Skip((productPage - 1) * pageSize).Take(pageSize);
        ViewBag.ProductCount = products.Count();
        ViewBag.SelectedPage = productPage;
        products = products.Count() != 0 ? products :
            products.Append(
                new Product()
                {
                    CategoryID = 0,
                    ProductID = 0,
                    ProductName = namePlaceholder,
                    ProductDescription = descriptionPlaceholder,
                    ProductPrice = 0.00M,
                });
        switch (sortOrder)
        {
            case SortOrder.PriceAsc:
                products = products.OrderBy(e => e.ProductPrice);
                ViewBag.SortOrder = ViewBag.PriceSortOrder;
                break;
            case SortOrder.PriceDesc:
                products = products.OrderByDescending(e => e.ProductPrice);
                ViewBag.SortOrder = ViewBag.PriceSortOrder;
                break;
            case SortOrder.NameAsc:
                products = products.OrderBy(e => e.ProductName);
                ViewBag.SortOrder = ViewBag.NameSortOrder;
                break;
            case SortOrder.NameDesc:
                products = products.OrderByDescending(e => e.ProductName);
                ViewBag.SortOrder = ViewBag.NameSortOrder;
                break;
            default:
                products = products.OrderBy(e => e.ProductID);
                ViewBag.SortOrder = SortOrder.Neutral;
                break;
        }
        ProductsListViewModel viewModel = new ProductsListViewModel
        {
            Products = products,
            PageViewModel = new PageViewModel
            {
                CurrenPage = ViewBag.SelectedPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                Pseudonym = "Products"
            },
            CurrentCategory = (CurrentCategory ?? new Category { CategoryName = null ?? "" }).CategoryName,
        };
        return View(viewModel);
    }
    [HttpPost]
    public IActionResult RedirectWithPageSizeSelected(string? category, SortOrder sortOrder = SortOrder.Neutral, int productPage = 1, int pageSize = 1)
    {
        if (sortOrder == SortOrder.NameAsc || sortOrder == SortOrder.NameDesc)
        {
            sortOrder = sortOrder == SortOrder.NameDesc ? SortOrder.NameAsc : SortOrder.NameDesc;
        }
        if (sortOrder == SortOrder.PriceAsc || sortOrder == SortOrder.PriceDesc)
        {
            sortOrder = sortOrder == SortOrder.PriceDesc ? SortOrder.PriceAsc : SortOrder.PriceDesc;
        }
        ViewBag.SelectedPageSize = pageSize;
        return RedirectToAction("ProductList", "Product", new { category = category, sortOrder = sortOrder, productPage = productPage, pageSize = pageSize });
    }
}
