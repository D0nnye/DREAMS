using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace ProjetoPI
{
    public partial class frmCarregaDataGridDBUsu : Form
    {
        public frmCarregaDataGridDBUsu()
        {
            InitializeComponent();
        } 

        
    private void btnCarregaDados_Click_1(object sender, EventArgs e)
        {

            dgvFuncionarios.DataSource = null;

            MySqlDataAdapter da = new MySqlDataAdapter("select codUsu as 'Código', nomeUsu as 'Nome', emailUsu as 'E-mail', senhaUsu as 'Senha', telefoneUsu as 'Telefone' from tbUsuario;", Conexao.obterConexao());
            
            DataTable dt = new DataTable();

            da.Fill(dt);

            dgvFuncionarios.DataSource = dt;

            Conexao.fecharConexao();
           
        }
        private void dgvFuncionarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmPesquisaFunc abrir = new frmPesquisaFunc();
            abrir.Show();
            this.Hide();
        }
    }
}
