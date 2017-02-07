using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Routing;
using MyCoreLib.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyCoreLib.BaseWeb.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Writes an opening FORM tag to the response and includes the html attribtues.
        /// The form uses the POST method, and the request is processed by the action method for the view.
        /// </summary>
        public static MvcForm BeginUploadForm(this HtmlHelper html, string actionName = null, object htmlAttributes = null, object routeValues = null)
        {
            RouteValueDictionary htmlValueMap = htmlAttributes == null
                ? new RouteValueDictionary()
                : new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            htmlValueMap["enctype"] = "multipart/form-data";

            RouteValueDictionary routeValueMap = routeValues == null
                ? new RouteValueDictionary()
                : new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues));

            return html.BeginForm(actionName, null, routeValueMap, FormMethod.Post, AppSettings.Antiforgery, htmlValueMap);
        }

        /// <summary>
        /// Writes an opening FORM tag to the response and includes the html attribtues.
        /// The form uses the POST method, and the request is processed by the action method for the view.
        /// </summary>
        public static MvcForm BeginPostForm(this HtmlHelper html, string actionName = null, object htmlAttributes = null, object routeValues = null)
        {
            return html.BeginForm(actionName, null, routeValues, FormMethod.Post, AppSettings.Antiforgery, htmlAttributes);
        }

        /// <summary>
        /// Replace all relative image paths as absolute ones. e.g. from "~/a.png" to "/shop/a.png".
        /// </summary>
        public static string ResolveImageUrls(this HtmlHelper html, string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return htmlText;
            }

            string vPath = AppSettings.AppDomainAppVirtualPath;
            if (vPath[vPath.Length - 1] != '/')
            {
                vPath += "/";
            }

            return htmlText.Replace("src=\"~/", "src=\"" + vPath);
        }

        /// <summary>
        /// Write an hidden input element with the specified 
        /// </summary>
        public static HtmlString TextBoxWithCustomizedValueFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string value, object htmlAttributes = null)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            IDictionary<string, object> dictionary = null;
            if (htmlAttributes != null)
            {
                dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            }
            return TextBoxWithCustomizedValueFor(html, expression, value, dictionary);
        }

        /// <summary>
        /// Write an hidden input element with the specified 
        /// </summary>
        public static HtmlString TextBoxWithCustomizedValueFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string value, IDictionary<string, object> htmlAttributes = null)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            string name = ExpressionHelper.GetExpressionText(expression);
            string fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentException("Failed to generate field name.");

            TagBuilder tagBuilder = new TagBuilder("input");

            if (htmlAttributes != null)
            {
                tagBuilder.MergeAttributes<string, object>(htmlAttributes);
            }
            tagBuilder.MergeAttribute("type", "text");
            tagBuilder.MergeAttribute("name", fullName);
            tagBuilder.MergeAttribute("value", value, true);
            tagBuilder.GenerateId(fullName, null);

            //ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            //tagBuilder.MergeAttributes<string, object>(html.GetUnobtrusiveValidationAttributes(name, metadata));
            return new HtmlString(tagBuilder.ToString());
        }

        /// <summary>
        /// Write an hidden input element with the specified 
        /// </summary>
        public static HtmlString HiddenFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string value)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            string name = ExpressionHelper.GetExpressionText(expression);
            string fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentException("Failed to generate field name.");

            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.MergeAttribute("type", "hidden");
            tagBuilder.MergeAttribute("name", fullName);
            tagBuilder.MergeAttribute("value", value, true);
            tagBuilder.GenerateId(fullName, null);

            //ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            //tagBuilder.MergeAttributes<string, object>(html.GetUnobtrusiveValidationAttributes(name, metadata));
            return new HtmlString(tagBuilder.ToString());
        }

        /// <summary>
        /// Write an hidden input element with the specified 
        /// </summary>
        public static HtmlString CheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            string name = ExpressionHelper.GetExpressionText(expression);
            string fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentException("Failed to generate field name.");

            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("name", fullName);
            tagBuilder.MergeAttribute("value", "true");
            tagBuilder.GenerateId(fullName, null);

            //ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            //if ((bool)metadata.Model)
            //{
            //    tagBuilder.MergeAttribute("checked", "checked");
            //}
            //tagBuilder.MergeAttributes<string, object>(html.GetUnobtrusiveValidationAttributes(name, metadata));
            return new HtmlString(tagBuilder.ToString());
        }

        /// <summary>
        /// Write an hidden input element with the specified 
        /// </summary>
        public static HtmlString SelectFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            string name = ExpressionHelper.GetExpressionText(expression);
            string fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentException("Failed to generate field name.");

            TagBuilder tagBuilder = new TagBuilder("select");

            tagBuilder.MergeAttribute("name", fullName);
            if (htmlAttributes != null)
            {
                RouteValueDictionary dictionary = new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
                tagBuilder.MergeAttributes<string, object>(dictionary);
            }
            tagBuilder.GenerateId(fullName, null);

            //ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            //tagBuilder.MergeAttributes<string, object>(html.GetUnobtrusiveValidationAttributes(name, metadata));
            return new HtmlString(tagBuilder.ToString());
        }

        /// <summary>
        /// Generate the content for background images.
        /// </summary>
        public static string GetSvgImageData(this HtmlHelper html, int width, int height, string text = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = string.Format("{0} x {1}", width, height);
            }

            const string ImageDataFormat = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{0}\" height=\"{1}\">"
                + "<rect width=\"{0}\" height=\"{1}\" fill=\"#eee\"/>"
                + "<text text-anchor=\"middle\" x=\"{2}\" y=\"{3}\" style=\"fill:#aaa;font-weight:bold;font-size:14px;font-family:Arial,Helvetica,sans-serif;dominant-baseline:central\">{4}</text>"
                + "</svg>";

            string data = string.Format(ImageDataFormat, width, height, width / 2, height / 2, text);
            return "data:image/svg+xml;base64," + System.Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Generate the content for background images.
        /// </summary>
        public static string GetSvgImageData(this HtmlHelper html, int width, int height, string[] text)
        {
            if (text == null || text.Length == 0)
                throw new ArgumentNullException("text");

            // TODO:
            // Need to fix this.
            //
            const string PreImageDataFormat = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{0}\" height=\"{1}\">"
                + "<rect width=\"{0}\" height=\"{1}\" fill=\"#eee\"/>"
                + "<text text-anchor=\"middle\" x=\"{2}\" y=\"{3}\" style=\"fill:#aaa;font-weight:bold;font-size:14px;font-family:Arial,Helvetica,sans-serif;dominant-baseline:central\">";
            const string PostImageDataFormat = "</text></svg>";

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(PreImageDataFormat, width, height, width / 2, height / 2);
            foreach (string item in text)
            {
                sb.AppendFormat("<tspan x=\"0\" dy=\"1.2em\">{0}</tspan>", item);
            }
            sb.Append(PostImageDataFormat);

            string data = sb.ToString();
            return "data:image/svg+xml;base64," + System.Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }
    }
}
