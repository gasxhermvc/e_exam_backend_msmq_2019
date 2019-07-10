using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_exam_backend_msmq_2019.Models.Responses.MSMQ.Count
{
    public class MSMQCountResponse
    {
        public string name { get; set; }

        public long count { get; set; }
    }
}