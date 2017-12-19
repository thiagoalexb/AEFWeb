using AEFWeb.Data.Entities;
using Flunt.Notifications;
using Flunt.Validations;

namespace AEFWeb.Data.Validations.UserNotifications
{
    class UserUpdateNotification
    {
        public Notifiable Validate(User user)
        {
            return new Contract()
                .Requires()
                .IsNull(user.Id, "User.Id", string.Empty)
                .IsNullOrEmpty(user.FirstName, "User.FirstName", "Nome é obrigatório")
                .IsNullOrEmpty(user.LastName, "User.LastName", "Sobrenome é obrigatório")
                .IsEmail(user.Email, "User.Email", "E-mail invalido")
                .IsNullOrEmpty(user.Password, "User.Password", "Senha é obrigatório");
        }
    }
}
