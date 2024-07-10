using CQRSApplication.Commands;
using CQRSApplication.Repositories;


namespace CQRSApplication.Handlers
{
    public class UpdateStudentHandler
    {
        private readonly IStudentRepository _studentRepository;

        public UpdateStudentHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<int> Handle(UpdateStudentCommand command)
        {
            var studentDetails = await _studentRepository.GetStudentByIdAsync(command.Id);
            if (studentDetails == null)
                return default;

            studentDetails.StudentName = command.StudentName;
            studentDetails.StudentEmail = command.StudentEmail;
            studentDetails.StudentAddress = command.StudentAddress;
            studentDetails.StudentAge = command.StudentAge;

            return await _studentRepository.UpdateStudentAsync(studentDetails);
        }
    }
}
