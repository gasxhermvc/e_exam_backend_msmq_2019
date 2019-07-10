using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace e_exam_backend_msmq_2019.Models.Responses.MSMQ.Purge
{
    public class MSMQPurgeOkResponse
    {
        public string message { get; set; } = "ล้างข้อมูล Messaging queue สำเร็จ";

        public string complierMessage { get; set; } = string.Empty;

        public DateTime now { get; set; } = DateTime.Now;

        public bool success { get; set; } = true;

        public HttpStatusCode statusCode { get; set; } = HttpStatusCode.OK;

        public object data { get; set; } = new object();
    }
}