using System;
using System.Collections.Generic;
using System.Web;

namespace ExamineSystem.action.inner
{
    public class ActionResult
    {
        public bool IsSuccess
        {
            get;
            set;
        }

        public string TipMessage
        {
            get;
            set;
        }

        public string ResponseData
        {
            get;
            set;
        }
    }
}