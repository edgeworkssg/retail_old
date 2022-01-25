using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using PowerPOS;

namespace PowerWeb.Appointment
{
	public class UserImage : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			using (Image image = GetImage(context.Request.QueryString["ID"]))
			{
				context.Response.ContentType = "image/jpeg";
				image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
			}
		}

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		public Image GetImage(string id)
		{
			var user = new UserMst(id);
			return user.Image;
		}
	}
}
