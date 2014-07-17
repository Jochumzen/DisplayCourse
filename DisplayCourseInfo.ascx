<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayCourseInfo.ascx.cs" Inherits="Plugghest.Modules.DisplayCourse.DisplayCourseInfo" %>

<asp:Label ID="lblCreatedBy" runat="server" resourcekey ="CreatedBy" Font-Bold="True"></asp:Label>
<asp:HyperLink ID="hlCreatedBy" runat="server" /><br />

<asp:Label ID="lblCreatedOn" runat="server" resourcekey ="CreatedOn" Font-Bold="True"></asp:Label>
<asp:Label ID="lbltheCreatedOn" runat="server"></asp:Label><br />

<asp:Label ID="lblWhoCanEdit" runat="server" resourcekey ="WhoCanEdit" Font-Bold="True"></asp:Label>
<asp:Label ID="lbltheWhoCanEdit" runat="server"></asp:Label>
&nbsp;<asp:Button ID="btnWhoCanEdit" runat="server" Text="Edit" Visible="False" OnClientClick="return ShowPopupWhoCanEdit();" OnClick="btnWhoCanEdit_Click"/><br />

<asp:Label ID="lblListed" runat="server" resourcekey ="Listed" Font-Bold="True"></asp:Label>
<asp:Label ID="lblTheListed" runat="server"></asp:Label>
&nbsp;<asp:Button ID="btnListed" runat="server" Text="Edit" Visible="False" OnClientClick="return ShowPopupListed();" OnClick="btnListed_Click" /><br />

<asp:Label ID="lblDescription" runat="server" resourcekey ="Description" Font-Bold="True"></asp:Label>
<asp:Label ID="lbltheDespription" runat="server"></asp:Label><br />
<asp:Button ID="btnDelete" runat="server" Text="DeleteCourse" Visible="False" OnClientClick ="return CheckDelete();" OnClick="btnDelete_Click" /><br /><br />


<div id="ModalListWhoCanEdit"  style="display:none" title="Who can edit">
    <asp:RadioButtonList ID="rblWhoCanEdit" runat="server">
    </asp:RadioButtonList>
</div>

<div id="ModalListListed"  style="display:none" title="List setting for Course">
    <asp:RadioButtonList ID="rblListed" runat="server">
    </asp:RadioButtonList>
</div>

<script type="text/javascript">

    function CheckDelete() {
        return confirm('<%= Localization.GetString("SureDelete.Text", this.LocalResourceFile)%>');
    }

    function ShowPopupWhoCanEdit()
    {
        $("#ModalListWhoCanEdit").dialog({
            resizable: false,
            height: 240,
            modal: true,
            buttons: {
                "Save": function () {
                    $(this).dialog("close");
                    __doPostBack('btnWhoCanEdit',
                                  $('#<%= rblWhoCanEdit.ClientID %> input:checked').val());
                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

    function ShowPopupListed() {
        $("#ModalListListed").dialog({
            resizable: false,
            height: 240,
            modal: true,
            buttons: {
                "Save": function () {
                    $(this).dialog("close");
                    __doPostBack('btnListed',
                                  $('#<%= rblListed.ClientID %> input:checked').val());
                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });
            return false;
    }

</script>



<link rel="stylesheet" href="//code.jquery.com/ui/1.11.0/themes/smoothness/jquery-ui.css">