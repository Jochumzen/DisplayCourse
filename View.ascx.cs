/*
' Copyright (c) 2014  Plugghest.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Plugghest.Modules.DisplayCourse.Components;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using Plugghest.Modules.PlugghestControls;
using Plugghest.Base2;

namespace Plugghest.Modules.DisplayCourse
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from DisplayCourseModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : PortalModuleBase, IActionable
    {

        public string CultureCode;
        public int CourseId;
        public CourseContainer cc;
        public bool InCreationLanguage;
        public bool IsAuthorized;
        public int Edit;
        public int Translate;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string coursePageName = ((DotNetNuke.Framework.CDefault)this.Page).Title;
                coursePageName = coursePageName.Replace("C", "");
                CourseId = Convert.ToInt32(coursePageName);
                CultureCode = (Page as DotNetNuke.Framework.PageBase).PageCulture.Name;
                BaseHandler bh = new BaseHandler();
                cc = new CourseContainer(CultureCode, CourseId);
                InCreationLanguage = (cc.TheCourse.CreatedInCultureCode == CultureCode);
                IsAuthorized = ((this.UserId != -1 && cc.TheCourse.WhoCanEdit == EWhoCanEdit.Anyone) || cc.TheCourse.CreatedByUserId == this.UserId || (UserInfo.IsInRole("Administator")));
                Edit = !string.IsNullOrEmpty(Page.Request.QueryString["edit"]) ? Convert.ToInt16(Page.Request.QueryString["edit"]) : -1;
                Translate = !string.IsNullOrEmpty(Page.Request.QueryString["translate"]) ? Convert.ToInt16(Page.Request.QueryString["translate"]) : -1;

                #region hide/display controls
                if (!InCreationLanguage && UserId > -1 && Translate == -1)
                {
                    pnlToCreationLanguage.Visible = true;
                    hlToCreationLanguage.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "", "language=" + cc.TheCourse.CreatedInCultureCode);
                    pnlTranslatePlugg.Visible = true;
                    hlTranslatePlugg.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "", "translate=0");
                }

                if (InCreationLanguage && UserId > -1 && Edit == -1)
                {
                    pnlEditPlugg.Visible = true;
                    hlEditPlugg.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "", "edit=0");
                }

                if (!InCreationLanguage && UserId > -1 && Translate > -1)
                {
                    pnlExitTranslateMode.Visible = true;
                    hlExitTranslateMode.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "", "");
                }

                if (InCreationLanguage && UserId > -1 && Edit > -1)
                {
                    pnlExitEditMode.Visible = true;
                    hlExitEditMode.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "", "");
                }

                cc.LoadPluggs();
                if (cc.ThePluggs.Count == 0)
                    lnkBeginCourse.Visible = false;
                else
                {
                    PluggContainer pc = new PluggContainer(CultureCode, cc.ThePluggs[0].PluggId);
                    lnkBeginCourse.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(pc.ThePlugg.TabId, "", "cp=" + cc.ThePluggs[0].CoursePluggId);
                }
                #endregion

                phComponents.Controls.Clear();
                int controlOrder = 1;
                bool editOrTranslateMode = (Edit > -1 || Translate > -1) && UserId > -1;

                if (editOrTranslateMode)
                {
                    string ComponentHead = "<hr /><h3>" + Localization.GetString("Subject", LocalResourceFile) + "</h3>";
                    phComponents.Controls.Add(new LiteralControl(ComponentHead));
                }
                LoadSubject(controlOrder);
                controlOrder++;

                if (editOrTranslateMode)
                {
                    string ComponentHead = "<hr /><h3>" + Localization.GetString("Title", LocalResourceFile) + "</h3>";
                    phComponents.Controls.Add(new LiteralControl(ComponentHead));
                    EditTitleAndDescription(controlOrder, ETextItemType.CourseTitle);
                    controlOrder++;

                    ComponentHead = "<hr /><h3>" + Localization.GetString("Description", LocalResourceFile) + "</h3>";
                    phComponents.Controls.Add(new LiteralControl(ComponentHead));
                    EditTitleAndDescription(controlOrder, ETextItemType.CourseDescription );
                    controlOrder++;
                }

                if (editOrTranslateMode)
                {
                    string ComponentHead = "<hr /><h3>" + Localization.GetString("Content", LocalResourceFile) + "</h3>";
                    phComponents.Controls.Add(new LiteralControl(ComponentHead));
                }

                LoadRichRich(controlOrder);



            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void LoadSubject(int controlOrder)
        {
            SubjectControl ucS = (SubjectControl)this.LoadControl("/DesktopModules/PlugghestControls/SubjectControl.ascx");
            if (ucS != null)
            {
                ucS.SubjectCase = ESubjectCase.Course;
                ucS.ControlOrder = controlOrder;
                ucS.CultureCode = CultureCode;
                ucS.SubjectId = cc.TheCourse.SubjectId;
                ucS.ItemId = CourseId;
                ucS.Case = EControlCase.View;
                if (InCreationLanguage & UserId > -1 & Edit > -1)
                    ucS.Case = EControlCase.ViewAllowEdit;
                if (InCreationLanguage & IsAuthorized & Edit == controlOrder)
                    ucS.Case = EControlCase.Edit;

                ucS.LocalResourceFile = "/DesktopModules/PlugghestControls/App_LocalResources/SubjectControl.ascx";
                phComponents.Controls.Add(ucS);
            }
        }

        private void EditTitleAndDescription(int controlOrder, ETextItemType textItemType)
        {
            PureTextControl ucL = (PureTextControl)this.LoadControl("/DesktopModules/PlugghestControls/PureTextControl.ascx");
            if (ucL != null)
            {
                ucL.ModuleConfiguration = this.ModuleConfiguration;
                ucL.ItemId = CourseId;
                ucL.CultureCode = CultureCode;
                ucL.CreatedInCultureCode = cc.TheCourse.CreatedInCultureCode;
                ucL.ControlOrder = controlOrder;
                ucL.ItemType = textItemType;
                ucL.Case = EControlCase.View;
                if (InCreationLanguage && IsAuthorized && Edit > -1)
                    ucL.Case = EControlCase.ViewAllowEdit;
                if (InCreationLanguage && IsAuthorized && Edit == controlOrder)
                    ucL.Case = EControlCase.Edit;
                if (!InCreationLanguage && UserId > -1 && Translate > -1)
                    ucL.Case = EControlCase.ViewAllowTranslate;
                if (!InCreationLanguage && UserId > -1 && Translate == controlOrder)
                    ucL.Case = EControlCase.Translate;

                ucL.LocalResourceFile = "/DesktopModules/PlugghestControls/App_LocalResources/PureTextControl.ascx";
                phComponents.Controls.Add(ucL);
            }
        }

        private void LoadRichRich(int controlOrder)
        {
            RichRichControl ucRR = (RichRichControl)this.LoadControl("/DesktopModules/PlugghestControls/RichRichControl.ascx");
            if ((ucRR != null))
            {
                ucRR.ModuleConfiguration = this.ModuleConfiguration;
                ucRR.ItemId = CourseId;
                ucRR.CultureCode = CultureCode;
                ucRR.CreatedInCultureCode = cc.TheCourse.CreatedInCultureCode;
                ucRR.ControlOrder = controlOrder;
                ucRR.ItemType = ETextItemType.CourseRichRichText;
                ucRR.Case = EControlCase.View;
                if (InCreationLanguage && IsAuthorized && Edit > -1)
                    ucRR.Case = EControlCase.ViewAllowEdit;
                if (InCreationLanguage && IsAuthorized && Edit == controlOrder)
                    ucRR.Case = EControlCase.Edit;
                if (!InCreationLanguage && UserId > -1 && Translate > -1)
                    ucRR.Case = EControlCase.ViewAllowTranslate;
                if (!InCreationLanguage && UserId > -1 && Translate == controlOrder)
                    ucRR.Case = EControlCase.Translate;

                ucRR.LocalResourceFile = "/DesktopModules/PlugghestControls/App_LocalResources/RichRichControl.ascx";
                phComponents.Controls.Add(ucRR);
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }
    }
}