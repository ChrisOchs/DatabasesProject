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
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            try
            {
                string str = "select distinct e.employeeid, e.firstname, e.lastname "
                            + "from physician p, employee e "
                            + "where p.employeeid = e.employeeid "
                            + "order by e.lastname, e.firstname";

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, str))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string employeeId = row.Field<string>(0);
                            string name = "Dr. " + row.Field<string>(1) + " " + row.Field<string>(2);

                            ListItem item = new ListItem();
                            item.Text = name;
                            item.Value = employeeId;

                            ddlPhysician.Items.Add(item);
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            resetForm();
            lblResult.Text = "Form Reset";
        }

        private void resetForm()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtSSN1.Text = "";
            txtSSN2.Text = "";
            txtSSN3.Text = "";
            txtDay.Text = "";
            txtDOBMonth.Text = "";
            txtDOBYear.Text = "";
            ddlBloodType1.SelectedIndex = 0;
            ddlBloodType2.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;
            ddlPhysician.SelectedIndex = 0;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string errors = "Errors Found: <br>";
            bool error = false;

            if (txtFirstName.Text == "")
            {
                errors += "Valid first name required. <br>";
                error = true;
            }

            if (txtLastName.Text == "")
            {
                errors += "Valid last name required. <br>";
                error = true;
            }

            if (txtAddress.Text == "")
            {
                errors += "Valid address required. <br>";
                error = true;
            }

            if (txtPhone.Text == "")
            {
                errors += "Valid phone number required. <br>";
                error = true;
            }

            if (txtSSN1.Text == "" || txtSSN2.Text == "" || txtSSN3.Text == "")
            {
                errors += "Valid Social Security number missing. <br>";
                error = true;
            }
            else
            {
                int val1 = Int32.Parse(txtSSN1.Text);
                int val2 = Int32.Parse(txtSSN2.Text);
                int val3 = Int32.Parse(txtSSN3.Text);

                bool valError = false;

                if (val1 < 100 || val1 > 999) 
                {
                    valError = true;
                }

                if (val2 < 10 || val2 > 99)
                {
                    valError = true;
                }

                if(val3 < 1000 || val3 > 9999) 
                {
                    valError = true;
                }

                if (valError)
                {
                    errors += "Invalid Social Security Number provided.<br>";
                }
            }

            if (txtDay.Text == "" || txtDOBMonth.Text == "" || txtDOBYear.Text == "")
            {
                errors += "Valid date of birth missing. <br>";
                error = true;
            }
            else
            {
                int val1 = Int32.Parse(txtDay.Text);
                int val2 = Int32.Parse(txtDOBMonth.Text);
                int val3 = Int32.Parse(txtDOBYear.Text);

                bool valError = false;

                if (val1 < 1 || val1 > 31)
                {
                    valError = true;
                }

                if (val1 < 1 || val2 > 12)
                {
                    valError = true;
                }

                if (val3 < 1900 || val3 > DateTime.Now.Year)
                {
                    valError = true;
                }

                if (valError)
                {
                    errors += "Invalid date of birth provided.<br>";
                }
            }

            if (error)
            {
                lblResult.Text = errors;
                return;
            }

            int patientNumber = new Random().Next(1000000);
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string address = txtAddress.Text;
            string phone = txtPhone.Text;
            string SSN = txtSSN1.Text + txtSSN2.Text + txtSSN3.Text;

            string dob = txtDOBMonth.Text + "/" + txtDay.Text + "/" + txtDOBYear.Text;            
            string gender = ddlGender.SelectedValue;
            string bloodType = ddlBloodType1.SelectedValue + ddlBloodType2.SelectedValue;
            string physicianId = ddlPhysician.SelectedValue;

            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            string query = String.Format("insert into PATIENT values ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', to_date('{6}', 'mm/dd/yyyy'), '{7}', '{8}', '{9}')",
                patientNumber, SSN, firstName, lastName, address, phone, dob, gender, bloodType, physicianId);

            db.ExecuteNonQuery(CommandType.Text, query);

            lblResult.Text = firstName + " " + lastName + " successfully added.";
            resetForm();
        }
    }
}