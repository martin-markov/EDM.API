using EDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDM.Services.Interfaces
{
    public interface IUserService
    {
        ICollection<UserDTO> GetAll();
        UserDTO GetById(int id);

        UserDTO GetByGuid(Guid userGuid);

        UserDTO Create(UserDTO user);

        UserDTO Update(int userId, UserDTO user);

        void Delete(int userId);

        UserDTO GetByUsernameAndPassword(string username, string password);
        ValidationResult Validate(UserDTO user);
    }
}
