using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

public class cKinghost
{
    private static string login = "email@mpresa.com.br"; //seu e-mail de acesso à API
    private static string senha = "senha"; //sua senha de acesso à API
    private static int clienteID = 123456; //seu ID de cliente na Kinghost

    /// <summary>
    /// Método para acesso à API
    /// </summary>
    /// <param name="method">Tipo de método REST</param>
    /// <param name="urlAPI">URL da API</param>
    /// <param name="parametro">Parâmetros</param>
    private static void API(string method, string urlAPI, string parametro = null)
    {
        HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(urlAPI);
        _req.Method = method;
        _req.Credentials = new NetworkCredential(login, senha);
        _req.ContentType = "application/x-www-form-urlencoded";

        if (!string.IsNullOrWhiteSpace(parametro))
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(parametro);
            _req.ContentLength = data.Length;

            Stream _s = _req.GetRequestStream();
            _s.Write(data, 0, data.Length);
            _s.Close();
        }
        else
            _req.ContentLength = 0;

        HttpWebResponse _resp = (HttpWebResponse)_req.GetResponse();
        StreamReader _reader = new StreamReader(_resp.GetResponseStream());
    }

    #region Domínio

    /// <summary>
    /// Cria um novo domínio na revenda
    /// </summary>
    /// <param name="dominio">Domínio a ser criado, sem www</param>
    /// <param name="planoID">ID do plano previamente cadastrado</param>
    /// <param name="pagoAte">Data na qual a hospedagem do domínio está paga</param>
    public static void CriarDominio(string dominio, int planoID, DateTime pagoAte)
    {
        string p = "idCliente=" + clienteID;
        p += "&dominio=" + dominio.Replace("www", string.Empty);
        p += "&senha=" + senha;
        p += "&planoId=" + planoID;
        p += "&pagoAte=" + pagoAte.ToString("yyyy-MM-dd");

        API("POST", "https://api.kinghost.net/dominio.xml", p);
    }

    /// <summary>
    /// Altera os dados de um domínio específico
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="planoID">ID do plano previamente cadastrado</param>
    /// <param name="periodicidade">1 - Mensal   3 - Trimestral   6 - Semestral   12 - Anual   24 - Bienal</param>
    /// <param name="discoWeb">Espaço em disco em MB</param>
    /// <param name="discoWebVirtual">Espaço em disco virtual em MB</param>
    public static void AlterarDominio(int dominioID, int planoID, int periodicidade, int discoWeb, int discoWebVirtual)
    {
        string p = "idCliente=" + clienteID;
        p += "&idDominio=" + dominioID;
        p += "&planoPeriodicidade=" + periodicidade;
        p += "&planoId=" + planoID;
        p += "&discoWeb=" + discoWeb;
        p += "&discoWebVirtual=" + discoWebVirtual;

        API("PUT", "https://api.kinghost.net/dominio.xml", p);
    }

    /// <summary>
    /// Lista as informações de um domínio específico
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    public static void InformacaoDominio(int dominioID)
    {
        API("GET", "https://api.kinghost.net/dominio/informacoes/" + dominioID + ".xml");
    }

    /// <summary>
    /// Lista todos os domínios da conta/revenda
    /// </summary>
    public static void ListarDominioRevenda()
    {
        API("GET", "https://api.kinghost.net/dominio.xml");
    }

    /// <summary>
    /// Altera o status do domínio para ativo ou bloqueado
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    public static void AlterarStatusDominio(int dominioID)
    {
        API("PUT", "https://api.kinghost.net/dominio/status/" + dominioID + ".xml");
    }

    /// <summary>
    /// Exclui um domínio específico
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    public static void ExcluirDominio(int dominioID)
    {
        API("DELETE", "https://api.kinghost.net/dominio/" + dominioID + ".xml");
    }

    #endregion

    #region FTP

    /// <summary>
    /// Altera a senha de FTP
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="usuario">Nome de usuário FTP</param>
    /// <param name="senha">Nova senha</param>
    public static void AlterarSenhaFTP(int dominioID, string usuario, string senha)
    {
        string p = "idDominio=" + dominioID;
        p += "&usuario=" + usuario;
        p += "&senha=" + senha;

        API("PUT", "https://api.kinghost.net/ftp/senha.xml", p);
    }

    /// <summary>
    /// Lita todos os usuários de FTP de um domínio específico
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    public static void ListarUsuarioFTP(int dominioID)
    {
        API("GET", "https://api.kinghost.net/ftp/" + dominioID + ".xml");
    }

    #endregion

    #region E-mail

    /// <summary>
    /// Lista todos os e-mails de um domínio
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    public static void ListarEmail(int dominioID)
    {
        API("GET", "https://api.uni5.net/email/" + dominioID + ".xml");
    }

    /// <summary>
    /// Cria um nova caixa postal para o domínio
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="email">E-mail a ser criado</param>
    /// <param name="senha">Senha do e-mail</param>
    /// <param name="espaco">Espaço em MB atribuído à caixa postal</param>
    public static void CriarEmail(int dominioID, string email, string senha, int espaco)
    {
        string p = "idDominio=" + dominioID;
        p += "&caixa=" + email;
        p += "&senha=" + senha;
        p += "&tamanho=" + espaco;

        API("POST", "https://api.kinghost.net/email/addmailbox.xml", p);
    }

    /// <summary>
    /// Cria um redirecionamento (alias) de e-mail
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="email">E-mail a ser criado</param>
    /// <param name="destino">E-mail de destino</param>
    public static void CriarEmailAlias(int dominioID, string email, string destino)
    {
        string p = "idDominio=" + dominioID;
        p += "&caixa=" + email;
        p += "&destino=" + destino;

        API("POST", "https://api.kinghost.net/email/addredir.xml", p);
    }

    /// <summary>
    /// Altera a senha de um e-mail específico
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="email">E-mail a ser alterado</param>
    /// <param name="senha">Nova senha</param>
    public static void AlterarSenhaEmail(int dominioID, string email, string senha)
    {
        string p = "idDominio=" + dominioID;
        p += "&caixa=" + email;
        p += "&senha=" + senha;

        API("PUT", "https://api.kinghost.net/email/editsenha.xml", p);
    }

    /// <summary>
    /// Exclui um e-mail específico do domínio
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="email">E-mail a ser excluído</param>
    public static void ExcluirEmail(int dominioID, string email)
    {
        API("DELETE", "https://api.kinghost.net/email/removemailbox/" + dominioID + "/" + email + ".xml");
    }

    #endregion

    #region MySQL

    /// <summary>
    /// Cria um nova base MySQL
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="senha">Senha da base de dados</param>
    public static void CriarBaseMySQL(int dominioID, string senha)
    {
        string p = "idDominio=" + dominioID;
        p += "&senha=" + senha;

        API("POST", "https://api.kinghost.net/mysql.xml", p);
    }

    /// <summary>
    /// Adiciona um IP de acesso a uma base MySQL específica
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="banco">Nome da base</param>
    /// <param name="ip">IP</param>
    public static void CriarAcessoMySQL(int dominioID, string banco, string ip)
    {
        string p = "idDominio=" + dominioID;
        p += "&nomeBanco=" + banco;
        p += "&host=" + ip;

        API("POST", "https://api.kinghost.net/mysql/acessoips.xml", p);
    }

    /// <summary>
    /// Altera a senha de uma base MySQL específica
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="banco">Nome da base</param>
    /// <param name="senha">Nova senha</param>
    public static void AlterarSenhaMySQL(int dominioID, string banco, string senha)
    {
        string p = "idDominio=" + dominioID;
        p += "&nomeBanco=" + banco;
        p += "&senha=" + senha;

        API("PUT", "https://api.kinghost.net/mysql/senha.xml", p);
    }

    /// <summary>
    /// Exclui uma base MySQL específica
    /// </summary>
    /// <param name="dominioID">ID do domínio</param>
    /// <param name="banco">Nome da base</param>
    public static void ExcluirBaseMySQL(int dominioID, string banco)
    {
        API("DELETE", "https://api.kinghost.net/mysql/" + dominioID + "/" + banco + ".xml");
    }

    #endregion
}