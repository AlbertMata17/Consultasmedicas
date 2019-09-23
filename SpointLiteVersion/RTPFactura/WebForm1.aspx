    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SpointLiteVersion.RTPFactura.WebForm1" %>

    <%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

    <!DOCTYPE html>

    <html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title></title>
        <script runat="server">
       
        </script>
    </head>
    <body>
        <form id="form1" runat="server">
            <div>
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                <rsweb:ReportViewer ID="ReportViewer1" runat="server"  ont-Names="Arial" Font-Size="10pt" Height="724px" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%">
                     <LocalReport ReportPath="RTPFactura/Report2.rdlc">

                </LocalReport>
                </rsweb:ReportViewer>
            </div>
        </form>
    </body>
    </html>
