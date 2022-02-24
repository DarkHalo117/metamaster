using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MetaMaster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string cwd = Directory.GetCurrentDirectory();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            // Initialization  


            //RequestObj requestObj = new RequestObj { Priority = "M", SalesChannel = "online" };

            // Call REST Web API.  
            //DataTable responseObj = (DataTable)Program.PostInfoAsync(requestObj).Result;
        }
        /*
        internal static async Task<object> PostInfoAsync(RequestObj requestObj)
        {
            //throw new NotImplementedException();
            // Initialization.  
            DataTable responseObj = new DataTable();

            // Posting.  
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri("https://localhost:44371/");

                // Setting content type.                   
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP POST  
                response = await client.PostAsJsonAsync("api/WebApi", requestObj).ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                    string result = response.Content.ReadAsStringAsync().Result;
                    responseObj = JsonConvert.DeserializeObject<DataTable>(result);
                }
            }

            return responseObj;
        }
        */
    }
}
