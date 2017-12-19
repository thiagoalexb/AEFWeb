using AEFWeb.Data.Entities.Core;
using System;

namespace AEFWeb.Data.Entities
{
    public class User : Entity
    {
        public User(Guid id, string firstName, string lastName, string email, string password, DateTime dateOfBirth, bool isVerified) : base (id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            DateOfBirth = dateOfBirth;
            IsVerified = isVerified;
        }

        public User() : base (Guid.NewGuid()) { }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public bool IsVerified { get; private set; }

        public void SetPassword(string password)
        {
            Password = password;
            IsVerified = true;
        }
    }
}
