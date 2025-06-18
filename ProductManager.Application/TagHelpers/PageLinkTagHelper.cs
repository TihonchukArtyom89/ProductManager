using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ProductManager.Application.ViewModels;
using SortOrder = ProductManager.Application.Models.SortOrder;

namespace ProductManager.Application.Infrastructure;

[HtmlTargetElement("div", Attributes = "page-model")]
public class PageLinkTagHelper : TagHelper
{
    private IUrlHelperFactory urlHelperFactory;
    public PageLinkTagHelper(IUrlHelperFactory _urlHelperFactory)
    {
        urlHelperFactory = _urlHelperFactory;
    }
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }
    public PageViewModel? PageModel { get; set; }
    public string? PageAction { get; set; }
    [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
    public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();
    public bool PageClassEnabled { get; set; } = false;
    public string PageClass { get; set; } = string.Empty;
    public string PageClassNormal { get; set; } = string.Empty;
    public string PageClassSelected { get; set; } = string.Empty;
    public string PageClassArrow { get; set; } = string.Empty;
    public SortOrder PageSortOrder { get; set; } = SortOrder.Neutral;
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext != null && PageModel != null)
        {
            if (PageSortOrder == SortOrder.NameAsc || PageSortOrder == SortOrder.NameDesc)
            {
                PageSortOrder = PageSortOrder == SortOrder.NameDesc ? SortOrder.NameAsc : SortOrder.NameDesc;
            }
            if (PageSortOrder == SortOrder.PriceAsc || PageSortOrder == SortOrder.PriceDesc)
            {
                PageSortOrder = PageSortOrder == SortOrder.PriceDesc ? SortOrder.PriceAsc : SortOrder.PriceDesc;
            }
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            string css1 = "", css2 = "";
            TagBuilder pagination = new TagBuilder("div");
            TagBuilder pageButton = new TagBuilder("a");
            if(PageModel.CurrentPage > 1)
            {
                pageButton = PageLinkBuilder(PageModel.CurrentPage == 1 ? 1 : PageModel.CurrentPage - 1, urlHelper, " <-  ", PageClass, PageClassArrow);
                pagination.InnerHtml.AppendHtml(pageButton);
            }
            if (PageModel.TotalPages == 1)
            {
                pageButton = PageLinkBuilder(1, urlHelper, "1", PageClass, PageClassSelected);
                pagination.InnerHtml.AppendHtml(pageButton);
            }
            else
            {
                int numberOfPageBeforeCurrentPage = PageModel.CurrentPage != 1 ? (PageModel.CurrentPage - 1) : 1;
                numberOfPageBeforeCurrentPage = PageModel.CurrentPage != PageModel.TotalPages ? numberOfPageBeforeCurrentPage : (PageModel.CurrentPage - 2);
                numberOfPageBeforeCurrentPage = numberOfPageBeforeCurrentPage == 0 ? 1 : numberOfPageBeforeCurrentPage;
                int numberOfPageAfterCurrentPage = PageModel.CurrentPage != PageModel.TotalPages ? (PageModel.CurrentPage + 1) : PageModel.TotalPages;
                numberOfPageAfterCurrentPage = PageModel.CurrentPage != 1 ? numberOfPageAfterCurrentPage : (PageModel.CurrentPage + 2);
                numberOfPageAfterCurrentPage = numberOfPageAfterCurrentPage > PageModel.TotalPages ? PageModel.TotalPages : numberOfPageAfterCurrentPage;
                for (int i = numberOfPageBeforeCurrentPage; i <= numberOfPageAfterCurrentPage; i++)
                {//cycle for display three pages
                    if (PageClassEnabled)
                    {
                        css1 = PageClass;
                        css2 = i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal;
                    }
                    pageButton = PageLinkBuilder(i, urlHelper, i.ToString(), css1, css2);
                    pagination.InnerHtml.AppendHtml(pageButton);
                }
                if (PageModel.CurrentPage < PageModel.TotalPages)
                {
                    pageButton = PageLinkBuilder(PageModel.CurrentPage == PageModel.TotalPages ? PageModel.TotalPages : PageModel.CurrentPage + 1, urlHelper, " -> ", PageClass, PageClassArrow);
                    pagination.InnerHtml.AppendHtml(pageButton);
                }
            }
            output.Content.AppendHtml(pagination.InnerHtml);
        }
    }
    public TagBuilder PageLinkBuilder(int pageNumber, IUrlHelper urlHelper, string pageText, string css1 = "", string css2 = "")
    {
        TagBuilder pageLink = new TagBuilder("a");
        PageUrlValues["sortOrder"] = PageSortOrder;
        string pageAction = PageAction ?? string.Empty;
        if (pageAction.Contains("ProductlistList"))
        {
            PageUrlValues["productPage"] = pageNumber;
        }
        if (pageAction.Contains("PricelistList"))
        {
            PageUrlValues["pricelistPage"] = pageNumber;
        }
        PageUrlValues["pageSize"] = (PageModel ?? new()).PageSize;
        pageLink.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
        pageLink.AddCssClass(css1);
        pageLink.AddCssClass(css2);
        pageLink.InnerHtml.Append(pageText);
        return pageLink;
    }
}
