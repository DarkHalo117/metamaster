using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Security.Principal;
using System.Net;
using System.Runtime.InteropServices;

namespace MetaMaster
{
    public partial class Form1 : Form
    {
        private Panel buttonPanel = new Panel();
        private DataGridView systemGridView = new DataGridView();
        private Button addTaskButton = new Button();
        private Button deleteTaskButton = new Button();
        private Button addColumnButton = new Button();
        private Button deleteColumnButton = new Button();
        KeystrokMessageFilter keyStrokeMessageFilter = new KeystrokMessageFilter();
        //Closing event
        //[System.ComponentModel.Browsable(false)]
        //public event System.ComponentModel.CancelEventHandler Closing;

        public Form1()
        {
            InitializeComponent();
            this.Text = "PWD: " + Directory.GetCurrentDirectory();
            Environment.SetEnvironmentVariable("PWD", Directory.GetCurrentDirectory(), EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable("Tools", logic.tools, EnvironmentVariableTarget.Machine);
            //Startup Logic
            logic.initialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Set up form to add data to for reporting.
            SetupLayout();
            SetupDataGridView();
            PopulateDataGridView();
            Application.AddMessageFilter(keyStrokeMessageFilter);
            //AllocConsole();
            //Check Admin
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
                   .IsInRole(WindowsBuiltInRole.Administrator) ? true : false;

            if (isAdmin)
            {
                //MessageBox.Show("You are an administrator!");
            }
            else
            {
                MessageBox.Show("You are not an administrator!");
            }
            //Load Env Variables
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.SetEnvironmentVariable("PWD", null, EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable("Tools", null, EnvironmentVariableTarget.Machine);
            if (Environment.GetEnvironmentVariable("PWD") == null)
                Console.WriteLine("PWD has been deleted.");
            if (Environment.GetEnvironmentVariable("Tools") == null)
                Console.WriteLine("Tools has been deleted.");
            try
            {
                System.IO.Directory.Delete(logic.root, true);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error during cleanup:\n" + err.ToString());
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SetupLayout()
        {
            this.Size = new Size(600, 500);
            /*
            addTaskButton.Text = "Add Row";
            addTaskButton.Location = new Point(10, 10);
            addTaskButton.Click += new EventHandler(addTaskButton_Click);

            deleteTaskButton.Text = "Del Row";
            deleteTaskButton.Location = new Point(100, 10);
            deleteTaskButton.Click += new EventHandler(deleteTaskButton_Click);

            addColumnButton.Text = "Add Column";
            addColumnButton.Location = new Point(200, 10);
            addColumnButton.Click += new EventHandler(addColumnButton_Click);

            deleteColumnButton.Text = "Del Column";
            deleteColumnButton.Location = new Point(300, 10);
            deleteColumnButton.Click += new EventHandler(deleteColumnButton_Click);

            buttonPanel.Controls.Add(addTaskButton);
            buttonPanel.Controls.Add(deleteTaskButton);
            buttonPanel.Controls.Add(addColumnButton);
            buttonPanel.Controls.Add(deleteColumnButton);
            buttonPanel.Height = 30;
            buttonPanel.Dock = DockStyle.Bottom;

            this.Controls.Add(this.buttonPanel);
            */

        }
        private void SetupDataGridView()
        {
            this.Controls.Add(systemGridView);

            systemGridView.ColumnCount = 7;
            systemGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            systemGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Green;
            systemGridView.ColumnHeadersDefaultCellStyle.Font = new Font(systemGridView.Font, FontStyle.Bold);
            systemGridView.AllowUserToAddRows = true;
            systemGridView.AllowUserToResizeColumns = true;
            systemGridView.AllowUserToDeleteRows = true;
            systemGridView.AllowUserToResizeRows = true;
            //systemGridView.AutoSizeColumnsMode = ;
            systemGridView.ColumnHeadersVisible = true;
            systemGridView.RowHeadersVisible = true;

            systemGridView.Name = "systemGridView";
            systemGridView.Location = new Point(8, 8);
            systemGridView.Size = new Size(500, 250);
            //systemGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            systemGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            systemGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            systemGridView.GridColor = Color.Gray;
            systemGridView.RowHeadersVisible = true;

            systemGridView.Columns[0].Name = "Task";
            systemGridView.Columns[1].Name = "Notes";
            systemGridView.Columns[2].Name = "Timestamp";
            //systemGridView.Columns[2].DefaultCellStyle.Font = new Font(systemGridView.DefaultCellStyle.Font, FontStyle.Italic);
            systemGridView.Dock = DockStyle.Fill;
        }
        private void PopulateDataGridView()
        {
            //Computer Name
            systemGridView.Rows.Add("Computer Name: ", System.Environment.GetEnvironmentVariable("COMPUTERNAME"), logic.timestamp());
            //Storage Drives
            systemGridView.Rows.Add("Storage");
            int count = 1;
            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    long size = logic.BtoGb(drive.TotalSize);
                    long avail = logic.GetTotalFreeSpace(drive.Name);
                    string[] row0 = { drive.Name, avail.ToString() + " of " + size.ToString() + "Gb Available", logic.timestamp() };
                    systemGridView.Rows.Add(row0);
                    count++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("A drive letter was not ready.\n" + ex.ToString());
            }
            
            //systemGridView.Rows.Add("Environment Variables: ", "Key","Value");
            //foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            //    systemGridView.Rows.Add("", de.Key, de.Value);

        }

        private void addTaskButton_Click(object sender, EventArgs e)
        {
            systemGridView.Rows.Add();
        }

        private void deleteTaskButton_Click(object sender, EventArgs e)
        {

        }
        private void addColumnButton_Click(object sender, EventArgs e)
        {
            systemGridView.ColumnCount++;
        }

        private void deleteColumnButton_Click(object sender, EventArgs e)
        {
            systemGridView.ColumnCount--;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }



        private void qCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            QualityControl qc = new QualityControl();
            qc.Show();
        }
        #region tools
        private void installSASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "CPS";
            string applicationName = "Install SAS.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Install SAS.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: SAS";
            statusStrip1.Update();
        }

        private void showExtractOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!logic.showOutput)
            {
                logic.showOutput = true;
            }
            else
            {
                logic.showOutput = false;
            }
        }

        private void aVGRemoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Anti-Virus Removers";
            string applicationName = "AVG Remover.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\AVG Remover.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: AVG Remover";
            statusStrip1.Update();
        }

        private void eSETRemoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Anti-Virus Removers";
            string applicationName = "ESET Remover.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\ESET Remover.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: ESET Remover";
            statusStrip1.Update();
        }

        private void mcafeeRemoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Anti-Virus Removers";
            string applicationName = "Mcafee Remover.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Mcafee Remover.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Mcafee Remover";
            statusStrip1.Update();
        }

        private void nortonRemoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Anti-Virus Removers";
            string applicationName = "Norton Remover.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Norton Remover.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Norton Remover";
            statusStrip1.Update();
        }

        private void clamPortableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Anti-Virus Scanners";
            string applicationName = "Clam Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Clam Portable\\ClamWinPortable.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Clam Portable";
            statusStrip1.Update();
        }

        private void eSETOnlineScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Anti-Virus Scanners";
            string applicationName = "ESET Online Scanner.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\ESET Online Scanner.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Clam Portable";
            statusStrip1.Update();
        }

        private void mBAMADWCleanerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Anti-Virus Scanners";
            string applicationName = "MBAM ADWCleaner.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\MBAM ADWCleaner.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: MBAM ADWCleaner";
            statusStrip1.Update();
        }

        private void cCleanerPortableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "CCleaner Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\CCleaner Portable\\CCleaner64.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: CCleaner Portable";
            statusStrip1.Update();
        }

        private void chromePortableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "Chrome Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Chrome Portable\\GoogleChromePortable.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Chrome Portable";
            statusStrip1.Update();
        }

        private void installESETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "CPS";
            string applicationName = "Install WebRoot.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Install WebRoot.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: WebRoot";
            statusStrip1.Update();
        }

        private void installSEBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "CPS";
            string applicationName = "Install SEB.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Install SEB.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: SEB";
            statusStrip1.Update();
        }

        private void robocopy27ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Data Transfer Tools";
            string applicationName = "Robocopy2.7.exe.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Robocopy2.7.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Robocopy2.7.exe";
            statusStrip1.Update();
        }

        private void testDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Data Transfer Tools";
            string applicationName = "TestDisk Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\TestDisk Portable\\testdisk_win.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: testdisk_win.exe";
            statusStrip1.Update();
        }

        private void unstopableCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Data Transfer Tools";
            string applicationName = "Unstopable Copy.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Unstopable Copy.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Unstopable Copy.exe";
            statusStrip1.Update();
        }

        private void spaceSnifferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Data Usage Info";
            string applicationName = "SpaceSniffer.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\SpaceSniffer.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: SpaceSniffer.exe";
            statusStrip1.Update();
        }

        private void windirstatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Data Usage Info";
            string applicationName = "Windirstat Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Windirstat Portable\\windirstat.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: windirstat.exe";
            statusStrip1.Update();
        }

        private void fileRemoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "File Perms and Removal";
            string applicationName = "File Remover.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\File Remover\\FileASSASSIN.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: FileASSASSIN.exe";
            statusStrip1.Update();
        }

        private void takeownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "File Perms and Removal";
            string applicationName = "Takeown.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Takeown.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Takeown.exe";
            statusStrip1.Update();
        }

        private void hWMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Monitoring";
            string applicationName = "HWMonitor.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\HWMonitor\\HWMonitor_x64.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: WMonitor_x64.exe";
            statusStrip1.Update();
        }

        private void realTempToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Monitoring";
            string applicationName = "RealTemp.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\RealTemp.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: RealTemp.exe";
            statusStrip1.Update();
        }

        private void batteryHealthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Testing";
            string applicationName = "Battery Health.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Battery Health\\Battery Info Portable\\BatteryInfoView.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: BatteryInfoView.exe";
            statusStrip1.Update();
        }

        private void burnInTestInstallerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Testing";
            string applicationName = "Burn In Test Installer.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Burn In Test Installer.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Burn In Test Installer.exe";
            statusStrip1.Update();
        }

        private void cPUTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Testing";
            string applicationName = "CPU Test.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\CPU Test\\Prime95.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Prime95.exe";
            statusStrip1.Update();
        }

        private void gPUTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Testing";
            string applicationName = "GPU Test.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\GPU Test\\FurMark Portable\\FurMark.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: FurMark.exe";
            statusStrip1.Update();
        }

        private void hDDHealthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Testing";
            string applicationName = "HDD Health.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            //string executionCMD = logic.tools + "\\" + applicationDir + "\\HDD Health\\gsmartcontrol.exe";
            string executionCMD = logic.tools + "\\" + applicationDir + "\\HDD Health\\";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: gsmartcontrol.exe";
            statusStrip1.Update();
        }

        private void sSDHealthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Testing";
            string applicationName = "SSD Health.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\SSD Health\\SSDlife.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: SSDlife.exe";
            statusStrip1.Update();
        }

        private void webcamTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Testing";
            string applicationName = "Webcam Test.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Webcam Test.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Webcam Test.exe";
            statusStrip1.Update();
        }

        private void keyfinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Key Pullers";
            string applicationName = "Keyfinder Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Keyfinder Portable\\keyfinder.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: keyfinder.exe";
            statusStrip1.Update();
        }

        private void produKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Key Pullers";
            string applicationName = "ProduKey Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\ProduKey Portable\\ProduKey.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: ProduKey.exe";
            statusStrip1.Update();
        }

        private void pullkey8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Key Pullers";
            string applicationName = "Pullkey 8 Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Pullkey 8 Portable\\pkeyui.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: pkeyui.exe";
            statusStrip1.Update();
        }

        private void pullkey10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Key Pullers";
            string applicationName = "Pullkey 10 Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Pullkey 10 Portable\\pkeyui.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: pkeyui.exe";
            statusStrip1.Update();
        }

        private void recoveryKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Key Pullers";
            string applicationName = "RecoverKeys Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\RecoverKeys Portable\\RecoverKeys.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: RecoverKeys.exe";
            statusStrip1.Update();
        }

        private void oA3ScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void officeDownloaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "Office Tools.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Office Tools\\Office Downloader.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Office Downloader.exe";
            statusStrip1.Update();
        }

        private void removeOfficeToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "Office Tools.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Office Tools\\Remove Office Tool.diagcab";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Remove Office Tool.diagca";
            statusStrip1.Update();
        }

        private void demoUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "PCL Scripts.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\PCL Scripts\\DemoUpdate.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: DemoUpdate.exe";
            statusStrip1.Update();
        }

        private void storeDemoKitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "PCL Scripts.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\PCL Scripts\\StoreDemoKit.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: StoreDemoKit.exe";
            statusStrip1.Update();
        }

        private void tUKInstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "PCL Scripts.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\PCL Scripts\\TUK-Install.cmd";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: TUK-Install.cmd";
            statusStrip1.Update();
        }

        private void tUKUpdateBiosLoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "PCL Scripts.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\PCL Scripts\\TUK-UpdateBiosLove.cmd";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: TUK-UpdateBiosLove.cmd";
            statusStrip1.Update();
        }

        private void tUKUpdateDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "PCL Scripts.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\PCL Scripts\\TUK-UpdateDrive.cmd";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: TUK-UpdateDrive.cmd";
            statusStrip1.Update();
        }

        private void updateDemoScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "PCL Scripts.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\PCL Scripts\\UpdateDemo.bat";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: UpdateDemo.bat";
            statusStrip1.Update();
        }

        private void qCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "QC Steps.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\QC Steps\\";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Opening QC Steps";
            statusStrip1.Update();
        }

        private void recuvaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Recover Data";
            string applicationName = "Recuva Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Recuva Portable\\recuva64.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: recuva64.exe";
            statusStrip1.Update();
        }

        private void shadowExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Recover Data";
            string applicationName = "Shadow Explorer.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Shadow Explorer.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Shadow Explorer.exe";
            statusStrip1.Update();
        }

        private void cPUZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "System Info";
            string applicationName = "CPU-Z.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\CPU-Z.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: CPU-Z.exe";
            statusStrip1.Update();
        }

        private void gPUZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "System Info";
            string applicationName = "GPU-Z.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\GPU-Z.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: GPU-Z.exe";
            statusStrip1.Update();
        }

        private void speccyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "System Info";
            string applicationName = "Speccy Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Speccy Portable\\Speccy64.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Speccy64.exe";
            statusStrip1.Update();
        }

        private void pCDecrapifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Uninstall Tools";
            string applicationName = "PCDecrapifier.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\PCDecrapifier.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: PCDecrapifier.exe";
            statusStrip1.Update();
        }

        private void revoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Uninstall Tools";
            string applicationName = "Revo Portable.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Revo Portable\\RevoUPort.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: RevoUPort.exe";
            statusStrip1.Update();
        }

        private void uninstallerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Uninstall Tools";
            string applicationName = "Uninstaller.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Uninstaller.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Uninstaller.exe";
            statusStrip1.Update();
        }

        private void win10MediaCreator1903ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Windows Tools";
            string applicationName = "Windows 10 Media Creation (1903).7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Windows 10 Media Creation (1903).exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Windows 10 Media Creation (1903).exe";
            statusStrip1.Update();
        }

        private void win10UpdateAss1809ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Windows Tools";
            string applicationName = "Windows 10 Upgrade Assistant (1809).7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Windows 10 Upgrade Assistant (1809).exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Windows 10 Upgrade Assistant (1809).exe";
            statusStrip1.Update();
        }

        private void win10UpdateAss1903ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Windows Tools";
            string applicationName = "Windows 10 Upgrade Assistant (1903).7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Windows 10 Upgrade Assistant (1903).exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Windows 10 Upgrade Assistant (1903).exe";
            statusStrip1.Update();
        }

        private void windowsDownloaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Windows Tools";
            string applicationName = "Windows Downloader.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Windows Downloader.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Windows Downloader.exe";
            statusStrip1.Update();
        }

        private void clevoControlCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Drivers";
            string applicationName = "Clevo Control Center.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Clevo Control Center\\setup.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Clevo CC: setup.exe";
            statusStrip1.Update();
        }

        private void win10MediaCreator1909ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Windows Tools";
            string applicationName = "Windows 10 Media Creation (1909).7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Windows 10 Media Creation (1909).exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Windows 10 Media Creation (1909).exe";
            statusStrip1.Update();
        }

        private void clevoFingerprintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Drivers";
            string applicationName = "Clevo Fingerprint.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Clevo Fingerprint\\SetupInf.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Clevo Fingerprint: SetupInf.exe";
            statusStrip1.Update();
        }

        private void debloatNvidiaDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "Drivers";
            string applicationName = "Debloat Nvidia Driver.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Debloat Nvidia Driver.bat";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Debloat Nvidia Driver.bat";
            statusStrip1.Update();
        }

        private void mouseJigglerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "mousejiggler.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\mousejiggler\\MouseJiggle.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: MouseJiggle.exe";
            statusStrip1.Update();
        }
        #endregion
        public abstract class logic
        {
            //Definitions
            public static string temp = System.IO.Path.GetTempPath();
            public static string root = System.IO.Path.GetTempPath() + "root";
            public static string tools = System.IO.Path.GetTempPath() + "root\\Tools";
            public static string extractor = System.IO.Path.GetTempPath() + "root\\7-Zip64";
            public static string choocher = System.IO.Path.GetTempPath() + "root\\curl64";
            public static string path7bin = System.IO.Path.GetTempPath() + "root\\7-Zip64\\7z.exe";
            public static string pdw = Directory.GetCurrentDirectory();
            public static bool showOutput = false;

            //Main
            public static void initialize()
            {
                //Initialize root. %temp%\root
                initializeRoot();
                //Initialize extractor. %temp%\root\7-Zip64
                initializeExtractor();
                //Initialize curl for chooching. %temp%\root\curl64
                initializeChooch();
            }
            public static long BtoGb(long bytes)
            {
                return (bytes / 1024 / 1024 / 1024);
            }
            //initializeRoot
            //PRE: logic.root
            //POST:
            static void initializeRoot()
            {
                //see if the directory exists; if not, create it.
                try
                {
                    Directory.CreateDirectory(logic.root);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Root folder creation caught an exception.\n" + ex.ToString());
                }
            }
            //initializeExtractor
            //PRE: logic.extractor
            //POST:
            static void initializeExtractor()
            {

                try
                {
                    ZipFile.ExtractToDirectory(logic.pdw + "\\Tools\\7-Zip64.zip", logic.root);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Extracting 7zip64 caught an exception.\n" + ex.ToString());
                }
            }
            //initializeChooch
            //PRE: logic.extractor
            //POST:
            static void initializeChooch()
            {
                try
                {
                    ZipFile.ExtractToDirectory(logic.pdw + "\\Tools\\curl64.zip", logic.root);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Extracting curl caught an exception.\n" + ex.ToString());
                }
            }

            //extractApplication
            //PRE: applicationName, applicationDir
            //POST: 
            public static void extractApplication(string name, string dir)
            {
                string path = logic.pdw + "\\Tools\\" + dir + "\\" + name;
                try
                {
                    Directory.CreateDirectory(logic.root + "\\Tools\\" + dir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                /*
                if (!Directory.Exists(logic.root + "\\Tools\\" + dir))
                {
                    Directory.CreateDirectory(logic.root + "\\Tools\\" + dir);
                }*/
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardOutput = true;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "cmd /c \"\"" + logic.path7bin + "\" x \"" + path + "\" -o\"" + logic.root + "\\Tools\\" + dir + "\" -y -r\"";
                //Check the args
                //MessageBox.Show(startInfo.Arguments);
                using (Process exeProcess = Process.Start(startInfo))
                {
                    int timeOut = 5000;
                    StreamReader reader = exeProcess.StandardOutput;
                    string output = reader.ReadToEnd();
                    if (showOutput)
                    {
                        MessageBox.Show(output);
                    }
                    exeProcess.WaitForExit(timeOut);
                }
            }
            public static long GetTotalFreeSpace(string driveName)
            {

                try
                {
                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        if (drive.IsReady && drive.Name == driveName)
                        {
                            return BtoGb(drive.TotalFreeSpace);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("One of the drives cannot be accessed!");
                    MessageBox.Show("One of the drives cannot be accessed!\n" + driveName);
                    return -1;
                }
                return -1;
            }
            //apiHandler()
            //PRE: string binCURL, string URL, int type, bool returns
            //POST: string[] error

            //timestamp()
            //PRE: 
            //POST: string
            public static string timestamp()
            {
                int deltaTinS = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                DateTime ConvertedUnixTime = DateTimeOffset.FromUnixTimeSeconds(deltaTinS).DateTime;
                return ConvertedUnixTime.ToString("MM:dd:yyyy @ HH:mm UTC");
            }
            public static void ExecuteCommand(string command)
            {
                var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;

                var process = Process.Start(processInfo);

                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine("output>>" + e.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine("error>>" + e.Data);
                process.BeginErrorReadLine();

                process.WaitForExit();

                Console.WriteLine("ExitCode: {0}", process.ExitCode);
                process.Close();
            }
            public static void ExecutePowershell(string command)
            {
                var processInfo = new ProcessStartInfo("powershell.exe", command);
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;

                var process = Process.Start(processInfo);

                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine("output>>" + e.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine("error>>" + e.Data);
                process.BeginErrorReadLine();

                process.WaitForExit();

                Console.WriteLine("ExitCode: {0}", process.ExitCode);
                process.Close();
            }
            public static void setEnvVars()
            {
                // Initialize
                string value;
                bool toDelete = false;

                // Check whether the environment variable exists.
                value = Environment.GetEnvironmentVariable("PDW");
                // If necessary, create it.
                if (value == null)
                {
                    Environment.SetEnvironmentVariable("PDW", Directory.GetCurrentDirectory());
                    toDelete = true;

                    // Now retrieve it.
                    value = Environment.GetEnvironmentVariable("PDW");
                }
                // Display the value.
                Console.WriteLine($"PDW: {value}\n");
                // Confirm that the value can only be retrieved from the process
                // environment block if running on a Windows system.
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Console.WriteLine("Attempting to retrieve PDW from:");
                    foreach (EnvironmentVariableTarget enumValue in
                                      Enum.GetValues(typeof(EnvironmentVariableTarget)))
                    {
                        value = Environment.GetEnvironmentVariable("PDW", enumValue);
                        Console.WriteLine($"   {enumValue}: {(value != null ? "found" : "not found")}");
                    }
                    Console.WriteLine();
                }

                // If we've created it, now delete it.
                if (toDelete)
                {
                    Environment.SetEnvironmentVariable("PDW", null);
                    // Confirm the deletion.
                    if (Environment.GetEnvironmentVariable("PDW") == null)
                        Console.WriteLine("PDW has been deleted.");
                }
            }
        }
        private void evaluationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //scaffoldCreds login = new scaffoldCreds();
            //login.ShowDialog();


            //Extract GSMARTCTL
            string applicationDir = "Hardware Testing";
            string applicationName = "HDD Health.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();

            //Extract ESET
            applicationDir = "Anti-Virus Scanners";
            applicationName = "ESET Online Scanner.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();

            //Extract MBAM
            applicationDir = "Anti-Virus Scanners";
            applicationName = "MBAM ADWCleaner.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();

            computerEvaluationForm eval = new computerEvaluationForm();
            eval.Show();
        }
        public void CreateMyRichTextBox(string output)
        {

        }
        public void updateRichTextBox(string output)
        {
            //richTextBox1.Text += output;
        }
        public class KeystrokMessageFilter : System.Windows.Forms.IMessageFilter
        {
            public KeystrokMessageFilter() { }

            #region Implementation of IMessageFilter

            public bool PreFilterMessage(ref Message m)
            {
                if ((m.Msg == 256 /*0x0100*/))
                {
                    switch (((int)m.WParam) | ((int)Control.ModifierKeys))
                    {
                        case (int)(Keys.Control | Keys.Alt | Keys.T):

                            string applicationDir = "misc";
                            string applicationName = "Tron.7z";
                            MessageBox.Show("Tuning up: This can take a while!");
                            logic.extractApplication(applicationName, applicationDir);
                            string executionCMD = "powershell " + logic.tools + "\\" + applicationDir + "\\Tron\\tron.bat -a -sa -p -x -sd -np";
                            logic.ExecuteCommand(executionCMD);
                            MessageBox.Show("Tune Up Completed.");
                            break;
                            //This does not work. It seems you can only check single character along with CTRL and ALT.
                            //case (int)(Keys.Control | Keys.Alt | Keys.K | Keys.P):
                            //    MessageBox.Show("You pressed ctrl + alt + k + p");
                            //    break;
                            /*
                            case (int)(Keys.Control | Keys.C):
                                MessageBox.Show("You pressed ctrl+c");
                                break;
                            case (int)(Keys.Control | Keys.V):
                                MessageBox.Show("You pressed ctrl+v");
                                break;
                            case (int)Keys.Up:
                                MessageBox.Show("You pressed up");
                                break;
                            */
                    }
                }
                return false;
            }

            #endregion


        }
        public enum httpVerb
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public enum authenticationType
        {
            Basic,
            NTLM
        }


        class RestClient
        {
            public string endPoint { get; set; }
            public httpVerb httpMethod { get; set; }
            public authenticationType authType { get; set; }
            public string userName { get; set; }
            public string userPassword { get; set; }
            public string postJSON { get; set; } //New Attribute

            public RestClient()
            {
                endPoint = string.Empty;
            }

            public string makeRequest()
            {
                string strResponseValue = string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
                request.Method = httpMethod.ToString();

                String authHeaer = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(userName + ":" + userPassword));
                request.Headers.Add("Authorization", "Basic " + authHeaer);


                //********* NEW CODE TO SUPPORT POSTING *********
                if (request.Method == "POST" && postJSON != string.Empty)
                {
                    request.ContentType = "application/json"; //Really Important
                    using (StreamWriter swJSONPayload = new StreamWriter(request.GetRequestStream()))
                    {
                        swJSONPayload.Write(postJSON);
                        swJSONPayload.Close();
                    }
                }

                HttpWebResponse response = null;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    //Proecess the resppnse stream... (could be JSON, XML or HTML etc..._
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                }
                finally
                {
                    if (response != null)
                    {
                        ((IDisposable)response).Dispose();
                    }
                }
                return strResponseValue;
            }//End of makeRequest
        }//End of Class

        private void aboutThisToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You may notice that some of these items don't work.\n" +
                            "That's because they don't exist in these folders:\n" +
                            "Tools:\n" +
                            "-Anti-Virus Removers\n" +
                            "-Anti-Virus Scanners\n" +
                            "-CPS\n" +
                            "-Data Transfer Tools\n" +
                            "-Data Usage Info\n" +
                            "-Drivers\n" +
                            "-File Perms and Removal\n" +
                            "-Hardware Monitoring\n" +
                            "-Hardware Testing\n" +
                            "-Key Pullers\n" +
                            "-misc\n" +
                            "-Recover Data\n" +
                            "-System Info\n" +
                            "-Uninstall Tools\n" +
                            "-Windows Tools\n\n" +
                            "You may be able to put your own copy of the folder for the tool in there as a .7z and see if it works. ;-) "
                            );
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Monitoring";
            string applicationName = "HWMonitor.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            string executionCMD = logic.tools + "\\" + applicationDir + "\\HWMonitor\\HWMonitor_x64.exe";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: WMonitor_x64.exe";
            statusStrip1.Update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string applicationDir = "Hardware Testing";
            string applicationName = "HDD Health.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            //string executionCMD = logic.tools + "\\" + applicationDir + "\\HDD Health\\gsmartcontrol.exe";
            string executionCMD = logic.tools + "\\" + applicationDir + "\\HDD Health\\";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName, executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: gsmartcontrol.exe";
            statusStrip1.Update();
        }

        private void scaffold_Click(object sender, EventArgs e)
        {
            SkookumChoocher meta = new SkookumChoocher();
            meta.Show();
        }

        private void dISMCleanUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationDir = "misc";
            string applicationName = "Fixes.7z";
            toolStripStatusLabel1.Text = "Extracting...";
            statusStrip1.Update();
            logic.extractApplication(applicationName, applicationDir);
            toolStripStatusLabel1.Text = "Extract Successful!";
            statusStrip1.Update();
            //string executionCMD = logic.tools + "\\" + applicationDir + "\\HDD Health\\gsmartcontrol.exe";
            string executionCMD = logic.tools + "\\" + applicationDir + "\\Win_10_DISM_clean.cmd";
            System.Diagnostics.Process.Start(executionCMD);
            systemGridView.Rows.Add("Task:");
            systemGridView.Rows.Add(applicationName + "\\Win_10_DISM_clean.cmd", executionCMD, logic.timestamp());
            toolStripStatusLabel1.Text = "Launching: Win_10_DISM_clean.cmd";
            statusStrip1.Update();
        }
    }
}