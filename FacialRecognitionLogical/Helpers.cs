using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacialRecognitionLogical
{
    public static class Helpers
    {
        public static string GenerateTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMdd-hhmmssff");
        }
    }
}
