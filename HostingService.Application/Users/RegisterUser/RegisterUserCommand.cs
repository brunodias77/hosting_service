using System;
using System.ComponentModel.DataAnnotations;
using HostingService.Application.Abstraction;

namespace HostingService.Application.Users.RegisterUser
{
    public class RegisterUserCommand : ICommand<RegisterUserResponse>
    {
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string FirstName { get; private set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string LastName { get; private set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email não é um endereço de email válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Password { get; private set; }

        public RegisterUserCommand(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }
    }
}

