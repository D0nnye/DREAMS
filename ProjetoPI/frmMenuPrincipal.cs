using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoPI
{
    public partial class frmMenuPrincipal : Form
    {
        public frmMenuPrincipal()
        {
            InitializeComponent();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmLogin voltar = new frmLogin();
            voltar.Show();
            this.Hide();
        }

        private void btnFuncionarios_Click(object sender, EventArgs e)
        {
            frmfuncionarios abrir = new frmfuncionarios ();
            abrir.Show();
            this.Hide();
        }

        private void btnParceiros_Click(object sender, EventArgs e)
        {
            frmParceiros abrir = new frmParceiros();
            abrir.Show();
            this.Hide();
        }

       

        private void btnLocalizacao_Click(object sender, EventArgs e)
        {
            frmLocalizacao abrir = new frmLocalizacao();
            abrir.Show();
            this.Hide();
        }
    }
}
