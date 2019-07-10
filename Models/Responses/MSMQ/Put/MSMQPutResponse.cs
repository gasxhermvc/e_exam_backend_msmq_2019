using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_exam_backend_msmq_2019.Models.Responses.MSMQ.Put
{
    public class MSMQPutResponse
    {
        public string msgId { get; set; } = string.Empty;

        public string name { get; set; } = string.Empty;

        public string message { get; set; } = string.Empty;
    }
}