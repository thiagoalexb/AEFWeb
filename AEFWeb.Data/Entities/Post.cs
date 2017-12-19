using AEFWeb.Data.Entities.Core;
using System;
using System.Collections.Generic;

namespace AEFWeb.Data.Entities
{
    public class Post : Entity
    {
        public Post(Guid id, string title, string subTitle, string mainImage, string content, DateTime publicationDate, Guid userId) : base(id)
        {
            Title = title;
            SubTitle = subTitle;
            MainImage = mainImage;
            Content = content;
            PublicationDate = publicationDate;
            UserId = userId;
            PostTags = new List<PostTag>();
        }

        public Post() : base(Guid.NewGuid()) { }

        public string Title { get; private set; }
        public string SubTitle { get; private set; }
        public string MainImage { get; private set; }
        public string Content { get; private set; }
        public DateTime PublicationDate { get; private set; }

        public ICollection<PostTag> PostTags { get; } = new List<PostTag>();

        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }
    }
}
