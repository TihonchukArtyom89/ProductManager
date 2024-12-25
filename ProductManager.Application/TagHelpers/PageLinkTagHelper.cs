using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient;
using ProductManager.Application.ViewModels;
using ProductManager.Application.Models;
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
            if(PageModel.CurrenPage > 1)
            {
                pageButton = PageLinkBuilder(PageModel.CurrenPage == 1 ? 1 : PageModel.CurrenPage - 1, urlHelper, " <-  ", PageClass, PageClassArrow);
                pagination.InnerHtml.AppendHtml(pageButton);
            }
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                if (PageClassEnabled)
                {
                    css1 = PageClass;
                    css2 = i == PageModel.CurrenPage ? PageClassSelected : PageClassNormal;
                }
                pageButton = PageLinkBuilder(i, urlHelper, i.ToString(), css1, css2);
                pagination.InnerHtml.AppendHtml(pageButton);
            }
            if(PageModel.CurrenPage < PageModel.TotalPages)
            {
                pageButton = PageLinkBuilder(PageModel.CurrenPage == PageModel.TotalPages ? PageModel.TotalPages : PageModel.CurrenPage + 1, urlHelper, " -> ", PageClass, PageClassArrow);
                pagination.InnerHtml.AppendHtml(pageButton);
            }
            output.Content.AppendHtml(pagination.InnerHtml);
        }
    }
    public TagBuilder PageLinkBuilder(int pageNumber, IUrlHelper urlHelper, string pageText, string css1 = "", string css2 = "")
    {
        TagBuilder pageLink = new TagBuilder("a");
        PageUrlValues["productPage"] = pageNumber;
        PageUrlValues["pageSize"] = (PageModel ?? new()).PageSize;
        PageUrlValues["sortOrder"] = PageSortOrder;
        pageLink.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
        pageLink.AddCssClass(css1);
        pageLink.AddCssClass(css2);
        pageLink.InnerHtml.Append(pageText);
        return pageLink;
    }
}
