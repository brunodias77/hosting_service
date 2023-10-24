using System;
using HostingService.Domain.Abstractions;
using HostingService.Domain.User.Event;
using HostingService.Domain.User.ValueObject;

namespace HostingService.Domain.User
{
    public class User : Entity
    {

        public UserId Id { get; private set; }
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public User(FirstName firstName, LastName lastName, Email email, Password password)
        {
            Id = new UserId(Guid.NewGuid());
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            NotifyDomainEvent(new UserCreatedDomainEvent(this.Id));
        }
        public static User CreateRegistered(FirstName firstName, LastName lastName, Email email, Password password)
        {
            return new User(firstName, lastName, email, password);
        }
    }
}

