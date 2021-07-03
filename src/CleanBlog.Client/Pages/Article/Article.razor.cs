using CleanBlog.Client.Utils.Statics;
using CleanBlog.Components.Article;
using CleanBlog.Shared.Dtos;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanBlog.Client.Pages.Article
{
    public partial class Article
    {
        [Parameter] public int Id { get; set; }
        [Parameter] public string Slug { get; set; }

        protected string closeModal = "";

        protected CommentDTO[] Comments;
        protected CommentDTO Comment = new();
        protected AddReplyDTO Reply { get; set; }
        protected PostDTO Post { get; set; }
        protected PostDTO[] Posts;

        protected UpateComment UpateComment { get; set; }
        protected ReplyComment ReplyComment { get; set; }
        protected Delete Delete { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Post = await http.GetFromJsonAsync<PostDTO>($"{Endpoints.Posts}{Id}/{Slug}");
            await GetCommentByPostId();
        }

        public async Task GetCommentByPostId()
        {
            Comments = await http.GetFromJsonAsync<CommentDTO[]>(Endpoints.Comments + Id);

        }

        public async Task GetPostByTag(int tagId)
        {
            if (tagId > 0)
            {
                Posts = await http.GetFromJsonAsync<PostDTO[]>(Endpoints.Posts + tagId);
            }
            else
            {
                Posts = await http.GetFromJsonAsync<PostDTO[]>(Endpoints.Posts);

            }
        }

        async Task AddReply(int commentId)
        {
            Reply = await http.GetFromJsonAsync<AddReplyDTO>(Endpoints.Comments + commentId);
        }

        async Task DeleteCommment(int id)
        {
            await http.DeleteAsync(Endpoints.Comments + id);
            await GetCommentByPostId();
        }


        private async Task GetCommentById(int id)
        {
            UpateComment.Open();
            Comment = await http.GetFromJsonAsync<CommentDTO>(Endpoints.Comments+ $"getById/{id}");
        }

        async Task EditComment()
        {
            await http.PutAsJsonAsync(Endpoints.Comments +Comment.Id, Comment);
            UpateComment.Close();
            await GetCommentByPostId();
        }
    }
}
