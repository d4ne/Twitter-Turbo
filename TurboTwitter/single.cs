using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TurboTwitter
{
    public partial class single : UserControl
    {
        CookieContainer cookie;

        public single()
        {
            InitializeComponent();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if ((bunifuMetroTextbox1.Text == string.Empty) || (bunifuMetroTextbox2.Text == string.Empty))
            {
                MessageBox.Show("Please enter your username and password!", "Twitter Turbo Info", MessageBoxButtons.OK);
            }
            else
            {
                new Thread(new ThreadStart(doLogin)) { IsBackground = true }.Start();
            }
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            if ((bunifuMetroTextbox3.Text == string.Empty))
            {
                MessageBox.Show("Please enter a handle!", "Twitter Turbo Info", MessageBoxButtons.OK);
            }
            else
            {
                new Thread(new ThreadStart(doClaim)) { IsBackground = true }.Start();
            }
        }

        // Thread functions start

        private void doClaim()
        {
            string username = bunifuMetroTextbox1.Text;
            string password = bunifuMetroTextbox2.Text;
            string handle = bunifuMetroTextbox3.Text;

            if (handle.Length <= 3)
            {
                MessageBox.Show("Handle is less than 4 letters! Please enter another handle.", "Twitter Turbo Info", MessageBoxButtons.OK);
            }
            else if (username == handle)
            {
                MessageBox.Show("Your handle is already: " + handle + "! Please enter another handle.", "Twitter Turbo Info", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        UpdateTurboStatus(true, "username");
                    });

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://instagram.com/");
                    request.CookieContainer = cookie;
                    request.AllowAutoRedirect = true;

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    string str = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();

                    string str2 = this.getToken(str, "_token\" value=\"", "\">", 0);
                    string str3 = this.getToken(str, "orig_uname\" value=\"", "\">", 0);
                    string str4 = this.getToken(str, "orig_email\" value=\"", "\">", 0);
                    string[] textArray1 = new string[] { "_method=PUT&authenticity_token=", str2, "&orig_uname=", handle, "&orig_email=", str4, "&user%5Bscreen_name%5D=", handle, "&user%5Bemail%5D=", str4, "&user%5Blang%5D=en&user%5Btime_zone%5D=Pacific+Time+%28US+%26+Canada%29&user%5Bcountry%5D=us&user%5Bnsfw_view%5D=0&user%5Bnsfw_user%5D=0&user%5Bautoplay_disabled%5D=0&user%5Bautoplay_disabled%5D=1&auth_password=", password };

                    HttpWebRequest httpWebRequest_0 = (HttpWebRequest)WebRequest.Create("https://twitter.com/settings/accounts/update");
                    httpWebRequest_0.CookieContainer = cookie;
                    httpWebRequest_0.Method = "POST";
                    httpWebRequest_0.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    httpWebRequest_0.Referer = "https://twitter.com/settings/account";
                    httpWebRequest_0.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                    httpWebRequest_0.AllowAutoRedirect = true;
                    httpWebRequest_0.ContentType = "application/x-www-form-urlencoded";

                    bool userChanged = false;

                    while (!userChanged)
                    {
                        try
                        {
                            WebClient client1 = new WebClient
                            {
                                Proxy = null
                            };

                            client1.Headers["Accept-Language"] = "en-us";

                            if (client1.DownloadString("https://twitter.com/" + handle).Contains("Sorry, that page doesn’t exist!"))
                            {
                                if (!userChanged)
                                {
                                    byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(textArray1));
                                    httpWebRequest_0.ContentLength = bytes.Length;
                                    Stream requestStream = httpWebRequest_0.GetRequestStream();
                                    requestStream.Write(bytes, 0, bytes.Length);

                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        UpdateTurboStatus(false, handle);
                                    });

                                    MessageBox.Show("Username successfully turboed!", "Info", MessageBoxButtons.OK);
                                    userChanged = true;
                                }
                            }
                            else
                            {
                                base.Invoke(new Action(method_5));
                            }

                            continue;
                        }
                        catch (WebException ex)
                        {
                            HttpWebResponse errorResponse = ex.Response as HttpWebResponse;

                            if (errorResponse.StatusCode == HttpStatusCode.NotFound && !userChanged)
                            {
                                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(textArray1));
                                httpWebRequest_0.ContentLength = bytes.Length;
                                Stream requestStream = httpWebRequest_0.GetRequestStream();
                                requestStream.Write(bytes, 0, bytes.Length);

                                this.Invoke((MethodInvoker)delegate
                                {
                                    UpdateTurboStatus(false, handle);
                                });

                                MessageBox.Show("Username successfully turboed!", "Info", MessageBoxButtons.OK);
                                userChanged = true;
                            }
                            else
                            {
                                MessageBox.Show(ex.Message);
                            }

                            continue;
                        }
                    }

                }
                catch
                {
                    Console.WriteLine("Error thrown");
                }
            }
        }

        private void method_5()
        {
            int num = Convert.ToInt32(bunifuCustomLabel10.Text);
            if (num > 0x77359400)
            {
                num = 0;
            }

            num++;
            bunifuCustomLabel10.Text = num.ToString();
        }

        bool UpdateTurboStatus(bool status, string handle)
        {
            if (status == true)
            {
                bunifuCustomLabel8.Text = "Turboing twitter handles.";
                return true;
            }
            else
            {
                bunifuCustomLabel8.Text = "Turbo finished. Username: " + handle + " was claimed.";
                return true;
            }
        }

        private void doLogin()
        {
            string username = bunifuMetroTextbox1.Text;
            string password = bunifuMetroTextbox2.Text;

            try
            {
                string str = "";
                cookie = new CookieContainer();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://twitter.com/login");
                request.CookieContainer = new CookieContainer();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                cookie.Add(response.Cookies);

                string token = getToken(new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd(), "_token\" value=\"", "\">", 0);

                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create("https://twitter.com/sessions");
                request2.CookieContainer = cookie;
                request2.Method = "POST";
                request2.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                request2.AllowAutoRedirect = true;
                request2.ContentType = "application/x-www-form-urlencoded";
                string[] textArray1 = new string[] { "username=" + username + "&password=" + password };
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(textArray1));
                request2.ContentLength = bytes.Length;

                Stream requestStream = request2.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                using (StreamReader reader = new StreamReader(response2.GetResponseStream()))
                {
                    str = reader.ReadToEnd();
                }

                if (str.Contains("forgot"))
                {
                    bool status = false;

                    bool cancel = (bool)this.Invoke((Func<bool, bool>)DoCheapGuiAccess, status);
                }
                else
                {
                    bool status = true;
                    bool cancel = (bool)this.Invoke((Func<bool, bool>)DoCheapGuiAccess, status);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unknown error has occurred. Please screenshot and report this.", "Twitter Turbo Error");
                Console.WriteLine(ex.Message);
            }
        }

        // Thread functions end

        bool DoCheapGuiAccess(bool status)
        {
            if (status == false)
            {
                bunifuCustomLabel6.Text = "Could not authenticate as: " + bunifuMetroTextbox1.Text;
                bunifuFlatButton2.Enabled = false;
                bunifuMetroTextbox3.Enabled = false;
                return true;
            }
            else
            {
                bunifuCustomLabel6.Text = "Authenticated as: " + bunifuMetroTextbox1.Text;
                bunifuFlatButton2.Enabled = true;
                bunifuMetroTextbox3.Enabled = true;
                return true;
            }
        }

        private string getToken(string string_0, string string_1, string string_2, int int_0)
        {
            string input = Regex.Split(string_0, string_1)[int_0 + 1];
            return Regex.Split(input, string_2)[0];
        }

    }
}
