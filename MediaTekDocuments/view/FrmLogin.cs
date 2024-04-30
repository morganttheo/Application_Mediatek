using MediaTekDocuments.controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class FrmLogin : Form
    {
        private FrmLoginController controller;
        public FrmLogin()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            txbIdentifiant.Text = "";
            txbMdp.Text = "";
            controller = new FrmLoginController();
            this.AcceptButton = btnValid;
        }

        private void btnValid_Click(object sender, EventArgs e)
        {
            if (controller.GetLogin(txbIdentifiant.Text, txbMdp.Text))
                this.Visible = false;
            else
            {
                MessageBox.Show("Mauvais mot de passe ou login utilisateur");
                txbIdentifiant.Text = "";
                txbMdp.Text = "";
            }

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
