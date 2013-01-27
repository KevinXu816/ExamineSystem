using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using log4net;
using ExamineSystem.utility;
using System.IO;
using System.Text;
using ExamineSystem.exception;

namespace ExamineSystem.action.inner
{
    public sealed class ActionHandler : IHttpHandler, IRequiresSessionState
    {
        private static ILog logger = TrackLogManager.GetLogger(typeof(ActionHandler));

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.AddHeader("Cache-Control", "no-cache");
            context.Response.Expires = 0;
            context.Response.Charset = "utf-8";
            context.Response.ContentType = "text/plain";

            ActionResult result = null;
            bool isOther = false;
            try
            {
                string other = (context.Request.QueryString["os"] as string ?? string.Empty).Trim();
                isOther = ("1".Equals(other));
                string action = (context.Request.QueryString["action"] as string ?? string.Empty).Trim();
                if (!string.IsNullOrEmpty(action))
                {
                    string typeStr = string.Format("ExamineSystem.action.{0}, ExamineSystem", action);
                    Type type = Type.GetType(typeStr, false, true);
                    if (type == null)
                    {
                        string error = string.Format("Missing Action Class With FullName {0}.", typeStr);
                        throw new ArgumentException(error, "action");
                    }
                    IAction instance = Activator.CreateInstance(type, true) as IAction;
                    if (instance == null)
                    {
                        string error = string.Format("Action Class ({0}) No Implements ExamineSystem.action.inner.IAction.", typeStr);
                        throw new ArgumentException(error, "action");
                    }
                    instance.CurrentContext = context;
                    instance.IsOtherRequest = isOther;
                    string actionData = GetOriginalData(context.Request);
                    string actionType = (context.Request.QueryString["type"] ?? string.Empty).Trim();
                    result = instance.DoAction(actionType, actionData);
                }
                else
                {
                    throw new ArgumentException("Missing Action Param.", "action");
                }
            }
            catch (ActionParseException e)
            {
                logger.Error(e.InnerException ?? e);
                result = new ActionResult();
                result.IsSuccess = false;
                result.TipMessage = e.ShowMessage;
            }
            catch (ActionHandlerParseException e)
            {
                logger.Error(e.InnerException ?? e);
                result = new ActionResult();
                result.IsSuccess = false;
                result.TipMessage = e.ShowMessage;
            }
            catch (Exception e)
            {
                logger.Error(e);
                result = new ActionResult();
                result.IsSuccess = false;
            }
            this.RenderAction(context.Response, result, isOther);
        }

        private void RenderAction(HttpResponse response, ActionResult result, bool isOther)
        {
            bool isSuccess = false;
            string message = "系统运行出现未知严重错误";
            string responseData = string.Empty;
            if (result != null)
            {
                isSuccess = result.IsSuccess;
                responseData = (result.ResponseData ?? string.Empty).Trim();
                if (!isSuccess)
                {
                    string mess = (result.TipMessage ?? string.Empty).Trim();
                    message = string.IsNullOrEmpty(mess) ? message : mess;
                }
            }
            StringBuilder output = new StringBuilder();
            output.Append(string.Format("{0} = {1};", (isOther) ? "Ssign" : "sign", (isSuccess) ? "true" : "false"));
            output.Append(responseData);
            if (!isSuccess)
            {
                if (!isOther)
                    output.Append(string.Format("BadInfo = '{0}<br>请重新操作！';", message));
            }
            response.ClearContent();
            response.Write(Escape.JsEscape(output.ToString()));
            response.End();
        }

        private string GetOriginalData(HttpRequest request)
        {
            StringBuilder data = new StringBuilder();
            try
            {
                using (Stream stream = request.InputStream)
                {
                    while (true)
                    {
                        int ch = stream.ReadByte();
                        if (ch == -1)
                            break;
                        data.Append((char)ch);
                    }
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                throw new ActionHandlerParseException("后台获取提交数据出现严重错误", e);
            }
            return data.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}