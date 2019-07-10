using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Web;

namespace e_exam_backend_msmq_2019.Extensions
{
    public static class MessageQueueExtension
    {
        public static string MQName(this MessageQueue messageQueue)
        {
            return messageQueue.QueueName.Split('\\').LastOrDefault();
        }

        public static long Count(this MessageQueue messageQueue)
        {
            var enumerator = messageQueue.GetMessageEnumerator2();
            long counter = 0;
            while (enumerator.MoveNext())
            {
                counter++;
            }

            return counter;
        }
    }
}