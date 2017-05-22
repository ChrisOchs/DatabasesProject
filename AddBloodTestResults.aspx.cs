using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1
{
    public partial class AddBloodTestResults : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            string patientNumber = Request.QueryString["patientnumber"];

            if (patientNumber == null || patientNumber == "")
            {
                Response.Redirect("/Patient.aspx");
            }

            string query = "select pa.firstname, pa.lastname "
                    + "from patient pa "
                    + "where pa.patientnumber = " + patientNumber;

            using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    string firstName = row.Field<string>(0);
                    string lastName = row.Field<string>(1);

                    lblPatientName.Text = firstName + " " + lastName;
                }
                else
                {
                    //TODO: Error
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Patient.aspx?patientnumber=" + Request.QueryString["patientnumber"]);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string errors = "Error Encountered: <br>";
            bool error = false;

            if (txtBloodSugar.Text == "")
            {
                errors += "Valid Blood Sugar Required. <br>";
                error = true;
            }

            if (txtHDL.Text == "")
            {
                errors += "Valid HDL Required. <br>";
                error = true;
            }

            if (txtLDL.Text == "")
            {
                errors += "Valid LDL Required. <br>";
                error = true;
            }

            if (txtTriglycerides.Text == "")
            {
                errors += "Valid Triglycerides Required. <br>";
                error = true;
            }

            if (error)
            {
                lblResult.Text = errors;
                return;
            }

            string patientNumber = Request.QueryString["patientnumber"];
            decimal bloodSugar = Decimal.Parse(txtBloodSugar.Text);
            decimal HDL = Decimal.Parse(txtHDL.Text);
            decimal LDL = Decimal.Parse(txtLDL.Text);
            decimal triglycerides = Decimal.Parse(txtTriglycerides.Text);

            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            string query = String.Format("insert into medicaldata values ('{0}', '{1}', '{2}', '{3}', '{4}', to_date('{5}', 'mm/dd/yyyy hh:mi:ss am'))",
                patientNumber, bloodSugar, HDL, LDL, triglycerides, DateTime.Now);

            db.ExecuteNonQuery(CommandType.Text, query);

            Response.Redirect("Patient.aspx?patientnumber=" + patientNumber);
        }
    }
}