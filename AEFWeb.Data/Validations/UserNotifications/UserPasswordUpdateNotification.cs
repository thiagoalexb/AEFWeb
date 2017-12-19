using AEFWeb.Data.Entities;
using Flunt.Notifications;
using Flunt.Validations;

namespace AEFWeb.Data.Validations.UserNotifications
{
    public class UserPasswordUpdateNotification
    {
        public Notifiable Validate(User user)
        {
            return new Contract()
                .Requires()
                .IsNull(user.Id, "User.Id", string.Empty)
                .IsNullOrEmpty(user.Password, "User.Password", "Senha é obrigatório");
        }
    }
}
