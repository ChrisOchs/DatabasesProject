using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WebApplication1
{
    public partial class AddNurseToSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string patientnumber = Request.QueryString["patientnumber"];
            string date = Request.QueryString["date"];

            lblDate.Text = date;

            if (patientnumber == null || date == null || patientnumber == "" || date == "")
            {
                Response.Redirect("/AdmittedPatients.aspx");
                return;
            }

             Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

             try
             {
                 string query = "select p.FIRSTNAME, p.LASTNAME "
                             + "from patient p "
                             + "where p.PATIENTNUMBER = " + patientnumber;

                 using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                 {
                     if (ds.Tables[0].Rows.Count > 0)
                     {
                         DataRow row = ds.Tables[0].Rows[0];
                         string patientName = row.Field<string>(1) + ", " + row.Field<string>(0);

                         lblPatientName.Text = patientName;
                     }
                     else
                     {
                     }
                 }

                 query = "select e.EMPLOYEEID, e.FIRSTNAME, e.LASTNAME "
                                + "from employee e "
                                + "where e.employeeid in (select employeeid from nurse) "
                                + "order by e.lastname, e.firstname";

                 using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                 {
                     if (ds.Tables[0].Rows.Count > 0)
                     {
                         foreach (DataRow row in ds.Tables[0].Rows)
                         {
                             string employeeId = row.Field<string>(0);
                             string nurseName = row.Field<string>(1) + " " + row.Field<string>(2) + " R.N.";

                             ListItem item = new ListItem();
                             item.Text = nurseName;
                             item.Value = employeeId;

                             ddlNurseList.Items.Add(item);
                         }
                     }
                     else
                     {
                     }
                 }
             }
             catch (SqlException sqle)
             {

             }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/AdmittedPatients.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string patientnumber = Request.QueryString["patientnumber"];
            string date = Request.QueryString["date"];

            string nurseID = ddlNurseList.SelectedValue;
            string shift = ddlShift.SelectedValue;

            string query = String.Format("insert into NURSECARESFOR values ('{0}', {1}, to_date('{2}', 'mm/dd/yyyy hh:mi:ss am'), '{3}')",
                nurseID, patientnumber, date, shift);

            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");
            db.ExecuteNonQuery(CommandType.Text, query);

            Response.Redirect("/AdmittedPatients.aspx");
        }
    }
}