using SMS_Speed.DTO;
using SMS_Speed.eSMS;
using SMS_Speed.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS_Speed
{
    public partial class frmSendSMS : Form
    {
        Dictionary<string, string> configs;
        public frmSendSMS()
        {
            InitializeComponent();
            cbxMonth.SelectedIndex = DateTime.Now.Month - 1;
            lbBalance.Text = SMSFunction.CheckBalance();
            InitConfig();
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            string month = cbxMonth.Text.Replace("Tháng ","");
            string query = @"Select FirstName, LastName,BirthDate,HomeTele from Member where MONTH(birthdate) BETWEEN '" + month + @"' and '" + month+@"'";
            DBConenection.getInstance().Open();
            OdbcCommand cmd = new OdbcCommand(query, DBConenection.getInstance());
            OdbcDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            DBConenection.getInstance().Close();

            List<MemberDTO> members = new List<MemberDTO>();
            members = (from DataRow dr in dt.Rows
                           select new MemberDTO()
                           {
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString(),
                               BirthDate = dr["BirthDate"].ToString(),
                               HomeTele = dr["HomeTele"].ToString()
                           }).ToList();
            List<string> nums = members.Where(w => w.HomeTele != "").Select(s=>s.HomeTele).ToList();
            string phoneNums = String.Join(",", nums);
            SMSFunction.Send(phoneNums);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void InitConfig()
        {
            configs = Config.ReadConfigFile();
            int x = 150;
            int width = 470;
            foreach (var config in configs)
            {
                var label = new Label();
                label.Text = config.Key + ": ";
                label.AutoSize = true;
                label.Location = new Point(0, 0);
                var textbox = new TextBox();
                textbox.Text = config.Value;
                textbox.Width = width;
                textbox.Location = new Point(x, 0);
                textbox.Name = config.Key;

                Panel panel = new Panel();
                panel.Name = config.Key;
                panel.Controls.Add(label);
                panel.Controls.Add(textbox);
                panel.AutoSize = true;

                flpConfig.Controls.Add(panel);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var panel = flpConfig.Controls.OfType<Panel>();
            var panellst = panel.ToList();
            foreach (var item in panellst)
            {
                TextBox tbx = item.Controls.OfType<TextBox>().ToList()[0];
                configs[tbx.Name] = tbx.Text;
            }
            bool result;
            string err = Config.WriteConfigFile(configs, out result);
            if (result)
            {
                MessageBox.Show("Save successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
