using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e_exam_backend_msmq_2019.Controllers
{
    public class ErrorController : Controller
    {
        private Logger log = LogManager.GetLogger("Error");

        // GET: Error
        [Route("")]
        public ActionResult Index(int statusCode)
        {
            log.Error(statusCode);

            return Redirect("http://www.google.com/");
        }
    }
}