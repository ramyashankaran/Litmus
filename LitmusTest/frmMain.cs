using Auth0.OidcClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LitmusApp
{
    public partial class frmMain : Form
    {
        #region Properties

        private Timer emailTimer = null;
        private string folderPath = @"C:\GmailImages";
        string userName = string.Empty;
      
   
        #endregion

        #region Constructor

        public frmMain(string userName)
        {
           
            InitializeComponent();

            folderPath = Path.Combine(folderPath,userName);
        }

        #endregion        

        #region Initialization

        private void InitializeControls()
        {
            wbMail.Navigate(@"https://mail.google.com");
        }

        public void InitializeTimer()
        {
            emailTimer = new Timer();
            emailTimer.Tick += EmailTimer_Tick;
            emailTimer.Interval += 30000;
            emailTimer.Start();
        }

        private void EmailTimer_Tick(object sender, EventArgs e)
        {
            if (wbMail.ReadyState == WebBrowserReadyState.Complete && !wbMail.IsBusy)
            {
                emailTimer.Stop();
                emailTimer = null;
            }
        }

        #endregion

        #region EventHandlers

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitializeControls();
        }
       
        private void WbMail_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
            if (wbMail.ReadyState == WebBrowserReadyState.Complete && !wbMail.IsBusy && e.Url.Host == "accounts.google.com")
             {
    
                PrintScreen();

                wbMail.DocumentCompleted -= WbMail_DocumentCompleted;
            }

            
            
        }


        private void pbPrint_Click(object sender, EventArgs e)
        {
            PrintScreen();
        }


        private void pbPrint_MouseEnter(object sender, EventArgs e)
        {
            splitContainer1.Panel2.BackColor = Color.Red;
        }


        private void pbPrint_MouseLeave(object sender, EventArgs e)
        {
            splitContainer1.Panel2.BackColor = Color.LightGray;
        }

        private void frmMain_FormClosedAsync(object sender, FormClosedEventArgs e)
        {
            wbMail.Navigate("https://google.com/accounts/logout");           
        }

        #endregion

        #region Private Methods

        private void PrintScreen()
        {
            string filePath = string.Empty;

            using (Bitmap bmp = new Bitmap(wbMail.Width, wbMail.Height))
            {
                Image img = wbMail.DrawToImage();

                filePath = GetFilePath();

                img.Save(filePath);
            }
 
            if(File.Exists(filePath))
            {
                string msg = "An image of your inbox has been saved at - " + folderPath;

                MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetFilePath()
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + DateTime.Now.ToString("HH-mm-ss") + ".png";

            string filePath = Path.Combine(folderPath, fileName);

            return filePath;
        }

        #endregion
    }
}
