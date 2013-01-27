using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EntityModel;
using ExamineSystem.utility;
using System.IO;
using ExamineSystem.action.inner;
using System.Text;

namespace ExamineSystem
{
    public partial class pageupfileback : System.Web.UI.Page
    {

        private UserLevelType[] _operationUserLevels = null;
        private bool _isValidateLogin = false;

        public pageupfileback()
        {
            this._isValidateLogin = true;
            this._operationUserLevels = new UserLevelType[] { UserLevelType.Admin, UserLevelType.Teacher };
        }

        protected override void OnInit(EventArgs e)
        {
            if (this._isValidateLogin)
            {
                SessionManager.UserExp = "";
                UserEntity entity = SessionManager.User;
                if (entity == null)
                {
                    UploadResult result = new UploadResult();
                    result.IsSuccess = false;
                    result.Message = "对不起<br>您的登陆已经过期请您重新登陆";
                    this.RenderUploadResult(result);
                }
                if (this._operationUserLevels != null)
                {
                    bool isOperation = false;
                    foreach (UserLevelType userLevel in this._operationUserLevels)
                    {
                        if (entity.UserLevel == userLevel)
                        {
                            isOperation = true;
                            break;
                        }
                    }
                    if (!isOperation)
                    {
                        Response.ClearContent();
                        Response.Write(@"
                            <html>
                                <head></head>
                                <body bgcolor='#cad7f7'>
                                    对不起！您没有此功能的操作权限
                                </body>
                            </html>
                         ");
                        Response.End();
                        return;
                    }
                }
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {

            UploadResult result = new UploadResult();
            string typeId = (Request.Form["typeid"] ?? string.Empty).Trim();
            result.CurrentTypeId = typeId;
            string formPath = "music/";
            int maxFileSize = SettingConfigUtility.UploadFileMaxSize * 1000;
            if (Request.Files.Count > 0)
            {
                HttpPostedFile file = Request.Files[0];
                if (file == null)
                {
                    result.IsSuccess = false;
                    result.Message = "没有检测到<br>系统上传的音频文件";
                    this.RenderUploadResult(result);
                }
                else
                {
                    string fileName = file.FileName;
                    fileName = Path.GetFileName(fileName);
                    int fileSize = file.ContentLength;
                    if (fileSize > 0)
                    {
                        if (fileSize > maxFileSize)
                        {
                            result.IsSuccess = false;
                            result.FileName = fileName;
                            result.Message = string.Format("因音频大于{0}KB", SettingConfigUtility.UploadFileMaxSize);
                            this.RenderUploadResult(result);
                        }
                        try
                        {
                            file.SaveAs(Server.MapPath(formPath + fileName));
                        }
                        catch (Exception)
                        {
                            result.IsSuccess = false;
                            result.FileName = fileName;
                            result.Message = "因上传出现未知错误";
                            this.RenderUploadResult(result);
                        }

                        result.IsSuccess = true;
                        result.FileName = fileName;
                        this.RenderUploadResult(result);
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.FileName = fileName;
                        result.Message = "因音频太小";
                        this.RenderUploadResult(result);
                    }
                }
            }

            base.OnLoad(e);
        }

        private void RenderUploadResult(UploadResult result)
        {
            bool isSuccess = false;
            string message = "系统运行出现未知严重错误";
            string attached = string.Empty;
            if (result != null)
            {
                string fileName = (result.FileName ?? string.Empty).Trim();
                string mess = (result.Message ?? string.Empty).Trim();
                isSuccess = result.IsSuccess;
                if (isSuccess)
                {
                    attached = string.Format("parent.{0}.upfileFuncEnd('{1}');", result.CurrentTypeId, fileName);
                    message = string.IsNullOrEmpty(mess) ? "" : mess;
                }
                else
                {
                    message = (string.IsNullOrEmpty(mess) ? message : mess);
                }
                if (!string.IsNullOrEmpty(fileName))
                    message = string.Format("{0}{1}系统上传音频({2})", string.IsNullOrEmpty(message) ? "" : "<br>", message, fileName);
            }
            message = string.Format("{0}<br>{1}", message, (isSuccess) ? "操作成功" : "操作失败");
            StringBuilder output = new StringBuilder();
            output.AppendFormat(@"
                      <html>
                        <head></head>
                        <body bgcolor='#cad7f7'>
                        <script>
                            parent.parent.left.Lock = false;
                            parent.parent.parent.warn_Win('{0}',1);
                            {1}
                            location.href = 'pageupfile.aspx';
                        </script>
                        </body>
                      </html>
                ", message, attached);
            Response.ClearContent();
            Response.Write(output.ToString());
            Response.End();
        }


        private class UploadResult
        {
            public string CurrentTypeId
            {
                get;
                set;
            }

            public string FileName
            {
                get;
                set;
            }

            public bool IsSuccess
            {
                get;
                set;
            }

            public string Message
            {
                get;
                set;
            }
        }

    }
}