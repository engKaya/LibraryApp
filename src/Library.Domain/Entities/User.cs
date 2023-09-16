using Library.Domain.BaseClasses;
using Library.Domain.DTOs.User;
using Library.Domain.Helper;

namespace Library.Domain.Entities
{
    public class User : BaseEntity
    {

        private string _fullName = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;

        public string FullName { get => _fullName; set => FullName = value; }
        public string Email { get => _email; set => _email = value; }
        public string Password
        {
            get => _password;
            protected set => this._password = PasswordHasher.HashPassword(value);
        }
        public void SetPassword(string password)
        {
            this._password = PasswordHasher.HashPassword(password);
        }

        /// <summary>
        /// Eğer verilen Şifre ile Hashlenmiş şifre doğru ise  "True" döner.
        /// </summary>
        /// <param name="password">Dışarıdan gelen açık şifre</param>
        /// <returns>Boolean</returns>
        public bool VerifyPassword(string password)
        {
            return PasswordHasher.VerifyHashedPassword(this._password, password);
        }
        public User()
        {

        }
        public User(CreateUserRequest request)
        {
            this._fullName = request.FullName;
            this._email = request.Email;
            this._password = PasswordHasher.HashPassword(request.Password);
        }
    }
}
