using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Application.Models.Email
{
    public class EmailMessageData
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
