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
    public partial class frmLocalizacao : Form
    {
        public frmLocalizacao()
        {
            InitializeComponent();
        }


        private void btnVoltar_Click_1(object sender, EventArgs e)
        {
            frmMenuPrincipal voltar = new frmMenuPrincipal();
            voltar.Show();
            this.Hide();
        }

        private void frmLocalizacao_Load(object sender, EventArgs e)
        {

        }
    }
}
