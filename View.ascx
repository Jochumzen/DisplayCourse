<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Plugghest.Modules.DisplayCourse.View" %>

<asp:Panel ID="pnlToCreationLanguage" runat="server" Visible ="false">
    <asp:HyperLink ID="hlToCreationLanguage" style="font-size: xx-small; float: right;" runat="server" resourcekey="ToCreationLanguage" /><br />
</asp:Panel>

<asp:Panel ID="pnlTranslatePlugg" runat="server" Visible ="false">
    <asp:HyperLink ID="hlTranslatePlugg" style="font-size: xx-small; float: right;" runat="server" resourcekey="TranslatePlugg" /><br />
</asp:Panel>

<asp:Panel ID="pnlEditPlugg" runat="server" Visible ="false">
    <asp:HyperLink ID="hlEditPlugg" style="font-size: xx-small; float: right;" runat="server" resourcekey="EditPlugg" /><br />
</asp:Panel>

<asp:Panel ID="pnlExitTranslateMode" runat="server" Visible ="false">
    <asp:HyperLink ID="hlExitTranslateMode" style="float: right;" runat="server" resourcekey="ExitTranslateMode" /><br />
</asp:Panel>

<asp:Panel ID="pnlExitEditMode" runat="server" Visible ="false">
    <asp:HyperLink ID="hlExitEditMode" style="float: right;" runat="server" resourcekey="ExitEditMode" /><br />
</asp:Panel>

<asp:PlaceHolder ID="phComponents" runat="server"></asp:PlaceHolder>

<asp:HyperLink ID="lnkBeginCourse" runat="server" Text="Begin course" CssClass="Button_default" ></asp:HyperLink>
<br />