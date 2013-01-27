using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamineSystem.action.inner
{
    public interface ITask
    {
        HttpContext CurrentContext { set; }
        bool IsOtherRequest { set; }
        bool IsEscape { get; }
        ActionResult DoTask(string data);
    }
}