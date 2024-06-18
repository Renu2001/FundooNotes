using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBL _collaboratorBL;
        public ResponseModel mod;
        public CollaboratorController(ICollaboratorBL collaboratorBL)
        {
            _collaboratorBL = collaboratorBL;
        }

        [HttpGet]
        public IActionResult GetCollaboratorById(int noteId)
        {
            try
            {
                var result = _collaboratorBL.GetCollaboratorById(noteId);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "All Notes",
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
                return StatusCode(404, mod);

            }
            return StatusCode(200, mod);
        }

        [HttpPost]
        public IActionResult AddCollaborator(CollaboratorModel model)
        {
            try
            {
                var result = _collaboratorBL.AddCollaborator(model);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "All Notes",
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
                return StatusCode(404, mod);

            }
            return StatusCode(200, mod);
        }

        [HttpDelete]
        public IActionResult DeleteCollaborator(int noteId, string email)
        {
            try
            {
                var result = _collaboratorBL.DeleteCollaborator(noteId,email);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "All Notes",
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
                return StatusCode(404, mod);

            }
            return StatusCode(200, mod);

        }
    }
}
