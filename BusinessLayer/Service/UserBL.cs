using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;

        public UserBL(IUserRL userRL)
        {
            this._userRL = userRL;
        }

        public UserEntity RegisterUser(UserModel model)
        {
            try
            {
                return _userRL.RegisterUser(model);
            }
            catch
            {
                throw;
            }
            
        }

        public void ResetPassword(string email, string newPassword)
        {
            try
            {
               _userRL.ResetPassword(email, newPassword);
            }
            catch
            {
                throw;
            }
        }
    }
}
