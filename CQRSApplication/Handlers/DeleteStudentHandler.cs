using CQRSApplication.Commands;
using CQRSApplication.Repositories;


namespace CQRSApplication.Handlers
{
    public class DeleteStudentHandler
    {
        private readonly IStudentRepository _studentRepository;

        public DeleteStudentHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<int> Handle(DeleteStudentCommand command)
        {
            var studentDetails = await _studentRepository.GetStudentByIdAsync(command.Id);
            if (studentDetails == null)
                return default;

            return await _studentRepository.DeleteStudentAsync(studentDetails.Id);
        }
    }
}
