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
    public partial class frmCarregaDataGridDB : Form
    {
        public frmCarregaDataGridDB()
        {
            InitializeComponent();
        }

        private void btnCarregaDados_Click(object sender, EventArgs e)
        {
            dgvFuncionarios.DataSource = null;

            MySqlDataAdapter da = new MySqlDataAdapter("select codfunc as 'Código', cargo as 'Cargo', nome as 'Nome', email as 'E-mail', endereco as 'Endereço', telefone as 'Telefone', cpf as 'CPF', cep as 'CEP', siglaEst as 'Estado', cidade as 'Cidade', bairro as 'Bairro', numero as 'Numero', complemento as 'Complemento' from tbFuncionarios;", Conexao.obterConexao());

            DataTable dt = new DataTable();

            da.Fill(dt);

            dgvFuncionarios.DataSource = dt;

            Conexao.fecharConexao();
        }

        private void frmCarregaDataGridDB_Load(object sender, EventArgs e)
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
