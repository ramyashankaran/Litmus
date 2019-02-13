using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LitmusApp
{
    public partial class frmAuth : Form
    {
        private bool authInProgress = false;

        #region Constructor

        public frmAuth()
        {
            InitializeComponent();
        }

        #endregion

        #region Auth0 

        private async void  StartAuthorizationAsync()
        {
 
            var client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "testytenant.auth0.com",
                ClientId = "MzBycPYSY5px0yM6MFDcLpX1uVxN0lzA"
            });

            var extraParameters = new Dictionary<string, string>();

            extraParameters.Add("connection", string.Empty);

            extraParameters.Add("audience", string.Empty);

            authInProgress = true;

            InitializeEmailWindow(await client.LoginAsync(extraParameters: extraParameters));
         
        }

        #endregion

        #region Private methods

        private void InitializeEmailWindow(LoginResult loginResult)
        {
            if (!loginResult.IsError)
            {
                frmMain emailForm = new frmMain(loginResult.User.Identity.Name);
                emailForm.ShowDialog();
            }

            authInProgress = false;
        }

        #endregion

        #region Event Handlers

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!authInProgress)
                StartAuthorizationAsync();
        }
        #endregion


    }
}
