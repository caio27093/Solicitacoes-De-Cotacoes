using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projeto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Projeto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static string Email_Pessoa_Logada;
        public static int id_pessoa_logada = 1;
        MySqlConnection connection = new MySqlConnection ( "DATABASE=PROJETO; port=3306; SERVER=localhost; UID=root; pwd=deidara337;" );

        public HomeController ( ILogger<HomeController> logger )
        {
            _logger = logger;
        }

        public IActionResult Index ( )
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

        public IActionResult Lista ( )
        {
            return View ( );
        }
        public IActionResult Upload ( )
        {
            return View ( );
        }
        public IActionResult CadLogin ( )
        {
            return View ( );
        }
        public ActionResult Logar ( CadLoginViewModel l )
        {
            try
            {

                if (ModelState.IsValid)
                {

                    DataSet ds = new DataSet ( );
                    string selectsql = "SELECT * FROM PESSOA where EMAIL='"+l.EMAIL+"' AND SENHA='"+l.SENHA+"'";
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
                        Email_Pessoa_Logada = l.EMAIL;
                        return RedirectToAction ( "Home", "Home" );
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
                    cmd.Parameters.Add ( new MySqlParameter ( "@SENHA", l.SENHA ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TEXTO_CONFIRMA", textoConfirma ) );
                    cmd.Parameters.Add ( new MySqlParameter ( "@TIPO_USER", 2 ) );

                    connection.Open ( );

                    cmd.ExecuteNonQuery ( );

                    connection.Close ( );


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



        public ActionResult CadSolicitacaoCotacao (  )
        {
            try
            {
                string selectsql = "INSERT INTO COTACAO(DATA_SOLICITACAO,IND_STATUS,DATA_ALTERACAO,ID_PESSOA) VALUES (now(),1,now(),@ID_PESSOA);";
                MySqlCommand cmd = new MySqlCommand ( selectsql, connection );

                cmd.Parameters.Add ( new MySqlParameter ( "@ID_PESSOA", id_pessoa_logada ) );

                connection.Open ( );

                cmd.ExecuteNonQuery ( );

                connection.Close ( );

                return RedirectToAction ( "Upload", "Home" );
            }
            catch (Exception ex)
            {
                connection.Close ( );
                return RedirectToAction ( "Index", "Home" );
                //throw;
            }
        }



    }
}
