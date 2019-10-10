using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpointLiteVersion.RTPFactura
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand comando;
        SqlDataAdapter adapter;
        SqlParameter param;
        string idConsulta;
        string nombreempresa;
        protected void Page_Load(object sender, EventArgs e)
        {
            con = new SqlConnection("Data Source=DESKTOP-MF01SN4\\SQLANALYSIS;Initial Catalog=hospoint;Integrated Security=True");
            if (!IsPostBack)
            {
                renderReport();

            }


        }
        public void renderReport()
        {
            idConsulta = Request.QueryString.Get("idconsulta");

            DataTable dt = cargar(idConsulta);
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "RTPFactura/Report2.rdlc";
            PageSettings pg = new PageSettings();
            pg.Margins.Left = 1;
            pg.Margins.Right = 1;
            pg.Margins.Top = 1;
            pg.Margins.Bottom = 1;
            this.ReportViewer1.SetPageSettings(pg);

            //parameters
            ReportParameter[] rptParams = new ReportParameter[]
            {
                new ReportParameter("idconsulta",idConsulta.ToString())
        };
            ReportViewer1.LocalReport.Refresh();

        }

        public DataTable cargar(string codigoventa)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-MF01SN4\\SQLANALYSIS;Initial Catalog=hospoint;Integrated Security=True"))
            {

                SqlCommand cmd = new SqlCommand("sp_reporte_HistorialClinico_back", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idConsulta", SqlDbType.Int).Value = idConsulta;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;

        }
    }
}