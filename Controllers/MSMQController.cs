using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Web.Mvc;
using System.Configuration;
using System.Collections.Specialized;
using e_exam_backend_msmq_2019.Models.Parameters.MSMQ;
using Newtonsoft.Json;
using e_exam_backend_msmq_2019.Models.Responses.MSMQ.Put;
using e_exam_backend_msmq_2019.Models.Responses.MSMQ.Count;
using e_exam_backend_msmq_2019.Extensions;
using e_exam_backend_msmq_2019.Models.Responses.MSMQ.Purge;
using e_exam_backend_msmq_2019.Models.Responses.Defaults;
using NLog;

namespace e_exam_backend_msmq_2019.Controllers
{
    public class MSMQController : Controller
    {
        private Logger log = LogManager.GetLogger("MSMQ");

        private NameValueCollection configuration { get; set; }

        private BaseResponse responseModel { get; set; } = new BaseResponse();

        public MSMQController()
        {
            configuration = ConfigurationManager.AppSettings;
        }

        /// <summary>
        /// 1. put
        /// เมท็อดสำหรับส่งข้อมูลเข้า msmq
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("put")]
        public ActionResult put(MSMQPutParameter param)
        {
            MSMQPutResponse data = new MSMQPutResponse();
            //=>ข้อความตอบกลับเมื่อคอมไพล์มีปัญหา
            string complierMessage = string.Empty;

            try
            {
                //=>ตรวจสอบพารามิเตอร์
                if (ModelState.IsValid)
                {
                    //=>เรียกคอนฟิค
                    var msmqName = configuration["mq:Name"];
                    int timeout = int.Parse(configuration["mq:Timeout"]);

                    //=>จัดรูปแบบข้อสอบให้อยู่ในรูปแบบเจสัน
                    var json = JsonConvert.SerializeObject(param, Formatting.None);

                    //https://blogs.msdn.microsoft.com/biztalknotes/2013/12/29/how-to-send-and-receive-messages-in-msmq-using-cvb/
                    //https://docs.microsoft.com/en-us/dotnet/api/system.messaging.messagequeue?view=netframework-4.8

                    //=>สร้างอินสแตนซ์ MessagingQueue
                    using (MessageQueue mq = new MessageQueue(msmqName))
                    {
                        //=>จัดรูปแบบให้รองรับข้อความ
                        mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                        //=>จัดเก็บข้อความ
                        Message msg = new Message()
                        {
                            Label = $"{param.guid} examinated {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}",
                            Body = json
                        };

                        //=>ส่งเข้า Messaging queue
                        mq.Send(msg);

                        data = new MSMQPutResponse()
                        {
                            msgId = msg.Id,
                            name = mq.MQName(),
                            message = $"Put msg label {msg.Label} Ok."
                        };


                        mq.Close();
                        mq.Dispose();
                    }

                    //=>ข้อความแสดงเมื่อทำงานสำเร็จ
                    responseModel.response = new MSMQPutOkResponse()
                    {
                        data = data,
                    };
                }
                else
                {
                    //=>ข้อความแสดงข้อผิดพลาดเมื่อ พารามิเตอร์ไม่สมบูรณ์
                    responseModel.response = new MSMQPutBadRequestResponse()
                    {
                        complierMessage = JsonConvert.SerializeObject(ModelState.Values, Formatting.None),
                    };
                }
            }
            catch (Exception e)
            {
                complierMessage = e.ToString();

                //=>เช็ทข้อผิดพลาดเมื่อเซิร์ฟเวอร์ทำงานผิดพลาด
                responseModel.response = new MSMQPutServerErrorResponse()
                {
                    complierMessage = complierMessage
                };
            }

            log.Trace(JsonConvert.SerializeObject(responseModel));

            return Json(responseModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 2. count
        /// เมท็อดนับจำนวน messaging queue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("count")]
        public JsonResult count(MSMQCountParameter param)
        {
            MSMQCountResponse data = new MSMQCountResponse();
            string complierMessage = string.Empty;

            //https://dejanstojanovic.net/aspnet/2018/january/get-number-of-messages-in-msmq-with-c/
            //https://stackoverflow.com/questions/3869022/is-there-a-way-to-check-how-many-messages-are-in-a-msmq-queue
            //https://blogs.msdn.microsoft.com/mab/2012/01/05/checking-msmq-message-queue-count-with-c/

            try
            {
                //=>ตรวจสอบพารามิเตอร์
                if (ModelState.IsValid)
                {
                    //=>เรียกคอนฟิค
                    string msmqName = configuration["mq:Name"];
                    string timeout = configuration["mq:Timeout"];

                    //=>ประกาศอินสแตนซ์ Messaging queue
                    using (MessageQueue mq = new MessageQueue(msmqName))
                    {
                        data = new MSMQCountResponse()
                        {
                            name = mq.MQName(),
                            count = mq.Count(),
                        };

                        //=>ปิด Messaging queue
                        mq.Close();
                        mq.Dispose();
                    }

                    //=>ข้อความแสดงเมื่อสำเร็จ
                    responseModel.response = new MSMQCountOkResponse()
                    {
                        //=>ประกาศตัวแปร data เก็บผลลัพธ์
                        data = data,
                    };
                }
                else
                {
                    //=>ข้อความแสดงข้อผิดพลาดเมื่อ พารามิเตอร์ไม่สมบูรณ์
                    complierMessage = JsonConvert.SerializeObject(ModelState.Values, Formatting.None);

                    responseModel.response = new MSMQCountBadRequestResponse()
                    {
                        complierMessage = complierMessage
                    };
                }
            }
            catch (Exception e)
            {
                complierMessage = e.ToString();

                //=>เช็ทข้อผิดพลาดเมื่อเซิร์ฟเวอร์ทำงานผิดพลาด
                responseModel.response = new MSMQCountServerErrorResponse()
                {
                    complierMessage = complierMessage
                };
            }

            log.Trace(JsonConvert.SerializeObject(responseModel));

            return Json(responseModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 3. purge
        /// เมท็อดล้าง messaging queue
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("purge")]
        public ActionResult purge(MSMQPurgeParameter param)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.messaging.messagequeue.purge?view=netframework-4.8
            //https://stackoverflow.com/questions/1804269/clear-message-queue-in-c-sharp
            //https://csharp.hotexamples.com/examples/System.Messaging/MessageQueue/Purge/php-messagequeue-purge-method-examples.html
            MSMQPurgeResponse data = new MSMQPurgeResponse();
            string complierMessage = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    //=>เช็ทคอนฟิค
                    string msmqName = configuration["mq:Name"];
                    int timeout = int.Parse(configuration["mq:Timeout"]);

                    //=>ประกาศอินสแตนซ์ Messaging queue
                    using (MessageQueue mq = new MessageQueue(msmqName))
                    {
                        //=>เช็ทผลลัพธ์การลบ
                        data = new MSMQPurgeResponse()
                        {
                            count = mq.Count(),
                            name = mq.MQName()
                        };

                        //=>เคลียร์ค่า Messaging queue
                        mq.Purge();

                        mq.Close();
                        mq.Dispose();
                    }

                    //=>ข้อความแสดงเมื่อสำเร็จ
                    responseModel.response = new MSMQPurgeOkResponse
                    {
                        data = data
                    };
                }
                else
                {
                    complierMessage = JsonConvert.SerializeObject(ModelState.Values, Formatting.None);

                    //=>ข้อความแสดงข้อผิดพลาดเมื่อ พารามิเตอร์ไม่สมบูรณ์
                    responseModel.response = new MSMQPurgeBadRequestResponse
                    {
                        complierMessage = complierMessage
                    };
                }
            }
            catch (Exception e)
            {
                complierMessage = e.ToString();

                //=>เช็ทข้อผิดพลาดเมื่อเซิร์ฟเวอร์ทำงานผิดพลาด
                responseModel.response = new MSMQCountServerErrorResponse
                {
                    complierMessage = complierMessage
                };
            }

            log.Trace(JsonConvert.SerializeObject(responseModel));

            return Json(responseModel.response, JsonRequestBehavior.AllowGet);
        }
    }
}
