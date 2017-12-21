using AEFWeb.Core.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Título é obrigatório")]
        [MaxLength(100, ErrorMessage = "Título pode ter no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "Título precisa conter no mínimo 3 caracteres")]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string MainImage { get; set; }

        [Required(ErrorMessage = "Conteúdo é obrigatório")]
        [MaxLength(100, ErrorMessage = "Conteúdo pode ter no máximo 5000 caracteres")]
        public string Content { get; set; }

        [Required(ErrorMessage = "A data da publicação é obrigatória")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime PublicationDate { get; set; }

        public List<TagViewModel> Tags { get; set; }
        public Guid UserId { get; set; }
        public UserViewModel User { get; set; }
    }
}