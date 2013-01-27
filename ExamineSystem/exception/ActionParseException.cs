using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamineSystem.exception
{
    public class ActionParseException : System.Exception
    {
        private string _showMessage = string.Empty;

        public ActionParseException()
            : base()
        {
            // do nothing
        }

        public ActionParseException(string showMessage)
            : base()
        {
            this._showMessage = showMessage;
        }

        public ActionParseException(Exception e)
            : base(e.Message, e)
        {
            // do nothing
        }

        public ActionParseException(string showMessage, Exception e)
            : this(e)
        {
            this._showMessage = showMessage;
        }

        public ActionParseException(string showMessage, string innerMessage)
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