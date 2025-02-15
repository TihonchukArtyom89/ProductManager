﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManager.Application.Models;
using ProductManager.Application.Models.DBEntities;
using ProductManager.Application.ViewModels;

namespace ProductManager.Application.Controllers;

public class ProductController : Controller
{
    private IProductRepository productRepository;
    private object? routeValues;

    public ProductController(IProductRepository _productRepository)
    {
        productRepository = _productRepository;
        routeValues = new RouteValueDictionary();
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
        ViewBag.Categories = new SelectList(productRepository.Categories, "CategoryID", "CategoryName");
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
        products = products.Where(p => CurrentCategory == null || p.CategoryID == CurrentCategory.CategoryID);
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
        if (products.Count() == 0 && productPage != 1)
        {
            productPage = 1;
            products = productRepository.Products.Where(p => CurrentCategory == null || p.CategoryID == CurrentCategory.CategoryID).OrderBy(p => p.ProductID);
        }
        if (productPage > (int)Math.Ceiling((decimal)totalItems / pageSize))
        {//check if current page is more than pages 
            productPage = 1;
        }
        products = products.Skip((productPage - 1) * pageSize).Take(pageSize);
        ViewBag.ProductCount = products.Count();
        ViewBag.SelectedPage = productPage;
        products = products.Count() != 0 ? products :
            products.Append(
                new Product()
                {
                    CategoryID = 1,
                    ProductID = 0,
                    ProductName = namePlaceholder,
                    ProductDescription = descriptionPlaceholder,
                    ProductPrice = 0.00M,
                });
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
        ViewBag.CurrentCategory = viewModel.CurrentCategory;
        return View(viewModel);
    }
    public SortOrder SaveSortOrderState(SortOrder sortOrder)
    {

        if (sortOrder == SortOrder.NameAsc || sortOrder == SortOrder.NameDesc)
        {
            sortOrder = sortOrder == SortOrder.NameDesc ? SortOrder.NameAsc : SortOrder.NameDesc;
        }
        if (sortOrder == SortOrder.PriceAsc || sortOrder == SortOrder.PriceDesc)
        {
            sortOrder = sortOrder == SortOrder.PriceDesc ? SortOrder.PriceAsc : SortOrder.PriceDesc;
        }
        return sortOrder;
    }
    [HttpGet]
    public IActionResult CreateProduct()
    {
        return PartialView(viewName: "../Shared/Product/_ProductCreatePartialView", model: new Product());
    }
    [HttpPost]
    public IActionResult CreateProduct(Product product, string? category, string? searchString, SortOrder sortOrder = SortOrder.Neutral, int productPage = 1, int pageSize = 1)
    {
        sortOrder = SaveSortOrderState(sortOrder);
        productRepository.CreateProduct(product);
        return RedirectToAction(actionName: "Productlist", controllerName: "Product", routeValues: new
        {
            controller = "Product",
            action = "Productlist",
            category = category,
            searchString = searchString,
            sortOrder = sortOrder,
            productPage = productPage,
            pageSize = pageSize
        });
    }
    [HttpGet]
    public IActionResult UpdateProduct(long id)
    {
        Product product = productRepository.Products.Where(e => e.ProductID == id).FirstOrDefault() ?? new Product()
        {
            CategoryID = 1,
            ProductID = 0,
            ProductName = "Нет в наличии!",
            ProductDescription = "Продуктов данной категории не имеется!",
            ProductPrice = 0.00M,
        };
        return PartialView(viewName: "../Shared/Product/_ProductUpdatePartialView", model: product);
    }
    [HttpPost]
    public IActionResult UpdateProduct(Product product, string? category, string? searchString, SortOrder sortOrder = SortOrder.Neutral, int productPage = 1, int pageSize = 1)
    {
        sortOrder = SaveSortOrderState(sortOrder);
        productRepository.UpdateProduct(product);
        return RedirectToAction(actionName: "Productlist", controllerName: "Product", routeValues: new
        {
            controller = "Product",
            action = "Productlist",
            category = category,
            searchString = searchString,
            sortOrder = sortOrder,
            productPage = productPage,
            pageSize = pageSize
        });
    }
    [HttpGet]
    public IActionResult DeleteProduct(long id)
    {
        Product product = productRepository.Products.Where(e => e.ProductID == id).FirstOrDefault() ?? new Product()
        {
            CategoryID = 1,
            ProductID = 0,
            ProductName = "Нет в наличии!",
            ProductDescription = "Продуктов данной категории не имеется!",
            ProductPrice = 0.00M,
        };
        return PartialView(viewName: "../Shared/Product/_ProductDeletePartialView", model: product);
    }
    [HttpPost]
    public IActionResult DeleteProduct(Product product, string? category, string? searchString, SortOrder sortOrder = SortOrder.Neutral, int productPage = 1, int pageSize = 1)
    {
        sortOrder = SaveSortOrderState(sortOrder);
        productRepository.DeleteProduct(product);
        return RedirectToAction(actionName: "Productlist", controllerName: "Product", routeValues: new
        {
            controller = "Product",
            action = "Productlist",
            category = category,
            searchString = searchString,
            sortOrder = sortOrder,
            productPage = productPage,
            pageSize = pageSize
        });
    }
    [HttpGet]
    public IActionResult ProductDetails(long id)
    {
        Product product = productRepository.Products.Where(e => e.ProductID == id).FirstOrDefault() ?? new Product()
        {
            CategoryID = 1,
            ProductID = 0,
            ProductName = "Нет в наличии!",
            ProductDescription = "Продуктов данной категории не имеется!",
            ProductPrice = 0.00M,
        };
        return PartialView(viewName: "../Shared/Product/_ProductDetailsPartialView", model: product);
    }
}
