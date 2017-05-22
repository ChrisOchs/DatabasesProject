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
    public partial class AdmitPatient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string patientNumber = Request.QueryString["patientnumber"];

            if (patientNumber == null || patientNumber == "")
            {
                Response.Redirect("/Patient.aspx");
            }

             Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

             try
             {
                 string query = "select p.FIRSTNAME, p.LASTNAME "
                             + "from patient p "
                             + "where p.PATIENTNUMBER = " + patientNumber;

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

                 calAdmitDate.SelectedDate = DateTime.Today;
             }
             catch (SqlException sqle)
             {

             }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string patientNumber = Request.QueryString["patientnumber"];

            Response.Redirect("/Patient.aspx?patientnumber=" + patientNumber);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            try
            {
                string query = "select bedid "
                            + "from clinicbed "
                            + "where bedid not in (select roomid from patientadmitted) "
                            + "and rownum < 2 "
                            + "order by wing, roomnumber, bedletter";

                string roomId = null;

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        roomId = row.Field<string>(0);
                    }
                    else
                    {
                    }
                }

                if (roomId != null)
                {
                    string patientNumber = Request.QueryString["patientnumber"];
                    string date = calAdmitDate.SelectedDate.ToString();
                    int duration = Int32.Parse(txtDays.Text);

                    query = String.Format("insert into patientadmitted values({0}, '{1}', to_date('{2}', 'mm/dd/yyyy hh:mi:ss am'), {3})",
                        patientNumber, roomId, date, duration);

                    db.ExecuteNonQuery(CommandType.Text, query);

                    Response.Redirect("/Patient.aspx?patientnumber=" + patientNumber);
                }
                else
                {
                    //TODO: Handle error
                }
            }
            catch (SqlException sqle)
            {

            }
        }
    }
}