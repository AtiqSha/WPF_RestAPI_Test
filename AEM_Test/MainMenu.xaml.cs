using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using AEM_Test.Dto;
using System.Data.SqlClient;

namespace AEM_Test
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private string bearerToken;


        public MainMenu()
        {
            InitializeComponent();
        }

        public MainMenu(string bearerToken)
        {
            InitializeComponent();
            this.bearerToken = bearerToken;

            var result = AsyncContext.Run(GetPlatformWellActualAsync);
            addPlatformToDB(result);

        }

        private void GPWActual_Click(object sender, RoutedEventArgs e)
        {
            var result = AsyncContext.Run(GetPlatformWellActualAsync);
            addPlatformToDB(result);
        }

        private void GPWDummy_Click(object sender, RoutedEventArgs e)
        {
            var result = AsyncContext.Run(GetPlatformWellDummyAsync);
            addPlatformToDB(result);
        }

        public async Task<List<Platform>> GetPlatformWellActualAsync()
        {
            List<Platform> platforms = new List<Platform>();

            var uri = new Uri("http://test-demo.aem-enersol.com/api/PlatformWell/GetPlatformWellActual");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Trim('"'));

                var response = await client.GetAsync(uri);

                Debug.WriteLine(response.StatusCode.ToString());

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return platforms;
                }

                //string responseString = response.Content.ReadAsStringAsync().Result;
                var result = await response.Content.ReadAsStringAsync();
                //IEnumerable<test> result = await response.Content.ReadAsAsync<IEnumerable<test>>();

                platforms = JsonConvert.DeserializeObject<List<Platform>>(result);

                return platforms;


            }
        }

        public async Task<List<Platform>> GetPlatformWellDummyAsync()
        {
            List<Platform> platforms = new List<Platform>();

            var uri = new Uri("http://test-demo.aem-enersol.com/api/PlatformWell/GetPlatformWellDummy");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Trim('"'));

                var response = await client.GetAsync(uri);

                Debug.WriteLine(response.StatusCode.ToString());

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return platforms;
                }

                //string responseString = response.Content.ReadAsStringAsync().Result;
                var result = await response.Content.ReadAsStringAsync();
                //IEnumerable<test> result = await response.Content.ReadAsAsync<IEnumerable<test>>();

                platforms = JsonConvert.DeserializeObject<List<Platform>>(result);

                return platforms;


            }
        }

        private void addPlatformToDB(List<Platform> platforms)
        {
            string connectionString;
            SqlConnection cnn;
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=aem_test;Trusted_Connection=True";
            cnn = new SqlConnection(connectionString);
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";
            cnn.Open();



            foreach (Platform platform in platforms)
            {
                var tempUniqueName = string.IsNullOrEmpty(platform.uniqueName) ? "null" : "'" + platform.uniqueName + "'";
                var tempCreatedAt = string.IsNullOrEmpty(platform.createdAt) ? "null" : "'" +platform.createdAt+ "'";
                var tempUpdatedAt = string.IsNullOrEmpty(platform.updatedAt) ? "null" : "'" + platform.updatedAt + "'";

                string tempLatitude = null;
                string tempLongitude = null;

                //checking for latitude null
                if (platform.latitude == null)
                {
                    tempLatitude = "null";
                }
                else
                {
                    tempLatitude = platform.latitude.ToString();
                }
                //checking for longitude null
                if (platform.longitude == null)
                {
                    tempLongitude = "null";
                }
                else
                {
                    tempLongitude = platform.longitude.ToString();
                }


                /*sql = "MERGE platform AS Target " +
                    "USING (SELECT " + platform.id + " AS id) as Source " +
                    "On Target.id = Source.id " +
                    "WHEN MATCHED THEN " +
                    "UPDATE SET Target.uniqueName='" + platform.uniqueName + "', Target.latitude=" + platform.latitude + ", Target.longitude=" + tempLongitude + ", Target.createdAt=" + tempCreatedAt + ", Target.updatedAt=" + tempUpdatedAt + ' ' +
                    " WHEN NOT MATCHED THEN " +
                    "Insert (id, uniqueName, latitude, longitude, createdAt, updatedAt) Values(" + platform.id + ", '" + platform.uniqueName + "', " + platform.latitude + ", " + tempLongitude + ", " + tempCreatedAt + ", " + tempUpdatedAt + ");";
*/

                sql = "MERGE platform AS Target " +
                    "USING (SELECT " + platform.id + " AS id) as Source " +
                    "On Target.id = Source.id " +
                    "WHEN MATCHED THEN " +
                    "UPDATE SET Target.uniqueName='" + platform.uniqueName + "', Target.latitude='" + tempLatitude + "', Target.longitude=" + tempLongitude + ", Target.createdAt=" + tempCreatedAt + ", Target.updatedAt=" + tempUpdatedAt + ' ' +
                    " WHEN NOT MATCHED THEN " +
                    "Insert (id, uniqueName, latitude, longitude, createdAt, updatedAt) Values(" + platform.id + ", '" + platform.uniqueName + "', '" + tempLatitude + "', " + tempLongitude + ", " + tempCreatedAt + ", " + tempUpdatedAt + ");";

                Debug.WriteLine(sql);
                Debug.WriteLine("Test " + platforms[0].createdAt);
                command = new SqlCommand(sql, cnn);
                adapter.InsertCommand = new SqlCommand(sql, cnn);
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();
            }

            foreach (Platform platform in platforms)
            {
                foreach (Wells well in platform.well)
                {
                    //sql = "Insert into well (id, platformId, uniqueName, latitude, longitude, createdAt, updatedAt) Values(" + well.id + ", " + well.platformId + ", '" + well.uniqueName + "', " + well.latitude + ", " + well.longitude + ", '" + well.createdAt + "', '" + well.updatedAt + "')";
                    var tempUniqueName = string.IsNullOrEmpty(well.uniqueName) ? "null" : "'" + well.uniqueName + "'";
                    var tempCreatedAtWell = string.IsNullOrEmpty(well.createdAt) ? "null" : "'" + well.createdAt + "'";
                    var tempUpdatedAtWell = string.IsNullOrEmpty(well.updatedAt) ? "null" : "'" + well.updatedAt + "'";
                    
                    string tempplatformIdWell = null;
                    string tempLatitudeWell = null;
                    string tempLongitudeWell = null;

                    //checking for platformId null
                    if (well.platformId == null)
                    {
                        tempplatformIdWell = "null";
                    }
                    else
                    {
                        tempplatformIdWell = well.platformId.ToString();
                    }
                    //checking for latitude null
                    if (well.latitude == null)
                    {
                        tempLatitudeWell = "null";
                    }
                    else
                    {
                        tempLatitudeWell = well.latitude.ToString();
                    }
                    //checking for longitude null
                    if (well.longitude == null)
                    {
                        tempLongitudeWell = "null";
                    }
                    else
                    {
                        tempLongitudeWell = well.longitude.ToString();
                    }

                    sql = "MERGE Well AS Target " +
                    "USING (SELECT " + well.id + " AS id) as Source " +
                    "On Target.id = Source.id " +
                    "WHEN MATCHED THEN " +
                    "UPDATE SET Target.platformId=" + tempplatformIdWell + ", Target.uniqueName=" + tempUniqueName + ", Target.latitude='" + tempLatitudeWell + "', Target.longitude='" + tempLongitudeWell + "', Target.createdAt=" + tempCreatedAtWell + ", Target.updatedAt=" + tempUpdatedAtWell + ' ' +
                    "WHEN NOT MATCHED THEN " +
                    "Insert (id, platformId, uniqueName, latitude, longitude, createdAt, updatedAt) Values(" + well.id + ", " + tempplatformIdWell + ", " + tempUniqueName + ", '" + tempLatitudeWell + "', '" + tempLongitudeWell + "', " + tempCreatedAtWell + ", " + tempUpdatedAtWell + ");";

                    command = new SqlCommand(sql, cnn);
                    adapter.InsertCommand = new SqlCommand(sql, cnn);
                    adapter.InsertCommand.ExecuteNonQuery();
                    command.Dispose();
                }

            }

            MessageBox.Show("Data has been inserted into LocalDB.");
            cnn.Close();
        }
    }
}
