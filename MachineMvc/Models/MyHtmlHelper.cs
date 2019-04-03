using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


//注意这里的空间路径，不能错，必须在System.Web.Mvc下，不然没法直接使用html调用
namespace System.Web.Mvc
{
    public static class MyHtmlHelper
    {
        //生成新闻展示模块
        public static HtmlString Newsinfodiv(this HtmlHelper htmlHelper, string content)
        {
            return new HtmlString(content.ToString());
        }
        public static HtmlString ShowPageNavigate(this HtmlHelper htmlHelper, PagingInfo pi)
        {
            var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            pi.PageSize = pi.PageSize == 0 ? 3 : pi.PageSize;
            var totalPages = Math.Max((pi.TotalItems + pi.PageSize - 1) / pi.PageSize, 1); //总页数
            var output = new StringBuilder();
            output.Append("<span class='page'>");
            if (totalPages > 1)
            {
                if (pi.PageIndex != 1)
                {//处理首页连接
                    output.AppendFormat("<a class='pageItem' href='javascript:window.location.href = \"{0}?pageIndex=1&pageSize={1}&\"+$(\"#searchform\").serialize();'>首页</a> ", redirectTo, pi.PageSize);
                }
                else
                {
                    output.Append("<a class='pageItemDisable'>首页</a>");
                }
                if (pi.PageIndex > 1)
                {//处理上一页的连接
                    output.AppendFormat("<a class='pageItem' href='javascript:window.location.href = \"{0}?pageIndex={1}&pageSize={2}&\"+$(\"#searchform\").serialize();'>上一页</a> ", redirectTo, pi.PageIndex - 1, pi.PageSize);
                }
                else
                {
                    output.Append("<a class='pageItemDisable'>上一页</a>");
                }

                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 8; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((pi.PageIndex + i - currint) >= 1 && (pi.PageIndex + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理
                            //output.Append(string.Format("[{0}]", currentPage));
                            output.AppendFormat("<a class='pageItemActive' href='javascript:window.location.href = \"{0}?pageIndex={1}&pageSize={2}&\"+$(\"#searchform\").serialize();'><b>{3}</b></a> ", redirectTo, pi.PageIndex, pi.PageSize, pi.PageIndex);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<a class='pageItem' href='javascript:window.location.href = \"{0}?pageIndex={1}&pageSize={2}&\"+$(\"#searchform\").serialize();'>{3}</a> ", redirectTo, pi.PageIndex + i - currint, pi.PageSize, pi.PageIndex + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (pi.PageIndex < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<a class='pageItem' href='javascript:window.location.href = \"{0}?pageIndex={1}&pageSize={2}&\"+$(\"#searchform\").serialize();'>下一页</a> ", redirectTo, pi.PageIndex + 1, pi.PageSize);
                }
                else
                {
                    output.Append("<a class='pageItemDisable'>下一页</a>");
                }
                output.Append(" ");
                if (pi.PageIndex != totalPages)
                {
                    output.AppendFormat("<a class='pageItem' href='javascript:window.location.href = \"{0}?pageIndex={1}&pageSize={2}&\"+$(\"#searchform\").serialize();'>末页</a> ", redirectTo, totalPages, pi.PageSize);
                }
                else
                {
                    output.Append("<a class='pageItemDisable'>末页</a>");
                }
                output.Append(" ");
            }
            output.AppendFormat("<a class='pageIteminfo'>第{0}页/共{1}页</a>", pi.PageIndex, totalPages);//这个统计加不加都行

            output.Append("</span>");
            return new HtmlString(output.ToString());
        }
    }
}