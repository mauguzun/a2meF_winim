using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;

namespace WiniumTest
{
    public partial class Form1 : Form
    {
        private string _addme = "AddmePoint";
        private string _host = "Winium.Desktop.Driver";

        private string _addmeFileName = "AddmePoint.exe";
        private string _hostFileName = @"driver\WinAppDriver.exe";

        private OpenAndReadAccount _db;
        private List<Account> _acc;
        private  WindowsDriver<WindowsElement> _driver;

        
        public Form1()
        {
            InitializeComponent();
            mynotifyicon.Click += Mynotifyicon_Click;
            _db = new OpenAndReadAccount();
        }

        private void Mynotifyicon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _SendToSave();
            if (_acc.Count == 0)
            {
                richTextBox1.Text = "ADD SOME ACCOUNT,TQ";
                return;
            }

            
            timer.Enabled = true;
            timer.Start();
            timer.Interval = 900000 * 4;


            _OpenServer();

            foreach(Account acc in this._acc)
            {
                _Login(acc.Name, acc.Passwrod);
            }
           
        }

        private void _SendToSave()
        {
            List<Account> acc = new List<Account>();
            foreach (string line in richTextBox1.Lines)
            {
                if (line.Contains(':'))
                {
                    string[] splitted = line.Split(':');
                    acc.Add(new Account() { Name = splitted[0], Passwrod = splitted[1] });

                }
            }
            if(acc.Count > 0)
            {
                this._acc = acc;
                _db.Save(this._acc);
            }
           
        }

        private void _Login(string login, string password)
        {

           
          
            var dc = new DesiredCapabilities();
            dc.SetCapability("app", this._addmeFileName);
            _driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), dc, new TimeSpan(0, 2, 0));
            _driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 3, 0);
           

            _FillData("TextBox1", login);
            if(password != "password")
            {
                _driver.FindElementByAccessibilityId("TextBox2").SendKeys(password);
            }
           
            while(_driver.FindElementByAccessibilityId("Button3").Enabled == false)
            {
                _driver.FindElementByAccessibilityId("Button1").Click();
            }
          
            
            for(int i = 0; i < 2;i++)
            {
                try
                {
                    System.Windows.Forms.SendKeys.Send("{ENTER}");

                    _driver.FindElementByAccessibilityId("Button3").Click();
                }

                catch { }
                
                _driver.FindElementByAccessibilityId("Button3").Click();
                Thread.Sleep(1000);
            }
           
           
        }

        private  bool  _FillData(string selector, string text)
        {
            while (_driver.FindElementByAccessibilityId(selector).Text != text)
            {
                _driver.FindElementByAccessibilityId(selector).Clear();
                _driver.FindElementByAccessibilityId(selector).SendKeys(text);

            }
            return true;
        }

        private void _OpenServer()
        
        {
            var pr = Process.GetProcessesByName(this._host);
            if (pr.Count() == 0)
            {
                Process.Start(this._hostFileName);
               
            }
               
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            
            _OpenServer();
            foreach (Process pr in Process.GetProcessesByName(this._addme))
                pr.Kill();

            foreach (Account acc in this._acc)
            {
                _Login(acc.Name, acc.Passwrod);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _acc = _db.Load();
            if (_acc == null )
                return;

            foreach (Account acc in _acc)
            {
                richTextBox1.Text += $"{acc.Name}:{acc.Passwrod}{Environment.NewLine}";
            }
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                mynotifyicon.Visible = true;
                mynotifyicon.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                mynotifyicon.Visible = false;
            }

        }

    }
}
