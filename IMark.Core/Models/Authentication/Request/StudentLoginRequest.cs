using System;
namespace IMark.Core.Models.Authentication.Request
{
    public class StudentLoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
