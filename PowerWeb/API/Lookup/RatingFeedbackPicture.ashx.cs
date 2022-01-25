using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using PowerPOS;

namespace PowerWeb.API.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class RatingFeedbackPicture : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request.QueryString["id"]; //get the querystring value that was pass on the ImageURL (see GridView MarkUp in Page1.aspx)

            if (id != null)
            {

                MemoryStream memoryStream = new MemoryStream();
                RatingFeedback rs = new RatingFeedback(id);

                //Get Image Data
                byte[] file = (byte[])rs.SelectionImage;

                memoryStream.Write(file, 0, file.Length);
                context.Response.Buffer = true;
                context.Response.BinaryWrite(file);
                memoryStream.Dispose();
            }
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
