using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace ExamineSystem.action.inner
{
    public interface IAction
    {
        HttpContext CurrentContext { set; }
        bool IsOtherRequest { set; }
        bool IsLogin { get; }
        ActionResult DoAction(string actionType, string actionData);
    }
}
