using Flunt.Notifications;
using Flunt.Validations;
using AEFWeb.Data.Entities;

namespace AEFWeb.Data.Validations.UserNotifications
{
    public class UserAddNotification
    {
        public Notifiable Validate(User user)
        {
            return new Contract()
                .Requires()
                .IsNullOrEmpty(user.FirstName, "User.FirstName", "Nome é obrigatório")
                .IsNullOrEmpty(user.LastName, "User.LastName", "Sobrenome é obrigatório")
                .IsEmail(user.Email, "User.Email", "E-mail invalido");
        }
    }
}