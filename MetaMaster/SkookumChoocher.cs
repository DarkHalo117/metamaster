using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using static MetaMaster.Form1;

namespace MetaMaster
{
    public partial class SkookumChoocher : Form
    {
        public SkookumChoocher()
        {
            InitializeComponent();
            //initialize saved scaffold emails to comboBox1
        }

        private void exec_chooch_Click(object sender, EventArgs e)
        {
            RestClient rClient = new RestClient();
            rClient.endPoint = "https://scaffold.pclaptops.com/api/index";
            rClient.httpMethod = httpVerb.POST;
            rClient.userName = textBox1.Text;
            rClient.userPassword = comboBox1.Text;
            string strResponse = string.Empty;
            if (!apiController.messageConstructed)
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(comboBox1.Text))
                {
                    //Set Action
                    apiController.cURLrequest += "{\"action\":\"" + comboBox2.Text + "\",";
                    apiController.cURLrequest += "\"user_email\":\"" + comboBox1.Text + "\",";
                    apiController.cURLrequest += "\"user_password\":\"" + textBox1.Text + "\",";
                    //Set Application
                    apiController.cURLrequest += "\"application\":\"" + listBox1.Text + "\",";
                    //Initialize Payload
                    apiController.setAction(comboBox2.SelectedIndex, listBox1.SelectedIndex, apiController.responseData);
                    apiController.cURLrequest += "}"; //Close Action
                    rClient.postJSON = apiController.cURLrequest; //Save request
                    //apiController.messageConstructed = true; //Message Constructed
                }
                else { MessageBox.Show("You must enter an Email and Password to submit."); }
            }
            else
            {
                MessageBox.Show("Error constucting request. Try again.");
                apiController.messageConstructed = false;
            }
            if(comboBox2.SelectedIndex == 6)
            {
                DialogResult dialogResult = MessageBox.Show("Sure?", "Delete?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //Send it
                    strResponse = rClient.makeRequest();
                    //Process response
                    apiController.processResponse(strResponse);
                    dataGridView1.DataSource = apiController.responseData;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    apiController.destructRequest();//Message Destruct
                }
            }
            else if (apiController.messageConstructed == true)
            {
                //Send it
                strResponse = rClient.makeRequest();
                //Process response
                apiController.processResponse(strResponse);
                dataGridView1.DataSource = apiController.responseData;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                apiController.destructRequest();//Message Destruct
            }
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
                String authHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(userName + ":" + userPassword));
                request.Headers.Add("Authorization", "Basic " + authHeader);


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

        public abstract class apiController
        {
            /***************************
             cURL Method Implemented here
            ****************************/
            public static bool messageConstructed = false;
            public static bool confirmDelete = false;
            public static string cURLrequest;
            public static DataTable responseData = new DataTable();

            public static void saveDatatable(DataTable RequestObj)
            {
                responseData = RequestObj;
            }
            public static void processResponse(string response)
            {
                response = response.Replace('\r', ' ');
                response = response.TrimEnd('\n');
                response = response.Replace('\n', ',');
                response = response.TrimStart('{');
                response = response.TrimEnd('}');
                string[] kvarray = Regex.Split(response, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                string[] finalarray = Array.Empty<string>();
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                //add try catch exception here
                try
                {
                    foreach (string d in kvarray)
                    {
                        //finalarray = d.Split(new[] { ':' }, 2).ToArray();
                        finalarray = Regex.Split(d, ":(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        finalarray[0] = finalarray[0].Replace('\"', ' ');
                        finalarray[1] = finalarray[1].Replace('\"', ' ');
                        KeyValuePair<string, string> pair = new KeyValuePair<string, string>(finalarray[0], finalarray[1]);
                        if (!dictionary.ContainsKey(pair.Key))
                        {
                            dictionary.Add(pair.Key, pair.Value);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error converting response data.\n" + ex.ToString());
                    MessageBox.Show("Error in Scaffold data \nResponse: " + response);
                }
                
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
                foreach (KeyValuePair<string, string> p in dictionary)
                {
                    dtRow = RequestObj.NewRow();
                    dtRow["key"] = p.Key;
                    dtRow["value"] = p.Value;
                    RequestObj.Rows.Add(dtRow);
                }
                saveDatatable(RequestObj);
            }

            public static void setAction(int action, int payload, DataTable inlineData)
            {
                //initialize
                string record_id = string.Empty;
                //code
                switch (action)
                {
                    case 0:
                        Console.WriteLine("Action:0:Search");
                        switch (payload)
                        {
                            case 0:
                                Console.WriteLine("Payload:0:software_license_fetch");
                                break;
                            case 1:
                                Console.WriteLine("Payload:1:computer_evaluation");
                                break;
                            case 2:
                                Console.WriteLine("Payload:2:users");
                                break;
                            case 3:
                                Console.WriteLine("Payload:3:customer_request_order");
                                break;
                            case 4:
                                Console.WriteLine("Payload:4:everest");
                                break;
                            default:
                                Console.WriteLine("Payload:default:none selected");
                                break;
                        }
                        break;
                    case 1:
                        Console.WriteLine("Action:1:everest_call");
                        switch (payload)
                        {
                            case 0:
                                Console.WriteLine("Payload:0:software_license_fetch");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 1:
                                Console.WriteLine("Payload:1:computer_evaluation");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 2:
                                Console.WriteLine("Payload:2:users");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 3:
                                Console.WriteLine("Payload:3:customer_request_order");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 4:
                                Console.WriteLine("Payload:4:everest");
                                //apiController.cURLrequest += "\"format_data\":\"edn\",";
                                /*
                                apiController.cURLrequest += "\"call\":\"getCustomerByPhone\",\"arg1\":\"";
                                ShowInputDialog(ref record_id, "PH#");
                                apiController.cURLrequest += record_id;
                                */
                                apiController.cURLrequest += "\"call\":\"getOrdersByCustomerId\",\"arg1\":\"";
                                ShowInputDialog(ref record_id, "ID#");
                                apiController.cURLrequest += record_id;
                                apiController.cURLrequest += "\",\"arg2\":10";
                                apiController.messageConstructed = true; //Message Constructed
                                break;
                            default:
                                Console.WriteLine("Payload:default:none selected");
                                MessageBox.Show("No function selected!");
                                apiController.destructRequest();//Message Destruct
                                break;
                        }
                        break;
                    case 2:
                        Console.WriteLine("Action:2:fetch_keys");
                        switch (payload)
                        {
                            case 0:
                                Console.WriteLine("Payload:0:software_license_fetch");
                                apiController.cURLrequest += "\"company\":\"pcl\",\"id_order\":\"";
                                ShowInputDialog(ref record_id, "SO#");
                                apiController.cURLrequest += record_id;
                                apiController.cURLrequest += "\"";
                                apiController.messageConstructed = true; //Message Constructed
                                break;
                            case 1:
                                Console.WriteLine("Payload:1:computer_evaluation");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 2:
                                Console.WriteLine("Payload:2:users");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 3:
                                Console.WriteLine("Payload:3:customer_request_order");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 4:
                                Console.WriteLine("Payload:4:everest");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            default:
                                Console.WriteLine("Payload:default:none selected");
                                MessageBox.Show("No function selected!");
                                apiController.destructRequest();//Message Destruct
                                break;
                        }
                        break;
                    case 3:
                        Console.WriteLine("Action:3:create");
                        switch (payload)
                        {
                            case 0:
                                Console.WriteLine("Payload:0:software_license_fetch");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 1:
                                Console.WriteLine("Payload:1:computer_evaluation");
                                //Extract GSMARTCTL
                                string applicationDir = "Hardware Testing";
                                string applicationName = "HDD Health.7z";
                                logic.extractApplication(applicationName, applicationDir);

                                //Extract ESET
                                applicationDir = "Anti-Virus Scanners";
                                applicationName = "ESET Online Scanner.7z";
                                logic.extractApplication(applicationName, applicationDir);

                                //Extract MBAM
                                applicationDir = "Anti-Virus Scanners";
                                applicationName = "MBAM ADWCleaner.7z";
                                logic.extractApplication(applicationName, applicationDir);

                                computerEvaluationForm eval = new computerEvaluationForm();
                                eval.Show();
                                break;
                            case 2:
                                Console.WriteLine("Payload:2:users");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 3:
                                Console.WriteLine("Payload:3:customer_request_order");
                                break;
                            case 4:
                                Console.WriteLine("Payload:4:everest");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            default:
                                Console.WriteLine("Payload:default:none selected");
                                MessageBox.Show("No function selected!");
                                apiController.destructRequest();//Message Destruct
                                break;
                        }
                        break;
                    case 4:
                        Console.WriteLine("Action:4:read");
                        switch (payload)
                        {
                            case 0:
                                Console.WriteLine("Payload:0:software_license_fetch");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 1:
                                Console.WriteLine("Payload:1:computer_evaluation");
                                apiController.cURLrequest += "\"payload\":{\"id_record\":\"";
                                ShowInputDialog(ref record_id, "Rec#");
                                apiController.cURLrequest += record_id;
                                apiController.cURLrequest += "\"";
                                apiController.cURLrequest += "}"; //Close Payload
                                apiController.messageConstructed = true; //Message Constructed
                                break;
                            case 2:
                                Console.WriteLine("Payload:2:users");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 3:
                                Console.WriteLine("Payload:3:customer_request_order");
                                apiController.cURLrequest += "\"payload\":{\"id_record\":\"";
                                ShowInputDialog(ref record_id, "Rec#");
                                apiController.cURLrequest += record_id;
                                apiController.cURLrequest += "\"";
                                apiController.cURLrequest += "}"; //Close Payload
                                apiController.messageConstructed = true; //Message Constructed
                                break;
                            case 4:
                                Console.WriteLine("Payload:4:everest");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            default:
                                Console.WriteLine("Payload:default:none selected");
                                MessageBox.Show("No function selected!");
                                apiController.destructRequest();//Message Destruct
                                break;
                        }
                        break;
                    case 5:
                        Console.WriteLine("Action:5:update");
                        switch (payload)
                        {
                            case 0:
                                Console.WriteLine("Payload:0:software_license_fetch");
                                MessageBox.Show("No function!");
                                apiController.messageConstructed = false; //Message Destruct
                                break;
                            case 1:
                                Console.WriteLine("Payload:1:computer_evaluation");
                                apiController.cURLrequest += "\"payload\":{";
                                constructPayload(inlineData);
                                apiController.cURLrequest += "}"; //Close Payload
                                apiController.messageConstructed = true; //Message Constructed
                                break;
                            case 2:
                                Console.WriteLine("Payload:2:users");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 3:
                                Console.WriteLine("Payload:3:customer_request_order");
                                apiController.cURLrequest += "\"payload\":{";
                                constructPayload(inlineData);
                                apiController.cURLrequest += "}"; //Close Payload
                                apiController.messageConstructed = true; //Message Constructed
                                break;
                            case 4:
                                Console.WriteLine("Payload:4:everest");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            default:
                                Console.WriteLine("Payload:default:none selected");
                                MessageBox.Show("No function selected!");
                                apiController.destructRequest();//Message Destruct
                                break;
                        }
                        break;
                    case 6:
                        Console.WriteLine("Action:6:delete");
                        switch (payload)
                        {
                            case 0:
                                Console.WriteLine("Payload:0:software_license_fetch");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 1:
                                Console.WriteLine("Payload:1:computer_evaluation");
                                apiController.cURLrequest += "\"payload\":{\"id_record\":\"";
                                ShowInputDialog(ref record_id, "Rec#");
                                apiController.cURLrequest += record_id;
                                apiController.cURLrequest += "\"";
                                apiController.cURLrequest += "}"; //Close Payload
                                break;
                            case 2:
                                Console.WriteLine("Payload:2:users");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            case 3:
                                Console.WriteLine("Payload:3:customer_request_order");
                                apiController.cURLrequest += "\"payload\":{\"id_record\":\"";
                                ShowInputDialog(ref record_id, "Rec#");
                                apiController.cURLrequest += record_id;
                                apiController.cURLrequest += "\"";
                                apiController.cURLrequest += "}"; //Close Payload
                                break;
                            case 4:
                                Console.WriteLine("Payload:4:everest");
                                MessageBox.Show("No function!");
                                apiController.destructRequest();//Message Destruct
                                break;
                            default:
                                Console.WriteLine("Payload:default:none selected");
                                MessageBox.Show("No function selected!");
                                apiController.destructRequest();//Message Destruct
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Action:default:none_selected");
                        MessageBox.Show("No action selected!");
                        apiController.destructRequest();//Message Destruct
                        break;
                }
            }//End of setAction
            private static DialogResult ShowInputDialog(ref string input, string valueName)
            {
                System.Drawing.Size size = new System.Drawing.Size(200, 70);
                Form inputBox = new Form();

                inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                inputBox.ClientSize = size;
                inputBox.Text = valueName;

                System.Windows.Forms.TextBox textBox = new TextBox();
                textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
                textBox.Location = new System.Drawing.Point(5, 5);
                textBox.Text = input;
                inputBox.Controls.Add(textBox);

                Button okButton = new Button();
                okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
                okButton.Name = "okButton";
                okButton.Size = new System.Drawing.Size(75, 23);
                okButton.Text = "&OK";
                okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
                inputBox.Controls.Add(okButton);

                Button cancelButton = new Button();
                cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                cancelButton.Name = "cancelButton";
                cancelButton.Size = new System.Drawing.Size(75, 23);
                cancelButton.Text = "&Cancel";
                cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
                inputBox.Controls.Add(cancelButton);

                inputBox.AcceptButton = okButton;
                inputBox.CancelButton = cancelButton;

                DialogResult result = inputBox.ShowDialog();
                input = textBox.Text;
                return result;
            } //End of DialogBox Function
            public static void constructPayload (DataTable data)
            {
                int length = data.Rows.Count;
                int count = 0;
                foreach (DataRow dataRow in data.Rows)
                {
                    apiController.cURLrequest += "\"";
                    bool first = true;
                    foreach (var item in dataRow.ItemArray)
                    {
                        string s = item.ToString();
                        s = s.Replace('\r', ' ');
                        s = s.Replace('\n', ' ');
                        s = s.TrimEnd();
                        //s = s.Replace('\r\n', ' ');
                        Console.WriteLine(s);
                        //apiController.cURLrequest += s;
                        if (first) { apiController.cURLrequest += s + "\":\""; first = false; }
                        else { apiController.cURLrequest += s + "\""; }
                    }
                    if (count <= length - 2)
                    {
                        apiController.cURLrequest += ",";
                        count++;
                    }
                }
            }
            public static void destructRequest()
            {
                apiController.cURLrequest = null;
                apiController.messageConstructed = false;
            }
        }//end of apiController Class
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
    }
}
