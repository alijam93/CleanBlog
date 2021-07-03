using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Components.Pagination
{
    public class PagingLink
    {
        public string Text { get; set; }
        public RenderFragment Icon { get; set; }
        public int Page { get; set; }
        public bool Enabled { get; set; }
        public bool Active { get; set; }

        public PagingLink(int page, bool enabled, string text, RenderFragment icon)
        {
            Page = page;
            Enabled = enabled;
            Text = text;
            Icon = icon;
        }
    }
}
