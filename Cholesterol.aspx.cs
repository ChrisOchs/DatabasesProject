using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WebApplication1
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");
            try
            {
                string query = "select p.firstname, p.lastname, p.patientnumber, md.hdl, md.ldl, md.triglycerides, md.datadate "
                    + "from patient p, medicaldata md "
                    + "where p.patientnumber = md.patientnumber "
                    + "order by p.lastname, p.firstname, md.datadate desc";

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int c = 1;

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            TableRow resultRow = new TableRow();

                            TableCell nameCell = new TableCell();
                            TableCell dateCell = new TableCell();
                            TableCell hdlCell = new TableCell();
                            TableCell ldlCell = new TableCell();
                            TableCell triglyceridesCell = new TableCell();
                            TableCell totalCholesterolCell = new TableCell();
                            TableCell tcRatioCell = new TableCell();
                            TableCell riskFactor = new TableCell();

                            HyperLink patientLink = new HyperLink();
                            patientLink.Text = row.Field<string>(1) + ", " + row.Field<string>(0);
                            patientLink.NavigateUrl = "/Patient.aspx?patientnumber=" + row.Field<decimal>(2).ToString();
                            nameCell.Controls.Add(patientLink);

                            dateCell.Text = row.Field<DateTime>(6).ToString();

                            decimal hdl = row.Field<decimal>(3);
                            decimal ldl = row.Field<decimal>(4);
                            decimal triglycerides = row.Field<decimal>(5);
                            double totalCholesterol = Convert.ToDouble(hdl + ldl) + 0.2 * Convert.ToDouble(triglycerides);
                            double tcRatio = totalCholesterol / Convert.ToDouble(ldl);

                            hdlCell.Text = String.Format("{0:0.##}", hdl);
                            ldlCell.Text = String.Format("{0:0.##}", ldl);
                            triglyceridesCell.Text = String.Format("{0:0.##}", triglycerides);
                            totalCholesterolCell.Text = String.Format("{0:0.##}", totalCholesterol);
                            tcRatioCell.Text = String.Format("{0:0.##}", tcRatio);

                            string risk = "";

                            if (tcRatio < 4)
                            {
                                risk = "None";
                                riskFactor.BackColor = System.Drawing.ColorTranslator.FromHtml("#00FF00");
                            }
                            else if (tcRatio < 5)
                            {
                                risk = "Low";
                                riskFactor.BackColor = System.Drawing.ColorTranslator.FromHtml("#888800");
                            }
                            else
                            {
                                risk = "Moderate";
                                riskFactor.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                            }

                            riskFactor.Text = risk;

                            resultRow.Cells.Add(nameCell);
                            resultRow.Cells.Add(dateCell);
                            resultRow.Cells.Add(hdlCell);
                            resultRow.Cells.Add(ldlCell);
                            resultRow.Cells.Add(triglyceridesCell);
                            resultRow.Cells.Add(totalCholesterolCell);
                            resultRow.Cells.Add(tcRatioCell);
                            resultRow.Cells.Add(riskFactor);

                            if (c % 2 == 0)
                            {
                                resultRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                            }

                            c++;

                            researchResultsTbl.Rows.Add(resultRow);
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (SqlException ex)
            {

            }
        }
    }
}