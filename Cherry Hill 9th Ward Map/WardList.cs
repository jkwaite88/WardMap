using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using JouniHeikniemi.Tools.Text;

namespace Cherry_Hill_9th_Ward_Map
{
    class WardList
    {
        //class instance variables
        private string _onWardListPath;
        private string _notOnWardListPath;
        private int onWardListFileType;
        private int notOnWardListFileType;
        public DataTable dtOnWardList;
        public DataTable dtNotOnWardList;
        private const int NONE = 0;
        private const int WEB_DOWNLOAD = 1;
        private const int NOT_ON_WARD_LIST_FORMAT = 2;
        private const int MLS_EXPORT_WARD_LIST = 3;
        private const int LDS_ORG_EXPORT_HOUSEHOLDS = 4;
        private const int VCF_WARD_LIST = 4;
        public const int MAX_NUM_MATCHED_NAMES_RETURNED = 5;


        //defualt Constructor
        public WardList()
        {
            onWardListPath = "";
            notOnWardListPath = "";
            onWardListFileType = NONE;
            notOnWardListFileType = NONE;
            dtOnWardList = new DataTable();
            dtNotOnWardList = new DataTable();


            dtOnWardList.TableName = "OnWardList";
            dtNotOnWardList.TableName = "NotOnWardList";

            DataColumn dc = new DataColumn("lastName", typeof(System.String));
            dc.Caption = "Last Name";
            dc.DefaultValue = "";
            dtOnWardList.Columns.Add(dc);

            dc = new DataColumn("lastName", typeof(System.String));
            dc.Caption = "Last Name";
            dc.DefaultValue = "";
            dtNotOnWardList.Columns.Add(dc);

            dc = new DataColumn("hoh1", typeof(System.String));
            dc.Caption = "Head of Househould 1";
            dc.DefaultValue = "";
            dtOnWardList.Columns.Add(dc);
            
            dc = new DataColumn("hoh1", typeof(System.String));
            dc.Caption = "Head of Househould 1";
            dc.DefaultValue = "";
            dtNotOnWardList.Columns.Add(dc);

            dc = new DataColumn("hoh2", typeof(System.String));
            dc.Caption = "Head of Househould 2";
            dc.DefaultValue = "";
            dtOnWardList.Columns.Add(dc);

            dc = new DataColumn("hoh2", typeof(System.String));
            dc.Caption = "Head of Househould 2";
            dc.DefaultValue = "";
            dtNotOnWardList.Columns.Add(dc);

           
            dc = new DataColumn("address", typeof(System.String));
            dc.Caption = "Address";
            dc.DefaultValue = "";
            dtOnWardList.Columns.Add(dc);

            dc = new DataColumn("address", typeof(System.String));
            dc.Caption = "Address";
            dc.DefaultValue = "";
            dtNotOnWardList.Columns.Add(dc);

            dc = new DataColumn("status", typeof(System.String));
            dc.Caption = "Status";
            dc.DefaultValue = "";
            dtNotOnWardList.Columns.Add(dc);     
        
        }

        public string onWardListPath
        {
            get{return _onWardListPath;}
            set{ _onWardListPath = value;}
        }
 
        public string notOnWardListPath
        {
            get { return _notOnWardListPath; }
            set { _notOnWardListPath = value; }
        }

        public int OnWardListFileType
        {
            get { return onWardListFileType; }
        }
        public int NotOnWardListFileType
        {
            get { return notOnWardListFileType; }
        }

        public void populateDtOnWardList()
        {
          if(File.Exists(onWardListPath))
          {
            string[] fields;
            string[] tmpStrs;
            string[] tmpStrs2;
            string tmpStr;
            //DataRow dr;
            string currentLastName, currentHoh1, currentHoh2, currentAddress;

            //Determin file typ
            tmpStrs = onWardListPath.Split(new char[] { '.' });
            if (string.Compare(tmpStrs[(tmpStrs.Length-1)],"csv") == 0)
            {
                using (CSVReader csv = new CSVReader(onWardListPath))
                {
                    
                    //determine what kind of file it is. 
                    //find header and type of file
                    onWardListFileType = NONE;
                
                    while ((fields = csv.GetCSVLine()) != null)
                    {
                        if (   (fields.Length >=3)
                            &&(string.Compare(fields[0], "familyname") == 0)
                            && (string.Compare(fields[1], "phone") == 0)
                            && (string.Compare(fields[2], "addr1") == 0)
                            && (string.Compare(fields[3], "addr2")== 0) )
                        {
                            onWardListFileType = WEB_DOWNLOAD;
                            break;
                        }
                        else if ((fields.Length >= 13)
                            && (string.Compare(fields[10], "Orem") == 0)
                            && (string.Compare(fields[11], "Utah") == 0)
                            && (string.Compare(fields[12], "84058") == 0)
                            && (string.Compare(fields[13], "United States") == 0)
                            )
                        {
                            onWardListFileType = MLS_EXPORT_WARD_LIST;
                            break;
                        }
                        else if (
                            (string.Compare(fields[0], "Family Name") == 0)
                            && (string.Compare(fields[1], "Couple Name") == 0)
                            && (string.Compare(fields[2], "Family Phone") == 0)
                            && (string.Compare(fields[3], "Family Email") == 0)
                            )
                        {
                            onWardListFileType = LDS_ORG_EXPORT_HOUSEHOLDS;
                            break;
                        }

                        }
                        //read in file
                        if (onWardListFileType == WEB_DOWNLOAD)
                    {
                        dtOnWardList.Clear();
                        while ((fields = csv.GetCSVLine()) != null)
                        {
                            currentLastName = "";
                            currentHoh1 = "";
                            currentHoh2 = "";
                            currentAddress = "";
                            //fill in lastName, hoh1, hoh2, address
                            //Last Name
                            tmpStrs = fields[0].Split(new char[] { ',' });
                            currentLastName = tmpStrs[0].Trim();
                            //hoh1
                            tmpStrs = fields[6].Split(new char[] { ' ' });
                            currentHoh1 = tmpStrs[0].Trim();
                            //hoh2
                            //make sure fields[7] is an hoh
                            if ((fields.Length >= 8) && (fields[0].Contains(fields[7])))
                            {
                                tmpStrs = fields[7].Split(new char[] { ' ' });
                                currentHoh2 = tmpStrs[0].Trim();
                            }

                            //address
                            currentAddress = fields[2].Trim();
                            //add data to table
                            addDataToWardList(currentLastName, currentHoh1, currentHoh2, currentAddress);
                        }
                    }
                    else if (onWardListFileType == MLS_EXPORT_WARD_LIST)
                    {
                        dtOnWardList.Clear();
                        do
                        {
                            currentLastName = "";
                            currentHoh1 = "";
                            currentHoh2 = "";
                            currentAddress = "";
                            //fill in lastName, hoh1, hoh2, address
                            //Last Name
                            tmpStrs = fields[0].Split(new char[] { ',' });
                            currentLastName = tmpStrs[0].Trim();
                            //hoh1 hoh2
                            if (tmpStrs[1].Contains("&"))
                            {
                                tmpStrs2 = tmpStrs[1].Split(new char[] { '&' });
                                currentHoh1 = tmpStrs2[0].Trim();
                                currentHoh2 = tmpStrs2[1].Trim();
                            }else{
                                tmpStrs[1] = tmpStrs[1].Trim();
                                tmpStrs2 = tmpStrs[1].Split(new char[] { ' ' });
                                currentHoh1 = tmpStrs2[0].Trim();
                            }

                            //address
                            currentAddress = fields[9].Trim(); ;
                            //add data to table
                            addDataToWardList(currentLastName, currentHoh1, currentHoh2, currentAddress);
                            //read extra data
                            while (((fields = csv.GetCSVLine()) != null)
                                    && (!fields[0].Contains("~~~")))
                            {
                                //do nothing, throw away lines
                            }
                            // read "Data Exported" line, throw away
                            fields = csv.GetCSVLine();
                        } while ((fields = csv.GetCSVLine()) != null);
                        
                    }
                    else if (onWardListFileType == LDS_ORG_EXPORT_HOUSEHOLDS)
                    {
                        dtOnWardList.Clear();
                        while ((fields = csv.GetCSVLine()) != null)
                        {
                            currentLastName = "";
                            currentHoh1 = "";
                            currentHoh2 = "";
                            currentAddress = "";
                            //fill in lastName, hoh1, hoh2, address
                            //Last Name
                            currentLastName = fields[0].Trim();
                            //hoh1
                            tmpStrs = fields[5].Split(new char[] { ',' });
                            currentHoh1 = tmpStrs[1].Trim();
                            //hoh2
                            //make sure fields[7] is an hoh
                            tmpStrs = fields[8].Split(new char[] { ',' });
                            if (tmpStrs.Length > 1)
                            {
                                currentHoh2 = tmpStrs[1].Trim();
                            }
                            else
                            {
                                currentHoh2 = "";
                            }
                            //address
                            tmpStrs = fields[4].Split(new char[] { ',' });
                            tmpStr = tmpStrs[0];
                            int lastIndex = tmpStr.LastIndexOf(' ');
                            if (lastIndex != -1)
                            {
                                tmpStr = tmpStr.Remove(lastIndex).Trim();
                            }
                            currentAddress = tmpStr;
                            //add data to table
                            addDataToWardList(currentLastName, currentHoh1, currentHoh2, currentAddress);
                        }
                    }
                    else
                    {
                    //did not recognize file format
                        MessageBox.Show("\"Ward list\" file format not recognized.", "Unrecognized Format", MessageBoxButtons.OK);
                    }
                    
                }// end using
            }
            else if (string.Compare(tmpStrs[(tmpStrs.Length - 1)], "vcf") == 0)
            {

            }
          }
          else
          {
            //file doesn't exist
            MessageBox.Show("Ward List file does not exist.", "File does not Exist",MessageBoxButtons.OK);
          }
        }

        public void addDataToWardList(string lastName, string hoh1, string hoh2, string address)
        {
            DataRow dr;
            //add new row to data table for this family
            dr = dtOnWardList.NewRow(); //dr gets all columns of dt
            dr["lastName"] = lastName;
            dr["hoh1"] = hoh1;
            dr["hoh2"] = hoh2;
            dr["address"] = address;
            //add row to table
            dtOnWardList.Rows.Add(dr);

        }
        public void populateDtNotOnWardList()
        {
          if(File.Exists(notOnWardListPath))
          {
                using (CSVReader csv = new CSVReader(notOnWardListPath))
                {
                    string[] fields;
                    string[] tmpStrs;
                    string tmpStr;
                    DataRow dr;

                    //determine what kind of file it is. There
                    //should only be one type of file (the one 
                    //created by this program)
                    notOnWardListFileType = NONE;
                    while ((fields = csv.GetCSVLine()) != null)
                    {
                        if ((string.Compare(fields[0], "lastName") == 0)
                            && (string.Compare(fields[1], "hoh1") == 0)
                            && (string.Compare(fields[2], "hoh2") == 0)
                            && (string.Compare(fields[3], "address") == 0)
                            && (string.Compare(fields[4], "status") == 0)
                            && fields.Length == 5)
                        {
                            notOnWardListFileType = NOT_ON_WARD_LIST_FORMAT;
                            break;
                        }

                    }
                    //read in file
                    if (notOnWardListFileType == NOT_ON_WARD_LIST_FORMAT)
                    {
                        dtNotOnWardList.Clear();
                        while ((fields = csv.GetCSVLine()) != null)
                        {
                            //add new row to data table for this family
                            dr = dtNotOnWardList.NewRow(); //dr gets all columns of dt
                            //Last Name
                            dr["lastName"] = fields[0];
                            //hoh1
                            dr["hoh1"] = fields[1];
                            //hoh2
                            dr["hoh2"] = fields[2];
                            //address
                            dr["address"] = fields[3];
                            //status
                            dr["status"] = fields[4];

                            //add row to table
                            dtNotOnWardList.Rows.Add(dr);

                        }
                    }
                    else if( notOnWardListFileType == NONE)
                    {
                        //did not recognize file format
                        MessageBox.Show("\"Families not on ward List\" file format not recognized.", "Unrecognized Format", MessageBoxButtons.OK);
                    }
                }// end using
            }
            else
            {
                //file doesn't exist
                //MessageBox.Show("Not on ward list file does not exist.", "File does not Exist", MessageBoxButtons.OK);
            }
        }

        public void saveNotOnWardListToFile()
        {
            string tmpStr = "";
            FileStream file = new FileStream(notOnWardListPath, FileMode.Truncate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
 
            //write out column headers if file is blank
            tmpStr = "lastName,hoh1,hoh2,address,status";
            sw.WriteLine(tmpStr);

            foreach (DataRow dr in dtNotOnWardList.Rows)
            {
                tmpStr = "";
                foreach (DataColumn dc in dtNotOnWardList.Columns)
                {
                    tmpStr = tmpStr + "\"";
                    tmpStr = tmpStr + dr[dc];
                    tmpStr = tmpStr + "\"";
                    if (dc != dtNotOnWardList.Columns[dtNotOnWardList.Columns.Count - 1])
                    {
                        tmpStr = tmpStr + ",";
                    }
                }
                sw.WriteLine(tmpStr);
            }

            sw.Close();
            file.Close();
        }

        public string[] matchPropertyToNames(string[] stringsToFind)
        {
            string[] returnStrings = new string[MAX_NUM_MATCHED_NAMES_RETURNED];
            int i = 0;
            string address;
            //DataRow currentRow = dtOnWardList.NewRow();
            //DataColumn dc = new DataColumn dtOnWardList.Columns["address"];
            //DataColumn dc = dtOnWardList.Columns["address"];
            //initialize return string
            for (i = 0; i < MAX_NUM_MATCHED_NAMES_RETURNED; i++)
            {
                returnStrings[i] = "";
            }
            //format the search strings
            for (i = 0; i < stringsToFind.Length; i++)
            {
                stringsToFind[i] = stringsToFind[i].Replace(" ", "").ToLower();
            }
            i = 0;
            //search  dtOnWardList
            foreach (DataRow dr in dtOnWardList.Rows)
            {
                address = dr["address"].ToString().Replace(" ", "").ToLower();
                if (address.Contains(stringsToFind[0])
                    && address.Contains(stringsToFind[1]))
                {
                    //found a match
                    if (i < MAX_NUM_MATCHED_NAMES_RETURNED)
                    {
                        returnStrings[i++] = dr["lastName"].ToString();
                    }
                }
            }
            //search dtNotOnWardList
            //get status values from the addFamilyForm
            Add_Family addFamilyForm = new Add_Family();
            
            foreach (DataRow dr in dtNotOnWardList.Rows)
            {
                address = dr["address"].ToString().Replace(" ","").ToLower();
                if(address.Contains(stringsToFind[0])
                    && address.Contains(stringsToFind[1]))
                {
                    //found a match
                    if(i < MAX_NUM_MATCHED_NAMES_RETURNED)
                    {
                        if (dr["status"].ToString() == addFamilyForm.statusValues[0])
                        {
                            returnStrings[i++] = "*" + dr["lastName"].ToString();
                        }
                        else if(dr["status"].ToString() == addFamilyForm.statusValues[1])
                        {
                            returnStrings[i++] = "~" + dr["lastName"].ToString();
                        }
                    }
                }
            }
            addFamilyForm.Close();
            return returnStrings;
        }
    }
}
