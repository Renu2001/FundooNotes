using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL _labelBL;
        public ResponseModel mod;
        public LabelController(ILabelBL labelBL)
        {
            _labelBL = labelBL;
        }

        [HttpGet("GetAllLabels")]
        public IActionResult GetAllLabels()
        {
            try
            {
                var result = _labelBL.GetAllLabels();
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Labels",
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

        [HttpPost("AddLabel")]
        public IActionResult AddLabel(LabelModel model, int id)
        {
            try
            {
                var result = _labelBL.AddLabel(model, id);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Label Created",
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
            return StatusCode(201, mod);
        }

        [HttpDelete("DeleteLabel")]
        public IActionResult DeleteLabel(int id)
        {
            try
            {
                var result = _labelBL.DeleteLabel(id);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Label Deleted",
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
        [HttpPut("UpdateLabel")]
        public IActionResult UpdateLabel(int id, LabelModel model)
        {
            try
            {
                var result = _labelBL.UpdateLabel(id, model);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Label Updated",
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
