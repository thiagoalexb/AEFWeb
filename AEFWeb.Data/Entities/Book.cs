using AEFWeb.Data.Entities.Core;
using System;

namespace AEFWeb.Data.Entities
{
    public class Book : Entity
    {
        public Book(Guid id, 
            string publishingCompany, 
            string edition, 
            string author, 
            string title, 
            bool isSale, 
            decimal value) : base(id)
        {
            PublishingCompany = publishingCompany;
            Edition = edition;
            Author = author;
            Title = title;
            IsSale = isSale;
            Value = value;
        }
        public Book() : base(Guid.NewGuid()) { }

        public string PublishingCompany { get; private set; }
        public string Edition { get; private set; }
        public string Author { get; private set; }
        public string Title { get; private set; }
        public bool IsSale { get; private set; }
        public decimal Value { get; private set; }
    }
}
