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
    public partial class AddSurgery : System.Web.UI.Page
    {
        private List<DropDownList> surgeonLists = new List<DropDownList>();
        private List<DropDownList> nurseLists = new List<DropDownList>();

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

                calSurgeryDate.SelectedDate = DateTime.Today;

                query = "select distinct e.employeeid, e.firstname, e.lastname "
                            + "from surgeon s, employee e "
                            + "where s.employeeid = e.employeeid "
                            + "order by e.lastname, e.firstname";

                List<string[]> surgeons = new List<string[]>();
                surgeons.Add(new string [] {"", "--    Select a Surgeon --"});

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string employeeId = row.Field<string>(0);
                            string name = "Dr. " + row.Field<string>(1) + " " + row.Field<string>(2);

                            surgeons.Add(new string[] { employeeId, name });
                        }
                    }
                    else
                    {

                    }
                }

                query = "select e.EMPLOYEEID, e.FIRSTNAME, e.LASTNAME "
                               + "from employee e "
                               + "where e.employeeid in (select employeeid from nurse) "
                               + "order by e.lastname, e.firstname";

                List<string[]> nurses = new List<string[]>();
                nurses.Add(new string[] {"", "--    Select a Nurse    --"});

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string employeeId = row.Field<string>(0);
                            string nurseName = row.Field<string>(1) + " " + row.Field<string>(2) + " R.N.";

                            nurses.Add(new string[] { employeeId, nurseName });
                        }
                    }
                    else
                    {
                    }
                }

                query = "select st.code, st.name "
                    + "from surgerytype st "
                    + "order by st.name";

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string code = row.Field<string>(0);
                            string name = row.Field<string>(1);

                            ddlSurgeryType.Items.Add(new ListItem(name, code));
                        }
                    }
                    else
                    {
                    }
                }

                for (int c = 0; c < 4; c++)
                {
                    DropDownList surgeonList = new DropDownList();
                    surgeonLists.Add(surgeonList);

                    foreach (string[] surgeon in surgeons)
                    {
                        surgeonList.Items.Add(new ListItem(surgeon[1], surgeon[0]));
                    }

                    pnlSurgeons.Controls.Add(surgeonList);
                    pnlSurgeons.Controls.Add(new LiteralControl("<p />"));
                }

                for (int c = 0; c < 4; c++)
                {
                    DropDownList nurseList = new DropDownList();
                    nurseLists.Add(nurseList);

                    foreach (string[] nurse in nurses)
                    {
                        nurseList.Items.Add(new ListItem(nurse[1], nurse[0]));
                    }

                    pnlNurses.Controls.Add(nurseList);
                    pnlNurses.Controls.Add(new LiteralControl("<p />"));
                }
            }
            catch (SqlException sqle)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string patientnumber = Request.QueryString["patientnumber"];

            Response.Redirect("/Patient.aspx?patientnumber=" + patientnumber);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string availableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            char[] idChars = new char[10];
            Random random = new Random();

            for (int c = 0; c < idChars.Length; c++)
            {
                idChars[c] = availableChars[random.Next(availableChars.Length)];
            }

            string patientNumber = Request.QueryString["patientnumber"];
            string surgeryId = new string(idChars);

            DateTime selectedDT = calSurgeryDate.SelectedDate;

            string date = String.Format("{0:MM/dd/yyyy} {1}:{2}:00 {3}", selectedDT, ddlHour.SelectedValue, ddlMinute.SelectedValue, ddlAM.SelectedValue);
            string type = ddlSurgeryType.SelectedValue;
            string theater = ddlSurgicalTheater.SelectedValue;

            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            string query = String.Format("insert into surgery values ('{0}', {1}, '{2}' , to_date('{3}', 'mm/dd/yyyy hh:mi:ss am'), '{4}')",
                surgeryId, patientNumber, type, date, theater);

            db.ExecuteNonQuery(CommandType.Text, query);

            foreach (DropDownList surgeonList in surgeonLists)
            {
                if (surgeonList.SelectedIndex > 0)
                {
                    query = String.Format("insert into suregonforsurgery values ('{0}', '{1}')", surgeonList.SelectedValue, surgeryId);
                    db.ExecuteNonQuery(CommandType.Text, query);
                }
            }

            foreach (DropDownList nurseList in nurseLists)
            {
                if (nurseList.SelectedIndex > 0)
                {
                    query = String.Format("insert into nurseforsurgery values ('{0}', '{1}')", nurseList.SelectedValue, surgeryId);
                    db.ExecuteNonQuery(CommandType.Text, query);
                }
            }

            Response.Redirect("/Patient.aspx?patientnumber=" + patientNumber);
        }
    }
}