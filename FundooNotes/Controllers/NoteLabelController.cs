using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
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

        [HttpGet("GetAllLabelsByNotes")]
        public IActionResult GetAllLabelsAndNotesFromAllNotes()
        {
            try
            {
                var result = _notelabelBL.GetAllLabelsAndNotesFromAllNotes();
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

        [HttpGet("GetNotesByLabel")]
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
    
        [HttpGet("GetLabelsByNotes")]
        public IActionResult GetAllLabelsFromNotes(int noteid)
        {
        try
        {
            var result = _notelabelBL.GetAllLabelsFromNotes(noteid);
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
        [HttpPost("AddLabelsToNotes")]
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
                return StatusCode(400, mod);

            }
            return StatusCode(200, mod);
        }
        [HttpDelete("RemoveLabelFromNote")]
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
