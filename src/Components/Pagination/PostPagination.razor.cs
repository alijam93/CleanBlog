using CleanBlog.Shared.Features.Pagination;

using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Components.Pagination
{
    public partial class PostPagination
    {
        [Parameter] public Paging Paging { get; set; }
        /// <summary>
        /// Number of page buttons (links) that will show before and after the currently selected page
        /// </summary>
        [Parameter] public int Spread { get; set; }
        [Parameter] public string Url { get; set; }
        [Parameter] public EventCallback<int> SelectedPage { get; set; }

        private List<PagingLink> _links;

        protected override  void OnParametersSet()
        {
             CreatePaginationLinks();
        }

        private void  CreatePaginationLinks()
        {
            _links = new List<PagingLink>();
            _links.Add(new PagingLink(Paging.CurrentPage - 1, Paging.HasPrevious, null, prevIcon));
            for (int i = 1; i <= Paging.TotalPages; i++)
            {
                if (i >= Paging.CurrentPage - Spread && i <= Paging.CurrentPage + Spread)
                {
                    _links.Add(new PagingLink(i, true, i.ToString(), null) { Active = Paging.CurrentPage == i });
                }
            }
            _links.Add(new PagingLink(Paging.CurrentPage + 1, Paging.HasNext, null, nextIcon));
        }

        private async Task OnSelectedPage(PagingLink link)
        {
            if (link.Page == Paging.CurrentPage || !link.Enabled)
                return;
            Paging.CurrentPage = link.Page;
            await SelectedPage.InvokeAsync(link.Page);
        }
    }
}
