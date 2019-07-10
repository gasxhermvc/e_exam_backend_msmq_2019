using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e_exam_backend_msmq_2019.Controllers
{
    public class DefaultController : Controller
    {
        private Logger log = LogManager.GetLogger("Default");

        // GET: Default
        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            log.Debug("OK");

            return Json(new
            {
                sucess = true,
            }, JsonRequestBehavior.AllowGet);
        }
    }
}