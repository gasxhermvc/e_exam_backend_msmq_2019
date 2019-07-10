using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace e_exam_backend_msmq_2019.Models.Responses.MSMQ.Purge
{
    public class MSMQPurgeServerErrorResponse
    {
        public string message { get; set; } = "ไม่สามารถล้างข้อมูล Messaging queue ได้เนื่องจากกระบวนการทำงานภายในมีปัญหา";

        public string complierMessage { get; set; } = string.Empty;

        public DateTime now { get; set; } = DateTime.Now;

        public bool success { get; set; } = false;

        public HttpStatusCode statusCode { get; set; } = HttpStatusCode.InternalServerError;

        public object data { get; set; } = new object();
    }
}