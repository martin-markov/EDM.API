using AutoMapper;
using EDM.Data;
using EDM.Data.Models;
using EDM.Models;
using EDM.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EDM.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> userRepo;
        private readonly IMapper mapper;
        public UserService(IGenericRepository<User> userRepo, IMapper mapper)
        {
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        public ICollection<UserDTO> GetAll()
        {
            var usersDb = this.userRepo.Query().ToList();
            return this.mapper.Map<List<UserDTO>>(usersDb);
        }

        public UserDTO GetById(int userId)
        {
            var userDb = this.userRepo.Query(x => x.UserId == userId).FirstOrDefault();
            return this.mapper.Map<UserDTO>(userDb);
        }

        public UserDTO GetByGuid(Guid userGuid)
        {
            var userDb = this.userRepo.Query(x => x.UserIdentifier == userGuid).FirstOrDefault();
            return this.mapper.Map<UserDTO>(userDb);
        }
        public UserDTO Create(UserDTO user)
        {
            var userDb = this.mapper.Map<User>(user);
            userDb.CreatedDate = DateTime.UtcNow;
            userDb.CreatedBy = 1;
            this.userRepo.Add(userDb);
            this.userRepo.SaveChanges();

            return this.mapper.Map<UserDTO>(userDb);
        }

        public UserDTO Update(int userId, UserDTO user)
        {
            var dbUser = this.userRepo.Query(x => x.UserId == userId).First();
            dbUser.UserName = user.UserName;
            dbUser.Email = user.Email;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Phone = user.Phone;
            dbUser.LastModifiedDate = DateTime.UtcNow;
            this.userRepo.SaveChanges();

            return this.mapper.Map<UserDTO>(dbUser);
        }

        public void Delete(int userId)
        {
            this.userRepo.Delete(userId);
            this.userRepo.SaveChanges();
        }

        public UserDTO GetByUsernameAndPassword(string username, string password)
        {
            var userDb = this.userRepo.Query(x => x.UserName.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
            return this.mapper.Map<UserDTO>(userDb);
        }

        public ValidationResult Validate(UserDTO user)
        {
            var result = new ValidationResult();
            if (user is null)
                throw new InvalidOperationException("User Model cannot be null");

            if (string.IsNullOrEmpty(user.UserName))
            {
                result.Errors.Add("User Name cannot be empty");
            }
            if (string.IsNullOrEmpty(user.FirstName))
            {
                result.Errors.Add("First Name cannot be empty");
            }
            if (string.IsNullOrEmpty(user.LastName))
            {
                result.Errors.Add("Last Name cannot be empty");
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                result.Errors.Add("Email cannot be empty");
            }
            if (!IsValidEmail(user.Email))
            {
                result.Errors.Add("Email is not valid");
            }
            if (!IsEmailUnique(user.Email))
            {
                result.Errors.Add("Email is not unique");
            }
            if (!IsUserNameUnique(user.UserName))
            {
                result.Errors.Add("User name is not unique");
            }
            
            return result;
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private bool IsEmailUnique(string email)
        {
            return this.userRepo.Query(x => x.Email.ToLower() == email.ToLower()).FirstOrDefault() is null;
        }

        private bool IsUserNameUnique(string username)
        {
            return this.userRepo.Query(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefault() is null;
        }
    }
}