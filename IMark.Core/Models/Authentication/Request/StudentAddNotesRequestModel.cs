using IMark.Core.Models.Notes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IMark.Core.Models.Authentication.Request
{
    public class StudentAddNotesRequestModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public string courseID { get; set; }
    }
}
