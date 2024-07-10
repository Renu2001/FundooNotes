using CQRSApplication.Models;
using CQRSApplication.Queries;
using CQRSApplication.Repositories;
namespace CQRSApplication.Handlers
{
    public class GetStudentListHandler 
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentListHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<StudentDetails>> Handle(GetStudentListQuery query)
        {
            return await _studentRepository.GetStudentListAsync();
        }
    }
}
