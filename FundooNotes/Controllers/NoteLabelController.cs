using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteLabelController : ControllerBase
    {
        private readonly INoteLabelBL _notelabelBL;
        public ResponseModel mod;
        public NoteLabelController(INoteLabelBL notelabelBL)
        {
            _notelabelBL = notelabelBL;
        }

        
        [HttpGet("bylabel")]
        public IActionResult GetAllNotesFromLabel(int labelId)
        {
            try
            {
                var result = _notelabelBL.GetAllNotesFromLabel(labelId);
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
    
        [HttpGet("alllabels")]
        public IActionResult GetAllLabelsFromNotes()
        {
        try
        {
            var result = _notelabelBL.GetAllLabelsFromNotes();
            if (result != null)
            {
                mod = new ResponseModel()
                {
                    Success = "true",
                    Message = "All Labels",
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
        public IActionResult AddLabelsToNotes(int labelId, int noteId)
        {
            try
            {
                var result = _notelabelBL.AddLabelsToNotes(labelId,noteId);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Note added",
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
        public IActionResult RemoveLabelsFromNotes(int labelId, int noteId)
        {
            try
            {
                var result = _notelabelBL.RemoveLabelsFromNotes(labelId,noteId);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Note Removed",
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
