﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManager.Application.Models;
using ProductManager.Application.Models.DBEntities;
using ProductManager.Application.ViewModels;
using System.Globalization;

namespace ProductManager.Application.Controllers;

public class ProductController : Controller
{
    private IProductRepository productRepository;
    private int[] productNumberOnPage = { 2, 4, 6, 8, 10 };
    public ProductController(IProductRepository _productRepository)
    {
        productRepository = _productRepository;
    }
    public ViewResult ProductList(string? category, string? searchString, SortOrder sortOrder = SortOrder.Neutral, int productPage = 1, int pageSize = 0)
    {
        pageSize = pageSize == 0 ? productNumberOnPage[0] : pageSize;
        ViewBag.SelectedPageSize = pageSize;
        ViewBag.SelectedCategory = category;
        ViewBag.PriceSortOrder = sortOrder == SortOrder.PriceDesc ? SortOrder.PriceAsc : SortOrder.PriceDesc;
        ViewBag.NameSortOrder = sortOrder == SortOrder.NameDesc ? SortOrder.NameAsc : SortOrder.NameDesc;
        ViewBag.PriceSortingText = sortOrder != SortOrder.PriceDesc ? "От дорогих к дешёвым" : "От дешёвых к дорогим";
        ViewBag.NameSortingText = sortOrder != SortOrder.NameDesc ? "От Я до А" : "От А до Я";
        Category? CurrentCategory = category == null ? null : productRepository.Categories.Where(e => e.CategoryName == category).FirstOrDefault();
        ViewBag.Categories = new SelectList(productRepository.Categories, "CategoryID", "CategoryName");
        IEnumerable<Product> products = productRepository.Products;
        Product SystemProduct = SystemValues.GetProductNull(CurrentCategory ?? new Category());
        int totalItems = category == null ? productRepository.Products.Count() : productRepository.Products.Where(e => e.CategoryID == CurrentCategory!.CategoryID).Count();
        if (CurrentCategory == null && category != null)//check if category not right transferred to product controller 
        {
            category = null;
            CurrentCategory = null;
        }
        if (!String.IsNullOrEmpty(searchString))
        {
            ViewBag.SearchString = searchString;
            SystemProduct = SystemValues.GetProductNotFound(ViewBag.SearchString);
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
        products = products.Count() != 0 ? products : products.Append(SystemProduct);
        ProductsListViewModel viewModel = new ProductsListViewModel
        {
            Products = products,
            PageViewModel = new PageViewModel
            {
                CurrentPage = ViewBag.SelectedPage,
                PageSize = pageSize,
                TotalItems = totalItems
            },
            CurrentCategory = (CurrentCategory ?? new Category { CategoryName = null ?? "" }).CategoryName,
            ControllerName = ControllerContext.ActionDescriptor.ControllerName ?? "",
            ActionName = ControllerContext.ActionDescriptor.ActionName ?? "",
            PageSizes = productNumberOnPage,
            SizeSelectorText = "Выберите количество отображаемых продуктов: "
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
    [ValidateAntiForgeryToken]
    public IActionResult CreateProduct(ProductPageViewModel productViewModel, string? category, string? searchString, SortOrder sortOrder = SortOrder.Neutral, int productPage = 1, int pageSize = 1)
    {
        sortOrder = SaveSortOrderState(sortOrder);
        SetProductData(productViewModel);
        productRepository.CreateProduct(productViewModel.Product);
        return RedirectToAction(actionName: "Productlist", controllerName: "Product", routeValues: new
        {
            controller = "Product",
            action = "Productlist",
            category = category,
            searchString = searchString,
            pageSize = pageSize,
            productPage = productPage,
            sortOrder = sortOrder
        });
    }
    [HttpGet]
    public IActionResult UpdateProduct(long id)
    {
        Product product = productRepository.Products.Where(e => e.ProductID == id).FirstOrDefault() ?? SystemValues.GetProductNull();
        return PartialView(viewName: "../Shared/Product/_ProductUpdatePartialView", model: product);
    }
    [HttpPost]
    public IActionResult UpdateProduct(ProductPageViewModel productViewModel, string? category, string? searchString, SortOrder sortOrder = SortOrder.Neutral, int productPage = 1, int pageSize = 1)
    {
        sortOrder = SaveSortOrderState(sortOrder);
        SetProductData(productViewModel);
        productRepository.UpdateProduct(productViewModel.Product);
        return RedirectToAction(actionName: "Productlist", controllerName: "Product", routeValues: new
        {
            controller = "Product",
            action = "Productlist",
            category = category,
            searchString = searchString,
            pageSize = pageSize,
            productPage = productPage,
            sortOrder = sortOrder
        });
    }
    [HttpGet]
    public IActionResult DeleteProduct(long id)
    {
        Product product = productRepository.Products.Where(e => e.ProductID == id).FirstOrDefault() ?? SystemValues.GetProductNull();
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
            pageSize = pageSize,
            productPage = productPage,
            sortOrder = sortOrder
        });
    }
    [HttpGet]
    public IActionResult ProductDetails(long id)
    {
        Product product = productRepository.Products.Where(e => e.ProductID == id).FirstOrDefault() ?? SystemValues.GetProductNull();
        return PartialView(viewName: "../Shared/Product/_ProductDetailsPartialView", model: product);
    }
    public void SetProductData(ProductPageViewModel productViewModel)
    {
        productViewModel.ProductPriceString = productViewModel.ProductPriceString.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
        if (decimal.TryParse(productViewModel.ProductPriceString, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal parsedPrice))
        {
            productViewModel.Product.ProductPrice = parsedPrice;
        }
        if (long.TryParse(productViewModel.ProductCategoryString, NumberStyles.Float, CultureInfo.InvariantCulture, out long parsedCategoryID))
        {
            productViewModel.Product.CategoryID = parsedCategoryID;
        }
    }
}
