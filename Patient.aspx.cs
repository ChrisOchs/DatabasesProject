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
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string patientNum = Request.QueryString["patientnumber"];

            TableRow subtitleRow = new TableRow();
            TableCell subtitleCell = new TableCell();
            Label subtitleLabel = new Label();

            if (patientNum == null || patientNum == "")
            {
                subtitleLabel.Text = "Newark Medical Associates Patient List";
                subtitleLabel.Font.Size = 14;
                subtitleCell.HorizontalAlign = HorizontalAlign.Center;
                subtitleCell.Controls.Add(subtitleLabel);

                subtitleRow.Cells.Add(subtitleCell);

                tblContent.Rows.Add(subtitleRow);

                loadPatientList();
            }
            else
            {
                subtitleLabel.Text = "Patient Details";
                subtitleLabel.Font.Size = 14;
                subtitleCell.HorizontalAlign = HorizontalAlign.Center;
                subtitleCell.Controls.Add(subtitleLabel);

                subtitleRow.Cells.Add(subtitleCell);

                tblContent.Rows.Add(subtitleRow);

                loadPatientInformation(patientNum);
            }
        }

        private void loadPatientList()
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            TableRow patientListRow = new TableRow();
            TableCell patientListCell = new TableCell();
            patientListCell.HorizontalAlign = HorizontalAlign.Center;
            patientListRow.Cells.Add(patientListCell);
            tblContent.Rows.Add(patientListRow);

            try
            {
                string query = "select pa.patientnumber, pa.firstname, pa.lastname, emp.employeeid, emp.firstname, emp.lastname "
                    + "from patient pa, employee emp "
                    + "where pa.primaryphysician = emp.employeeid "
                    + "order by pa.lastname, pa.firstname";

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Table patientListTbl = new Table();
                        TableRow titleRow = new TableRow();

                        string[] titles = { "Patient Number", "Last Name", "First Name", "Primary Physician" };

                        foreach(string title in titles)
                        {
                            TableCell cell = new TableCell();
                            Label titleLabel = new Label();
                            titleLabel.Text = title;
                            titleLabel.Font.Bold = true;
                            titleLabel.Font.Size = 10;

                            cell.Width = Unit.Percentage(0.25);
                            cell.Controls.Add(titleLabel);
                            titleRow.Cells.Add(cell);
                        }

                        patientListTbl.Width = Unit.Percentage(80);
                        patientListTbl.Rows.Add(titleRow);
                        patientListCell.Controls.Add(patientListTbl);

                        int c = 1;

                        foreach(DataRow row in ds.Tables[0].Rows)
                        {
                            TableRow patientRow = new TableRow();

                            TableCell pNumCell = new TableCell();
                            HyperLink pNumLink = new HyperLink();

                            decimal patientNumber = row.Field<decimal>(0);

                            pNumLink.Text = patientNumber.ToString();
                            pNumLink.NavigateUrl = "/Patient.aspx?patientnumber=" + patientNumber;

                            pNumCell.Controls.Add(pNumLink);

                            TableCell lastNameCell = new TableCell();
                            TableCell firstNameCell = new TableCell();
                            TableCell physicianCell = new TableCell();

                            Label lastNamelbl = new Label();
                            Label firstNamelbl = new Label();
                            HyperLink physicianLink = new HyperLink();

                            lastNamelbl.Text = row.Field<string>(2);
                            firstNamelbl.Text = row.Field<string>(1);

                            physicianLink.Text = "Dr. " + row.Field<string>(4) + " " + row.Field<string>(5);
                            physicianLink.NavigateUrl = "/Physician.aspx?employeeid=" + row.Field<string>(3);

                            lastNameCell.Controls.Add(lastNamelbl);
                            firstNameCell.Controls.Add(firstNamelbl);
                            physicianCell.Controls.Add(physicianLink);

                            patientRow.Cells.Add(pNumCell);
                            patientRow.Cells.Add(lastNameCell);
                            patientRow.Cells.Add(firstNameCell);
                            patientRow.Cells.Add(physicianCell);

                            if (c % 2 == 0)
                            {
                                patientRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                            }

                            c++;
                            patientListTbl.Rows.Add(patientRow);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
            }
        }

        private void loadPatientInformation(string patientNumber)
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            TableRow patientInfoRow = new TableRow();
            TableCell patientInfoCell = new TableCell();
            patientInfoCell.HorizontalAlign = HorizontalAlign.Center;
            patientInfoRow.Cells.Add(patientInfoCell);
            tblContent.Rows.Add(patientInfoRow);

            try
            {
                string query = "select pa.firstname, pa.lastname, pa.ssn, pa.address, pa.phonenumber, pa.dateofbirth, "
                     +"pa.gender, pa.bloodtype, emp.employeeid, emp.firstname, emp.lastname "
                    + "from patient pa, employee emp "
                    + "where pa.primaryphysician = emp.employeeid "
                    + "and pa.patientnumber = " + patientNumber;

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];

                        string firstName = row.Field<string>(0);
                        string lastName = row.Field<string>(1);
                        string ssn = row.Field<string>(2);
                        string address = row.Field<string>(3);
                        string phoneNum = row.Field<string>(4);
                        DateTime birthDate = row.Field<DateTime>(5);
                        string gender = row.Field<string>(6);
                        string bloodType = row.Field<string>(7);

                        string physicianId = row.Field<string>(8);
                        string physicianName = "Dr. " + row.Field<string>(9) + " " + row.Field<string>(10);

                        Label patientNameLbl = new Label();
                        patientNameLbl.Text = firstName + " " + lastName + "   (Patient #" + patientNumber + ")";
                        patientNameLbl.Font.Size = 14;
                        patientNameLbl.Font.Bold = true;

                        Label ssnLbl = new Label();
                        ssnLbl.Text = "<b>Social Security #: </b>" + ssn;

                        Label addressLbl = new Label();
                        addressLbl.Text = "<b>Home Address: </b>" + address;

                        Label phoneLbl = new Label();
                        phoneLbl.Text = "<b>Preferred Phone #: </b>" + phoneNum;

                        Label birthDateLbl = new Label();
                        birthDateLbl.Text = "<b>Date of Birth: </b>" + String.Format("{0:M/d/yyyy}", birthDate);

                        Label genderLbl = new Label();
                        genderLbl.Text = "<b>Gender: </b>" + gender;

                        Label bloodTypeLbl = new Label();
                        bloodTypeLbl.Text = "Blood Type: " + bloodType;
                        bloodTypeLbl.Font.Bold = true;
                        bloodTypeLbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF2222");

                        Label primaryPhysicianLbl = new Label();
                        primaryPhysicianLbl.Text = "<b>Primary Physician: </b>";

                        HyperLink primaryPhysicianLink = new HyperLink();
                        primaryPhysicianLink.Text = physicianName;
                        primaryPhysicianLink.NavigateUrl = "/Physician.aspx?employeeid=" + physicianId;

                        Table generalInfoTbl = new Table();
                        TableRow patientNameRow = new TableRow();
                        TableCell nameCell = new TableCell();
                        nameCell.Controls.Add(patientNameLbl);
                        patientNameRow.Cells.Add(nameCell);

                        generalInfoTbl.Width = Unit.Percentage(80);
                        generalInfoTbl.Rows.Add(patientNameRow);

                        TableRow generalInfoRow = new TableRow();
                        TableCell generalInfoCell = new TableCell();

                        generalInfoCell.Controls.Add(bloodTypeLbl);
                        generalInfoCell.Controls.Add(new LiteralControl("<br />"));
                        generalInfoCell.Controls.Add(ssnLbl);
                        generalInfoCell.Controls.Add(new LiteralControl("<br />"));
                        generalInfoCell.Controls.Add(birthDateLbl);
                        generalInfoCell.Controls.Add(new LiteralControl("<br />"));
                        generalInfoCell.Controls.Add(genderLbl);
                        generalInfoCell.Controls.Add(new LiteralControl("<br />"));
                        generalInfoCell.Controls.Add(primaryPhysicianLbl);
                        generalInfoCell.Controls.Add(primaryPhysicianLink);
                        generalInfoCell.Controls.Add(new LiteralControl("<br />"));
                        generalInfoCell.Controls.Add(addressLbl);
                        generalInfoCell.Controls.Add(new LiteralControl("<br />"));
                        generalInfoCell.Controls.Add(phoneLbl);

                        generalInfoRow.Cells.Add(generalInfoCell);
                        generalInfoTbl.Rows.Add(generalInfoRow);

                        patientInfoCell.Controls.Add(generalInfoTbl);
                    }
                    else
                    {
                        //TODO: Error
                    }
                }

                query = "select c.consultationid, a.DESCRIPTION "
                       + "from consultation c, allergydiagnosis ad, allergy a "
                       + "where c.CONSULTATIONID = ad.CONSULTATIONID " 
                       + "and a.CODE = ad.ALLERGYCODE "
                       + "and c.patientnumber = " + patientNumber
                       + " order by c.consultationdate desc";

                Dictionary<string, List<string>> allergies = new Dictionary<string, List<string>>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string consultationId = row.Field<string>(0);
                            string description = row.Field<string>(1);

                            if (!allergies.ContainsKey(consultationId))
                            {
                                allergies.Add(consultationId, new List<string>());
                            }

                            allergies[consultationId].Add(description);
                        }
                    }
                    else
                    {
                        //TODO: Error
                    }
                }

                query = "select c.consultationid, i.DESCRIPTION, id.diagnosiscomment "
                    + "from consultation c, illnessdiagnosis id, illness i "
                    + "where c.CONSULTATIONID = id.CONSULTATIONID "
                    + "and i.CODE = id.ILLNESSCODE "
                    + "and c.patientnumber = " + patientNumber
                    + " order by c.consultationdate desc";

                Dictionary<string, List<string[]>> illnesses = new Dictionary<string, List<string[]>>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string consultationId= row.Field<string>(0);
                            string description = row.Field<string>(1);
                            string comment = row.Field<string>(2);

                            if (!illnesses.ContainsKey(consultationId))
                            {
                                illnesses.Add(consultationId, new List<string[]>());
                            }

                            illnesses[consultationId].Add(new string[] { description, comment });
                        }
                    }
                    else
                    {
                        //TODO: Error
                    }
                }

                query = "select c.consultationid, e.firstname, e.lastname, e.employeeid, c.type, c.consultationdate, c.notes "
                       + "from consultation c, employee e "
                       + "where c.physicianid = e.employeeid "
                       + "and c.patientnumber = " + patientNumber
                       + " order by c.consultationdate desc";

                List<object[]> consultationDetails = new List<object[]>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string consultationId = row.Field<string>(0);
                            string physicianName = "Dr. " + row.Field<string>(1) + " " + row.Field<string>(2);
                            string physicianId = row.Field<string>(3);
                            string type = row.Field<string>(4);
                            DateTime consultationDate = row.Field<DateTime>(5);
                            string notes = row.Field<string>(6);

                            consultationDetails.Add(new object[] { consultationId, physicianName, physicianId, type, consultationDate, notes });
                        }
                    }
                    else
                    {
                        //TODO: Error
                    }
                }

                query = "select c.consultationid, m.code, m.name, p.dosage, p.duration, p.frequency "
                        + "from consultation c, prescription p, medicine m "
                        + "where c.consultationid = p.consultationid "
                        + "and p.medicinecode = m.code "
                        + "and c.patientnumber = " + patientNumber
                        + " order by consultationdate desc ";

                Dictionary<string, List<object[]>> prescriptions = new Dictionary<string, List<object[]>>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string consultationId = row.Field<string>(0);
                            string code = row.Field<string>(1);
                            string medName = row.Field<string>(2);
                            decimal dosage = row.Field<decimal>(3);
                            decimal duration = row.Field<decimal>(4);
                            string frequency = row.Field<string>(5);

                            if (!prescriptions.ContainsKey(consultationId))
                            {
                                prescriptions.Add(consultationId, new List<object[]>());
                            }

                            prescriptions[consultationId].Add(new object[] { code, medName, dosage, duration, frequency });
                        }
                    }
                    else
                    {
                        //TODO: Error
                    }
                }

                query = "select bloodsugar, hdl, ldl, triglycerides, datadate "
                    + "from medicaldata "
                    + "where patientnumber = " + patientNumber
                    + "order by datadate desc";

                List<object[]> medicalTests = new List<object[]>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            decimal bloodSugar = row.Field<decimal>(0);
                            decimal hdl = row.Field<decimal>(1);
                            decimal ldl = row.Field<decimal>(2);
                            decimal triglycerides = row.Field<decimal>(3);
                            DateTime time = row.Field<DateTime>(4);
                            double totalCholesterol = Convert.ToDouble(hdl + ldl) + 0.2 * Convert.ToDouble(triglycerides);

                            medicalTests.Add(new object[] { bloodSugar, hdl, ldl, triglycerides, time, totalCholesterol });
                        }
                    }
                    else
                    {
                        //TODO: Error
                    }
                }

                query = "select distinct s.SURGERYID, s.SURGERYDATE, st.NAME "
                        + "from surgery s, surgerytype st "
                        + "where s.SURGERYTYPECODE = st.CODE "
                        + "and s.PATIENTNUMBER = " + patientNumber
                        + " order by s.SURGERYDATE";

                List<object[]> surgeries = new List<object[]>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string surgeryId = row.Field<string>(0);
                            DateTime surgeryDate = row.Field<DateTime>(1);
                            string surgeryType = row.Field<string>(2);

                            surgeries.Add(new object[] { surgeryId, surgeryDate, surgeryType });
                        }
                    }
                    else
                    {
                        //TODO: Error
                    }
                }

                query = "select distinct pa.DATEADMITTED, pa.DURATION, cb.WING, cb.ROOMNUMBER, cb.BEDLETTER "
                        + "from patientadmitted pa, clinicbed cb "
                        + "where pa.ROOMID = cb.BEDID "
                        + "and pa.patientnumber = " + patientNumber;

                List<object[]> admissions = new List<object[]>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            DateTime admissionDate = row.Field<DateTime>(0);
                            decimal admissionDuration = row.Field<decimal>(1);
                            string wing = row.Field<string>(2);
                            decimal roomNumber = row.Field<decimal>(3);
                            string letter = row.Field<string>(4);

                            admissions.Add(new object[] { admissionDate, admissionDuration, wing, roomNumber, letter });
                        }
                    }
                    else
                    {
                        //TODO: Error
                    }
                }

                patientInfoCell.Controls.Add(new LiteralControl("<p />"));
                patientInfoCell.Controls.Add(new LiteralControl("<p />"));

                Table medicalDataTable = new Table();
                medicalDataTable.Width = Unit.Percentage(95);

                TableRow dataRow = new TableRow();
                TableCell medicalTestCell = new TableCell();

                Label testDataLbl = new Label();
                testDataLbl.Text = "Recent Blood Test Results";
                testDataLbl.Font.Size = 12;
                testDataLbl.Font.Bold = true;

                medicalTestCell.Controls.Add(testDataLbl);
                medicalTestCell.Controls.Add(new LiteralControl("<br />"));

                int c = 1; 

                foreach (object[] data in medicalTests)
                {
                    string entry = String.Format("{0:M/d/yyyy} | <b>Blood Sugar:</b> {1:0.##} | <b>HDL:</b> {2:0.##} | <b>LDL:</b> {3:0.##} | <b>Triglycerides:</b> {4:0.##} | <b>Total Cholesterol:</b> {5:0.##}",
                            data[4], data[0], data[1], data[2], data[3], data[5]);

                    Label entryLabel = new Label();
                    entryLabel.Text = entry;

                    if (c % 2 == 0)
                    {
                        entryLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                    }

                    c++;

                    entryLabel.Width = Unit.Percentage(100);
                    medicalTestCell.Controls.Add(entryLabel);
                    medicalTestCell.Controls.Add(new LiteralControl("<br />"));
                }

                HyperLink addDataLink = new HyperLink();
                addDataLink.Text = "Add New Test Results";
                addDataLink.NavigateUrl = "/AddBloodTestResults.aspx?patientnumber=" + patientNumber;

                medicalTestCell.Controls.Add(addDataLink);

                medicalTestCell.Width = Unit.Percentage(75);
                dataRow.Cells.Add(medicalTestCell);

                TableCell allergyCell = new TableCell();

                Label allergyDataLbl = new Label();
                allergyDataLbl.Text = "Known Allergies";
                allergyDataLbl.Font.Size = 12;
                allergyDataLbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF3333");
                allergyDataLbl.Font.Bold = true;

                allergyCell.Controls.Add(allergyDataLbl);
                allergyCell.Controls.Add(new LiteralControl("<br />"));

                foreach (var allAllergies in allergies.Values)
                {
                    foreach (string allergy in allAllergies)
                    {
                        Label entryLabel = new Label();
                        entryLabel.Text = allergy;

                        allergyCell.Controls.Add(entryLabel);
                        allergyCell.Controls.Add(new LiteralControl("<br />"));
                    }
                }

                allergyCell.BorderWidth = 2;
                allergyCell.Style.Add("padding", "4px");
                allergyCell.BorderColor = System.Drawing.ColorTranslator.FromHtml("#FF3333");
                dataRow.Cells.Add(allergyCell);

                medicalDataTable.Rows.Add(dataRow);
                patientInfoCell.Controls.Add(medicalDataTable);

                patientInfoCell.Controls.Add(new LiteralControl("<p />"));
                patientInfoCell.Controls.Add(new LiteralControl("<p />"));

                Label admissionLbl = new Label();
                admissionLbl.Text = "ADMISSIONS";
                admissionLbl.Font.Size = 14;
                admissionLbl.Font.Bold = true;

                patientInfoCell.Controls.Add(admissionLbl);
                patientInfoCell.Controls.Add(new LiteralControl("<p />"));

                Table admissionTbl = new Table();
                admissionTbl.Width = Unit.Percentage(80);
                admissionTbl.BorderWidth = 2;
                admissionTbl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");

                TableRow admissionTitleRow = new TableRow();

                //new object[] { admissionDate, admissionDuration, wing, roomNumber, letter }

                Label admissionDateLabel = new Label();
                admissionDateLabel.Text = "Admission Date";
                admissionDateLabel.Font.Bold = true;

                Label admissionDurationLabel = new Label();
                admissionDurationLabel.Text = "Admission Duration";
                admissionDurationLabel.Font.Bold = true;

                Label admissionLocationLabel = new Label();
                admissionLocationLabel.Text = "Admitted To";
                admissionLocationLabel.Font.Bold = true;

                TableCell admissionDateCell = new TableCell();
                admissionDateCell.Controls.Add(admissionDateLabel);
                admissionDateCell.Width = Unit.Percentage(33);

                TableCell durationLabelCell = new TableCell();
                durationLabelCell.Controls.Add(admissionDurationLabel);
                durationLabelCell.Width = Unit.Percentage(33);

                TableCell locationLabelCell = new TableCell();
                locationLabelCell.Controls.Add(admissionLocationLabel);
                locationLabelCell.Width = Unit.Percentage(33);

                admissionTitleRow.Cells.Add(admissionDateCell);
                admissionTitleRow.Cells.Add(durationLabelCell);
                admissionTitleRow.Cells.Add(locationLabelCell);

                admissionTbl.Rows.Add(admissionTitleRow);

                c = 1;

                foreach (object[] admission in admissions)
                {
                    TableRow entryRow = new TableRow();

                    TableCell dateCell = new TableCell();
                    TableCell durationCell = new TableCell();
                    TableCell locationCell = new TableCell();

                    Label dateLabel = new Label();
                    dateLabel.Text = admission[0].ToString();
                    dateCell.Controls.Add(dateLabel);

                    Label durationLabel = new Label();
                    durationLabel.Text = admission[1].ToString() + " days";
                    durationCell.Controls.Add(durationLabel);

                    Label locationLabel = new Label();
                    locationLabel.Text = String.Format("<b>Wing:</b> {0} / <b>Room #:</b> {1} / <b>Bed: </b> {2}", admission[2], admission[3], admission[4]);
                    locationCell.Controls.Add(locationLabel);

                    entryRow.Cells.Add(dateCell);
                    entryRow.Cells.Add(durationCell);
                    entryRow.Cells.Add(locationCell);

                    if (c % 2 == 0)
                    {
                        entryRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                    }

                    admissionTbl.Rows.Add(entryRow);
                }

                TableRow admitRow = new TableRow();
                TableCell admitCell = new TableCell();

                HyperLink admitLink = new HyperLink();
                admitLink.Text = "Admit Patient";
                admitLink.NavigateUrl = "/AdmitPatient.aspx?patientnumber=" + patientNumber;

                admitCell.Controls.Add(admitLink);
                admitRow.Cells.Add(admitCell);
                admissionTbl.Rows.Add(admitRow);

                patientInfoCell.Controls.Add(admissionTbl);

                patientInfoCell.Controls.Add(new LiteralControl("<p />"));
                patientInfoCell.Controls.Add(new LiteralControl("<p />"));

                Label consultationLbl = new Label();
                consultationLbl.Text = "PREVIOUS CONSULTATIONS";
                consultationLbl.Font.Size = 14;
                consultationLbl.Font.Bold = true;

                patientInfoCell.Controls.Add(consultationLbl);
                patientInfoCell.Controls.Add(new LiteralControl("<p />"));

                HyperLink addConsultLink = new HyperLink();
                addConsultLink.Text = "Add New Consultation";
                addConsultLink.NavigateUrl = "/AddConsultation.aspx?patientnumber=" + patientNumber;

                patientInfoCell.Controls.Add(addConsultLink);
                patientInfoCell.Controls.Add(new LiteralControl("<p />"));

                foreach (object[] consultation in consultationDetails)
                {
                    string consultationId = (string)consultation[0];
                    string physicianName = (string)consultation[1];
                    string physicianId = (string)consultation[2];
                    string type = (string)consultation[3];
                    DateTime date = (DateTime)consultation[4];
                    string notes = (string)consultation[5];


                    List<string[]> illnessDiagnosis = (illnesses.ContainsKey(consultationId)) ? illnesses[consultationId] : new List<string[]>();
                    List<string> allergyDiagnosis = (allergies.ContainsKey(consultationId)) ? allergies[consultationId] : new List<string>();
                    List<object[]> scripts = (prescriptions.ContainsKey(consultationId)) ? prescriptions[consultationId] : new List<object[]>();

                    Table consultationTable = new Table();
                    consultationTable.BorderWidth = 2;
                    consultationTable.BorderColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");

                    consultationTable.Width = Unit.Percentage(90);
                    TableRow titleRow = new TableRow();

                    TableCell typeCell = new TableCell();
                    typeCell.Width = Unit.Percentage(10);
                    TableCell physicianCell = new TableCell();
                    physicianCell.Width = Unit.Percentage(70);
                    TableCell dateCell = new TableCell();
                    dateCell.Width = Unit.Percentage(20);

                    Label physicianLabel = new Label();
                    physicianLabel.Text = "Consulting Physician: ";
                    physicianLabel.Font.Bold = true;

                    HyperLink physicianLink = new HyperLink();
                    physicianLink.Text = physicianName;
                    physicianLink.Font.Bold = true;
                    physicianLink.NavigateUrl = "/Physician.aspx?physicianid=" + physicianId;

                    physicianCell.Controls.Add(physicianLabel);
                    physicianCell.Controls.Add(physicianLink);

                    Label dateLabel = new Label();
                    dateLabel.Text = date.ToString();
                    dateLabel.Font.Bold = true;
                    dateCell.Controls.Add(dateLabel);

                    Label typeLabel = new Label();
                    typeLabel.Text = type;
                    typeLabel.Font.Bold = true;
                    typeCell.Controls.Add(typeLabel);

                    titleRow.Cells.Add(typeCell);
                    titleRow.Cells.Add(physicianCell);
                    titleRow.Cells.Add(dateCell);

                    consultationTable.Rows.Add(titleRow);

                    TableRow notesRow = new TableRow();

                    TableCell notesTitleCell = new TableCell();
                    Label notesTitleLabel = new Label();
                    notesTitleLabel.Text = "Notes";
                    notesTitleLabel.Font.Bold = true;

                    notesTitleCell.Controls.Add(notesTitleLabel);
                    notesTitleCell.Width = Unit.Percentage(10);

                    TableCell notesCell = new TableCell();
                    Label notesLabel = new Label();
                    notesLabel.Text = notes;
                    notesCell.Controls.Add(notesLabel);
                    notesCell.BorderWidth = 1;

                    notesRow.Cells.Add(notesTitleCell);
                    notesRow.Cells.Add(notesCell);

                    TableRow diagnosisRow = new TableRow();

                    TableCell diagnosisTitleCell = new TableCell();
                    Label diagnosisTitleLabel = new Label();
                    diagnosisTitleLabel.Text = "Diagnosis";
                    diagnosisTitleLabel.Font.Bold = true;

                    diagnosisTitleCell.Controls.Add(diagnosisTitleLabel);
                    diagnosisTitleCell.Width = Unit.Percentage(10);

                    TableCell diagnosisCell = new TableCell();
                    diagnosisCell.BorderWidth = 1;

                    if (illnessDiagnosis == null && allergyDiagnosis == null)
                    {
                        Label illnessLabel = new Label();
                        illnessLabel.Text = "No Diagnosis";
                        illnessLabel.Font.Bold = true;
                        illnessLabel.Font.Italic = true;
                        diagnosisCell.Controls.Add(illnessLabel);
                    }
                    
                    if (illnessDiagnosis != null)
                    {
                        Label illnessLabel = new Label();
                        illnessLabel.Text = "Illnesses";
                        illnessLabel.Font.Bold = true;
                        illnessLabel.Font.Italic = true;
                        diagnosisCell.Controls.Add(illnessLabel);
                        diagnosisCell.Controls.Add(new LiteralControl("<br />"));

                        int count = 1;

                        foreach(string[] diagnosis in illnessDiagnosis) 
                        {
                            Label diagnosisLabel = new Label();
                            diagnosisLabel.Text = String.Format("<b>{0}</b> | {1}", diagnosis[0], diagnosis[1]);
                            diagnosisLabel.Width = Unit.Percentage(100);

                            if (count % 2 == 0)
                            {
                                diagnosisLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                            }

                            count++;

                            diagnosisCell.Controls.Add(diagnosisLabel);
                            diagnosisCell.Controls.Add(new LiteralControl("<br />"));
                        }

                        diagnosisCell.Controls.Add(new LiteralControl("<br />"));
                    }

                    if (allergyDiagnosis != null)
                    {
                        Label allergyLabel = new Label();
                        allergyLabel.Text = "Allergies";
                        allergyLabel.Font.Bold = true;
                        allergyLabel.Font.Italic = true;
                        diagnosisCell.Controls.Add(allergyLabel);
                        diagnosisCell.Controls.Add(new LiteralControl("<br />"));

                        int count = 1;

                        foreach (string diagnosis in allergyDiagnosis)
                        {
                            Label diagnosisLabel = new Label();
                            diagnosisLabel.Text = String.Format("<b>{0}</b>", diagnosis);
                            diagnosisLabel.Width = Unit.Percentage(100);

                            if (count % 2 == 0)
                            {
                                diagnosisLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                            }

                            count++;

                            diagnosisCell.Controls.Add(diagnosisLabel);
                            diagnosisCell.Controls.Add(new LiteralControl("<br />"));
                        }
                    }

                    diagnosisRow.Cells.Add(diagnosisTitleCell);
                    diagnosisRow.Cells.Add(diagnosisCell);

                    consultationTable.Rows.Add(notesRow);
                    consultationTable.Rows.Add(diagnosisRow);

                    TableRow prescriptionRow = new TableRow();

                    TableCell prescriptionTitleCell = new TableCell();
                    Label prescriptionTitleLabel = new Label();
                    prescriptionTitleLabel.Text = "Prescriptions";
                    prescriptionTitleLabel.Font.Bold = true;

                    prescriptionTitleCell.Controls.Add(prescriptionTitleLabel);
                    notesTitleCell.Width = Unit.Percentage(10);

                    TableCell prescriptionCell = new TableCell();
                    prescriptionCell.BorderWidth = 1;

                    if (scripts != null)
                    {
                        foreach (object[] prescription in scripts)
                        {
                            HyperLink medicineLink = new HyperLink();
                            medicineLink.Text = (string)prescription[1];
                            medicineLink.NavigateUrl = "/Medication.aspx?code=" + prescription[0];

                            prescriptionCell.Controls.Add(medicineLink);

                            Label detailsLabel = new Label();
                            detailsLabel.Text = String.Format("&nbsp&nbsp&nbsp {0}mg / {1} days / {2}", prescription[2], prescription[3], prescription[4]);
                            prescriptionCell.Controls.Add(detailsLabel);

                            prescriptionCell.Controls.Add(new LiteralControl("<br />"));
                        }

                        diagnosisCell.Controls.Add(new LiteralControl("<br />"));
                    }

                    prescriptionRow.Cells.Add(prescriptionTitleCell);
                    prescriptionRow.Cells.Add(prescriptionCell);

                    consultationTable.Rows.Add(prescriptionRow);

                    patientInfoCell.Controls.Add(consultationTable);
                    patientInfoCell.Controls.Add(new LiteralControl("<p />"));
                }

                Label surgeryLbl = new Label();
                surgeryLbl.Text = "PREVIOUS SURGERIES";
                surgeryLbl.Font.Size = 14;
                surgeryLbl.Font.Bold = true;

                patientInfoCell.Controls.Add(surgeryLbl);
                patientInfoCell.Controls.Add(new LiteralControl("<p />"));

                Table surgeryTbl = new Table();
                surgeryTbl.Width = Unit.Percentage(80);
                surgeryTbl.BorderWidth = 2;
                surgeryTbl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");

                TableRow surgeryTitleRow = new TableRow();

                Label surgeryDateLabel = new Label();
                surgeryDateLabel.Text = "Surgery Date";
                surgeryDateLabel.Font.Bold = true;

                Label surgeryTypeLabel = new Label();
                surgeryTypeLabel.Text = "Surgery Type";
                surgeryTypeLabel.Font.Bold = true;

                Label surgeryDetailsLabel = new Label();
                surgeryDetailsLabel.Text = "Surgery Details";
                surgeryDetailsLabel.Font.Bold = true;

                TableCell dateLabelCell = new TableCell();
                dateLabelCell.Controls.Add(surgeryDateLabel);
                dateLabelCell.Width = Unit.Percentage(33);

                TableCell typeLabelCell = new TableCell();
                typeLabelCell.Controls.Add(surgeryTypeLabel);
                typeLabelCell.Width = Unit.Percentage(33);

                TableCell detailsLabelCell = new TableCell();
                detailsLabelCell.Controls.Add(surgeryDetailsLabel);
                detailsLabelCell.Width = Unit.Percentage(33);

                surgeryTitleRow.Cells.Add(dateLabelCell);
                surgeryTitleRow.Cells.Add(typeLabelCell);
                surgeryTitleRow.Cells.Add(detailsLabelCell);

                surgeryTbl.Rows.Add(surgeryTitleRow);

                c = 1;

                foreach (object[] surgery in surgeries)
                {
                    TableRow entryRow = new TableRow();

                    TableCell dateCell = new TableCell();
                    TableCell typeCell = new TableCell();
                    TableCell detailsCell = new TableCell();

                    Label dateLabel = new Label();
                    dateLabel.Text = surgery[1].ToString();
                    dateCell.Controls.Add(dateLabel);

                    Label typeLabel = new Label();
                    typeLabel.Text = (string)surgery[2];
                    typeCell.Controls.Add(typeLabel);

                    HyperLink detailsLink = new HyperLink();
                    detailsLink.Text = "Click For Details";
                    detailsLink.NavigateUrl = "/Surgery.aspx?surgeryid=" + (string)surgery[0];
                    detailsCell.Controls.Add(detailsLink);

                    entryRow.Cells.Add(dateCell);
                    entryRow.Cells.Add(typeCell);
                    entryRow.Cells.Add(detailsCell);

                    if (c % 2 == 0)
                    {
                        entryRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                    }

                    surgeryTbl.Rows.Add(entryRow);
                }

                TableRow addSurgeryRow = new TableRow();
                TableCell addSurgeryCell = new TableCell();

                HyperLink addSurgeryLink = new HyperLink();
                addSurgeryLink.Text = "Add New Surgery";
                addSurgeryLink.NavigateUrl = "/AddSurgery.aspx?patientnumber=" + patientNumber;

                addSurgeryCell.Controls.Add(addSurgeryLink);
                addSurgeryRow.Cells.Add(addSurgeryCell);
                surgeryTbl.Rows.Add(addSurgeryRow);

                patientInfoCell.Controls.Add(surgeryTbl);
                patientInfoCell.Controls.Add(new LiteralControl("<p />"));
                patientInfoCell.Controls.Add(new LiteralControl("<p />"));

            }
            catch (SqlException ex)
            {
                //TODO: Handle SQL error
            }
        }
    }
}