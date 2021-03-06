using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projeto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting;

namespace Projeto.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;
        public static string Email_Pessoa_Logada;
        public static string Email_Pessoa_DonaChamado;
        public static int id_pessoa_logada;
        public static int nivel_acesso;
        public static int IDORC;
        public static bool isSuccess;
        public static string textoMensagem;

        public static string texto_pessoa_nao_confirmada;
        MySqlConnection connection = new MySqlConnection ( "Server=MYSQL5040.site4now.net;Database=db_a77bac_root;Uid=a77bac_root;Pwd=Project@2021" );
        public HomeController ( ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment )
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult CadOrc ( )
        {
            textoMensagem = null;
            return View ( );
        }
        public ActionResult Index ( )
        {
            if (isSuccess)
                ViewData["success"] = textoMensagem;
            else
                ViewData["error"] = textoMensagem;

            return View ( );
        }
        public IActionResult AdmHome ( )
        {
            textoMensagem = null;
            return View ( );
        }
        public ActionResult AdmLista ( )
        {

            try
            {
                if (isSuccess)
                    ViewData["success"] = textoMensagem;
                else
                    ViewData["error"] = textoMensagem;
                List<CotacaoViewModel> cot = new List<CotacaoViewModel> ( );
                DataSet ds = new DataSet ( );
                string selectsql = "SELECT * from COTACAO  where IND_STATUS != 3;";
                MySqlCommand cmd = new MySqlCommand ( selectsql, connection );
                connection.Open ( );
                MySqlDataAdapter da = new MySqlDataAdapter ( cmd );
                da.Fill ( ds );
                connection.Close ( );

                int contadorMax = ds.Tables[0].Rows.Count;
                string tipoStatus = string.Empty;

                for (int contadorVar = 0; contadorVar < contadorMax; contadorVar++)
                {
                    switch (Convert.ToInt32 ( ds.Tables[0].Rows[contadorVar]["IND_STATUS"].ToString ( ) ))
                    {
                        case 1:
                            tipoStatus = "Esperando Análise";
                            break;
                        case 2:
                            tipoStatus = "Em Análise";
                            break;
                        case 3:
                            tipoStatus = "Finalizado";
                            break;
                        default:
                            break;
                    }

                    cot.Add ( new CotacaoViewModel ( )
                    {
                        ID = Convert.ToInt32 ( ds.Tables[0].Rows[contadorVar]["ID"].ToString ( ) ),
                        DATA_SOLICITACAO = (ds.Tables[0].Rows[contadorVar]["DATA_SOLICITACAO"].ToString ( )),
                        IND_STATUS = tipoStatus,
                        DATA_ALTERACAO = ds.Tables[0].Rows[contadorVar]["DATA_ALTERACAO"].ToString ( ),
                        COTACAO = ds.Tables[0].Rows[contadorVar]["COTACAO"].ToString ( ),
                        ID_PESSOA = id_pessoa_logada

                    } );

                }


                return View ( cot );
            }
            catch (Exception ex)
            {
                return RedirectToAction ( "Index", "Home" );
            }
            finally
            {
                connection.Close ( );
            }

        }
        public IActionResult AdmUpload ( )
        {
            textoMensagem = null;
            return View ( );
        }
        public IActionResult ConfirmaEmail ( )
        {
            textoMensagem = null;
            return View ( );
        }
        public IActionResult EsqueciMinhaSenha ( )
        {
            textoMensagem = null;
            return View ( ); 
        }
        public IActionResult Home ( )
        {
            try
            {
                textoMensagem = null;
                return View ( );
            }
            catch (Exception ex)
            {
                textoMensagem = null;
                return View ( );
            }
        }
        public ActionResult Lista ()
        {
            try
            {
                textoMensagem = null;
                List<CotacaoViewModel> cot = new List<CotacaoViewModel> ( );
                DataSet ds = new DataSet ( );
                string selectsql = "SELECT * from COTACAO  where ID_PESSOA ="+id_pessoa_logada+";";
                MySqlCommand cmd = new MySqlCommand ( selectsql, connection );
                connection.Open ( );
                MySqlDataAdapter da = new MySqlDataAdapter ( cmd );
                da.Fill ( ds );
                connection.Close ( );

                int contadorMax = ds.Tables[0].Rows.Count;
                string tipoStatus = string.Empty;

                for (int contadorVar = 0; contadorVar < contadorMax; contadorVar++)
                {
                    switch (Convert.ToInt32 ( ds.Tables[0].Rows[contadorVar]["IND_STATUS"].ToString ( ) ))
                    {
                        case 1:
                            tipoStatus = "Esperando Análise";
                            break;
                        case 2:
                            tipoStatus = "Em Análise";
                            break;
                        case 3:
                            tipoStatus = "Finalizado";
                            break;
                        default:
                            break;
                    }

                    cot.Add ( new CotacaoViewModel ( )
                    {
                        ID = Convert.ToInt32 ( ds.Tables[0].Rows[contadorVar]["ID"].ToString ( ) ),
                        DATA_SOLICITACAO = (ds.Tables[0].Rows[contadorVar]["DATA_SOLICITACAO"].ToString ( )),
                        IND_STATUS = tipoStatus,
                        DATA_ALTERACAO = ds.Tables[0].Rows[contadorVar]["DATA_ALTERACAO"].ToString ( ),
                        COTACAO =  ds.Tables[0].Rows[contadorVar]["COTACAO"].ToString ( ),
                        ID_PESSOA = id_pessoa_logada

                    } );

                }


                return View (cot);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                textoMensagem = "Erro ao exibir a lista, contate o administrador do sistema.";
                return RedirectToAction ( "Index", "Home" );
            }
            finally
            {
                connection.Close ( );
            }
        }
        public IActionResult Upload ( )
        {
            if (isSuccess)
                ViewData["success"] = textoMensagem;
            else
                ViewData["error"] = textoMensagem;
            return View ( );
        }
        public IActionResult CadLogin ( )
        {
            textoMensagem = null;
            return View ( );
        }
        public ActionResult EsquecimentoDeSenha ( CadLoginViewModel l )
        {
            try
            {
                textoMensagem = null;
                if (ModelState.IsValid)
                {
                    DataSet ds = new DataSet ( );
                    string selectsql = "SELECT * FROM PESSOA WHERE EMAIL='" + l.EMAIL + "'";
                    MySqlCommand cmd = new MySqlCommand ( selectsql, connection );
                    connection.Open ( );
                    MySqlDataAdapter da = new MySqlDataAdapter ( cmd );
                    da.Fill ( ds );
                    connection.Close ( );
                    if (ds.Tables[0].Rows.Count == 0)
                    {

                        return RedirectToAction ( "EsqueciMinhaSenha", "Home" );
                    }
                    else
                    {
                        EnviaEmail ( "Recuperação de Senha", "Sua senha é: "+Criptografia.Decrypt( ds.Tables[0].Rows[0]["SENHA"].ToString ( )), ds.Tables[0].Rows[0]["EMAIL"].ToString ( ), null );
                        isSuccess = true;
                        textoMensagem = "Sua senha ja foi enviada ao e-mail cadastrado";
                        return RedirectToAction ( "Index", "Home" );
                    }
                }
                else
                {
                    isSuccess = false;
                    textoMensagem = "Erro";
                    return RedirectToAction ( "Index", "Home" );
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                textoMensagem = "Contate o administrador do sistema";
                return RedirectToAction ( "Index", "Home" );
            }
            finally
            {
                connection.Close ( );
            }
        }
        public ActionResult ConfirmacaoMail ( CadLoginViewModel l )
        {
            try
            {
                textoMensagem = null;
                if (ModelState.IsValid)
                {
                    DataSet ds = new DataSet ( );
                    string selectsql = "UPDATE PESSOA SET TEXTO_CONFIRMA=NULL where ID =" + id_pessoa_logada+" ;";
                    MySqlCommand cmd = new MySqlCommand ( selectsql, connection );
                    connection.Open ( );
                    if (texto_pessoa_nao_confirmada==l.TEXTO_CONFIRMA)
                    {
                        MySqlDataAdapter da = new MySqlDataAdapter ( cmd );
                        da.Fill ( ds );
                        connection.Close ( );
                        if (nivel_acesso == 1)
                            return RedirectToAction ( "AdmLista", "Home" );
                        else
                            return RedirectToAction ( "Lista", "Home" );
                    }
                    else
                    {
                        connection.Close ( );
                        return RedirectToAction ( "ConfirmaEmail", "Home" );
                    }
                }
                else
                {
                    return RedirectToAction ( "Index", "Home" );
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                textoMensagem = "Contate o administardor do sistema";
                return RedirectToAction ( "Index", "Home" );
            }
            finally
            {
                connection.Close ( );
            }
        }
        public ActionResult Logar ( CadLoginViewModel l )
        {
            try
            {
                textoMensagem = null;
                if (ModelState.IsValid)
                {
                    DataSet ds = new DataSet ( );
                    string selectsql = "SELECT * FROM PESSOA where EMAIL='"+l.EMAIL+"' AND SENHA='"+Criptografia.Encrypt( l.SENHA)+"'";
                    MySqlCommand cmd = new MySqlCommand ( selectsql, connection );
                    connection.Open ( );
                    MySqlDataAdapter da = new MySqlDataAdapter ( cmd );
                    da.Fill ( ds );
                    connection.Close ( );
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        isSuccess = false;
                        textoMensagem = "E-mail nao cadastrado";
                        return RedirectToAction ( "Index", "Home" );
                    }
                    else
                    {
                        id_pessoa_logada = Convert.ToInt32( ds.Tables[0].Rows[0]["ID"].ToString ( ));
                        Email_Pessoa_Logada = ds.Tables[0].Rows[0]["EMAIL"].ToString ( );
                        texto_pessoa_nao_confirmada = ds.Tables[0].Rows[0]["TEXTO_CONFIRMA"].ToString ( );
                        nivel_acesso = Convert.ToInt32 ( ds.Tables[0].Rows[0]["TIPO_USER"].ToString ( ) );
                        if (String.IsNullOrEmpty( texto_pessoa_nao_confirmada))
                        {
                            if (nivel_acesso==1)
                                return RedirectToAction ( "AdmLista", "Home" );
                            else
                                return RedirectToAction ( "Lista", "Home" );
                        }
                        else
                        {
                            return RedirectToAction ( "ConfirmaEmail", "Home" );
                        }
                    }
                }
                else
                {
                    return RedirectToAction ( "Index", "Home" );
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                textoMensagem = "Contate o administrador do sistema";
                return RedirectToAction ( "Index", "Home" );
            }
            finally
            {
                connection.Close ( );
            }
        }       
        public ActionResult CadastrarUserAdm ( CadLoginViewModel l )
        {
            try
            {
                textoMensagem = null;
                if (ModelState.IsValid)
                {
                    string textoConfirma = GeraTextoDeConfirmacao ( );
                    string selectsql = "INSERT INTO PESSOA(NOME,EMAIL,TELEFONE,SENHA,TEXTO_CONFIRMA,TIPO_USER) VALUES (@NOME,@EMAIL,@TELEFONE,@SENHA,@TEXTO_CONFIRMA,@TIPO_USER);";
                    MySqlCommand cmd = new MySqlCommand ( selectsql, connection );

                    cmd.Parameters.Add ( new MySqlParameter ( "@NOME", l.NOME ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@EMAIL", l.EMAIL ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TELEFONE", l.TELEFONE ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@SENHA", Criptografia.Encrypt ( l.SENHA ) ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TEXTO_CONFIRMA", textoConfirma ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TIPO_USER", 1 ) );

                    connection.Open ( );

                    cmd.ExecuteNonQuery ( );

                    connection.Close ( );
                    EnviaEmail ( "Verificação de e-mail", "Seu código para verificação é: " + textoConfirma, l.EMAIL, null );
                    isSuccess = true;
                    textoMensagem = "Usuario com permissao de administrador cadastrado com sucesso";
                    return RedirectToAction ( "Index", "Home" );
                }
                else
                {
                    return RedirectToAction ( "CadLogin", "Home" );
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction ( "CadLogin", "Home" );
            }
            finally
            {
                connection.Close ( );
            }
        }
        public ActionResult CadastrarCliente( CadLoginViewModel l )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!IsCpf(l.CPF))
                    {

                        isSuccess = false;
                        textoMensagem = "CPF invalido";
                        return RedirectToAction ( "Index", "Home" );
                    }
                    textoMensagem = null;
                    string textoConfirma = GeraTextoDeConfirmacao ( );
                    string selectsql = "INSERT INTO PESSOA(NOME,EMAIL,TELEFONE,SENHA,TEXTO_CONFIRMA,TIPO_USER,CPF,RG,DATANASC,NUMCONVENIO) VALUES (@NOME,@EMAIL,@TELEFONE,@SENHA,@TEXTO_CONFIRMA,@TIPO_USER,@CPF,@RG,@DATANASC,@NUMCONVENIO);";
                    MySqlCommand cmd = new MySqlCommand ( selectsql, connection );

                    cmd.Parameters.Add ( new MySqlParameter ( "@NOME", l.NOME ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@EMAIL", l.EMAIL ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TELEFONE", l.TELEFONE ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@SENHA", Criptografia.Encrypt( l.SENHA) ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TEXTO_CONFIRMA", textoConfirma ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TIPO_USER", 2 ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@CPF", l.CPF ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@RG", l.RG ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@DATANASC", Convert.ToDateTime( l.DATANASC )) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@NUMCONVENIO", l.NUMCOVENIO ) );

                    connection.Open ( );

                    cmd.ExecuteNonQuery ( );

                    connection.Close ( );
                    EnviaEmail ( "Verificação de e-mail", "Seu código para verificação é: " + textoConfirma ,l.EMAIL, null );
                    isSuccess = true;
                    textoMensagem = "Usuario cadastrado com sucesso";
                    return RedirectToAction ( "Index", "Home" );
                }
                else
                {
                    return RedirectToAction ( "CadLogin", "Home" );
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction ( "CadLogin", "Home" );
            }
            finally
            {
                connection.Close ( );
            }
        }
        public static string GeraTextoDeConfirmacao ( )
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random ( );
            var result = new string (
                Enumerable.Repeat ( chars, 6 )
                          .Select ( s => s[random.Next ( s.Length )] )
                          .ToArray ( ) );
            return result;
        }
        public static bool IsCpf ( string cpf )
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim ( );
            cpf = cpf.Replace ( ".", "" ).Replace ( "-", "" );
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring ( 0, 9 );
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse ( tempCpf[i].ToString ( ) ) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString ( );
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse ( tempCpf[i].ToString ( ) ) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString ( );
            return cpf.EndsWith ( digito );
        }
        public async Task<ActionResult> CadSolicitacaoCotacao ( FileUploadModel f )
        {
            textoMensagem = null;
            try
            {


                string contentRootPath = _hostingEnvironment.WebRootPath;//ContentRootPath; content funciona bem sem estar no ar
                var filePath = contentRootPath + "\\Arquivos\\" + f.file.FileName;
                if (f.file.Length>0)
                {
                    using (var stream = new FileStream ( filePath, FileMode.Create ))
                    {
                        await f.file.CopyToAsync ( stream );
 
                    }
                }


                string selectsql = "INSERT INTO COTACAO(DATA_SOLICITACAO,IND_STATUS,DATA_ALTERACAO,ID_PESSOA) VALUES (now(),1,now(),@ID_PESSOA);";
                MySqlCommand cmd = new MySqlCommand ( selectsql, connection );

                cmd.Parameters.Add ( new MySqlParameter ( "@ID_PESSOA", id_pessoa_logada ) );

                connection.Open ( );

                cmd.ExecuteNonQuery ( );

                connection.Close ( );



                EnviaEmail ( "Solicitação de cotação", "Código do usuário" + id_pessoa_logada+" e usa o e-mail:"+Email_Pessoa_Logada, "projetoSMTP27093@gmail.com", filePath );



                isSuccess = true;
                textoMensagem = "Arquivo cadastrado com sucesso";
                return RedirectToAction ( "Upload", "Home" );
            }
            catch (Exception ex)
            {
                connection.Close ( ); 
                isSuccess = false;
                textoMensagem = "Contate o administrador do sistema";
                return RedirectToAction ( "Index", "Home" );
                //throw;
            }
        }
        public static void EnviaEmail(string titulo,string corpo,string para,string caminho)
        {
            try
            {
                SmtpClient client = new SmtpClient ("smtp.gmail.com",587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential ( "projetoSMTP27093@gmail.com", "mailsmtptester" );
                MailMessage msgobj = new MailMessage ( );
                msgobj.To.Add ( para );
                msgobj.From = new MailAddress ( "projetoSMTP27093@gmail.com" );
                msgobj.Subject = titulo;
                msgobj.Body = corpo;



                if (caminho != null)
                {
                    msgobj.Attachments.Add (new Attachment( caminho ) );
                }
                client.Send ( msgobj );


            }
            catch (Exception ex)
            {

                //throw;
            }
        }
        public ActionResult Orc ( int? id,int? idpessoa )
        {
            try
            {
                textoMensagem = null;
                DataSet ds = new DataSet ( );
                DataSet ds1 = new DataSet ( );
                string selectsql = "UPDATE COTACAO SET IND_STATUS=2 where ID =" + Convert.ToInt32 ( id ) + " ;";
                string selectemail = "SELECT EMAIL FROM PESSOA WHERE" + Convert.ToInt32 ( idpessoa ) + " ;";
                MySqlCommand cmd = new MySqlCommand ( selectsql, connection );
                MySqlCommand cmd1 = new MySqlCommand ( selectemail, connection );
                connection.Open ( );

                MySqlDataAdapter da = new MySqlDataAdapter ( cmd );
                MySqlDataAdapter da1 = new MySqlDataAdapter ( cmd1 );
                da.Fill ( ds );
                da1.Fill ( ds1 );
                connection.Close ( );
                IDORC = Convert.ToInt32(id);
                Email_Pessoa_DonaChamado = ds1.Tables[0].Rows[0]["EMAIL"].ToString ( );
                return RedirectToAction ( "CadOrc", "Home" );
            }
            catch (Exception ex)
            {

                return RedirectToAction ( "AdmLista", "Home" );
                //throw;
            }
            finally
            {

            }

        }
        public ActionResult CadPrecOrc ( CotacaoViewModel cot )
        {
            try
            {
                textoMensagem = null;
                DataSet ds = new DataSet ( );
                string selectsql = "UPDATE COTACAO SET IND_STATUS=3,COTACAO="+cot.COTACAO+" where ID =" + IDORC + " ;";
                MySqlCommand cmd = new MySqlCommand ( selectsql, connection );
                connection.Open ( );

                MySqlDataAdapter da = new MySqlDataAdapter ( cmd );
                da.Fill ( ds );
                connection.Close ( ); EnviaEmail ( "Finalização de solicitação", "Sua solicitação foi realizada com sucesso agora é só entrar no site para visualizá-la", Email_Pessoa_DonaChamado, null );
                isSuccess = true;
                textoMensagem = "Preco da cotacao cadastrado com sucesso";
                return RedirectToAction ( "AdmLista", "Home" );
            }
            catch (Exception ex)
            {

                isSuccess = false;
                textoMensagem = "Falha ao registrar preco";
                return RedirectToAction ( "AdmLista", "Home" );
                //throw;
            }
            finally
            {

            }

        }

    }
}
