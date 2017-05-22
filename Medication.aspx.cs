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
    public partial class Medication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string medicineCode = Request.QueryString["code"];

            if (medicineCode == null || medicineCode == "")
            {
                loadMedicationList();
            }
            else
            {
                this.medicationTbl.Visible = false;
                this.medListLbl.Visible = false;

                this.medInfoLbl.Visible = true;
                this.medCostLbl.Visible = true;
                this.medCostTitleLbl.Visible = true;
                this.medMfgLbl.Visible = true;
                this.medMfgTitleLbl.Visible = true;
                this.medNameLbl.Visible = true;
                this.medNameTitleLbl.Visible = true;
                this.medOrderLbl.Visible = true;
                this.medOrderQtyTitleLbl.Visible = true;
                this.medUsageLbl.Visible = true;
                this.medUsageTitleLbl.Visible = true;

                this.medReactionLbl.Visible = true;
                this.reactionTbl.Visible = true;

                loadMedicationInfo(medicineCode);
            }
        }

        private void loadMedicationList()
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");
            
            try
            {
                string query = "select * "
                    + "from medicine "
                    + "order by name";

                List<object[]> medications = new List<object[]>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string code = row.Field<string>(0);
                            string name = row.Field<string>(1);
                            decimal cost = row.Field<decimal>(2);
                            decimal usage = row.Field<decimal>(3);
                            string mfg = row.Field<string>(4);

                            medications.Add(new object[] { code, name, cost, usage, mfg });
                        }
                    }
                    else
                    {

                    }
                }

                query = "select * from medicineorders";

                Dictionary<string, decimal> orderedQty = new Dictionary<string, decimal>();

                using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string code = row.Field<string>(0);
                            decimal amount = row.Field<decimal>(1);

                            orderedQty.Add(code, amount);
                        }
                    }
                    else
                    {

                    }
                }

                int c = 1;

                foreach (object[] medication in medications)
                {
                    TableRow resultRow = new TableRow();

                    TableCell nameCell = new TableCell();
                    TableCell unitCostCell = new TableCell();
                    TableCell usageCell = new TableCell();
                    TableCell manufacturerCell = new TableCell();
                    TableCell orderedCell = new TableCell();

                    HyperLink medLink = new HyperLink();
                    medLink.Text = medication[1].ToString();
                    medLink.NavigateUrl = "/Medication.aspx?code=" + medication[0].ToString();
                    nameCell.Controls.Add(medLink);

                    unitCostCell.Text = "$" + medication[2].ToString();
                    usageCell.Text = String.Format("{0}", medication[3]);
                    manufacturerCell.Text = (string)medication[4];

                    if (orderedQty.ContainsKey(medication[0].ToString()))
                    {
                        orderedCell.Text = orderedQty[medication[0].ToString()].ToString();
                    }
                    else
                    {
                        orderedCell.Text = "0";
                    }

                    resultRow.Cells.Add(nameCell);
                    resultRow.Cells.Add(unitCostCell);
                    resultRow.Cells.Add(usageCell);
                    resultRow.Cells.Add(orderedCell);
                    resultRow.Cells.Add(manufacturerCell);

                    if (c % 2 == 0)
                    {
                        resultRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                    }

                    c++;

                    medicationTbl.Rows.Add(resultRow);
                }
            }
            catch (SqlException ex)
            {

            }
        }

        private void loadMedicationInfo(string medicineCode)
        {
            Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("Clinic db Instance");

            string query = "select * "
                    + "from medicine "
                    + "where code = '" + medicineCode + "'";

            using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string name = row.Field<string>(1);
                        decimal cost = row.Field<decimal>(2);
                        decimal usage = row.Field<decimal>(3);
                        string mfg = row.Field<string>(4);

                        this.medNameLbl.Text = name;
                        this.medCostLbl.Text = cost.ToString() + " / unit";
                        this.medUsageLbl.Text = usage + " units";
                        this.medMfgLbl.Text = mfg;
                    }
                }
                else
                {

                }
            }

            query = "select * "
                    + " from medicineorders "
                    + " where medicineCode = '" + medicineCode + "'";

            using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    medOrderLbl.Text = ds.Tables[0].Rows[0].Field<decimal>(1) + " units";
                }
                else
                {
                    medOrderLbl.Text = "0 units";
                }
            }

            query = "(select a.code, a.name, mr.SEVERITY "
                     + "from medicine a, medicinereactions mr "
                     + "where mr.MEDICINECODE1 = a.CODE "
                     + "and mr.MEDICINECODE2 = '" + medicineCode + "') "

                     + "UNION "

                     + "(select b.code, b.name, mr.severity "
                     + "from medicine b, medicinereactions mr "
                     + "where mr.MEDICINECODE2 = b.CODE "
                     + "and mr.MEDICINECODE1 = '" + medicineCode + "') "
                     + "order by severity desc, name";

            Dictionary<string, List<string[]>> medicineReactions = new Dictionary<string, List<string[]>>();

            medicineReactions.Add("S", new List<string[]>());
            medicineReactions.Add("N", new List<string[]>());
            medicineReactions.Add("M", new List<string[]>());
            medicineReactions.Add("L", new List<string[]>());

            using (DataSet ds = db.ExecuteDataSet(CommandType.Text, query))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string code = row.Field<string>(0);
                        string name = row.Field<string>(1);
                        string severity = row.Field<string>(2);

                        medicineReactions[severity].Add(new string[] { code, name });
                    }
                }
                else
                {
                    
                }
            }

            addSeverityRows(medicineReactions["S"], "S");
            addSeverityRows(medicineReactions["M"], "M");
            addSeverityRows(medicineReactions["L"], "L");
            addSeverityRows(medicineReactions["N"], "N");
        }

        private void addSeverityRows(List<string[]> medicines, string severity)
        {
            int c = 1;
            foreach (string[] medicine in medicines)
            {
                TableRow resultRow = new TableRow();

                TableCell nameCell = new TableCell();
                TableCell severityCell = new TableCell();
                
                HyperLink medLink = new HyperLink();
                medLink.Text = medicine[1].ToString();
                medLink.NavigateUrl = "/Medication.aspx?code=" + medicine[0].ToString();
                nameCell.Controls.Add(medLink);

                switch (severity)
                {
                    case "S":
                        severityCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#CC0000");
                        severityCell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                        severityCell.Text = "Severe";
                        break;
                    case "M":
                        severityCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCC00");
                        severityCell.Text = "Moderate";
                        break;
                    case "L":
                        severityCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#00CC00");
                        severityCell.Text = "Little";
                        break;
                    case"N":
                        severityCell.Text = "None";
                        break;
                }

                resultRow.Cells.Add(nameCell);
                resultRow.Cells.Add(severityCell);

                if (c % 2 == 0)
                {
                    resultRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                }

                c++;

                reactionTbl.Rows.Add(resultRow);
            }
        }
    }
}