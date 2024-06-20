using BusinessLayer.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;

namespace FundooNotes.Controllers
{
    [EnableCors("_myspecificPolicy2")]
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
        public async Task<IActionResult> GetAllLabels()
        {
            try
            {
                var result = await _labelBL.GetAllLabels();
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

        [DisableCors]
        [HttpPost("AddLabel")]
        public async Task<IActionResult> AddLabel(LabelModel model)
        {
            try
            {
                var result = await _labelBL.AddLabel(model);
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
                return StatusCode(400, mod);

            }
            return StatusCode(201, mod);
        }

        [HttpDelete("DeleteLabel")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            try
            {
                var result = await _labelBL.DeleteLabel(id);
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
        public async Task<IActionResult> UpdateLabel(int id, LabelModel model)
        {
            try
            {
                var result = await _labelBL.UpdateLabel(id, model);
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
