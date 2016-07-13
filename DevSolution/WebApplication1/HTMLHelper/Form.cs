using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1
{
    public static class Form
    {
        public static MvcHtmlString LoginClientFormBuilder(this HtmlHelper helper,
            string css,object parameters = null)
        {
            TagBuilder tag = new TagBuilder("form");
            if (!string.IsNullOrWhiteSpace(css))
            {
                tag.AddCssClass(css);
            }

            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(parameters));
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("row");

            TagBuilder divInput = new TagBuilder("div");
            divInput.AddCssClass("input-field col s12");

            TagBuilder username = new TagBuilder("input");
            username.MergeAttribute("type", "text");
            username.MergeAttribute("placeholder", "username");
            username.GenerateId("username");

            divInput.InnerHtml = username.ToString();
            div.InnerHtml = divInput.ToString();

            tag.InnerHtml += div.ToString();

            TagBuilder password = new TagBuilder("input");
            password.MergeAttribute("type", "password");
            password.MergeAttribute("placeholder", "password");
            password.GenerateId("password");


            divInput.InnerHtml = password.ToString();
            div.InnerHtml = divInput.ToString();
            tag.InnerHtml += div.ToString();

            TagBuilder submit = new TagBuilder("button");
            submit.SetInnerText("Submit");

            tag.InnerHtml += submit.ToString();

            return new MvcHtmlString(tag.ToString());
        }
    }
}