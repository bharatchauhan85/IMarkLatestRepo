using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Core.Models.Authentication.Request
{
    public class StudentVerficationCodeRequestModel
    {
        public string email { get; set; }
        public string verification_code { get; set; }
    }
}
