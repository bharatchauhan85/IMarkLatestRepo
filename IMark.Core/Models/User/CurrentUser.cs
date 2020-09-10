using System;
using IMark.Core.Helpers;
using IMark.Core.Models.Authentication;
using IMark.Core.Models.Common;

namespace IMark.Core.Models.User
{
    public class CurrentUser : BindableBase
    {

        private StudentDetail _currentStudentDetails;
        public StudentDetail CurrentStudentDetails { get => _currentStudentDetails; set { SetProperty(ref _currentStudentDetails, value); } }

        public CurrentUser()
        {

        }
        public CurrentUser(StudentDetail studnetDetail)
        {
            CurrentStudentDetails = studnetDetail;
        }
    }
}
