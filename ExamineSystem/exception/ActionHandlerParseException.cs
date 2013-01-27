using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamineSystem.exception
{
    public class ActionHandlerParseException : System.Exception
    {
        private string _showMessage = string.Empty;

        public ActionHandlerParseException()
            : base()
        {
            // do nothing
        }

        public ActionHandlerParseException(string showMessage)
            : base()
        {
            this._showMessage = showMessage;
        }

        public ActionHandlerParseException(Exception e)
            : base(e.Message, e)
        {
            // do nothing
        }

        public ActionHandlerParseException(string showMessage, Exception e)
            : this(e)
        {
            this._showMessage = showMessage;
        }

        public ActionHandlerParseException(string showMessage, string innerMessage)
            : base(innerMessage)
        {
            this._showMessage = showMessage;
        }

        public string ShowMessage
        {
            get { return this._showMessage; }
        }
    }
}