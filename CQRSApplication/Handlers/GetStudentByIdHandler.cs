using CQRSApplication.Models;
using CQRSApplication.Queries;
using CQRSApplication.Repositories;

namespace CQRSApplication.Handlers
{
    public class GetStudentByIdHandler 
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentByIdHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentDetails> Handle(GetStudentByIdQuery query)
        {
            return await _studentRepository.GetStudentByIdAsync(query.Id);
        }
    }
}
