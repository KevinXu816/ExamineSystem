using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Caching;

namespace ExamineSystem.action.inner
{
    public abstract class ATask : ITask
    {
        private bool _isEscape = true;
        private HttpContext _context = null;
        private bool _isOther = false;

        protected ATask(bool isEscape)
        {
            this._isEscape = isEscape;
        }

        protected ATask()
            : this(true)
        {
            // do nothing
        }

        protected abstract ActionResult DoTask(string data);

        #region implement ITask interface
        HttpContext ITask.CurrentContext
        {
            set { this._context = value; }
        }
        bool ITask.IsOtherRequest
        {
            set { this._isOther = value; }
        }
        bool ITask.IsEscape
        {
            get { return this._isEscape; }
        }
        ActionResult ITask.DoTask(string data)
        {
            return this.DoTask(data);
        }
        #endregion
        
        #region Common Property
        protected bool IsOtherRequest
        {
            get
            {
                return this._isOther;
            }
        }

        protected HttpRequest Request
        {
            get
            {
                return this.Context.Request;
            }
        }

        protected HttpResponse Response
        {
            get
            {
                return this.Context.Response;
            }
        }

        protected HttpSessionState Session
        {
            get
            {
                return this.Context.Session;
            }
        }

        protected Cache Cache
        {
            get
            {
                return this.Context.Cache;
            }
        }

        protected HttpServerUtility Server
        {
            get
            {
                return this.Context.Server;
            }
        }

        protected HttpApplicationState Application
        {
            get
            {
                return this.Context.Application;
            }
        }

        protected internal HttpContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current;
                }
                return _context;
            }
        }
        #endregion
    }
}