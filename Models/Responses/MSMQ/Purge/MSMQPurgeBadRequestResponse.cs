﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace e_exam_backend_msmq_2019.Models.Responses.MSMQ.Purge
{
    public class MSMQPurgeBadRequestResponse
    {
        public string message { get; set; } = "ไม่สามารถล้างข้อมูล Messaging queue ได้เนื่องจากพารามิเตอร์ไม่เหมาะสม";

        public string complierMessage { get; set; } = string.Empty;

        public DateTime now { get; set; } = DateTime.Now;

        public bool success { get; set; } = false;

        public HttpStatusCode statusCode { get; set; } = HttpStatusCode.BadRequest;

        public object data { get; set; } = new object();
    }
}