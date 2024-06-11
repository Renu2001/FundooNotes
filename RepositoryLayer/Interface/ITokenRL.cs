using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ITokenRL
    {
        public string AuthenticateUser(LoginML login);
        public string GenerateToken(UserEntity user);
    }
}
