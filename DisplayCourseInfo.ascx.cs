using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using Plugghest.Base2;
using DotNetNuke.Services.Localization;

namespace Plugghest.Modules.DisplayCourse
{
    public partial class DisplayCourseInfo : PortalModuleBase 
    {
        public int CourseId;
        public string CultureCode;
        public CourseContainer cc;
        public bool IsAuthorized;

        protected void Page_Load(object sender, EventArgs e)
        {
            string coursePageName = ((DotNetNuke.Framework.CDefault)this.Page).Title;
            coursePageName = coursePageName.Replace("C", "");
            CourseId = Convert.ToInt32(coursePageName);
            CultureCode = (Page as DotNetNuke.Framework.PageBase).PageCulture.Name;
            BaseHandler bh = new BaseHandler();
            cc = new CourseContainer(CultureCode, CourseId);
            IsAuthorized = ((this.UserId != -1 && cc.TheCourse.WhoCanEdit == EWhoCanEdit.Anyone) || cc.TheCourse.CreatedByUserId == this.UserId || (UserInfo.IsInRole("Administator")));


            if (Request.Form["__EVENTTARGET"] == "btnWhoCanEdit")
            {
                // Fire event
                btnWhoCanEdit_Click(this, new EventArgs());
            }

            if (Request.Form["__EVENTTARGET"] == "btnListed")
            {
                // Fire event
                btnListed_Click(this, new EventArgs());
            }

            UserController uc = new UserController();
            UserInfo u = uc.GetUser(PortalId, cc.TheCourse.CreatedByUserId);
            hlCreatedBy.Text = u.DisplayName;
            hlCreatedBy.NavigateUrl = DotNetNuke.Common.Globals.UserProfileURL(cc.TheCourse.CreatedByUserId);
            lbltheCreatedOn.Text = cc.TheCourse.CreatedOnDate.ToString();
            rblWhoCanEdit.Items.Clear();
            rblWhoCanEdit.Items.Add("Anyone");
            rblWhoCanEdit.Items.Add("Only me");
            rblWhoCanEdit.SelectedValue = "Anyone";
            switch (cc.TheCourse.WhoCanEdit)
            {
                case EWhoCanEdit.Anyone:
                    lbltheWhoCanEdit.Text = "Anyone";
                    break;
                case EWhoCanEdit.OnlyMe:
                    lbltheWhoCanEdit.Text = "Only me";
                    rblWhoCanEdit.SelectedValue = "Only me";
                    break;
                case EWhoCanEdit.NotSet :
                    lbltheWhoCanEdit.Text = "Not set";
                    break;
            }
            rblListed.Items.Clear();
            rblListed.Items.Add(Localization.GetString("Listed.Text", this.LocalResourceFile));
            rblListed.Items.Add(Localization.GetString("NotListed.Text", this.LocalResourceFile));
            rblListed.SelectedIndex = 0;

            if (cc.TheCourse.IsListed)
                lblTheListed.Text = "Yes";
            else
            {
                lblTheListed.Text = "No";
                rblListed.SelectedIndex = 1;
            }
            if (cc.TheCourse.CreatedByUserId == this.UserId)
            {
                btnWhoCanEdit.Visible = true;
                btnListed.Visible = true;
            }
            cc.LoadDescription();
            if (cc.TheDescription != null)
                lbltheDespription.Text = cc.TheDescription.Text;
            else
                lbltheDespription.Text = "-";

            if(cc.TheCourse.CreatedByUserId == this.UserId || UserInfo.IsInRole("Administator"))
            {
                btnDelete.Visible = true;
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            cc.TheCourse.IsDeleted = true;
            cc.UpdateCourseEntity();
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.HomeTabId));
        }

        protected void btnWhoCanEdit_Click(object sender, EventArgs e)
        {
            string commandArgument = !string.IsNullOrEmpty(Request.Form["__EVENTARGUMENT"])?Request.Form["__EVENTARGUMENT"]:string.Empty;
            switch (commandArgument)
            {
                case "Anyone":
                    cc.TheCourse.WhoCanEdit = EWhoCanEdit.Anyone;
                    break;
                case "Only me":
                    cc.TheCourse.WhoCanEdit = EWhoCanEdit.OnlyMe;
                    break;
            }
            cc.UpdateCourseEntity();
        }

        protected void btnListed_Click(object sender, EventArgs e)
        {
            if (rblListed.SelectedIndex == 0)
                cc.TheCourse.IsListed = true;
            else
                cc.TheCourse.IsListed = false;
            cc.UpdateCourseEntity();
        }
    }
}