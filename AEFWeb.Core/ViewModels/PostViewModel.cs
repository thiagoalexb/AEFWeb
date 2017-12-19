using AEFWeb.Core.ViewModels.Core;
using System;
using System.Collections.Generic;

namespace AEFWeb.Core.ViewModels
{
    public class PostViewModel : ViewModelBase
    {
        public PostViewModel(Guid id, string title, string subTitle, string mainImage, string content, DateTime publicationDate, Guid userId)
        {
            Id = id;
            Title = title;
            SubTitle = subTitle;
            MainImage = mainImage;
            Content = content;
            PublicationDate = publicationDate;
            UserId = userId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string MainImage { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }

        public List<TagViewModel> Tags { get; set; }
        public Guid UserId { get; set; }
        public UserViewModel User { get; set; }
    }
}