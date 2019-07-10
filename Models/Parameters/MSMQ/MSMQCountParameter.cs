using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_exam_backend_msmq_2019.Models.Parameters.MSMQ
{
    public class MSMQCountParameter
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "ต้องมีเลขกำกับ(GUID) ที่ออกโดยระบบ")]
        public string guid { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "ต้องการรหัสผ่าน(PasswordCode) ที่ออกโดยระบบ")]
        public string passwordCode { get; set; } = string.Empty;
    }
}