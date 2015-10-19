using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cherry_Hill_9th_Ward_Map
{
    public partial class Add_Family : Form
    {
        //instance Variables
        //public const string[] statusValues = new string[]{"Not On Ward List", "Attending Another Ward"};
        public readonly string[] statusValues = new string[] { "Not On Ward List", "Attending Another Ward" };

        public Form1 MyParentForm;

        public string lastName
        {
           get { return txBxLastName.Text; }
        }

        public string hoh1
        {
            get { return txBxHoh1.Text; }
        }
        public string hoh2
        {
            get { return txBxHoh2.Text; }
        }
        public string address
        {
            get { return txBxAddress.Text; }
        }
        public string status
        {
            get { return comboBox1.SelectedItem.ToString(); }
        }
        public Add_Family()
        {
            InitializeComponent();
            myInitialize();
            comboBox1.SelectedIndex = 0;

        }
        public Add_Family(DataRow dr) //edit family
        {
            InitializeComponent();
            myInitialize();
            myInitializeEditFamily();
            txBxLastName.Text = dr["lastName"].ToString();
            txBxHoh1.Text = dr["hoh1"].ToString();
            txBxHoh2.Text = dr["hoh2"].ToString();
            txBxAddress.Text = dr["address"].ToString();
            int i = 0;
            foreach(string statusStr in statusValues)
            {
                if(dr["status"].ToString() == statusStr)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
                i++;
            }
        }
        public void myInitialize()
        {
            foreach (string myStr in statusValues)
            {
                comboBox1.Items.Add(myStr);
            }
        }
        public void myInitializeEditFamily()
        {
            this.Text = "Edit Family";
        }
        private void SaveFamilyInfoBtn_Click(object sender, EventArgs e)
        {
            //Check to see if that the neccesarry data has been entered
            if (txBxLastName.Text != ""
                && txBxHoh1.Text != ""
                && txBxAddress.Text != "")
            {
                //data is valid
                this.DialogResult = DialogResult.OK;
                //SaveFamilyInfoBtn.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                string tmpStr = "The following fields cannot be blank:\n";
               
                if(txBxLastName.Text == "")
                {
                    tmpStr = tmpStr + "Last Name\n";
                }
                if(txBxHoh1.Text == "")
                {
                    tmpStr = tmpStr + "Head of Household 1 First Name\n";
                }                    
                if(txBxAddress.Text == "")
                {
                    tmpStr = tmpStr + "Address\n";
                }
                MessageBox.Show(tmpStr,"Blank Fields", MessageBoxButtons.OK);
            }

        }

        private void CancelFamilyInfoBtn_Click(object sender, EventArgs e)
        {
            //CancelFamilyInfoBtn.DialogResult = DialogResult.Cancel;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

   }
}