using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace MetaMaster
{
    public partial class computerEvaluationForm : Form
    {
        public computerEvaluationForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Initialize datatable
            DataTable RequestObj = new DataTable();
            DataColumn dtColumn;
            DataRow dtRow;
            DataSet dtSet;
            //string [,] payload;
            //Set up key value pairing
            //Column Key
            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(string);
            dtColumn.ColumnName = "key";
            dtColumn.Caption = "Key";
            dtColumn.ReadOnly = true;
            RequestObj.Columns.Add(dtColumn);
            //Column Value
            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(string);
            dtColumn.ColumnName = "value";
            dtColumn.Caption = "Value";
            dtColumn.ReadOnly = false;
            RequestObj.Columns.Add(dtColumn);
            //Create DataSet
            dtSet = new DataSet();
            //Add RequestObj to the DataSet
            dtSet.Tables.Add(RequestObj);
            //Add Rows
            //initialize array
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "service_order";
            dtRow["value"] = textBox1.Text;
            //payload += { "service_order", textBox1.Text};
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "id_location";
            dtRow["value"] = comboBox1.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "customer_name";
            dtRow["value"] = textBox2.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "id_user_technician";
            dtRow["value"] = textBox14.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "trade_in_estimate";
            dtRow["value"] = textBox5.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "application_comment";
            dtRow["value"] = richTextBox1.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "physical_condition";
            dtRow["value"] = textBox3.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "max_testing_temperature";
            dtRow["value"] = textBox4.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "data_condition";
            dtRow["value"] = textBox6.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "data_retrievable";
            dtRow["value"] = checkBox4.Checked.ToString();
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "using_cps";
            if (checkBox1.Checked) dtRow["value"] = "1"; else dtRow["value"] = "0";
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "using_seb";
            if (checkBox2.Checked) dtRow["value"] = "1"; else dtRow["value"] = "0";
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "current_antivirus";
            dtRow["value"] = checkBox3.Checked.ToString();
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "pc_make";
            dtRow["value"] = textBox7.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "pc_model";
            dtRow["value"] = textBox8.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "pc_operating_system";
            dtRow["value"] = textBox9.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "pc_cpu";
            dtRow["value"] = textBox10.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "pc_gpu";
            dtRow["value"] = textBox11.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "pc_memory";
            dtRow["value"] = textBox12.Text;
            RequestObj.Rows.Add(dtRow);
            dtRow = RequestObj.NewRow();
            dtRow["key"] = "pc_storage";
            dtRow["value"] = textBox13.Text.Replace(":", "\\");
            RequestObj.Rows.Add(dtRow);
            scaffoldCreds auth = new scaffoldCreds(RequestObj);
            auth.ShowDialog();
            //Chooch to scaffold
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            //Make computer work with me
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript("winrm quickconfig -force");
                powerShell.AddCommand("Out-String");
                Collection<PSObject> PSOutput = powerShell.Invoke();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject pSObject in PSOutput)
                    stringBuilder.AppendLine(pSObject.ToString());
                toolStripStatusLabel1.Text = stringBuilder.ToString();
                statusStrip1.Update();
            }
            //Fill text box with raw data. That info is put into form automatically but can be modified as needed.
            //Make
            toolStripStatusLabel1.Text = "Grabbing: Make";
            statusStrip1.Update();
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript("Get-CimInstance win32_ComputerSystem | Select-Object Manufacturer -ExpandProperty Manufacturer");
                powerShell.AddCommand("Out-String");
                Collection<PSObject> PSOutput = powerShell.Invoke();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject pSObject in PSOutput)
                    stringBuilder.AppendLine(pSObject.ToString());
                textBox7.Text = stringBuilder.ToString();
            }

            //Model
            toolStripStatusLabel1.Text = "Grabbing: Model";
            statusStrip1.Update();
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript("Get-CimInstance win32_ComputerSystem | Select-Object Model -ExpandProperty Model");
                powerShell.AddCommand("Out-String");
                Collection<PSObject> PSOutput = powerShell.Invoke();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject pSObject in PSOutput)
                    stringBuilder.AppendLine(pSObject.ToString());
                textBox8.Text = stringBuilder.ToString();
            }

            //Operating System
            toolStripStatusLabel1.Text = "Grabbing: OS";
            statusStrip1.Update();
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript("Get-CimInstance win32_OperatingSystem | Select-Object Caption -ExpandProperty Caption");
                powerShell.AddCommand("Out-String");
                Collection<PSObject> PSOutput = powerShell.Invoke();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject pSObject in PSOutput)
                    stringBuilder.AppendLine(pSObject.ToString());
                textBox9.Text = stringBuilder.ToString();
            }

            //CPU
            toolStripStatusLabel1.Text = "Grabbing: CPU";
            statusStrip1.Update();
            //textBox10.Text = System.Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript("Get-CimInstance win32_processor | Select-Object Name -ExpandProperty Name");
                powerShell.AddCommand("Out-String");
                Collection<PSObject> PSOutput = powerShell.Invoke();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject pSObject in PSOutput)
                    stringBuilder.AppendLine(pSObject.ToString());
                textBox10.Text = stringBuilder.ToString();
            }

            //GPU
            toolStripStatusLabel1.Text = "Grabbing: GPU";
            statusStrip1.Update();
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript("Get-CimInstance Win32_VideoController | Select-Object description -ExpandProperty description");
                powerShell.AddCommand("Out-String");
                Collection<PSObject> PSOutput = powerShell.Invoke();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject pSObject in PSOutput)
                    stringBuilder.AppendLine(pSObject.ToString());
                textBox11.Text = stringBuilder.ToString();
            }
            //Memory
            toolStripStatusLabel1.Text = "Grabbing: Memory";
            statusStrip1.Update();
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript("$mem = (Get-CimInstance win32_physicalmemory | Measure-Object -Property Capacity -Sum | Select-Object Sum -ExpandProperty Sum); $mem/1GB.ToString(\"F2\");");
                powerShell.AddCommand("Out-String");
                Collection<PSObject> PSOutput = powerShell.Invoke();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject pSObject in PSOutput)
                    stringBuilder.AppendLine(pSObject.ToString());
                textBox12.Text = stringBuilder.ToString();
                textBox12.Text += " Gb";
            }
            textBox12.Text += " @ ";
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript("$memspeed = (Get-CimInstance win32_physicalmemory | Select-Object Configuredclockspeed -ExpandProperty Configuredclockspeed | Select-Object -first 1); $memspeed;");
                powerShell.AddCommand("Out-String");
                Collection<PSObject> PSOutput = powerShell.Invoke();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject pSObject in PSOutput)
                    stringBuilder.AppendLine(pSObject.ToString());
                textBox12.Text += stringBuilder.ToString() + " Mhz";
            }
            //Storage
            toolStripStatusLabel1.Text = "Grabbing: Storage";
            statusStrip1.Update();
            int count = 0;
            textBox13.Text = " ";
            StringBuilder finalString = new StringBuilder();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                try
                {
                    //Grab usage
                    long size = drive.TotalSize;
                    string freespace = (drive.TotalFreeSpace / 1024 / 1024 / 1024).ToString();
                    string sizeGb = (drive.TotalSize / 1024 / 1024 / 1024).ToString();
                    char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();
                    if (freespace.Length > 5)
                    {
                        textBox13.Text += drive.Name + " " + freespace.Remove(5) + " of " + sizeGb + "Gb available; ";
                    }
                    else
                        textBox13.Text += drive.Name + " " + freespace + " of " + sizeGb + "Gb available; ";

                    //Grab health
                    toolStripStatusLabel1.Text = "Grabbing: SMART for " + drive.Name;
                    statusStrip1.Update();
                    using (PowerShell powerShell = PowerShell.Create())
                    {
                        powerShell.AddScript("cd $env:TEMP; cd \'root\\Tools\\Hardware Testing\\HDD Health\'; $output = (.\\smartctl.exe --tolerance=verypermissive -a /dev/sd" + alpha[count].ToString() + "); $output;"); ;
                        powerShell.AddCommand("Out-String");
                        Collection<PSObject> PSOutput = powerShell.Invoke();
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (PSObject pSObject in PSOutput)
                            stringBuilder.AppendLine(pSObject.ToString());

                        finalString.AppendLine("============================================\n");
                        finalString.AppendLine(drive.Name.ToString() + " Health\n");
                        finalString.AppendLine("============================================\n");
                        finalString.AppendLine(stringBuilder.ToString());
                    }
                    count++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Smart data couldn't be accessed.");
                    //MessageBox.Show("One of the drives SMART cannot be accessed!\n" + ex.ToString());
                }

            }
            //Write smartctl output to file
            toolStripStatusLabel1.Text = "Writing SmartCtl output to file";
            statusStrip1.Update();
            try
            {
                if (finalString != null)
                {
                    using (StreamWriter bw = new StreamWriter(File.Create(System.IO.Path.GetTempPath() + "\\HDDHealthOutput.txt")))
                    {
                        bw.Write(finalString.ToString());
                        bw.Close();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            //Upload smartctl output to ix.io
            toolStripStatusLabel1.Text = "Uploading file to ix.io";
            statusStrip1.Update();
            try
            {
                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.AddScript("$output = (Get-Content " + System.IO.Path.GetTempPath() + "HDDHealthOutput.txt | " + System.IO.Path.GetTempPath() + "root\\curl64\\bin\\curl.exe -s -F \'f:1=<-\' ix.io); $output;");
                    powerShell.AddCommand("Out-String");
                    Collection<PSObject> PSOutput = powerShell.Invoke();
                    //Thread.Sleep(3000);
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (PSObject pSObject in PSOutput)
                        stringBuilder.AppendLine(pSObject.ToString());
                    richTextBox1.Text = "SMART Data Link: " + stringBuilder.ToString() + richTextBox1.Text;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

            ////////////////////////////////////////
            toolStripStatusLabel1.Text = "Grabbing: CPS & SEB";
            statusStrip1.Update();
            if (File.Exists("C:\\Program Files\\ESET\\ESET Security\\egui.exe") && File.Exists("C:\\Program Files\\SUPERAntiSpyware\\SUPERAntiSpyware.exe"))
                checkBox1.Checked = true;
            if (File.Exists("C:\\Program Files (x86)\\Super Easy Backup\\Endpoint\\DCProtect.exe"))
                checkBox2.Checked = true;

            //Show Completed
            toolStripStatusLabel1.Text = "Auto-Fill Completed";
            statusStrip1.Update();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string executionCMD = System.IO.Path.GetTempPath() + "root\\Tools\\Anti-Virus Scanners\\MBAM ADWCleaner.exe";
            System.Diagnostics.Process.Start(executionCMD);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string executionCMD = System.IO.Path.GetTempPath() + "root\\Tools\\Anti-Virus Scanners\\ESET Online Scanner.exe";
            System.Diagnostics.Process.Start(executionCMD);
        }
    }
}
