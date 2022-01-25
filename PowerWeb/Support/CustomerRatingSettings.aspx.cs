using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.IO;
using System.Data;
using SubSonic;

namespace PowerWeb.Support
{
    public partial class CustomerRatingSetting : System.Web.UI.Page
    {
        protected string PATH_PICTURE = "~/Rating/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request.QueryString["rating"] != null)
                {
                    string RatingID = Request.QueryString["rating"];
                    LoadRatingEditor(RatingID);
                }
                else if (Request.QueryString["feedback"] != null)
                {
                    string FeedbackID = Request.QueryString["feedback"];
                    LoadRatingFeedback(FeedbackID);
                }
                else
                {
                    PanelSetting.Visible = true;
                    PanelRatingSystem.Visible = false;
                    PanelRatingFeedback.Visible = false;
                    LoadSetting();
                    LoadRatingSystem();
                    LoadRatingFeedback();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }

        protected void BtnAddNewRatingSystem_Click(object sender, EventArgs e)
        { 
            Response.Redirect("CustomerRatingSettings.aspx?rating=0");
        }

        protected void btnRatingSystemSave_Click(object sender, EventArgs e)
        {
            try
            {
                int RatingID = 0;
                SaveRatingSystem(out RatingID);
                Response.Redirect("CustomerRatingSettings.aspx?rating=" + RatingID.ToString() + "&&msg=Rating system saved");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    lblResultRatingSystem.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Rating System not saved. Rating ID:") + lblRatingID.Text + " " + LanguageManager.GetTranslation("has already been used. Choose another name") + "</span> ";
                }
                else
                {
                    //haveError = true;
                    lblResultRatingSystem.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Rating System not saved:") + "</span> " + ex.Message;
                }
            }
        }

        protected void btnRatingSystemDelete_Click(object sender, EventArgs e)
        {
            string RatingID = lblRatingID.Text;

            RatingMasterController ctr = new RatingMasterController();
            ctr.Delete(RatingID);

            Response.Redirect("CustomerRatingSettings.aspx");
        }

        protected void btnRatingSystemReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerRatingSettings.aspx");
        }

        protected void gvRatingSystem_Sorting(object sender, EventArgs e)
        { 
        
        }

        protected void gvGoodFeedback_Sorting(object sender, EventArgs e)
        {

        }

        protected void gvBadFeedback_Sorting(object sender, EventArgs e)
        {

        }
        
        protected void btnRemoveImage_Click(object sender, EventArgs e)
        {
            Image2.ImageUrl = "";
        }

        protected void BtnAddNewGoodFeedback_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerRatingSettings.aspx?feedback=0&&type=good");
        }

        protected void BtnAddNewBadFeedback_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerRatingSettings.aspx?feedback=0&&type=Bad");
        }

        protected void btnRatingFeedbackSave_Click(object sender, EventArgs e)
        {
            try
            {
                int FeedbackID = 0;
                SaveRatingFeedback(out FeedbackID);
                Response.Redirect("CustomerRatingSettings.aspx?feedback=" + FeedbackID.ToString() + "&&msg=Rating feedback saved");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    lblResultRatingSystem.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Rating Feedback not saved. Rating ID:") + lblRatingID.Text + " " + LanguageManager.GetTranslation("has already been used. Choose another name") + "</span> ";
                }
                else
                {
                    //haveError = true;
                    lblResultRatingSystem.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Rating Feedback not saved:") + "</span> " + ex.Message;
                }
            }
        }

        protected void btnRatingFeedbackDelete_Click(object sender, EventArgs e)
        {
            string FeedbackID = hFeedbackID.Value;

            RatingFeedbackController ctr = new RatingFeedbackController();
            ctr.Delete(FeedbackID);

            Response.Redirect("CustomerRatingSettings.aspx");
        }

        protected void btnRatingFeedbackReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerRatingSettings.aspx");
        }

        private void LoadSetting()
        {

            Rating_GreetingText.Text = AppSetting.GetSetting(AppSetting.SettingsName.Rating.GreetingText);
            Rating_FooterText.Text = AppSetting.GetSetting(AppSetting.SettingsName.Rating.FooterText);
            Rating_ThankYouGoodRating.Text = AppSetting.GetSetting(AppSetting.SettingsName.Rating.ThankYouGoodRating);
            Rating_ThankYouBadRating.Text = AppSetting.GetSetting(AppSetting.SettingsName.Rating.ThankYouBadRating);
            Rating_ThankYouInterval.Text = AppSetting.GetSetting(AppSetting.SettingsName.Rating.ThankYouInterval);

            Rating_AllowGoodRatingFeedback.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Rating.AllowGoodRatingFeedback), false);
            Rating_GoodFeedbackGreeting.Text = AppSetting.GetSetting(AppSetting.SettingsName.Rating.GoodFeedbackGreeting);
            Rating_AllowBadRatingFeedback.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Rating.AllowBadRatingFeedback), false);
            Rating_BadFeedbackGreeting.Text = AppSetting.GetSetting(AppSetting.SettingsName.Rating.BadFeedbackGreeting);

        }

        private void LoadRatingSystem()
        {
            //RatingMasterCollection rcol = new RatingMasterCollection();
            //rcol.Where(RatingMaster.Columns.Deleted, false);
            //rcol.Load();

            DataTable dt = RatingController.GetRatingSystemList();
            
            gvRatingSystem.DataSource = dt;
            gvRatingSystem.DataBind();
        }

        private void LoadRatingFeedback()
        {
            //RatingFeedbackCollection goodcol = new RatingFeedbackCollection();
            //goodcol.Where(RatingFeedback.Columns.Deleted, false);
            //goodcol.Where(RatingFeedback.Columns.RatingType, "Good Rating");
            //goodcol.Load();

            DataTable goodcol = RatingController.GetRatingFeedbackList("Good Rating");

            gvGoodFeedback.DataSource = goodcol;
            gvGoodFeedback.DataBind();

            //RatingFeedbackCollection badcol = new RatingFeedbackCollection();
            //badcol.Where(RatingFeedback.Columns.Deleted, false);
            //badcol.Where(RatingFeedback.Columns.RatingType, "Bad Rating");
            //badcol.Load();

            DataTable badcol = RatingController.GetRatingFeedbackList("Bad Rating");

            gvBadFeedback.DataSource = badcol;
            gvBadFeedback.DataBind();
        }

        private void SaveSetting()
        {
            AppSetting.SetSetting(AppSetting.SettingsName.Rating.GreetingText, Rating_GreetingText.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Rating.FooterText, Rating_FooterText.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Rating.ThankYouGoodRating, Rating_ThankYouGoodRating.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Rating.ThankYouBadRating, Rating_ThankYouBadRating.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Rating.ThankYouInterval, Rating_ThankYouInterval.Text);

            AppSetting.SetSetting(AppSetting.SettingsName.Rating.AllowGoodRatingFeedback, Rating_AllowGoodRatingFeedback.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Rating.GoodFeedbackGreeting, Rating_GoodFeedbackGreeting.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Rating.AllowBadRatingFeedback, Rating_AllowBadRatingFeedback.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Rating.BadFeedbackGreeting, Rating_BadFeedbackGreeting.Text);

            LoadSetting();
        }

        private void LoadRatingEditor(string RatingID)
        {
            PanelSetting.Visible = false;
            PanelRatingSystem.Visible = true;
            PanelRatingFeedback.Visible = false;

            lblRatingID.Text = RatingID;
           

            if (Request.QueryString["msg"] != null)
            {
                string msg = Request.QueryString["msg"];
                lblResultRatingSystem.Text = "<span style=\"font-weight:bold; color:#22bb22\">"+ msg +"</span>";
            }

            if (RatingID != "0" && !string.IsNullOrEmpty(RatingID))
            {
                string deleted = Request.QueryString["delete"];

                if (!string.IsNullOrEmpty(deleted) && deleted.ToLower() == "true")
                {
                    RatingMasterController ctr = new RatingMasterController();
                    ctr.Delete(RatingID);

                    Response.Redirect("CustomerRatingSettings.aspx");
                }
                else
                {
                    RatingMaster rat = new RatingMaster(RatingID);

                    lblRatingID.Text = rat.Rating.ToString();
                    txtRatingName.Text = rat.RatingName;
                    ddlRatingType.SelectedValue = rat.RatingType;
                    lblWeight.Text = rat.Weight.ToString();
                    cbEnabled.Checked = !rat.Deleted.Value;
                    if (rat.RatingImageUrl != null)
                    {
                        //Image2.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(rat.RatingImage, 0, rat.RatingImage.Length);
                        Image2.ImageUrl = PATH_PICTURE + rat.RatingImageUrl;
                    }
                }

                //btnRatingSystemDelete.Visible = true;
            }
        }

        private void LoadRatingFeedback(string FeedbackID)
        {
            PanelSetting.Visible = false;
            PanelRatingSystem.Visible = false;
            PanelRatingFeedback.Visible = true;

            hFeedbackID.Value = FeedbackID;
            btnRatingFeedbackDelete.Visible = false;
            if (Request.QueryString["type"] != null)
            {
                string rating = Request.QueryString["type"];
                if (rating.ToLower() == "bad")
                    lblRatingType.Text = "Bad Rating";
                else
                    lblRatingType.Text = "Good Rating";
            }

            if (Request.QueryString["msg"] != null)
            {
                string msg = Request.QueryString["msg"];
                lblResultRatingFeedback.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + msg + "</span>";
            }

            if (FeedbackID != "0" && !string.IsNullOrEmpty(FeedbackID))
            {
                string deleted = Request.QueryString["delete"];

                if (!string.IsNullOrEmpty(deleted) && deleted.ToLower() == "true")
                {
                    RatingFeedbackController ctr = new RatingFeedbackController();
                    ctr.Delete(FeedbackID);

                    Response.Redirect("CustomerRatingSettings.aspx");
                }
                else
                {
                    RatingFeedback rat = new RatingFeedback(FeedbackID);

                    hFeedbackID.Value = rat.RatingFeedbackID.ToString();
                    txtSelectionText.Text = rat.SelectionText;
                    lblRatingType.Text = rat.RatingType;
                    if (rat.SelectionImageUrl != null)
                    {
                        //ImageFeedback.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(rat.SelectionImage, 0, rat.SelectionImage.Length);
                        ImageFeedback.ImageUrl = PATH_PICTURE + rat.SelectionImageUrl;
                    }
                }

                btnRatingFeedbackDelete.Visible = true;
            }
            
        }

        private void SaveRatingSystem(out int RatingID)
        {
            string imageurl = "";
            
            RatingID = lblRatingID.Text.GetIntValue();
            RatingMaster rm;

            if (RatingID == 0)
            {
                rm = new RatingMaster();
            }
            else
            {
                rm = new RatingMaster(RatingID);
            }

            rm.RatingName = txtRatingName.Text;
            rm.RatingType = ddlRatingType.Text;
            rm.Deleted = !cbEnabled.Checked;
            
            if (fuRatingSystem.HasFile)
            {
                //rm.RatingImage = ImageCompressor.ResizeAndCompressImage(new MemoryStream(fuRatingSystem.FileBytes));
                string fileName = Path.GetFileName(fuRatingSystem.PostedFile.FileName);

                string path = Server.MapPath(PATH_PICTURE);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                fuRatingSystem.PostedFile.SaveAs(path + fileName);
                imageurl = fileName;

                rm.RatingImageUrl = imageurl;
            }
           

            //if (fuRatingSystem.HasFile)
            //{
            //    //rm.RatingImage = ImageCompressor.ResizeAndCompressImage(new MemoryStream(fuRatingSystem.FileBytes));
            //}

            rm.Save(Session["username"].ToString());

            RatingID = rm.Rating;
            
        }

        private void SaveRatingFeedback(out int FeedbackID)
        {
            string imageurl = "";
           
            FeedbackID = hFeedbackID.Value.GetIntValue();
            RatingFeedback rm;

            if (FeedbackID == 0)
            {
                rm = new RatingFeedback();
            }
            else {
                rm = new RatingFeedback(FeedbackID);
            }

            rm.SelectionText = txtSelectionText.Text;
            rm.RatingType = lblRatingType.Text;
            rm.Deleted = false;
      
            if (FileUploadFeedback.HasFile)
            {
                //rm.RatingImage = ImageCompressor.ResizeAndCompressImage(new MemoryStream(fuRatingSystem.FileBytes));
                string fileName = Path.GetFileName(FileUploadFeedback.PostedFile.FileName);

                string path = Server.MapPath(PATH_PICTURE);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileUploadFeedback.PostedFile.SaveAs(path + fileName);
                imageurl = fileName;

                rm.SelectionImageUrl = imageurl;
            }

            //if (FileUploadFeedback.HasFile)
            //    rm.SelectionImage = ImageCompressor.ResizeAndCompressImage(new MemoryStream(FileUploadFeedback.FileBytes));

            rm.Save(Session["username"].ToString());

            FeedbackID = rm.RatingFeedbackID;
        }
    }
}
