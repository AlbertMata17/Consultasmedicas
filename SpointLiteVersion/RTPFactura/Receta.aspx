<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Receta.aspx.cs" Inherits="SpointLiteVersion.RTPFactura.prefactura" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
  <form id="form1" runat="server">
        <div>
        </div>
          <asp:ScriptManager ID="ScriptManager3" runat="server"></asp:ScriptManager>

            <rsweb:ReportViewer ID="ReportViewer3" runat="server"  ont-Names="Arial" Font-Size="10pt" Height="724px" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%">
                 <LocalReport ReportPath="RTPFactura/Report1.rdlc">

            </LocalReport>
            </rsweb:ReportViewer>
    </form>
</body>
</html>
