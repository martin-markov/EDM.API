using EDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDM.Services.Interfaces
{
    public interface ITokenProvider
    {
        string GetToken(UserDTO user);
    }
}
