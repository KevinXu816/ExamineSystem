using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EntityModel;
using ExamineSystem.utility;

namespace ExamineSystem
{
    public partial class pageexit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserEntity entity = SessionManager.User;
            if (entity != null)
            {
                entity.IsLogin = false;
                entity.Save();
            }
            SessionManager.RemoveAll();
            Response.ClearContent();
            Response.Write(@"
                <html>
                    <head></head>
                    <body bgcolor='#cad7f7' scroll='no'>
                        <script>alert('您已经安全退出了校园网络在线考试系统！');location.href='login.aspx';</script>
                    </body>
                </html>
            ");
            Response.End();
        }
    }
}
