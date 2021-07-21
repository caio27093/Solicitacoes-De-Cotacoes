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

namespace Projeto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static string Email_Pessoa_Logada;
        public static string Email_Pessoa_DonaChamado;
        public static int id_pessoa_logada;
        public static int nivel_acesso;
        public static int IDORC;
        public static string texto_pessoa_nao_confirmada;
        MySqlConnection connection = new MySqlConnection ( "DATABASE=PROJETO; port=3306; SERVER=localhost; UID=root; pwd=deidara337;" );
        public HomeController ( ILogger<HomeController> logger )
        {
            _logger = logger;
        }
        public IActionResult CadOrc ( )
        {
            return View ( );
        }
        public IActionResult Index ( )
        {
            return View ( );
        }
        public IActionResult AdmHome ( )
        {
            return View ( );
        }
        public ActionResult AdmLista ( )
        {

            try
            {
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
            return View ( );
        }
        public IActionResult ConfirmaEmail ( )
        {
            return View ( );
        }
        public IActionResult EsqueciMinhaSenha ( )
        {
            return View ( ); 
        }
        public IActionResult Home ( )
        {
            try
            {
                return View ( );
            }
            catch (Exception ex)
            {
                return View ( );
            }
        }
        public ActionResult Lista ()
        {
            try
            {
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
                return RedirectToAction ( "Index", "Home" );
            }
            finally
            {
                connection.Close ( );
            }
        }
        public IActionResult Upload ( )
        {
            return View ( );
        }
        public IActionResult CadLogin ( )
        {
            return View ( );
        }
        public ActionResult EsquecimentoDeSenha ( CadLoginViewModel l )
        {
            try
            {
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
                        return RedirectToAction ( "Index", "Home" );
                    }
                }
                else
                {
                    return RedirectToAction ( "Index", "Home" );
                }
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
        public ActionResult ConfirmacaoMail ( CadLoginViewModel l )
        {
            try
            {
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
                            return RedirectToAction ( "AdmHome", "Home" );
                        else
                            return RedirectToAction ( "Home", "Home" );
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
                                return RedirectToAction ( "AdmHome", "Home" );
                            else
                                return RedirectToAction ( "Home", "Home" );
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
                    string textoConfirma = GeraTextoDeConfirmacao ( );
                    string selectsql = "INSERT INTO PESSOA(NOME,EMAIL,TELEFONE,SENHA,TEXTO_CONFIRMA,TIPO_USER) VALUES (@NOME,@EMAIL,@TELEFONE,@SENHA,@TEXTO_CONFIRMA,@TIPO_USER);";
                    MySqlCommand cmd = new MySqlCommand ( selectsql, connection );

                    cmd.Parameters.Add ( new MySqlParameter ( "@NOME", l.NOME ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@EMAIL", l.EMAIL ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TELEFONE", l.TELEFONE ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@SENHA", Criptografia.Encrypt( l.SENHA) ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TEXTO_CONFIRMA", textoConfirma ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TIPO_USER", 2 ) );

                    connection.Open ( );

                    cmd.ExecuteNonQuery ( );

                    connection.Close ( );
                    EnviaEmail ( "Verificação de e-mail", "Seu código para verificação é: " + textoConfirma ,l.EMAIL, null );

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
        public ActionResult CadSolicitacaoCotacao (UploadViewModel u )
        {
            try
            {
                string selectsql = "INSERT INTO COTACAO(DATA_SOLICITACAO,IND_STATUS,DATA_ALTERACAO,ID_PESSOA) VALUES (now(),1,now(),@ID_PESSOA);";
                MySqlCommand cmd = new MySqlCommand ( selectsql, connection );

                cmd.Parameters.Add ( new MySqlParameter ( "@ID_PESSOA", id_pessoa_logada ) );

                connection.Open ( );

                cmd.ExecuteNonQuery ( );

                connection.Close ( );



                EnviaEmail ( "Solicitação de cotação", "Código do usuário" + id_pessoa_logada+" e usa o e-mail:"+Email_Pessoa_Logada, "projetoSMTP27093@gmail.com", u.CAMINHO );



                return RedirectToAction ( "Upload", "Home" );
            }
            catch (Exception ex)
            {
                connection.Close ( );
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
                    Attachment arq = new Attachment ( caminho);
                    msgobj.Attachments.Add ( arq );
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

                return RedirectToAction ( "AdmHome", "Home" );
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
                DataSet ds = new DataSet ( );
                string selectsql = "UPDATE COTACAO SET IND_STATUS=3,COTACAO="+cot.COTACAO+" where ID =" + IDORC + " ;";
                MySqlCommand cmd = new MySqlCommand ( selectsql, connection );
                connection.Open ( );

                MySqlDataAdapter da = new MySqlDataAdapter ( cmd );
                da.Fill ( ds );
                connection.Close ( ); EnviaEmail ( "Finalização de solicitação", "Sua solicitação foi realizada com sucesso agora é só entrar no site para visualizá-la", Email_Pessoa_DonaChamado, null );
                return RedirectToAction ( "AdmLista", "Home" );
            }
            catch (Exception ex)
            {

                return RedirectToAction ( "AdmHome", "Home" );
                //throw;
            }
            finally
            {

            }

        }

    }
}
