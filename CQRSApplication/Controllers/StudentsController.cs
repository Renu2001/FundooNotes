using CQRSApplication.Commands;
using CQRSApplication.Handlers;
using CQRSApplication.Models;
using CQRSApplication.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CQRSApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly CreateStudentHandler _createStudentHandler;
        private readonly UpdateStudentHandler _updateStudentHandler;
        private readonly DeleteStudentHandler _deleteStudentHandler;
        private readonly GetStudentByIdHandler _getStudentByIdHandler;
        private readonly GetStudentListHandler _getStudentListHandler;


        public StudentsController(CreateStudentHandler createStudentHandler, UpdateStudentHandler updateStudentHandler, DeleteStudentHandler deleteStudentHandler, GetStudentByIdHandler getStudentByIdHandler, GetStudentListHandler getStudentListHandler)
        {
            _createStudentHandler = createStudentHandler;
            _updateStudentHandler = updateStudentHandler;
            _deleteStudentHandler = deleteStudentHandler;
            _getStudentByIdHandler = getStudentByIdHandler;
            _getStudentListHandler = getStudentListHandler;
        }

        [HttpGet]
        public async Task<List<StudentDetails>> GetStudentListAsync()
        {
            var studentDetails = await _getStudentListHandler.Handle(new GetStudentListQuery());

            return studentDetails;
        }

        [HttpGet("studentId")]
        public async Task<StudentDetails> GetStudentByIdAsync(int studentId)
        {
            var studentDetails = await _getStudentByIdHandler.Handle(new GetStudentByIdQuery() { Id = studentId });

            return studentDetails;
        }

        [HttpPost]
        public async Task<StudentDetails> AddStudentAsync(StudentDetails studentDetails)
        {
            var studentDetail = await _createStudentHandler.Handle(new CreateStudentCommand(
                studentDetails.StudentName,
                studentDetails.StudentEmail,
                studentDetails.StudentAddress,
                studentDetails.StudentAge));
            return studentDetail;
        }

        [HttpPut]
        public async Task<int> UpdateStudentAsync(StudentDetails studentDetails)
        {
            var isStudentDetailUpdated = await _updateStudentHandler.Handle(new UpdateStudentCommand(
               studentDetails.Id,
               studentDetails.StudentName,
               studentDetails.StudentEmail,
               studentDetails.StudentAddress,
               studentDetails.StudentAge));
            return isStudentDetailUpdated;
        }

        [HttpDelete]
        public async Task<int> DeleteStudentAsync(int Id)
        {
            return await _deleteStudentHandler.Handle(new DeleteStudentCommand() { Id = Id });
        }
    }
}
