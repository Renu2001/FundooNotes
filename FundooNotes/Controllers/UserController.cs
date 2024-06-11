using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FundooNotes.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        private readonly ITokenBL tokenBL;
        public ResponseModel mod;

        public UserController(IUserBL userBL, ITokenBL tokenBL)
        {
            this.userBL = userBL;
            this.tokenBL = tokenBL;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = userBL.GetUsers();
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Users List",
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginML login)
        { 
            try
            {
                var token = tokenBL.AuthenticateUser(login);
                if (token != null)
                {
                   
                    mod = new ResponseModel()
                        {
                            Success = "true",
                            Message = "Login Successfully",
                            Data = token
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
