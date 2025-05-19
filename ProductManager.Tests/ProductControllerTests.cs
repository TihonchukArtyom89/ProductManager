using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManager.Application.Models.DBEntities;
using ProductManager.Application.ViewModels;
using System.Globalization;

namespace ProductManager.Tests;

public class ProductControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2}
        }).AsQueryable<Product>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        ProductsListViewModel result = productController.ProductList(null, null, SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        Product[] products = result.Products.ToArray();
        Assert.Equal(2, products.Length);
        Assert.Equal("P1", products[0].ProductName);
        Assert.Equal(1, products[0].ProductID);
        Assert.Equal("P2", products[1].ProductName);
        Assert.Equal(2, products[1].ProductID);
    }
    [Fact]
    public void Can_Paginate()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 2}
        }).AsQueryable<Product>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        ProductsListViewModel result = productController.ProductList(category: null, searchString: null, sortOrder: SortOrder.PriceDesc, productPage: 2, pageSize: 3)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        Product[] p = result.Products.ToArray();
        Assert.True(2 == p.Length);
        Assert.Equal("P4", p[0].ProductName);
        Assert.Equal(4, p[0].ProductID);
        Assert.Equal("P5", p[1].ProductName);
        Assert.Equal(5, p[1].ProductID);
    }
    [Fact]
    public void Can_Send_Pagination_View_Model()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 2}
        }).AsQueryable<Product>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        ProductsListViewModel productsListViewModel = productController.ProductList(category: null, searchString: null, sortOrder: SortOrder.PriceDesc, productPage: 2, pageSize: 3)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        PageViewModel pageViewModel = productsListViewModel.PageViewModel;
        Assert.Equal(2, pageViewModel.CurrenPage);
        Assert.Equal(2, pageViewModel.TotalPages);
        Assert.Equal(3, pageViewModel.PageSize);
        Assert.Equal(5, pageViewModel.TotalItems);
    }
    [Fact]
    public void Can_Filter_Products()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" }
        }).AsQueryable<Category>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        Product[] result = (productController.ProductList("C1", null, SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).Products.ToArray();
        //Assert
        Assert.Equal(2, result.Length);
        Assert.True(result[0].ProductName == "P1" && result[0].ProductID == 1);
        Assert.True(result[1].ProductName == "P3" && result[1].ProductID == 3);
    }
    [Fact]
    public void Can_Get_Product_Count()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" },
        }).AsQueryable<Category>());
        ProductController productController = new ProductController(mockRepository.Object);
        Func<ViewResult, ProductsListViewModel?> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;
        //Act
        int? countC1 = GetModel(productController.ProductList("C1", null, SortOrder.PriceDesc, 1, 3))?.PageViewModel.TotalItems;
        int? countC2 = GetModel(productController.ProductList("C2", null, SortOrder.PriceDesc, 1, 3))?.PageViewModel.TotalItems;
        int? countC3 = GetModel(productController.ProductList("C3", null, SortOrder.PriceDesc, 1, 3))?.PageViewModel.TotalItems;
        int? countAll = GetModel(productController.ProductList(null, null, SortOrder.PriceDesc, 1, 3))?.PageViewModel.TotalItems;
        //Assert
        Assert.Equal(2, countC1);
        Assert.Equal(3, countC2);
        Assert.Equal(0, countC3);
        Assert.Equal(5, countAll);
    }
    [Fact]
    public void Can_Get_Page_Size()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" },
        }).AsQueryable<Category>());
        ProductController productController = new ProductController(mockRepository.Object);
        Func<ViewResult, ProductsListViewModel?> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;
        //Act
        int? pageCount = (productController.ProductList("C1", null, SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.PageSize;
        //Assert
        Assert.Equal(3, pageCount);
    }
    [Fact]
    public void Can_Get_Page_Count_Of_All_Products()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
                new Category{CategoryID = 1, CategoryName = "C1" },
                new Category{CategoryID = 2, CategoryName = "C2" }
        }).AsQueryable<Category>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        int result = (productController.ProductList(null, null, SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.TotalPages;
        //Assert
        Assert.Equal(2, result);
    }
    [Fact]
    public void Can_Get_Page_Count_Of_Specific_Products()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 2},
            new Product{ProductID = 6, ProductName = "P6", CategoryID = 1},
            new Product{ProductID = 7, ProductName = "P7", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
                new Category{CategoryID = 1, CategoryName = "C1" },
                new Category{CategoryID = 2, CategoryName = "C2" }
        }).AsQueryable<Category>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        int result = (productController.ProductList("C2", null, SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.TotalPages;
        //Assert
        Assert.Equal(2, result);
    }
    [Fact]
    public void Can_Select_Page_Sizes()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 3},
            new Product{ProductID = 6, ProductName = "P6", CategoryID = 1},
            new Product{ProductID = 7, ProductName = "P7", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
                new Category{CategoryID = 1, CategoryName = "C1" },
                new Category{CategoryID = 2, CategoryName = "C2" },
                new Category{CategoryID = 3, CategoryName = "C3" }
        }).AsQueryable<Category>());
        ProductController productController = new(mockRepository.Object);
        //Act
        int[] results = (productController.ProductList("C2", null, SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageSizes;
        //Assert
        Assert.Equal(new int[] { 1, 2, 3, 5, 10 }, results);
    }
    [Fact]
    public void Can_Right_Sort_Product_On_Name()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 7, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 6, ProductName = "P5", CategoryID = 3},
            new Product{ProductID = 5, ProductName = "P6", CategoryID = 1},
            new Product{ProductID = 1, ProductName = "P7", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" }
        }).AsQueryable<Category>());
        ProductController productController = new(mockRepository.Object);
        //Act
        ProductsListViewModel? resultNameAsc1 = productController.ProductList(null, null, SortOrder.NameAsc, 1, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultNameAsc2 = productController.ProductList(null, null, SortOrder.NameAsc, 2, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultNameDesc1 = productController.ProductList(null, null, SortOrder.NameDesc, 1, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultNameDesc2 = productController.ProductList(null, null, SortOrder.NameDesc, 2, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        Assert.Equal("P1", resultNameAsc1.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P2", resultNameAsc1.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("P3", resultNameAsc1.Products.Skip(2).FirstOrDefault()!.ProductName);
        Assert.Equal("P4", resultNameAsc1.Products.Skip(3).FirstOrDefault()!.ProductName);
        Assert.Equal("P5", resultNameAsc1.Products.Skip(4).FirstOrDefault()!.ProductName);
        Assert.Equal("P6", resultNameAsc2.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P7", resultNameAsc2.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("P7", resultNameDesc1.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P6", resultNameDesc1.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("P5", resultNameDesc1.Products.Skip(2).FirstOrDefault()!.ProductName);
        Assert.Equal("P4", resultNameDesc1.Products.Skip(3).FirstOrDefault()!.ProductName);
        Assert.Equal("P3", resultNameDesc1.Products.Skip(4).FirstOrDefault()!.ProductName);
        Assert.Equal("P2", resultNameDesc2.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P1", resultNameDesc2.Products.Skip(1).FirstOrDefault()!.ProductName);
    }
    [Fact]
    public void Can_Right_Sort_Product_On_Price()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 7, ProductName = "P1", CategoryID = 1, ProductPrice=1.00M},
            new Product{ProductID = 4, ProductName = "P2", CategoryID = 2, ProductPrice=2.00M},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1, ProductPrice=3.00M},
            new Product{ProductID = 2, ProductName = "P4", CategoryID = 2, ProductPrice=4.00M},
            new Product{ProductID = 6, ProductName = "P5", CategoryID = 3, ProductPrice=5.00M},
            new Product{ProductID = 5, ProductName = "P6", CategoryID = 1, ProductPrice=6.00M},
            new Product{ProductID = 1, ProductName = "P7", CategoryID = 2, ProductPrice=7.00M}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" }
        }).AsQueryable<Category>());
        ProductController productController = new(mockRepository.Object);
        //Act
        ProductsListViewModel? resultPriceAsc1 = productController.ProductList(null, null, SortOrder.PriceAsc, 1, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultPriceAsc2 = productController.ProductList(null, null, SortOrder.PriceAsc, 2, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultPriceDesc1 = productController.ProductList(null, null, SortOrder.PriceDesc, 1, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultPriceDesc2 = productController.ProductList(null, null, SortOrder.PriceDesc, 2, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        Assert.Equal("P1", resultPriceAsc1.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P2", resultPriceAsc1.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("P3", resultPriceAsc1.Products.Skip(2).FirstOrDefault()!.ProductName);
        Assert.Equal("P4", resultPriceAsc1.Products.Skip(3).FirstOrDefault()!.ProductName);
        Assert.Equal("P5", resultPriceAsc1.Products.Skip(4).FirstOrDefault()!.ProductName);
        Assert.Equal("P6", resultPriceAsc2.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P7", resultPriceAsc2.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("P7", resultPriceDesc1.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P6", resultPriceDesc1.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("P5", resultPriceDesc1.Products.Skip(2).FirstOrDefault()!.ProductName);
        Assert.Equal("P4", resultPriceDesc1.Products.Skip(3).FirstOrDefault()!.ProductName);
        Assert.Equal("P3", resultPriceDesc1.Products.Skip(4).FirstOrDefault()!.ProductName);
        Assert.Equal("P2", resultPriceDesc2.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P1", resultPriceDesc2.Products.Skip(1).FirstOrDefault()!.ProductName);
    }
    [Fact]
    public void Can_Right_Sort_Product_On_Default()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 7, ProductName = "P1", CategoryID = 1, ProductPrice=1.00M},
            new Product{ProductID = 4, ProductName = "P2", CategoryID = 2, ProductPrice=2.00M},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1, ProductPrice=3.00M},
            new Product{ProductID = 2, ProductName = "P4", CategoryID = 2, ProductPrice=4.00M},
            new Product{ProductID = 6, ProductName = "P5", CategoryID = 3, ProductPrice=5.00M},
            new Product{ProductID = 5, ProductName = "P6", CategoryID = 1, ProductPrice=6.00M},
            new Product{ProductID = 1, ProductName = "P7", CategoryID = 2, ProductPrice=7.00M}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" }
        }).AsQueryable<Category>());
        ProductController productController = new(mockRepository.Object);
        //Act
        ProductsListViewModel? resultPriceNeutral1 = productController.ProductList(null, null, SortOrder.Neutral, 1, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultPriceNeutral2 = productController.ProductList(null, null, SortOrder.Neutral, 2, 5)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        Assert.Equal("P7", resultPriceNeutral1.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P4", resultPriceNeutral1.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("P3", resultPriceNeutral1.Products.Skip(2).FirstOrDefault()!.ProductName);
        Assert.Equal("P2", resultPriceNeutral1.Products.Skip(3).FirstOrDefault()!.ProductName);
        Assert.Equal("P6", resultPriceNeutral1.Products.Skip(4).FirstOrDefault()!.ProductName);
        Assert.Equal("P5", resultPriceNeutral2.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P1", resultPriceNeutral2.Products.Skip(1).FirstOrDefault()!.ProductName);
    }
    [Fact]
    public void Can_Right_Search_Products()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 7, ProductName = "Pr1", CategoryID = 1, ProductPrice=1.00M},
            new Product{ProductID = 4, ProductName = "P2", CategoryID = 2, ProductPrice=2.00M},
            new Product{ProductID = 3, ProductName = "Pr3", CategoryID = 1, ProductPrice=3.00M},
            new Product{ProductID = 2, ProductName = "P4", CategoryID = 2, ProductPrice=4.00M},
            new Product{ProductID = 6, ProductName = "Pr5", CategoryID = 3, ProductPrice=5.00M},
            new Product{ProductID = 5, ProductName = "P6", CategoryID = 1, ProductPrice=6.00M},
            new Product{ProductID = 1, ProductName = "Pr7", CategoryID = 2, ProductPrice=7.00M}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" }
        }).AsQueryable<Category>());
        ProductController productController = new(mockRepository.Object);
        //Act
        ProductsListViewModel? resultSearch1 = productController.ProductList(null, "Pr", SortOrder.Neutral, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultSearch2 = productController.ProductList(null, "Pr", SortOrder.Neutral, 2, 3)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultSearch3 = productController.ProductList("C1", "Pr", SortOrder.Neutral, 1, 2)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        Assert.Equal("Pr7", resultSearch1.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("Pr3", resultSearch1.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("Pr5", resultSearch1.Products.Skip(2).FirstOrDefault()!.ProductName);
        Assert.Equal("Pr1", resultSearch2.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("Pr3", resultSearch3.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("Pr1", resultSearch3.Products.Skip(1).FirstOrDefault()!.ProductName);
    }
    [Fact]
    public void Can_Create_New_Product()
    {
        //Arrange
        Category[] categories = new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" }
        };
        Product product = new() { ProductID = 8, ProductName = "P8", CategoryID = 3, ProductPrice = 8.00M };
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = categories[0].CategoryID, ProductPrice=1.00M},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = categories[1].CategoryID, ProductPrice=2.00M},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = categories[0].CategoryID, ProductPrice=3.00M},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = categories[1].CategoryID, ProductPrice=4.00M},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = categories[2].CategoryID, ProductPrice=5.00M},
            new Product{ProductID = 6, ProductName = "P6", CategoryID = categories[0].CategoryID, ProductPrice=6.00M},
            new Product{ProductID = 7, ProductName = "P7", CategoryID = categories[1].CategoryID, ProductPrice=7.00M}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((categories).AsQueryable<Category>());
        SelectList categoriesList = new SelectList(categories, "CategoryID", "CategoryName");
        categoriesList.Where(e => e.Value == categories[2].CategoryID.ToString()).FirstOrDefault()!.Selected = true;
        ProductViewModel productViewModel = new ProductViewModel(product, categoriesList);
        ProductController productController = new(mockRepository.Object);
        //Act
        PartialViewResult actionGet = Assert.IsType<PartialViewResult>(productController.CreateProduct());
        IActionResult? actionPost = productController.CreateProduct(productViewModel, categories[2].CategoryID.ToString(), null);
        //Assert
        Assert.Equal("../Shared/Product/_ProductCreatePartialView", actionGet.ViewName);
        Assert.IsType<Product>(actionGet.Model);
        Assert.IsType<RedirectToActionResult>(actionPost);
        Assert.Equal("Productlist", (actionPost as RedirectToActionResult)?.ActionName);
    }
    [Fact]
    public void Can_Read_Product_Details()
    {
        //Arrange
        Category[] categories = new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" }
        };
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = categories[0].CategoryID, ProductPrice=1.00M},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = categories[1].CategoryID, ProductPrice=2.00M},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = categories[0].CategoryID, ProductPrice=3.00M},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = categories[1].CategoryID, ProductPrice=4.00M},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = categories[2].CategoryID, ProductPrice=5.00M},
            new Product{ProductID = 6, ProductName = "P6", CategoryID = categories[0].CategoryID, ProductPrice=6.00M},
            new Product{ProductID = 7, ProductName = "P7", CategoryID = categories[1].CategoryID, ProductPrice=7.00M}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((categories).AsQueryable<Category>());
        ProductController productController = new(mockRepository.Object);
        //Act
        PartialViewResult actionGet = Assert.IsType<PartialViewResult>(productController.ProductDetails(5));
        //Assert
        Assert.Equal("../Shared/Product/_ProductDetailsPartialView", actionGet.ViewName);
        Assert.IsType<Product>(actionGet.Model);
        Assert.Equal(3, (actionGet.Model as Product)?.CategoryID);
        Assert.Equal("P5", (actionGet.Model as Product)?.ProductName);
        Assert.Equal(5.00M, (actionGet.Model as Product)?.ProductPrice);
    }
    [Fact]
    public void Can_Update_Existing_Product()
    {
        //Arrange
        Category[] categories = new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" }
        };
        Product[] products = new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = categories[0].CategoryID, ProductPrice=1.00M},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = categories[1].CategoryID, ProductPrice=2.00M},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = categories[0].CategoryID, ProductPrice=3.00M},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = categories[1].CategoryID, ProductPrice=4.00M},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = categories[2].CategoryID, ProductPrice=5.00M},
            new Product{ProductID = 6, ProductName = "P6", CategoryID = categories[0].CategoryID, ProductPrice=6.00M},
            new Product{ProductID = 7, ProductName = "P7", CategoryID = categories[1].CategoryID, ProductPrice=7.00M}
        };
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((products).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((categories).AsQueryable<Category>());
        SelectList categoriesList = new SelectList(categories, "CategoryID", "CategoryName");
        categoriesList.Where(e => e.Value == categories[2].CategoryID.ToString()).FirstOrDefault()!.Selected = true;
        ProductController productController = new(mockRepository.Object);
        products[3].ProductName = "_P4";
        products[3].ProductPrice = 44.00M;
        ProductViewModel productViewModel = new ProductViewModel(products[3], categoriesList);
        //Act
        PartialViewResult actionGet = Assert.IsType<PartialViewResult>(productController.UpdateProduct(products[3].ProductID ?? 0));
        IActionResult? actionPost = productController.UpdateProduct(productViewModel, categories[2].CategoryID.ToString(), null);
        //Assert
        Assert.Equal("../Shared/Product/_ProductUpdatePartialView", actionGet.ViewName);
        Assert.IsType<Product>(actionGet.Model);
        Assert.Equal(4, mockRepository.Object.Products.ToArray()[3].ProductID);
        Assert.Equal("_P4", mockRepository.Object.Products.ToArray()[3].ProductName);
        Assert.Equal(44.00M, mockRepository.Object.Products.ToArray()[3].ProductPrice);
        Assert.IsType<RedirectToActionResult>(actionPost);
        Assert.Equal("Productlist", (actionPost as RedirectToActionResult)?.ActionName);
    }
    [Fact]
    public void Can_Delete_Existing_Product()
    {
        //Arrange
        Category[] categories = new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" }
        };

        Product[] products = new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = categories[0].CategoryID, ProductPrice=1.00M},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = categories[1].CategoryID, ProductPrice=2.00M},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = categories[0].CategoryID, ProductPrice=3.00M},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = categories[1].CategoryID, ProductPrice=4.00M},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = categories[2].CategoryID, ProductPrice=5.00M},
            new Product{ProductID = 6, ProductName = "P6", CategoryID = categories[0].CategoryID, ProductPrice=6.00M},
            new Product{ProductID = 7, ProductName = "P7", CategoryID = categories[1].CategoryID, ProductPrice=7.00M}
        };
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((products).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((categories).AsQueryable<Category>());
        SelectList categoriesList = new SelectList(categories, "CategoryID", "CategoryName");
        categoriesList.Where(e => e.Value == categories[2].CategoryID.ToString()).FirstOrDefault()!.Selected = true;
        ProductViewModel productViewModel = new ProductViewModel(products[3], categoriesList);
        ProductController productController = new(mockRepository.Object);
        //Act
        PartialViewResult actionGet = Assert.IsType<PartialViewResult>(productController.DeleteProduct(2));
        IActionResult? actionPost = productController.DeleteProduct(mockRepository.Object.Products.ToArray()[1], categories[1].CategoryID.ToString(), null);
        //Assert
        Assert.Equal("../Shared/Product/_ProductDeletePartialView", actionGet.ViewName);
        Assert.IsType<Product>(actionGet.Model);
        //Assert.Equal(null, mockRepository.Object.Products.ToArray()[1].ProductID);
        //Assert.Equal("Нет в наличии", mockRepository.Object.Products.ToArray()[1].ProductName);
        //Assert.Equal(2.00M, mockRepository.Object.Products.ToArray()[1].ProductPrice);
        Assert.IsType<RedirectToActionResult>(actionPost);
        Assert.Equal("Productlist", (actionPost as RedirectToActionResult)?.ActionName);
    }
}