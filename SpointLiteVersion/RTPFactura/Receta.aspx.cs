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
    public partial class prefactura : System.Web.UI.Page
    {

        SqlConnection con;
        SqlCommand comando;
        SqlDataAdapter adapter;
        SqlParameter param;
        string id;
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
            id = Request.QueryString.Get("id");

            DataTable dt = cargar(id);
            ReportDataSource rds = new ReportDataSource("DataSet2", dt);
            ReportViewer3.LocalReport.DataSources.Add(rds);
            ReportViewer3.LocalReport.ReportPath = "RTPFactura/Report1.rdlc";
            PageSettings pg = new PageSettings();
            pg.Margins.Left = 0;
            pg.Margins.Right = 0;
            pg.Margins.Top = 1;
            pg.Margins.Bottom =1;
            this.ReportViewer3.SetPageSettings(pg);

            //parameters
            ReportParameter[] rptParams = new ReportParameter[]
            {
                new ReportParameter("id",id.ToString())
        };
            ReportViewer3.LocalReport.Refresh();

        }

        public DataTable cargar(string codigoventa)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-MF01SN4\\SQLANALYSIS;Initial Catalog=hospoint;Integrated Security=True"))
            {

                SqlCommand cmd = new SqlCommand("sp_reporte_receta_back", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;

        }
    }

}