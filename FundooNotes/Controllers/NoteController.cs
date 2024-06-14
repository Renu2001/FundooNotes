﻿using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Interface;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBL _noteBL;
        public ResponseModel mod;
        public NoteController(INoteBL noteBL)
        {
            _noteBL = noteBL;
        }

        //[Authorize]
        [HttpPost("CreateNote")]
        public IActionResult CreateNote(NoteModel model)
        {
            try { 
            var result = _noteBL.CreateNote(model);
            if (result != null)
            {
                mod = new ResponseModel()
                {
                    Success = "true",
                    Message = "Note Created",
                    //Data = result
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
                return StatusCode(404, mod);

            }
            return StatusCode(201, mod);
        }

        //[Authorize]
        [HttpGet("GetAll")]
        public IActionResult GetAllNote() 
        {
            try
            {
                var result = _noteBL.GetAllNote();
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

        //[Authorize]
        [HttpGet("{id:int}")]
        public IActionResult GetNoteById(int id)
        {
            try
            {
                var result = _noteBL.GetNoteById(id);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Note By Id",
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

        //[Authorize] 
        [HttpDelete("DeleteByid")]
        public IActionResult DeleteNoteById(int id)
        {
            try
            {
                var result = _noteBL.DeleteNoteById(id);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Deleted",
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

        //[Authorize]
        [HttpPut("UpdateByid")]
        public IActionResult UpdateById(int id,NoteModel model)
        {
            try
            {
                var result = _noteBL.UpdateById(id, model);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Updated",
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

        [HttpPost("Archive")]
        public IActionResult ArchiveById(int id)
        {
            try
            {
                var result = _noteBL.ArchieveById(id);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Archived",
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

        [HttpPost("Delete")]
        public IActionResult TrashById(int id)
        {
            try
            {
                var result = _noteBL.TrashById(id);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "Deleted",
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