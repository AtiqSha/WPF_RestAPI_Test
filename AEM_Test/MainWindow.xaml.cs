using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx;
using Nito.AsyncEx.Synchronous;

namespace AEM_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loginAccount();
        }

        private void loginAccount()
        {
            var result = AsyncContext.Run(PostGetAsync);
            MainMenu mainMenu = new MainMenu(result);
            Close();
            mainMenu.Show();

            //Debug.WriteLine(result);
        }

        public async Task<string> PostGetAsync()
        {
            var uri = new Uri("http://test-demo.aem-enersol.com/api/Account/Login");

            using (var client = new HttpClient())
            {
               // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", yourTokenString);

                var dict = new Dictionary<string, string>();
                dict.Add("username", username_txt.Text);
                dict.Add("password", password_txt.Password);

                var output = Newtonsoft.Json.JsonConvert.SerializeObject(dict);


                var content = new StringContent(output.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);

                Debug.WriteLine(response.StatusCode.ToString());

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return "Error posting KeyValue";
                }

                string responseString = response.Content.ReadAsStringAsync().Result;

                return responseString;
                

            }
        }

    }
}
