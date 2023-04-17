using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace ProjetoPI
{
    public partial class frmfuncionarios : Form
    {
        //Criando variáveis para controle do menu
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public void desabilitaCampos()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = false;
            txtCargo.Enabled = false;
            txtEmail.Enabled = false;
            txtEndereco.Enabled = false;
            mskTelefone.Enabled = false;
            mskCPF.Enabled = false;
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
            txtCargo.Enabled = true;
            txtEmail.Enabled = true;
            txtEndereco.Enabled = true;
            mskTelefone.Enabled = true;
            mskCPF.Enabled = true;
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
            txtCargo.Text = "";
            txtEmail.Text = "";
            txtEndereco.Text = "";
            mskTelefone.Text = "";
            mskCPF.Text = "";
            mskCEP.Text = "";
            cbbEstado.Text = "";
            txtCidade.Text = "";
            txtBairro.Text = "";
            txtNum.Clear();
            txtComplemento.Clear();
        }
        
        public void excluirdoBanco()
        {
            
            txtCodigo.Enabled = false;
            txtNome.Enabled = false;
            txtCargo.Enabled = false;
            txtEmail.Enabled = false;
            txtEndereco.Enabled = false;
            mskTelefone.Enabled = false;
            mskCPF.Enabled = false;
            mskCEP.Enabled = false;
            cbbEstado.Enabled = false;
            txtCidade.Enabled = false;
            txtBairro.Enabled = false;
            txtNum.Enabled = false;
            txtComplemento.Enabled = false;
        }

        //carregar a combobox
        public void carregarCombBox()
        {
            cbbEstado.Items.Add("");
            cbbEstado.Items.Add("SP");
            cbbEstado.Items.Add("RJ");
            cbbEstado.Items.Add("BH");
            cbbEstado.Items.Add("BA");
            cbbEstado.Items.Add("RN");
        }

        //contrutor da classe
        public frmfuncionarios()
        {
            InitializeComponent();
            desabilitaCampos();
            carregarCombBox();
        }
       string cargo = "";

        bool flag = false;

        //contrutor com parametros
        public frmfuncionarios(string cargo)
        {
            InitializeComponent();
            desabilitaCampos();
            carregarCombBox();
            txtCargo.Text = cargo;
            habilitarCampos();
            pesquisarCampo(cargo);
            ativarUpdate();
        }

        public void ativarUpdate()
        {
            btnAlterar.Enabled = true;
            btnExcluir.Enabled = true;
            btnCadastrar.Enabled = false;
            btnNovo.Enabled = false;
            btnLimpar.Enabled = false;
        }

        public void pesquisarCampo(string cargo)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "select * from tbFuncionarios where nome = '" + cargo + "';";
            comm.CommandType = CommandType.Text;

            comm.Connection = Conexao.obterConexao();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();
            DR.Read();

            txtCodigo.Text = Convert.ToString(DR.GetInt32(0));
            txtCargo.Text = DR.GetString(1);
            txtNome.Text = DR.GetString(2);
            txtEmail.Text = DR.GetString(3);
            txtEndereco.Text = DR.GetString(4);
            mskTelefone.Text = DR.GetString(5);
            mskCPF.Text = DR.GetString(6);
            mskCEP.Text = DR.GetString(7);
            cbbEstado.Text = DR.GetString(8);
            txtCidade.Text = DR.GetString(9);
            txtBairro.Text = DR.GetString(10);
            txtNum.Text = DR.GetString(11);
            txtComplemento.Text = DR.GetString(12);

            Conexao.fecharConexao();
        }


        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Hide();
        }

        private void frmfuncionarios_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            btnNovo.Enabled = false;
            btnCadastrar.Enabled = true;
            btnVoltar.Enabled = true;
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            frmPesquisaFunc abrir = new frmPesquisaFunc ();
            abrir.Show();
            this.Hide();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            //Executando o método verificarCampo
            verificarCampo();
            //Executar o método de cadastrar paciente
            cadastrarFuncionario();
        }

        public void cadastrarFuncionario()
        {
            MySqlCommand comm = new MySqlCommand();

            comm.CommandText = "insert into tbFuncionarios(cargo, nome, email, endereco, telefone, cpf, cep, siglaEst, cidade, bairro, numero, complemento)" +
    "values(@cargo, @nome, @email, @endereco, @telefone, @cpf, @cep, @siglaEst, @cidade, @bairro, @numero, @complemento); ";

            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();

            comm.Parameters.Add("@cargo", MySqlDbType.VarChar, 100).Value = txtCargo.Text;
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = txtEmail.Text;
            comm.Parameters.Add("@endereco", MySqlDbType.VarChar, 100).Value = txtEndereco.Text;
            comm.Parameters.Add("@telefone", MySqlDbType.VarChar, 20).Value = mskTelefone.Text;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar, 14).Value = mskCPF.Text;
            comm.Parameters.Add("@siglaEst", MySqlDbType.VarChar, 2).Value = cbbEstado.Text;
            comm.Parameters.Add("@cep", MySqlDbType.VarChar, 10).Value = mskCEP.Text;   
            comm.Parameters.Add("@cidade", MySqlDbType.VarChar, 50).Value = txtCidade.Text;
            comm.Parameters.Add("@bairro", MySqlDbType.VarChar, 50).Value = txtBairro.Text;
            comm.Parameters.Add("@numero", MySqlDbType.VarChar, 14).Value = txtNum.Text;
            comm.Parameters.Add("@complemento", MySqlDbType.VarChar, 50).Value = txtComplemento.Text;

            comm.CommandType = CommandType.Text;

            comm.Connection = Conexao.obterConexao();

            int i = comm.ExecuteNonQuery();

            MessageBox.Show("Funcionario cadastrado com sucesso!!!" + i);
            limparCampos();
            desabilitaCampos();

            Conexao.fecharConexao();
        }



        //metodo para verificar campo vazio 
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

            if ( txtCargo.Text.Equals("") || txtNome.Text.Equals("") || txtEmail.Text.Equals("")
              || txtEndereco.Text.Equals("") || mskTelefone.Text.Equals("(  )      -")
                || mskCPF.Text.Equals("   .   .   -") || mskCEP.Text.Equals("     -")
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

        public void alterarFuncionario(int codFunc)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "update tbFuncionarios set cargo = @cargo , nome = @nome, email = @email, telefone = @telefone,endereco = @endereco, cpf = @cpf, siglaEst = @siglaEst, cep = @cep, cidade = @cidade, bairro = @bairro, numero = @numero, complemento = @complemento where codFunc = " + codFunc + ";";
            comm.CommandType = CommandType.Text;
            comm.Connection = Conexao.obterConexao();

            comm.Parameters.Clear();

            comm.Parameters.Add("@cargo", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = txtEmail.Text;
            comm.Parameters.Add("@endereco", MySqlDbType.VarChar, 100).Value = txtEndereco.Text;
            comm.Parameters.Add("@telefone", MySqlDbType.VarChar, 20).Value = mskTelefone.Text;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar, 14).Value = mskCPF.Text;
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
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();
        }

        public void excluirUsuario(int codUsu)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "delete from tbUsuario where codUsu = " + codUsu + ";";
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

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            excluirUsuario(Convert.ToInt32(txtCodigo.Text));
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

        private void mskCPF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool valida = validaCPF(mskCPF.Text);

                if (valida == true)
                {
                    mskCEP.Focus();
                }
                else
                {
                    MessageBox.Show("Insira CPF válido",
                   "Mensagem do Sistema",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error,
                   MessageBoxDefaultButton.Button1);
                    mskCPF.Clear();
                    mskCPF.Focus();
                }

            }
        }
        public static bool validaCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");

            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                  valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;
            return true;
        }

        private void txtEndereco_KeyDown(object sender, KeyEventArgs e)
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
            {   //Expressão regular

                /*
                 * O que esperava de cada trecho:
                 * [a-z0-9.]+ - parte antes do @ do e-mail, nome do usuário;
                 * @ - caractere de arroba obrigatório;
                 * [a-z0-9]+ - parte depois do @ do e-mail, nome do provedor;
                 * \. - caractere de ponto depois do nome do provedor;
                 * [a-z]+ - geralmente onde é colocado o .com;
                 * \. - caractere de ponto depois do .com, só deveria ser obrigatório caso haja por exemplo um .br ou a abreviação de qualquer outro país no final do e-mail;
                 * ([a-z]+)? - geralmente onde é colocado a abreviação do país.
                 */

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

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            alterarFuncionario(Convert.ToInt32(txtCodigo.Text));
        }
    }
}
