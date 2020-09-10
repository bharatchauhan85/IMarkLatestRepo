using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Request
{
  public  class SortPageModelRequest
    {
        public string title { set; get; }
        public string sortKey { set; get; }
        public bool Condition { set; get; }
    }
}
