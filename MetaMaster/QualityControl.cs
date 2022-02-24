using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MetaMaster.Form1;

namespace MetaMaster
{
    public partial class QualityControl : Form
    {
        public QualityControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                //Run SAS
                string applicationDir = "CPS";
                string applicationName = "Install SAS.7z";
                logic.extractApplication(applicationName, applicationDir);
                string executionCMD = logic.tools + "\\" + applicationDir + "\\Install SAS.exe";
                try
                {
                    System.Diagnostics.Process.Start(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SAS failed to run.\n" + ex.ToString());
                }
            }
            if (checkBox3.Checked)
            {
                //Run WebRoot
                string applicationDir = "CPS";
                string applicationName = "Install WebRoot.7z";
                logic.extractApplication(applicationName, applicationDir);
                string executionCMD = logic.tools + "\\" + applicationDir + "\\Install WebRoot.exe";
                try
                {
                    System.Diagnostics.Process.Start(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Webroot failed to run.\n" + ex.ToString());
                }
            }
            if (checkBox4.Checked)
            {
                //Run SEB
                string applicationDir = "CPS";
                string applicationName = "Install SEB.7z";
                logic.extractApplication(applicationName, applicationDir);
                string executionCMD = logic.tools + "\\" + applicationDir + "\\Install SEB.exe";
                try
                {
                    System.Diagnostics.Process.Start(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SEB failed to run.\n" + ex.ToString());
                }
            }
            if (checkBox5.Checked)
            {
                //Extract Adblock
                string applicationDir = "misc";
                string applicationName = "Edge.7z";
                logic.extractApplication(applicationName, applicationDir);
                //Console.WriteLine(File.Exists(logic.tools + "\\" + applicationDir + "\\Edge.Appx") ? "File exists." : "File does not exist.");
                //string executionCMD = "DISM /Online /Add-ProvisionedAppxPackage /PackagePath:" + logic.tools + "\\" + applicationDir + "\\Edge.Appx /SkipLicense";
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardOutput = true;
                //Build Command
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "cmd /c DISM /Online /Add-ProvisionedAppxPackage /PackagePath:" + logic.tools + "\\" + applicationDir + "\\Edge.Appx /SkipLicense";
                
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    //Check the args
                    //MessageBox.Show(startInfo.Arguments);
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        int timeOut = 5000;
                        StreamReader reader = exeProcess.StandardOutput;
                        //string output = reader.ReadToEnd();
                        //MessageBox.Show(output);
                        exeProcess.WaitForExit(timeOut);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Adblock:DISM failed.\n" + ex.ToString());
                    MessageBox.Show("Adblock:DISM failed.\n" + ex.ToString());
                }
                //Edge
                //executionCMD = "cmd /c start microsoft-edge:https://microsoftedge.microsoft.com/addons/detail/adblock-plus-free-ad-bl/gmgoamodcdcjnbaobigkjelfplakmdhh";
                startInfo.FileName = "powershell.exe";
                startInfo.Arguments = "[system.Diagnostics.Process]::Start('msedge','https://microsoftedge.microsoft.com/addons/detail/adblock-plus-free-ad-bl/gmgoamodcdcjnbaobigkjelfplakmdhh')";
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    //Check the args
                    MessageBox.Show(startInfo.Arguments);
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        int timeOut = 5000;
                        StreamReader reader = exeProcess.StandardOutput;
                        //string output = reader.ReadToEnd();
                        //MessageBox.Show(output);
                        exeProcess.WaitForExit(timeOut);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Adblock:Edge failed to run.\n" + ex.ToString());
                    MessageBox.Show("Adblock:Edge failed to run.\n" + ex.ToString());
                }
                //Chrome
                //executionCMD = "cmd /c start C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe https://chrome.google.com/webstore/detail/adblock-plus-free-ad-bloc/cfhdojbkjhnklbpkdaibdccddilifddb";
                startInfo.FileName = "powershell.exe";
                startInfo.Arguments = "[system.Diagnostics.Process]::Start('chrome','https://chrome.google.com/webstore/detail/adblock-plus-free-ad-bloc/cfhdojbkjhnklbpkdaibdccddilifddb')";
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    //Check the args
                    MessageBox.Show(startInfo.Arguments);
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        int timeOut = 5000;
                        StreamReader reader = exeProcess.StandardOutput;
                        //string output = reader.ReadToEnd();
                        //MessageBox.Show(output);
                        exeProcess.WaitForExit(timeOut);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Adblock:Chrome failed to run.\n" + ex.ToString());
                    MessageBox.Show("Adblock:Chrome failed to run.\n" + ex.ToString());
                }
            }
            if (checkBox12.Checked)
            {
                //Run VLC
                string applicationDir = "misc";
                string applicationName = "VLC.7z";
                logic.extractApplication(applicationName, applicationDir);
                string executionCMD = logic.tools + "\\" + applicationDir + "\\VLC.exe";
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("VLC failed to run.\n" + ex.ToString());
                }
            }
            if (checkBox6.Checked)
            {
                //Run Chrome Default
                //Kill Chrome
                string executionCMD = "taskkill /F /IM chrome.exe";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Theme failed to set.\n" + ex.ToString());
                    MessageBox.Show("Command caught an exception: \n" + executionCMD);
                }
                //Extract files
                string applicationDir = "misc";
                string applicationName = "Default.7z";
                logic.extractApplication(applicationName, applicationDir);
                //Copy Files
                executionCMD = "Xcopy /E /I /Q /Y " + logic.tools + applicationDir + " \"%localappdata%\\Google\\Chrome\\User Data\\Default";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Theme failed to set.\n" + ex.ToString());
                    MessageBox.Show("Command caught an exception: \n" + executionCMD);
                }
                //Run Chrome Again
                executionCMD = "Start-Process -FilePath 'C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe' -ArgumentList 'https://chrome.google.com/webstore/detail/adblock-plus-free-ad-bloc/cfhdojbkjhnklbpkdaibdccddilifddb'";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Theme failed to set.\n" + ex.ToString());
                    MessageBox.Show("Command caught an exception: \n" + executionCMD);
                }
            }
            if (checkBox7.Checked)
            {
                //Extract Laptop Power
                string applicationDir = "misc";
                string applicationName = "Laptop.7z";
                logic.extractApplication(applicationName, applicationDir);
                //Run Laptop Power
                string executionCMD = "powercfg.exe -import \"" + logic.tools + "\\" + applicationDir + "\\Laptop.pow\"";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Theme failed to set.\n" + ex.ToString());
                    MessageBox.Show("Command caught an exception: \n" + executionCMD);
                }
                executionCMD = "powercfg.exe /hibernate off";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Theme failed to set.\n" + ex.ToString());
                    MessageBox.Show("Command caught an exception: \n" + executionCMD);
                }
            }
            if (checkBox16.Checked)
            {
                //Extract Desktop Power
                string applicationDir = "misc";
                string applicationName = "Desktop.7z";
                logic.extractApplication(applicationName, applicationDir);
                //Run Desktop Power
                string executionCMD = "powercfg.exe -import \"" + logic.tools + "\\" + applicationDir + "\\Desktop.pow\"";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Theme failed to set.\n" + ex.ToString());
                    MessageBox.Show("Command caught an exception: \n" + executionCMD);
                }
                executionCMD = "powercfg.exe /hibernate off";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Theme failed to set.\n" + ex.ToString());
                    MessageBox.Show("Command caught an exception: \n" + executionCMD);
                }
            }
            if (checkBox8.Checked)
            {
                //Extract Theme
                string applicationDir = "misc";
                string applicationName = "Theme.7z";
                logic.extractApplication(applicationName, applicationDir);
                //Run Theme
                string executionCMD = "start /b " + logic.tools + "\\" + applicationDir + "\\Theme.theme";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Theme failed to set.\n" + ex.ToString());
                    MessageBox.Show("Command caught an exception: \n" + executionCMD);
                }
            }
            if (checkBox10.Checked)
            {
                //Run Notifications
                string executionCMD = "start ms-settings:notifications";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Notifications failed to launch.\n" + ex.ToString());
                }
            }
            if (checkBox11.Checked)
            {
                //Run Lock Screen
                string executionCMD = "start ms-settings:lockscreen";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lock Screen failed to launch.\n" + ex.ToString());
                }
            }
            if (checkBox13.Checked)
            {
                //Create Restore Point
                //string executionCMD = "Enable-ComputerRestore; Checkpoint-Computer -Description \"PCL QC\"";
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardOutput = true;
                //Build Command
                startInfo.FileName = "powershell.exe";
                startInfo.Arguments = "Enable-ComputerRestore -Drive \"C:\"; Checkpoint-Computer -Description \"PCL_QC\";";

                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    //Check the args
                    MessageBox.Show(startInfo.Arguments);
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        int timeOut = 5000;
                        StreamReader reader = exeProcess.StandardOutput;
                        string output = reader.ReadToEnd();
                        MessageBox.Show(output);
                        exeProcess.WaitForExit(timeOut);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SystemRestore failed.\n" + ex.ToString());
                    MessageBox.Show("SystemRestore failed.\n" + ex.ToString());
                }
            }
            if (checkBox14.Checked)
            {
                //Remove Xbox
                string executionCMD = "Get-ProvisionedAppxPackage -Online | Where-Object { $_.PackageName -match \"xbox\" } | ForEach-Object { Remove-ProvisionedAppxPackage -Online -AllUsers -PackageName $_.PackageName }";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Xbox failed to remove.\n" + ex.ToString());
                }
            }
            if (checkBox15.Checked)
            {
                //Set Environment Variable
                string value;
                // Check whether the environment variable exists.
                value = Environment.GetEnvironmentVariable("MappedDrive");
                // If necessary, create it.
                if (value == null)
                {
                    Environment.SetEnvironmentVariable("MappedDrive", (comboBox1.Text + ":"), EnvironmentVariableTarget.Machine);
                    MessageBox.Show("Set ENV VAR: MappedDrive to " + comboBox1.Text + ":");
                    // Now retrieve it.
                    value = Environment.GetEnvironmentVariable("MappedDrive");
                }
                // Display the value.
                Console.WriteLine($"MappedDrive: {value}\n");
                //MessageBox.Show($"MappedDrive: {value}\n");
                // Confirm that the value can only be retrieved from the process
                // environment block if running on a Windows system.
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Console.WriteLine("Attempting to retrieve MappedDrive from:");
                    foreach (EnvironmentVariableTarget enumValue in
                                      Enum.GetValues(typeof(EnvironmentVariableTarget)))
                    {
                        value = Environment.GetEnvironmentVariable("MappedDrive", enumValue);
                        Console.WriteLine($"   {enumValue}: {(value != null ? "found" : "not found")}");
                        //MessageBox.Show($"   {enumValue}: {(value != null ? "found" : "not found")}");
                    }
                }
                //Remap Users
                //Let's make directories
                try
                {
                    Directory.CreateDirectory("%MappedDrive%\\Users");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder " + comboBox1.Text + ":\\Users: " + ex.ToString());
                }
                try
                {
                    Directory.CreateDirectory("%MappedDrive%\\Users\\Default");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder " + comboBox1.Text + ":\\Users\\Default: " + ex.ToString());
                }
                try
                {
                    Directory.CreateDirectory("%MappedDrive%\\Users\\Public");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder " + comboBox1.Text + ":\\Users\\Public: " + ex.ToString());
                }
                //MessageBox.Show(Environment.GetEnvironmentVariable("MappedDrive"));
                string executionCMD = "Set-ItemProperty -path 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList' -Name 'ProfilesDirectory' -Value '%MappedDrive%\\Users'";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Users Registry failed to set.\n" + ex.ToString());
                    MessageBox.Show("Users Registry failed to set.\n" + ex.ToString());
                }
                executionCMD = "Set-ItemProperty -path 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList' -Name 'Default' -Value '%MappedDrive%\\Users\\Default'";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Users Registry failed to set.\n" + ex.ToString());
                    MessageBox.Show("Users Registry failed to set.\n" + ex.ToString());
                }
                executionCMD = "Set-ItemProperty -path 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList' -Name 'Public' -Value '%MappedDrive%\\Users\\Public'";
                MessageBox.Show("Running: " + executionCMD);
                try
                {
                    //System.Diagnostics.Process.Start(executionCMD);
                    logic.ExecutePowershell(executionCMD);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Users Registry failed to set.\n" + ex.ToString());
                    MessageBox.Show("Users Registry failed to set.\n" + ex.ToString());
                }
            }
        }
    }
}
