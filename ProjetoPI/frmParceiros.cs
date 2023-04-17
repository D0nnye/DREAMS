using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoPI
{
    public partial class frmParceiros : Form
    {

        public frmParceiros()
        {
            InitializeComponent();
            desabilitaCampos();
            carregarCombBox();
        }
        public void carregarCombBox()
        {
            cbbEstado.Items.Add("");
            cbbEstado.Items.Add("SP");
            cbbEstado.Items.Add("RJ");
            cbbEstado.Items.Add("BH");
            cbbEstado.Items.Add("BA");
            cbbEstado.Items.Add("RN");
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal voltar = new frmMenuPrincipal();
            voltar.Show();
            this.Hide();
        }

        private void mskCEP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buscaCEP(mskCEP.Text);
                txtNum.Focus();
            }
        }
        public void buscaCEP(string numCEP)
        {
            WSCorreios.AtendeClienteClient ws = new WSCorreios.AtendeClienteClient();

            try
            {
                WSCorreios.enderecoERP end = ws.consultaCEP(numCEP);

                txtEndereco.Text = end.end;
                txtBairro.Text = end.bairro;
                txtCidade.Text = end.cidade;
                cbbEstado.Text = end.uf;
            }
            catch (Exception)
            {
                MessageBox.Show("Insira CEP válido!!!",
                    "Mensagem do Sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                mskCEP.Clear();
                mskCEP.Focus();

            }
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Tab))
            {
                bool valida = validaEmail(txtEmail.Text);

                if (valida == true)
                {
                    mskCEP.Focus();
                }
                else
                {
                    MessageBox.Show("Insira e-mail válido",
                    "Mensagem do Sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                    txtEmail.Clear();
                    txtEmail.Focus();
                }
            }
        }
        public static bool validaEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {

                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;

            }
            catch (ArgumentException)
            {
                return false;
            }
            try
            {   
                return Regex.IsMatch(email,
                  @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                  RegexOptions.IgnoreCase,
                  TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            btnNovo.Enabled = false;
            btnCadastrar.Enabled = true;
            btnVoltar.Enabled = true;
        }
        private void btnCadastrar_KeyDown(object sender, KeyEventArgs e)
        {
            verificarCampo();
            cadastrarParceiro();
        }
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            alterarParceiro(Convert.ToInt32(txtCodigo.Text));
        }
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            excluirParceiro(Convert.ToInt32(txtCodigo.Text));
        }
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            frmPesquisaFunc abrir = new frmPesquisaFunc();
            abrir.Show();
            this.Hide();
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();
        }
        private void btnVoltar_Click_1(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Hide();
        }

        public void excluirParceiro(int codPar)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "delete from tbParceiro where codPar = " + codPar + ";";
            comm.CommandType = CommandType.Text;
            comm.Connection = Conexao.obterConexao();
            comm.Parameters.Clear();
            comm.Parameters.Add("@codProd", MySqlDbType.Int32).Value = txtCodigo.Text;

            DialogResult vresp = MessageBox.Show("Deseja Realizar a Exclusão?", "Mensagem do Sistema", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (vresp == DialogResult.Yes)
            {
                int res = comm.ExecuteNonQuery();
                MessageBox.Show("Registro excluído com sucesso." + res);
            }
            else
            {
                MessageBox.Show("Não foi excluido.");
            }
            Conexao.fecharConexao();
        }

        public void alterarParceiro(int codFunc)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "update tbParceiro set cargo = @cargo , nome = @nome, email = @email, telefone = @telefone,endereco = @endereco, cnpj = @cnpj, siglaEst = @siglaEst, cep = @cep, cidade = @cidade, bairro = @bairro, numero = @numero, complemento = @complemento where codFunc = " + codFunc + ";";
            comm.CommandType = CommandType.Text;
            comm.Connection = Conexao.obterConexao();

            comm.Parameters.Clear();

            comm.Parameters.Add("@cargo", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = txtEmail.Text;
            comm.Parameters.Add("@endereco", MySqlDbType.VarChar, 100).Value = txtEndereco.Text;
            comm.Parameters.Add("@telefone", MySqlDbType.VarChar, 20).Value = mskTelefone.Text;
            comm.Parameters.Add("@cnpj", MySqlDbType.VarChar, 14).Value = mskCNPJ.Text;
            comm.Parameters.Add("@siglaEst", MySqlDbType.VarChar, 2).Value = cbbEstado.Text;
            comm.Parameters.Add("@cep", MySqlDbType.VarChar, 10).Value = mskCEP.Text;
            comm.Parameters.Add("@cidade", MySqlDbType.VarChar, 50).Value = txtCidade.Text;
            comm.Parameters.Add("@bairro", MySqlDbType.VarChar, 50).Value = txtBairro.Text;
            comm.Parameters.Add("@numero", MySqlDbType.VarChar, 14).Value = txtNum.Text;
            comm.Parameters.Add("@complemento", MySqlDbType.VarChar, 50).Value = txtComplemento.Text;


            int res = comm.ExecuteNonQuery();
            MessageBox.Show("Registro alterado com sucesso." + res);
            Conexao.fecharConexao();
        }

        public void verificarCampo()
        {
            if (txtNome.Text == "")
            {
                MessageBox.Show("Favor inserir valores");
            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Favor inserir valores");
            }

            if (txtNome.Text.Equals("") || txtEmail.Text.Equals("")
              || txtEndereco.Text.Equals("") || mskTelefone.Text.Equals("(  )      -")
                || mskCNPJ.Text.Equals("  .   .   /    -  ") || mskCEP.Text.Equals("     -")
                || txtCidade.Text.Equals("") || txtBairro.Text.Equals("") ||
                txtNum.Text.Equals("") || cbbEstado.Text.Equals(""))
            {
                MessageBox.Show("Favor inserir valores!!!",
                    "Mensagem do Sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtNome.Focus();
            }

        }
        public void cadastrarParceiro()
        {
            MySqlCommand comm = new MySqlCommand();

            comm.CommandText = "insert into tbParceiro(cargo, nome, email, endereco, telefone, cnpj, cep, siglaEst, cidade, bairro, numero, complemento)" +
    "values(@cargo, @nome, @email, @endereco, @telefone, @cnpj, @cep, @siglaEst, @cidade, @bairro, @numero, @complemento); ";

            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();

            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = txtEmail.Text;
            comm.Parameters.Add("@endereco", MySqlDbType.VarChar, 100).Value = txtEndereco.Text;
            comm.Parameters.Add("@telefone", MySqlDbType.VarChar, 20).Value = mskTelefone.Text;
            comm.Parameters.Add("@cnpj", MySqlDbType.VarChar, 14).Value = mskCNPJ.Text;
            comm.Parameters.Add("@siglaEst", MySqlDbType.VarChar, 2).Value = cbbEstado.Text;
            comm.Parameters.Add("@cep", MySqlDbType.VarChar, 10).Value = mskCEP.Text;
            comm.Parameters.Add("@cidade", MySqlDbType.VarChar, 50).Value = txtCidade.Text;
            comm.Parameters.Add("@bairro", MySqlDbType.VarChar, 50).Value = txtBairro.Text;
            comm.Parameters.Add("@numero", MySqlDbType.VarChar, 14).Value = txtNum.Text;
            comm.Parameters.Add("@complemento", MySqlDbType.VarChar, 50).Value = txtComplemento.Text;

            comm.CommandType = CommandType.Text;

            comm.Connection = Conexao.obterConexao();

            int i = comm.ExecuteNonQuery();

            MessageBox.Show("Parceiro cadastrado com sucesso!!!" + i);
            limparCampos();
            desabilitaCampos();

            Conexao.fecharConexao();
        }

        public void desabilitaCampos()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = false;
            txtEmail.Enabled = false;
            txtEndereco.Enabled = false;
            mskTelefone.Enabled = false;
            mskCNPJ.Enabled = false;
            mskCEP.Enabled = false;
            cbbEstado.Enabled = false;
            txtCidade.Enabled = false;
            txtBairro.Enabled = false;
            txtNum.Enabled = false;
            txtComplemento.Enabled = false;
            btnNovo.Enabled = true;
            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnPesquisar.Enabled = true;
            btnLimpar.Enabled = false;
            btnVoltar.Enabled = true;
        }
        public void habilitarCampos()
        {
            txtNome.Enabled = true;
            txtEmail.Enabled = true;
            txtEndereco.Enabled = true;
            mskTelefone.Enabled = true;
            mskCNPJ.Enabled = true;
            mskCEP.Enabled = true;
            cbbEstado.Enabled = true;
            txtCidade.Enabled = true;
            txtBairro.Enabled = true;
            txtNum.Enabled = true;
            txtComplemento.Enabled = true;
            btnNovo.Enabled = true;
            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnPesquisar.Enabled = true;
            btnLimpar.Enabled = true;
            btnVoltar.Enabled = true;
            txtNome.Focus();
        }
        public void limparCampos()
        {
            txtNome.Text = "";
            txtEmail.Text = "";
            txtEndereco.Text = "";
            mskTelefone.Text = "";
            mskCNPJ.Text = "";
            mskCEP.Text = "";
            cbbEstado.Text = "";
            txtCidade.Text = "";
            txtBairro.Text = "";
            txtNum.Clear();
            txtComplemento.Clear();
        }

    }
}
