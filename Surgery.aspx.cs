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
    public partial class Surgery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            string surgeryId = Request.QueryString["surgeryid"];

            if (surgeryId != null && surgeryId != "")
            {
                try
                {
                    string query = "select p.PATIENTNUMBER, p.FIRSTNAME, p.LASTNAME, s.SURGERYDATE, s.SURGICALTHEATER, st.NAME "
                                    + "from surgery s, patient p, surgerytype st "
                                    + "where s.PATIENTNUMBER = p.PATIENTNUMBER "
                                    + "and s.SURGERYTYPECODE = st.CODE "
                                    + "and s.surgeryid = '" + surgeryId + "'";

                    using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                    {
                        DataRow row = ds.Tables[0].Rows[0];

                        patientLnk.Text = row.Field<string>(2) + ", " + row.Field<string>(1);
                        patientLnk.NavigateUrl = "/Patient.aspx?patientnumber=" + row.Field<decimal>(0).ToString();

                        dateLabel.Text = row.Field<DateTime>(3).ToString();
                        theaterLabel.Text = row.Field<string>(4);

                        typeLabel.Text = row.Field<string>(5);
                    }

                    query = "select e.EMPLOYEEID, e.FIRSTNAME, e.LASTNAME "
                            + "from SUREGONFORSURGERY s, EMPLOYEE e "
                            + "where s.SURGEONID = e.EMPLOYEEID "
                            + "and s.surgeryid = '" + surgeryId + "'";

                    using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string employeeId = row.Field<string>(0);
                            string surgeonName = "Dr. " + row.Field<string>(1) + " " + row.Field<string>(2);

                            HyperLink physicianLink = new HyperLink();
                            physicianLink.NavigateUrl = "/Physician.aspx?physicianid=" + employeeId;
                            physicianLink.Text = surgeonName;

                            surgeonPanel.Controls.Add(physicianLink);
                            surgeonPanel.Controls.Add(new LiteralControl("<br />"));
                        }
                    }

                    query = "select e.EMPLOYEEID, e.FIRSTNAME, e.LASTNAME "
                            + "from NURSEFORSURGERY n, EMPLOYEE e "
                            + "where n.NURSEID = e.EMPLOYEEID "
                            + "and n.surgeryid = '" + surgeryId + "'";

                    using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string employeeId = row.Field<string>(0);
                            string nurseName = row.Field<string>(1) + " " + row.Field<string>(2) + " R.N.";

                            HyperLink nurseLink = new HyperLink();
                            nurseLink.NavigateUrl = "/Nurse.aspx?physicianid=" + employeeId;
                            nurseLink.Text = nurseName;

                            nursePanel.Controls.Add(nurseLink);
                            nursePanel.Controls.Add(new LiteralControl("<br />"));
                        }
                    }
                }
                catch (SqlException ex)
                {

                }
            }
            else
            {
                Response.Redirect("/Patient.aspx");
            }
        }
    }
}