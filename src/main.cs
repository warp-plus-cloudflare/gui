using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace warp_plus_cloudflare___by_aliilapro__.FRM
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }
        private List<string> ProxyList = new List<string>();
        private static Random rnd = new Random();
        private bool _start;
        private int _error;
        private int _test;
        private int _send;
        private string GenerateUniqCode(int len)
        {
            string _allstring = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string _re = "";
            while (_re.Length < len)
            {
                _re += _allstring[rnd.Next(0, _allstring.Length - 1)].ToString();
            }
            return _re;
        }

        private string Generateint(int len)
        {
            string _allstring = "0123456789";
            string _re = "";
            while (_re.Length < len)
            {
                _re += _allstring[rnd.Next(0, _allstring.Length - 1)].ToString();
            }
            return _re;
        }
        private void ch_hideid_CheckedChanged(object sender, EventArgs e)
        {
            bool @true = this.ch_hideid.Checked;
            if (@true)
            {
                txtid.PasswordChar = '*';
            }
            else
            {
                txtid.PasswordChar = '\0';
            }
        }

        private void ch_proxy_CheckedChanged(object sender, EventArgs e)
        {
            this.ProxyList.Clear();
            this.lblproxy.Text = "0";
            bool @true = this.ch_proxy.Checked;
            if (@true)
            {
                btnloadproxy.Enabled = true;
                btngetproxy.Enabled = false;
                btngetproxy2.Enabled = false;
            }
            else
            {
                btnloadproxy.Enabled = false;
                btngetproxy.Enabled = true;
                btngetproxy2.Enabled = true;
            }
        }

        private void btnloadproxy_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text File (*.txt)|*.txt|All File (*.*)|*.*";
            openFileDialog.Title = "Open Proxylist ( ONLY HTTP-S )";
            this.ProxyList.Clear();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.ProxyList.AddRange(File.ReadAllLines(openFileDialog.FileName));
                this.lblproxy.Text = this.ProxyList.Count.ToString();
            }
        }

        private void btngetproxy_Click(object sender, EventArgs e)
        {
            this.ProxyList.Clear();
            this.lblproxy.Text = "0";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/ALIILAPRO/Proxy/main/http.txt")).GetResponse();
                string end = new StreamReader(response.GetResponseStream()).ReadToEnd();
                MatchCollection matchCollections = new Regex("[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}:[0-9]{1,5}").Matches(end);
                try
                {
                    try
                    {
                        foreach (object obj in matchCollections)
                        {
                            Match objectValue = (Match)RuntimeHelpers.GetObjectValue(obj);
                            this.ProxyList.Add(objectValue.Value);
                        }
                    }
                    finally
                    {
                    }
                }
                finally
                {
                }
                int List = this.ProxyList.Count;
                this.lblproxy.Text = string.Concat(new string[]
                {
                    List.ToString()
                });
                List = this.ProxyList.Count;
                this.lblproxy.Text = List.ToString();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnstart_Click(object sender, EventArgs e)
        {
            if (this.ProxyList.Count != 0)
            {
                this._start = true;
                int num = Convert.ToInt32(this.speed.Text);
                ThreadPool.SetMinThreads(num, num);
                ThreadPool.SetMaxThreads(num, num);
                try
                {
                    List<string>.Enumerator enumerator = this.ProxyList.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        string current = enumerator.Current;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.mtd), current);
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                }
            }
            else
            {
                MessageBox.Show("First Load Proxylist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void mtd(object Proxiess)
        {
            bool start = this._start;

            if (start)
            {
                try
                {
                    string host = Proxiess.ToString().Split(new char[]
                              {
                        ':'
                              })[0];
                    string value = Proxiess.ToString().Split(new char[]
                    {
                        ':'
                    })[1];

                    WebProxy proxy = new WebProxy(host, Convert.ToInt32(value));
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.cloudflareclient.com/v0a"+Generateint(3)+"/reg");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                    httpWebRequest.ContentType = "application/json; charset=UTF-8";
                    httpWebRequest.Host = "api.cloudflareclient.com";
                    httpWebRequest.KeepAlive = true;
                    httpWebRequest.UserAgent = "okhttp/3.12.1";
                    httpWebRequest.Proxy = proxy;
                    string install_id = this.GenerateUniqCode(22);
                    string key        = this.GenerateUniqCode(43) + "=";
                    string tos        = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fff") + "+07:00";
                    string fcm_token  = install_id + ":APA91b" + this.GenerateUniqCode(134);
                    string referer    = this.txtid.Text;
                    string type       = "Android";
                    string locale     = "en-GB";
                    var body = new
                    {
                        install_id = install_id,
                        key = key,
                        tos = tos,
                        fcm_token = fcm_token,
                        referrer = referer,
                        warp_enabled = false,
                        type = type,
                        locale = locale
                    };
                    string jsonBody = JsonConvert.SerializeObject(body);
                    using (StreamWriter sw = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        sw.Write(jsonBody);
                    }
                    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader sw = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = sw.ReadToEnd();
                    }
                    httpResponse = null;
                    this._test++;
                    this._send++;
                    this.lblgood.Text = this._send.ToString();
                    this.lbltest.Text = this._test.ToString();
                    this.lblgb.Text = lblgood.Text;
                }
                catch
                {
                    this._test++;
                    this._error++;
                    this.lblbad.Text = this._error.ToString();
                    this.lbltest.Text = this._test.ToString();
                }

            }
        }

        private void btnstop_Click(object sender, EventArgs e)
        {
            this._start = false;
        }

        private void telegramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://t.me/aliilapro");
        }

        private void darkmod_CheckedChanged(object sender, EventArgs e)
        {
            bool @true = this.darkmod.Checked;
            if (@true)
            {
                label1.ForeColor = Color.White;
                groupBox1.ForeColor = Color.White;
                groupBox2.ForeColor = Color.White;
                groupBox3.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                lblproxy.ForeColor = Color.White;
                lblgb.ForeColor = Color.White;
                lbltest.ForeColor = Color.White;
                ch_hideid.ForeColor = Color.White;
                ch_proxy.ForeColor = Color.White;
                txtid.ForeColor = Color.White;
                txtid.BackColor = Color.Black;
                speed.ForeColor = Color.White;
                speed.BackColor = Color.Black;
                statusStrip1.BackColor = Color.Black;
                statusStrip1.ForeColor = Color.White;
                darkmod.BackColor = Color.Black;
                darkmod.ForeColor = Color.White;
                toolStripDropDownButton1.ForeColor = Color.White;
                BackColor = Color.Black;

            }
            else
            {
                label1.ForeColor = Color.Black;
                groupBox1.ForeColor = Color.Black;
                groupBox2.ForeColor = Color.Black;
                groupBox3.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                lblproxy.ForeColor = Color.Black;
                lblgb.ForeColor = Color.Black;
                lbltest.ForeColor = Color.Black;
                ch_hideid.ForeColor = Color.Black;
                ch_proxy.ForeColor = Color.Black;
                txtid.ForeColor = Color.Black;
                txtid.BackColor = Color.White;
                speed.ForeColor = Color.Black;
                speed.BackColor = Color.White;
                statusStrip1.BackColor = Color.White;
                statusStrip1.ForeColor = Color.Black;
                darkmod.BackColor = Color.White;
                darkmod.ForeColor = Color.Black;
                toolStripDropDownButton1.ForeColor = Color.Black;
                BackColor = Color.White;
            }
        }

        private void btngetproxy2_Click(object sender, EventArgs e)
        {
            this.ProxyList.Clear();
            this.lblproxy.Text = "0";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)WebRequest.Create("https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all")).GetResponse();
                string end = new StreamReader(response.GetResponseStream()).ReadToEnd();
                MatchCollection matchCollections = new Regex("[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}:[0-9]{1,5}").Matches(end);
                try
                {
                    try
                    {
                        foreach (object obj in matchCollections)
                        {
                            Match objectValue = (Match)RuntimeHelpers.GetObjectValue(obj);
                            this.ProxyList.Add(objectValue.Value);
                        }
                    }
                    finally
                    {
                    }
                }
                finally
                {
                }
                int List = this.ProxyList.Count;
                this.lblproxy.Text = string.Concat(new string[]
                {
                    List.ToString()
                });
                List = this.ProxyList.Count;
                this.lblproxy.Text = List.ToString();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void githubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/warp-plus-cloudflare");
        }
    }
}
