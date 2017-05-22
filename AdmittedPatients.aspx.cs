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
    public partial class AdmittedPatients : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            string action = Request.QueryString["action"];

            if (action != null && action == "remove")
            {
                string nurseId = Request.QueryString["nurseid"];
                string patientNumber = Request.QueryString["patientnumber"];
                string date = Request.QueryString["date"];
                string shift = Request.QueryString["shift"];

                if (nurseId != null && patientNumber != null && date != null && shift != null)
                {
                    string query = String.Format("delete from nursecaresfor where employeeid = '{0}' "
                        + "and patientnumber = {1} "
                        + "and caredate = to_date('{2}', 'mm/dd/yyyy hh:mi:ss am') "
                        + "and shift = '{3}'", nurseId, patientNumber, date, shift);

                    db.ExecuteNonQuery(CommandType.Text, query);   
                }

                Response.Redirect("/AdmittedPatients.aspx");
                return;
            }

            try
            {
                string query = "select p.PATIENTNUMBER, p.FIRSTNAME, p.LASTNAME, pa.dateadmitted, pa.duration, cb.UNIT, cb.ROOMNUMBER, cb.WING, cb.BEDLETTER "
                            + "from patient p, patientadmitted pa, clinicbed cb "
                            + "where p.PATIENTNUMBER = pa.PATIENTNUMBER "
                            + "and cb.BEDID = pa.ROOMID "
                            + "order by pa.DATEADMITTED desc, p.lastname, p.firstname";

                Dictionary<DateTime, List<decimal>> patientsInOnDate = new Dictionary<DateTime, List<decimal>>();
                Dictionary<decimal, object[]> admittedPatients = new Dictionary<decimal, object[]>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            decimal patientNumber = row.Field<decimal>(0);
                            string firstName = row.Field<string>(1);
                            string lastName = row.Field<string>(2);
                            DateTime dateAdmitted = row.Field<DateTime>(3);
                            dateAdmitted = new DateTime(dateAdmitted.Year, dateAdmitted.Month, dateAdmitted.Day);
                            decimal duration = row.Field<decimal>(4);
                            DateTime expectedRelease = dateAdmitted.AddDays((int)duration);

                            decimal unit = row.Field<decimal>(5);
                            decimal roomNumber = row.Field<decimal>(6);
                            string wing = row.Field<string>(7);
                            string bedLetter = row.Field<string>(8);

                            for (int c = 0; c < duration; c++)
                            {
                                DateTime inHospitalDate = dateAdmitted.AddDays(c);

                                if (!patientsInOnDate.ContainsKey(inHospitalDate))
                                {
                                    patientsInOnDate.Add(inHospitalDate, new List<decimal>());
                                }

                                patientsInOnDate[inHospitalDate].Add(patientNumber);
                            }

                            admittedPatients.Add(patientNumber, new object[] { firstName, lastName, dateAdmitted, duration, expectedRelease, unit, roomNumber, wing, bedLetter });
                        }
                    }
                    else
                    {
                    }
                }

                query = "select e.EMPLOYEEID, e.FIRSTNAME, e.lastname, ncf.PATIENTNUMBER, ncf.CAREDATE, ncf.SHIFT "
                        + "from employee e, nursecaresfor ncf "
                        + "where e.EMPLOYEEID = ncf.EMPLOYEEID "
                        + "order by ncf.caredate desc, ncf.shift";

                List<object[]> nurses = new List<object[]>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string employeeid = row.Field<string>(0);
                            string employeeName = row.Field<string>(1) + " " + row.Field<string>(2) + ", R.N.";
                            decimal patientNumber = row.Field<decimal>(3);
                            DateTime careDate = row.Field<DateTime>(4);
                            string shift = row.Field<string>(5);

                            nurses.Add(new object[] { employeeid, employeeName, patientNumber, careDate, shift });
                        }
                    }
                    else
                    {
                    }
                }

                List<DateTime> sortedKeys = new List<DateTime>(patientsInOnDate.Keys);
                sortedKeys.Sort();

                for (int c = sortedKeys.Count - 1; c >= 0; c--)
                {
                    DateTime dt = sortedKeys[c];

                    Label dateLabel = new Label();
                    dateLabel.Text = String.Format("{0:MM/dd/yyyy}", dt);
                    dateLabel.Font.Size = 14;

                    contentPanel.Controls.Add(dateLabel);
                    contentPanel.Controls.Add(new LiteralControl("<br />"));

                    Table dateTable = new Table();

                    dateTable.Width = Unit.Percentage(90);

                    TableRow titleRow = new TableRow();

                    TableCell nameLabelCell = new TableCell();
                    nameLabelCell.Text = "Patient Name";
                    nameLabelCell.Font.Bold = true;

                    TableCell admittedLabelCell = new TableCell();
                    admittedLabelCell.Text = "Admitted Date";
                    admittedLabelCell.Width = Unit.Pixel(100);
                    admittedLabelCell.Font.Bold = true;

                    TableCell releaseLabelCell = new TableCell();
                    releaseLabelCell.Text = "Expected Release";
                    releaseLabelCell.Width = Unit.Pixel(100);
                    releaseLabelCell.Font.Bold = true;

                    TableCell nurseLabelCell = new TableCell();
                    nurseLabelCell.Text = "Nurse";
                    nurseLabelCell.Font.Bold = true;

                    TableCell unitLabelCell = new TableCell();
                    unitLabelCell.Text = "Unit";
                    unitLabelCell.Font.Bold = true;

                    TableCell roomNumLabelCell = new TableCell();
                    roomNumLabelCell.Text = "Room #";
                    roomNumLabelCell.Width = Unit.Pixel(48);
                    roomNumLabelCell.Font.Bold = true;

                    TableCell wingLabelCell = new TableCell();
                    wingLabelCell.Text = "Clinic Wing";
                    wingLabelCell.Width = Unit.Pixel(100);
                    wingLabelCell.Font.Bold = true;

                    TableCell bedLabelCell = new TableCell();
                    bedLabelCell.Text = "Bed";
                    bedLabelCell.Width = Unit.Pixel(32);
                    bedLabelCell.Font.Bold = true;

                    titleRow.Cells.Add(nameLabelCell);
                    titleRow.Cells.Add(admittedLabelCell);
                    titleRow.Cells.Add(releaseLabelCell);
                    titleRow.Cells.Add(nurseLabelCell);
                    titleRow.Cells.Add(unitLabelCell);
                    titleRow.Cells.Add(wingLabelCell);
                    titleRow.Cells.Add(roomNumLabelCell);
                    titleRow.Cells.Add(bedLabelCell);

                    dateTable.Rows.Add(titleRow);

                    List<decimal> patients = patientsInOnDate[dt];

                    int count = 1;

                    foreach (decimal patient in patients)
                    {
                        TableRow patientRow = new TableRow();

                        object[] patientInfo = admittedPatients[patient];

                        TableCell nameCell = new TableCell();
                        HyperLink patientLink = new HyperLink();
                        patientLink.Text = patientInfo[1] + ", " + patientInfo[0];
                        patientLink.NavigateUrl = "/Patient.aspx?patientnumber=" + patient;
                        nameCell.Controls.Add(patientLink);

                        TableCell admittedCell = new TableCell();
                        admittedCell.Text = String.Format("{0:MM/dd/yyyy}", patientInfo[2]);

                        TableCell releaseCell = new TableCell();
                        releaseCell.Text = String.Format("{0:MM/dd/yyyy}", patientInfo[4]);

                        TableCell nurseCell = new TableCell();

                        foreach (object[] nurse in nurses)
                        {
                            if((decimal)nurse[2] == patient && dt.Equals(nurse[3])) 
                            {
                                HyperLink nurseLink = new HyperLink();
                                nurseLink.Text = (string)nurse[1];
                                nurseLink.NavigateUrl = "/Nurse.aspx?nurseid=" + nurse[0];

                                Label shift = new Label();
                                shift.Text = "       " + nurse[4] + "       ";

                                HyperLink removeNurseLink = new HyperLink();
                                removeNurseLink.Font.Bold = true;
                                removeNurseLink.Text = "(X)";
                                removeNurseLink.NavigateUrl = "/AdmittedPatients.aspx?action=remove&nurseid=" + nurse[0] + "&patientnumber=" + patient + "&date=" + dt.ToString() + "&shift=" + nurse[4];
                                removeNurseLink.ToolTip = "Remove Nurse";

                                nurseCell.Controls.Add(nurseLink);
                                nurseCell.Controls.Add(shift);
                                nurseCell.Controls.Add(removeNurseLink);
                                nurseCell.Controls.Add(new LiteralControl("<br />"));
                            }
                        }

                        HyperLink addNurseLink = new HyperLink();
                        addNurseLink.Text = "Add Nurse";
                        addNurseLink.Font.Bold = true;
                        addNurseLink.NavigateUrl = String.Format("/AddNurseToSchedule.aspx?patientnumber={0}&date={1}", patient, dt);
                        nurseCell.Controls.Add(addNurseLink);

                        TableCell unitCell = new TableCell();
                        unitCell.Text = patientInfo[5].ToString();

                        TableCell roomNumberCell = new TableCell();
                        roomNumberCell.Text = patientInfo[6].ToString();

                        TableCell wingCell = new TableCell();
                        wingCell.Text = patientInfo[7].ToString();

                        TableCell bedLetterCell = new TableCell();
                        bedLetterCell.Text = patientInfo[8].ToString();

                        patientRow.Cells.Add(nameCell);
                        patientRow.Cells.Add(admittedCell);
                        patientRow.Cells.Add(releaseCell);
                        patientRow.Cells.Add(nurseCell);
                        patientRow.Cells.Add(unitCell);
                        patientRow.Cells.Add(wingCell);
                        patientRow.Cells.Add(roomNumberCell);
                        patientRow.Cells.Add(bedLetterCell);

                        if (count % 2 == 0)
                        {
                            patientRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                        }

                        count++;

                        dateTable.Rows.Add(patientRow);
                    }

                    contentPanel.Controls.Add(dateTable);

                    contentPanel.Controls.Add(new LiteralControl("<p />"));
                }

            }
            catch (SqlException ex)
            {
            }
        }
    }
}