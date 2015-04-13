using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kinghost.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Domínio
            cKinghost.CriarDominio("meudominio.com.br", 1, DateTime.Today.AddMonths(1));
            cKinghost.AlterarDominio(123456, 1, 3, 100, 100);
            cKinghost.InformacaoDominio(123456);
            cKinghost.ListarDominioRevenda();
            cKinghost.AlterarStatusDominio(123456);
            cKinghost.ExcluirDominio(123456);
            
            //FTP
            cKinghost.AlterarSenhaFTP(123456, "meuusuarioftp", "n0v@$3nh@");
            cKinghost.ListarUsuarioFTP(123456);

            //E-mail
            cKinghost.ListarEmail(123456);
            cKinghost.CriarEmail(123456, "nome@dominio.com.br", "m1nh@$3nh@", 300);
            cKinghost.CriarEmailAlias(123456, "nome@dominio.com.br", "destino@gmail.com");
            cKinghost.AlterarSenhaEmail(123456, "nome@dominio.com.br", "n0v@$3nh@");
            cKinghost.ExcluirEmail(123456, "nome@dominio.com.br");

            //MySQL
            cKinghost.CriarBaseMySQL(123456, "m1nh@$3nh@");
            cKinghost.CriarAcessoMySQL(123456, "meubanco", "201.57.23.5");
            cKinghost.AlterarSenhaMySQL(123456, "meubanco", "n0v@$3nh@");
            cKinghost.ExcluirBaseMySQL(123456, "meubanco");

            return View();
        }
    }
}