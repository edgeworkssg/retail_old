using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using SubSonic;
using System.IO;

namespace PowerWeb.Scaffolds
{
    public partial class CompanyScaffold : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Query qu = new Query("Company");
                qu.AddWhere(Company.Columns.Deleted, false);
                CompanyCollection col = new CompanyCollection();
                col.LoadAndCloseReader(qu.ExecuteReader());

                if (col.Count > 0)
                {
                    Company com = col[0];

                    ctrlCompanyName.Text = com.CompanyName;
                    CompanyID.Value = com.CompanyID.ToString();
                    ctrlStreetName.Text = com.Address1;
                    ctrlStreetName2.Text = com.Address2;
                    ctrlZipCode.Text = com.ZipCode;
                    ctrlCity.Text = com.City;
                    ctrlCountry.Text = com.Country;
                    ctrlMobile.Text = com.Mobile;
                    ctrlOffice.Text = com.Fax;

                    if (com.CompanyImage != null)
                    {
                        Image2.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(com.CompanyImage, 0, com.CompanyImage.Length);
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Company com;
                if (string.IsNullOrEmpty(CompanyID.Value))
                {
                    com = new Company();
                    com.CompanyID = Guid.NewGuid();
                }
                else
                {
                    com = new Company(CompanyID.Value);
                }

                com.CompanyName = ctrlCompanyName.Text;
                com.ReceiptName = ctrlReceiptName.Text;
                com.Address1 = ctrlStreetName.Text;
                com.Address2 = ctrlStreetName2.Text;
                com.ZipCode = ctrlZipCode.Text;
                com.City = ctrlCity.Text;
                com.Country = ctrlCountry.Text;
                com.Mobile = ctrlMobile.Text;
                com.Fax = ctrlOffice.Text;
                com.Deleted = false;
                if (fuItemPicture.HasFile)
                    com.CompanyImage = ImageCompressor.ResizeAndCompressImage(new MemoryStream(fuItemPicture.FileBytes));

                com.Save();

                lblResult.Text = "Company saved succesfully";

                if (com.CompanyImage != null)
                {
                    Image2.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(com.CompanyImage, 0, com.CompanyImage.Length);
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = string.Format("Error when save company : {0}", ex.Message);
            }
        }

    }
}
