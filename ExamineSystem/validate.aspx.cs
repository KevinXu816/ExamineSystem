using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Web;
using ExamineSystem.utility;

namespace ExamineSystem
{
    public partial class validate : System.Web.UI.Page
    {
        private void CreateCheckCodeImage(string checkCode)
        {
            checkCode = checkCode ?? string.Empty;
            if (!string.IsNullOrEmpty(checkCode))
            {
                Bitmap image = new Bitmap(80, 15);
                Graphics graphics = Graphics.FromImage(image);
                try
                {
                    Random random = new Random();
                    graphics.Clear(Color.White);
                    Font font = new Font("Fixedsys", 12f, FontStyle.Bold);
                    LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.FromArgb(random.Next(0xff), random.Next(0xff), random.Next(0xff)), Color.FromArgb(random.Next(200), random.Next(200), random.Next(200)), 1.2f, true);
                    graphics.DrawString(checkCode, font, brush, (float)-3f, (float)-2f);
                    for (int i = 0; i < 80; i++)
                    {
                        int x = random.Next(image.Width);
                        int y = random.Next(image.Height);
                        image.SetPixel(x, y, Color.FromArgb(random.Next()));
                    }
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Gif);
                    base.Response.ClearContent();
                    base.Response.ContentType = "image/Gif";
                    base.Response.BinaryWrite(stream.ToArray());
                }
                finally
                {
                    graphics.Dispose();
                    image.Dispose();
                }
            }
        }

        private string GenerateCheckCode()
        {
            string drawCode = " ";
            string valCode = "";
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                int num = random.Next();
                char ch = (char)(0x30 + ((ushort)(num % 10)));
                drawCode = drawCode + ch.ToString() + " ";
                valCode = valCode + ch.ToString();
            }
            SessionManager.ValidateCode = valCode;
            return drawCode;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.CreateCheckCodeImage(this.GenerateCheckCode());
        }

    }
}