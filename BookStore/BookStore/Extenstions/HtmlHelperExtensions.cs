﻿using BookStore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Extenstions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 将视图模型生成json格式的字符串。
        /// </summary>
        /// <param name="model">需要转换为json的视图模型</param>
        /// <returns></returns>
        public static HtmlString HtmlConvertToJson(this HtmlHelper htmlHelper,object model)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            return new HtmlString(JsonConvert.SerializeObject(model, settings));
        }
        /// <summary>
        /// 生成带图标的排序按钮链接
        /// </summary>
        /// <param name="fieldName">链接文本显示名称</param>
        /// <param name="actionName">链接的action名称</param>
        /// <param name="sortField">模型的排序字段名称</param>
        /// <param name="queryOptions">包含当前排序分页对象排序使用的QueryOptions。用于判断当前字段是否是正在排序的字段，如果是，则返回排序方向。</param>
        /// <returns></returns>
        public static MvcHtmlString BuildSortableLink(this HtmlHelper htmlHelper, string fieldName,string actionName,string sortField,QueryOptions queryOptions)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var isCurrentSortField = queryOptions.SortField == sortField;
            return new MvcHtmlString(string.Format("<a href=\"{0}\">{1} {2}</a>",
                urlHelper.Action(actionName,
                new
                {
                    SortField = sortField,
                    SortOrder = (isCurrentSortField && queryOptions.SortOrder == SortOrder.ASC) ? SortOrder.DESC : SortOrder.ASC
                }),
                fieldName,
                BuildSortIcon(isCurrentSortField, queryOptions)));
        }

        private static string BuildSortIcon ( bool isCurrentSortField, QueryOptions queryOptions)
        {
            /*
             * Bootstrap 4 取消了glyphicon，本项目采用fontawesome作为替代品。
            string sortIcon = "sort";
            if (isCurrentSortField)
            {
                sortIcon += "-by-alphabet";
                if(queryOptions.SortOrder== SortOrder.DESC)
                {
                    sortIcon += "-alt";
                }
            }
            return string.Format("<span class=\"{0} {1}{2}\"></span>", "glyphicon", "glyphicon-", sortIcon);
            */
            string sortIcon = "fa-sort-alpha-";
            if (isCurrentSortField)
            {
                if (queryOptions.SortOrder == SortOrder.DESC)
                {
                    sortIcon += "desc";
                }
                else
                {
                    sortIcon += "asc";
                }
            }
            return string.Format("<i class=\"{0} {1}\" aria-hidden=\"true\"></i>", "fa", sortIcon);
        }

        /// <summary>
        /// 生成分页链接（previous和next）。
        /// </summary>
        /// <param name="queryOptions">包含当前排序分页对象分页使用的QueryOptions。</param>
        /// <param name="actionName">链接的action名称</param>
        /// <returns></returns>
        public static MvcHtmlString BuildNextPreviousLinks(this HtmlHelper htmlHelper,QueryOptions queryOptions, string actionName)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            return new MvcHtmlString(string.Format("<nav><ul class=\"pager\"><li class=\"previous {0}\">{1}</li><li class=\"next {2}\">{3}</li></ul></nav>",
                IsPreviousDisabled(queryOptions),
                BuildPreviousLink(urlHelper, queryOptions, actionName),
                IsNexDisabled(queryOptions),
                BuildNextLink(urlHelper, queryOptions, actionName)
                ));
        }

        private static string IsPreviousDisabled(QueryOptions queryOptions)
        {
            return (queryOptions.CurrentPage == 1) ? "disabled" : string.Empty;
        }

        private static string IsNexDisabled (QueryOptions queryOptions)
        {
            return (queryOptions.CurrentPage == queryOptions.TotalPages) ? "disabled" : string.Empty;
        }

        private static string BuildPreviousLink(UrlHelper urlHelper, QueryOptions queryOptions,string actionName)
        {
            int currentPage = queryOptions.CurrentPage - 1;
            if (currentPage <= 0) currentPage = 1;
            return string.Format("<a href=\"{0}\"><span aria-hidden=\"true\">&larr;</span> Previous</a>",
                urlHelper.Action(actionName, new
                {
                    SortOrder = queryOptions.SortOrder,
                    SortField = queryOptions.SortField,
                    CurrentPage = currentPage,
                    PageSize = queryOptions.PageSize
                }));
        }

        private static string BuildNextLink(UrlHelper urlHelper, QueryOptions queryOptions, string actionName)
        {
            int currentPage = queryOptions.CurrentPage + 1;
            if (currentPage > queryOptions.TotalPages) currentPage = queryOptions.TotalPages;
            return string.Format("<a href=\"{0}\">Next <span aria-hidden=\"true\">&rarr;</span></a>",
                urlHelper.Action(actionName, new
                {
                    SortOrder = queryOptions.SortOrder,
                    SortField = queryOptions.SortField,
                    CurrentPage = currentPage,
                    PageSize = queryOptions.PageSize
                }));
        }
    }
}