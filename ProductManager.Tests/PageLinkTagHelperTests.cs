using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductManager.Application.Components;
using ProductManager.Application.Infrastructure;
using ProductManager.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Tests;

public class PageLinkTagHelperTests
{
    [Fact]
    public void Can_Generate_Page_Links()
    {
        //Arrange
        Mock<IUrlHelper> mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.SetupSequence(e => e.Action(It.IsAny<UrlActionContext>())).Returns("Test/Page1").Returns("Test/Page1").Returns("Test/Page2").Returns("Test/Page3").Returns("Test/Page3");
        Mock<IUrlHelperFactory> mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
        mockUrlHelperFactory.Setup(e=>e.GetUrlHelper(It.IsAny<ActionContext>())).Returns(mockUrlHelper.Object);
        Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
        PageLinkTagHelper pageLinkTagHelper = new PageLinkTagHelper(mockUrlHelperFactory.Object) 
        {
            PageModel = new PageViewModel() 
            {
                CurrenPage = 2,
                TotalItems = 28,
                PageSize = 10
            },
            ViewContext = mockViewContext.Object,
            PageAction = "Test"
        };
        TagHelperContext tagHelperContext = new TagHelperContext(new TagHelperAttributeList(),new Dictionary<object,object>(),"");
        Mock<TagHelperContent> mockTagHelperContent = new Mock<TagHelperContent>();  
        TagHelperOutput tagHelperOutput = new TagHelperOutput("div",new TagHelperAttributeList(),(cache,encoder)=> Task.FromResult(mockTagHelperContent.Object));
        //Act
        pageLinkTagHelper.Process(tagHelperContext, tagHelperOutput);
        //Assert
        Assert.Equal(@"<a class="" "" href=""Test/Page1""> &lt;-  </a><a class="" "" href=""Test/Page1"">1</a><a class="" "" href=""Test/Page2"">2</a><a class="" "" href=""Test/Page3"">3</a><a class="" "" href=""Test/Page3""> -&gt; </a>", tagHelperOutput.Content.GetContent());
    }
    [Fact]
    public void Can_Redirect_To_Page_With_Saving_Number_Of_Page()
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
        int allProductsNumber = (productController.ProductList(null, null, SortOrder.PriceDesc,2, 1)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.CurrenPage;
        int c2ProductsNumber = (productController.ProductList("C2", null, SortOrder.PriceDesc, 2, 1)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.CurrenPage;
        int c1ProductsNumber = (productController.ProductList("C1", null, SortOrder.PriceDesc, 2, 1)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.CurrenPage;
        //Assert
        Assert.Equal(2, allProductsNumber);
        Assert.Equal(2, c2ProductsNumber);
        Assert.Equal(2, c1ProductsNumber);
    }
    [Fact]
    public void Can_Redirect_To_First_Page_When_Products_Count_Is_Zero()
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
        int allProductsNumber = (productController.ProductList(null, null, SortOrder.PriceDesc, 5, 1)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.CurrenPage;
        int c2ProductsNumber = (productController.ProductList("C2", null, SortOrder.PriceDesc, 5, 1)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.CurrenPage;
        int c1ProductsNumber = (productController.ProductList("C1", null, SortOrder.PriceDesc, 5, 1)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.CurrenPage;
        //Assert
        Assert.Equal(5, allProductsNumber);
        Assert.Equal(1, c2ProductsNumber);
        Assert.Equal(1, c1ProductsNumber);
    }
}
