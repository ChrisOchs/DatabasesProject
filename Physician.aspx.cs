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
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           /*
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");
            DataSet ds = db.ExecuteDataSet(CommandType.Text, "Select * from patient");
            //    SqlDataReader reader = ds;
            //   DataRowCollection 

            foreach (DataTable thisTable in ds.Tables)
            {
                // For each row, print the values of each column.
                foreach (DataRow myRow in thisTable.Rows)
                {
                    foreach (DataColumn myCol in thisTable.Columns)
                    {
                        Label3.Text += myRow[myCol].ToString();
                    }
                }
            }*/
        }

        protected void LoadData()
        {

            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");
            try
            {
                string str =  "select e.employeeid, e.firstname, e.lastname, p.specialty "
                    +"from employee e, physician p where e.EMPLOYEEID = p.EMPLOYEEID and e.EMPLOYEEID NOT IN (select employeeid from surgeon)"
                    +" and e.firstname like '%"+Fname.Text+"%' and e.lastname like '%"+Lname.Text+"%'";
                using (DataSet ds = db.ExecuteDataSet(CommandType.Text,str))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session.Add("Data", ds);
                        PhysEntries.DataSource = ds;
                        PhysEntries.DataBind();
                        //tcSearch.ActiveTabIndex = 1;
                        PhysEntries.Visible = true;
                        lblError.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "No data found for the search criteria!";
                        lblError.Visible = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message.ToString();
                lblError.Visible = true;
               
            }
        }
        protected void Physician_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {       
             try
                {
                    PhysEntries.PageIndex = e.NewPageIndex;
                    if (Session["data"] != string.Empty || Session["Data"] != null)
                    {
                        PhysEntries.DataSource = Session["data"];
                        PhysEntries.DataBind();
                    }
                    else
                        LoadData();
                }
                catch (Exception ex)
                {
                
                }
        }
        protected void txt_search(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void select_Physician(object sender, EventArgs e)
        {
            TabContainer1.ActiveTab = Physician_tap;

            //test.Text = PhysEntries.Rows[e.RowIndex].FindControl("employeeid");
            //    TextBox txtAcc = (TextBox)gvJEEntries.Rows[e.RowIndex].FindControl("txAccountNo");

        }
        protected void LoadPhysician(object sender, EventArgs e)
        {
           // test.Text = (TextBox)PhysEntries.Rows[e.RowIndex].FindControl("emplyeeid");
        }
    }
}