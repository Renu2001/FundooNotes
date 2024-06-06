using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace FundooNotes.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        public ResponseModel mod;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUser(UserModel model)
        {
            
            try
            {
                var result = userBL.RegisterUser(model);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Data Added Successfully",
                        Data = result
                    };
                }
            }
            catch(CustomizeException ex) 
            {
                mod = new ResponseModel()
                {
                    Success = "false",
                    Message = ex.Message
                };
                return StatusCode(400, mod);
            }

            return StatusCode(200, mod);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginML login)
        {
            try
            {
                var result = userBL.Login(login);
                if(result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Login Successfully",
                        Data = result
                    };
                }
            }
            catch (CustomizeException ex)
            {
                mod = new ResponseModel()
                {
                    Success = "false",
                    Message = ex.Message
                };
                return StatusCode(400, mod);
            }
            return StatusCode(200, mod);

        }

    }
}
