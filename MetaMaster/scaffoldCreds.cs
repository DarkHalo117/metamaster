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

namespace MetaMaster
{
    public partial class scaffoldCreds : Form
    {
        public scaffoldCreds(DataTable RequestObj)
        {
            InitializeComponent();
            //bool doesItChooch = false;
            apiController.saveDatatable(RequestObj);
            
            //MessageBox.Show(apiController.cURLrequest);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //textBox1.Text, textBox2.Text
            //email, pass
            RestClient rClient = new RestClient();
            rClient.endPoint = "https://scaffold.pclaptops.com/api/index";
            //Need to add functionality for more post methods here!
            /*
             * 
                         switch(cboVerb.Text)
              {
                case "POST":
                  rClient.httpMethod = httpVerb.POST;
                  rClient.postJSON = txtPOSTData.Text;
                  break;
                default:
                  rClient.httpMethod = httpVerb.GET;
                  break;
              } 
             * 
             */
            rClient.httpMethod = httpVerb.POST;
            rClient.userName = textBox1.Text;
            rClient.userPassword = textBox2.Text;
            //Here I'll just change a boolean value to allow the transmission of the eval
            //makeItSo(true);
            
            if (!apiController.messageConstructed)
            {
                //apiController.cURLrequest = "curl " + "-ku " + textBox1.Text + ":" + textBox2.Text;
                //apiController.cURLrequest += " --request POST --header \"Content-Type: Application/json\" --data '{\"action\":\"create\",";
                apiController.cURLrequest += "{\"action\":\"create\",";
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
                {
                    apiController.cURLrequest += "\"user_email\":\"" + textBox1.Text + "\",";
                    apiController.cURLrequest += "\"user_password\":\"" + textBox2.Text + "\",";
                    apiController.cURLrequest += "\"application\":\"computer_evaluation\",";
                    apiController.cURLrequest += "\"payload\": {";
                    apiController.addDatatable();
                    apiController.cURLrequest += "}";
                    apiController.cURLrequest += "}";
                    //MessageBox.Show(apiController.cURLrequest);
                    apiController.messageConstructed = true;
                }
                else { MessageBox.Show("You must enter an Email and Password to submit."); }
            }
            else 
            { 
                MessageBox.Show("Error constucting request. Retrying!");
                //MessageBox.Show(apiController.cURLrequest);
            }
            rClient.postJSON = apiController.cURLrequest;
            string strResponse = string.Empty;
            strResponse = rClient.makeRequest();
            MessageBox.Show(strResponse);
            
        }
        public abstract class apiController
        {
            /***************************
             cURL Method Implemented here
            ****************************/
            public static bool messageConstructed = false;
            public static string cURLrequest;
            public static DataTable requestData = new DataTable();

            //Add data from DataTable
            public static void addDatatable()
            {
                char[] charsToTrim = { ',' };
                int length = requestData.Rows.Count;
                int count = 0;
                foreach (DataRow dataRow in requestData.Rows)
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
            public static void saveDatatable(DataTable RequestObj)
            {
                requestData = RequestObj;
            }
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
    }
}
