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
    public partial class AddConsultation : System.Web.UI.Page
    {
        private class IllnessCombo
        {
            public DropDownList ddlDiagnosis;
            public TextBox comment;

            public IllnessCombo(DropDownList diagnosis, TextBox comment)
            {
                this.ddlDiagnosis = diagnosis;
                this.comment = comment;
            }
        }

        private class PrescriptionCombo
        {
            public DropDownList ddlPrescription;
            public TextBox dosageBox;
            public TextBox durationBox;
            public TextBox frequencyBox;

            public PrescriptionCombo(DropDownList ddlPrescription, TextBox dosageBox, TextBox durationBox, TextBox frequencyBox)
            {
                this.ddlPrescription = ddlPrescription;
                this.dosageBox = dosageBox;
                this.durationBox = durationBox;
                this.frequencyBox = frequencyBox;
            }
        }

        private class LazyLabel : Label
        {
            public LazyLabel(string text)
            {
                this.Text = text;
            }
        }

        List<IllnessCombo> illnessDiagnosis = new List<IllnessCombo>();
        List<PrescriptionCombo> prescriptionMeds = new List<PrescriptionCombo>();
        List<DropDownList> allergyDiagnosis = new List<DropDownList>();

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

                lblDate.Text = DateTime.Now.ToString();

                List<string[]> illnesses = new List<string[]>();
                List<string[]> allergies = new List<string[]>();
                List<string[]> medications = new List<string[]>();

                medications.Add(new string[] {"", "-- Select a Medication --"});
                illnesses.Add(new string[] { "", "-- Select an Illness --" });
                allergies.Add(new string[] { "", "-- Select an Allergy --" });

                query = "select i.code, i.description "
                            + "from illness i "
                            + "order by i.description";

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            illnesses.Add(new string[] {row.Field<string>(0), row.Field<string>(1)});
                        }
                    }
                    else
                    {
                    }
                }

                query = "select a.code, a.description "
                            + "from allergy a "
                            + "order by a.description";

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            allergies.Add(new string[] { row.Field<string>(0), row.Field<string>(1) });
                        }
                    }
                    else
                    {
                    }
                }

                query = "select m.code, m.name "
                    + "from medicine m "
                    + "order by m.name";

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            medications.Add(new string[] { row.Field<string>(0), row.Field<string>(1) });
                        }
                    }
                    else
                    {
                    }
                }

                query = "select distinct e.employeeid, e.firstname, e.lastname "
                            + "from physician p, employee e "
                            + "where p.employeeid = e.employeeid "
                            + "order by e.lastname, e.firstname";

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string employeeId = row.Field<string>(0);
                            string name = "Dr. " + row.Field<string>(1) + " " + row.Field<string>(2);

                            ddlPhysicians.Items.Add(new ListItem(name, employeeId));
                        }
                    }
                    else
                    {

                    }
                }

                for (int c = 0; c < 6; c++)
                {
                    DropDownList illnessDiagnosis = new DropDownList();
                    illnessDiagnosis.Width = Unit.Pixel(300);

                    foreach (string[] illness in illnesses)
                    {
                        illnessDiagnosis.Items.Add(new ListItem(illness[1], illness[0]));
                    }

                    TextBox textBox = new TextBox();
                    textBox.Width = Unit.Pixel(256);

                    this.illnessDiagnosis.Add(new IllnessCombo(illnessDiagnosis, textBox));

                    pnlIllnessDiagnosis.Controls.Add(illnessDiagnosis);
                    pnlIllnessDiagnosis.Controls.Add(textBox);
                    pnlIllnessDiagnosis.Controls.Add(new LiteralControl("<p />"));
                }

                for (int c = 0; c < 6; c++)
                {
                    DropDownList allergyDiagnosis = new DropDownList();
                    allergyDiagnosis.Width = Unit.Pixel(300);

                    foreach (string[] allergy in allergies)
                    {
                        allergyDiagnosis.Items.Add(new ListItem(allergy[1], allergy[0]));
                    }

                    this.allergyDiagnosis.Add(allergyDiagnosis);

                    pnlAllergyDiagnosis.Controls.Add(allergyDiagnosis);
                    pnlAllergyDiagnosis.Controls.Add(new LiteralControl("<p />"));
                }

                for (int c = 0; c < 6; c++)
                {
                    DropDownList ddlPrescriptionName = new DropDownList();
                    ddlPrescriptionName.Width = Unit.Pixel(300);

                    foreach(string [] medication in medications) 
                    {
                        ddlPrescriptionName.Items.Add(new ListItem(medication[1], medication[0]));
                    }

                    TextBox dosageBox = new TextBox();
                    dosageBox.Width = Unit.Pixel(64);
                    TextBox durationBox = new TextBox();
                    durationBox.Width = Unit.Pixel(64);
                    TextBox frequencyBox = new TextBox();
                    frequencyBox.Width = Unit.Pixel(128);

                    pnlPrescriptions.Controls.Add(ddlPrescriptionName);
                    pnlPrescriptions.Controls.Add(new LazyLabel("   Dosage: "));
                    pnlPrescriptions.Controls.Add(dosageBox);
                    pnlPrescriptions.Controls.Add(new LazyLabel(" mg, Duration:"));
                    pnlPrescriptions.Controls.Add(durationBox);
                    pnlPrescriptions.Controls.Add(new LazyLabel(" days, Frequency:"));
                    pnlPrescriptions.Controls.Add(frequencyBox);

                    this.prescriptionMeds.Add(new PrescriptionCombo(ddlPrescriptionName, dosageBox, durationBox, frequencyBox));
                }
            }
            catch (SqlException sqle)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string patientNumber = Request.QueryString["patientnumber"];

            Response.Redirect("/Patient.aspx?patientnumer=" + patientNumber);
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
            string consultationId = new string(idChars);
            string date = DateTime.Now.ToString();
            string physicianId = ddlPhysicians.SelectedValue;
            string type = ddlType.SelectedValue;

            string notes = txtSOAP.Text;

            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            string query = String.Format("insert into consultation values ('{0}', {1}, '{2}', '{3}', to_date('{4}', 'mm/dd/yyyy hh:mi:ss am'), '{5}')",
                consultationId, patientNumber, physicianId, type, date, notes);

            db.ExecuteNonQuery(CommandType.Text, query);

            foreach (IllnessCombo illness in illnessDiagnosis)
            {
                if (illness.ddlDiagnosis.SelectedIndex > 0)
                {
                    query = String.Format("insert into illnessdiagnosis values ('{0}', '{1}', '{2}')", 
                        consultationId, illness.ddlDiagnosis.SelectedValue, illness.comment.Text);

                    db.ExecuteNonQuery(CommandType.Text, query);
                }
            }

            foreach (DropDownList allergy in allergyDiagnosis)
            {
                if (allergy.SelectedIndex > 0)
                {
                    query = String.Format("insert into allergydiagnosis values ('{0}', '{1}')",
                        consultationId, allergy.SelectedValue);

                    db.ExecuteNonQuery(CommandType.Text, query);
                }
            }

            foreach (PrescriptionCombo prescription in prescriptionMeds)
            {
                if (prescription.ddlPrescription.SelectedIndex > 0)
                {
                    query = String.Format("insert into prescription values ('{0}', '{1}', {2}, {3}, '{4}')",
                        consultationId, prescription.ddlPrescription.SelectedValue, prescription.dosageBox.Text, prescription.durationBox.Text, prescription.frequencyBox.Text);

                    db.ExecuteNonQuery(CommandType.Text, query);
                }
            }

            Response.Redirect("/Patient.aspx?patientnumber=" + patientNumber);
        }
    }
}