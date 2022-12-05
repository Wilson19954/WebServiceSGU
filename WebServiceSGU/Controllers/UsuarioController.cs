using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace WebServiceSGU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpPost]
        public IActionResult cadastrar([FromBody] Usuario usuario)
        {

            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());
            try {

                string cod_ver = "";

                MySqlCommand comando = new MySqlCommand("insert into usuario(endereco_user, nome_user, desc_user, doc_user, telefone_user, email_user, img_user, tipo_user, senha_user, codigo_verificacao) values(@endereco,@nome,@desc,@doc,@telefone,@email,@img,@tipo,@senha, @cod_ver)", con);
                comando.Parameters.AddWithValue("@endereco", usuario.endereco);
                comando.Parameters.AddWithValue("@nome", usuario.nome);
                comando.Parameters.AddWithValue("@desc", usuario.desc);
                comando.Parameters.AddWithValue("@doc", usuario.doc);
                comando.Parameters.AddWithValue("@telefone", usuario.telefone);
                comando.Parameters.AddWithValue("@email", usuario.email);
                comando.Parameters.AddWithValue("@img", usuario.img);
                comando.Parameters.AddWithValue("@tipo", usuario.tipo);
                comando.Parameters.AddWithValue("@senha", criptografar(usuario.senha));
                comando.Parameters.AddWithValue("@cod_ver", cod_ver);

                con.Open();
                if (comando.ExecuteNonQuery() != 0)
                {
                    return Ok(new { result = "sucesso", status = 200 });
                }
                else
                {
                    return NoContent();
                }
            } catch(Exception) {
                throw;  
            } finally {
                con.Close();
            }    
        }

        [HttpPost()]
        [Route("/api/[Controller]/login")]
        public IActionResult login([FromBody] Usuario usuario)
        {
            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());
            try
            {
                MySqlCommand cmd = new MySqlCommand("select senha_user from usuario where doc_user = @doc;", con);
                cmd.Parameters.AddWithValue("@doc", usuario.doc);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (criptografar(usuario.senha).Equals(reader.GetString("senha_user"))){
                        return Ok(new { result = "sucesso", status = 200 });
                    }
                    else
                    {
                        return NoContent();
                    }
                }
            }
            catch (Exception){
                throw;
            }
            finally{
                con.Close();
            }

            return NoContent();
        }


        [HttpGet]
        [Route("/api/[Controller]/buscar/{doc}")] 
        public IActionResult buscarUsuario(string doc)
        {
            List<Usuario> listaUsuario = new List<Usuario>();

            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());

            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from usuario WHERE doc_user = @doc;", con);
                cmd.Parameters.AddWithValue("@doc", doc);
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                   Usuario u = new Usuario(reader.GetString("endereco_user"),
                        reader.GetString("nome_user"),
                        reader.GetString("desc_user"),
                        reader.GetString("doc_user"),
                        reader.GetString("telefone_user"),
                        reader.GetString("email_user"),
                        reader.GetString("img_user"),
                        reader.GetString("tipo_user"),
                        reader.GetString("senha_user"),
                        reader.GetString("codigo_verificacao"));
                        listaUsuario.Add(u);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return listaUsuario.Count == 0 ? NoContent() : Ok(listaUsuario);
        }


       [HttpGet]
        public IActionResult buscarTodos()
        {
            List<Usuario> listaUsuario = new List<Usuario>();

            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());

            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from publicacoes,usuario WHERE publicacoes.doc_user = usuario.doc_user;", con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                        Usuario u = new Usuario(reader.GetString("endereco_user"),
                        reader.GetString("nome_user"),
                        reader.GetString("desc_user"),
                        reader.GetString("doc_user"),
                        reader.GetString("telefone_user"),
                        reader.GetString("email_user"),
                        reader.GetString("img_user"),
                        reader.GetString("tipo_user"),
                        reader.GetString("senha_user"),
                        reader.GetString("codigo_verificacao")
                        );
                        listaUsuario.Add(u);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return listaUsuario.Count == 0 ? NoContent() : Ok(listaUsuario);
        }

        [HttpGet]
        [Route("/api/[Controller]/search/{doc}")]
        public IActionResult buscarEspecifico(string doc)
        {
            List<Usuario> listaUsuario = new List<Usuario>();

            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());


            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from publicacoes, usuario WHERE  usuario.doc_user = @doc AND publicacoes.doc_user = @doc;", con);
                cmd.Parameters.AddWithValue("@doc", doc);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Usuario u = new Usuario(reader.GetString("endereco_user"),
                    reader.GetString("nome_user"),
                    reader.GetString("desc_user"),
                    reader.GetString("doc_user"),
                    reader.GetString("telefone_user"),
                    reader.GetString("email_user"),
                    reader.GetString("img_user"),
                    reader.GetString("tipo_user"),
                    reader.GetString("senha_user"), 
                    reader.GetString("codigo_verificacao"));
                    listaUsuario.Add(u);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return listaUsuario.Count == 0 ? NoContent() : Ok(listaUsuario);
        }

        [HttpPut]
        public IActionResult atualizar([FromBody] Usuario usuario)
        {
            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());
            try
            {
                MySqlCommand cmd = new MySqlCommand("update usuario set endereco_user=@ed, nome_user=@n, desc_user=@dc, telefone_user=@tel, email_user=@e, img_user=@img, tipo_user=@tp, senha_user=@s WHERE doc_user = @docuser;", con);
                cmd.Parameters.AddWithValue("@ed", usuario.endereco);
                cmd.Parameters.AddWithValue("@n", usuario.nome);
                cmd.Parameters.AddWithValue("@dc", usuario.desc);
                cmd.Parameters.AddWithValue("@tel", usuario.telefone);
                cmd.Parameters.AddWithValue("@e", usuario.email);
                cmd.Parameters.AddWithValue("@img", usuario.img); 
                cmd.Parameters.AddWithValue("@tp", usuario.tipo);
                cmd.Parameters.AddWithValue("@s", criptografar(usuario.senha));
                cmd.Parameters.AddWithValue("@docuser", usuario.doc);

                con.Open();

                if (cmd.ExecuteNonQuery() != 0)
                {
                    return Ok(new { result = "sucesso", status = 200 });
                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPost()]
        [Route("/api/[Controller]/token/{token}/{email}")]
        public IActionResult enviarToken(string token, string email)
        {
            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());
            
            try
            {
                MySqlCommand cmd = new MySqlCommand("select codigo_verificacao from usuario where email_user = @email;", con);
                cmd.Parameters.AddWithValue("@email", email);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (token.Equals(reader.GetString("codigo_verificacao")))
                    {
                        return Ok(new { result = "sucesso", status = 200 });
                    }
                    else
                    {
                        return NoContent();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return NoContent();
        }


        [HttpPut]
        [Route("/api/[Controller]/recuperar/{novasenha}/{email}")]
        public IActionResult enviarNovaSenha(string novasenha, string email)
        {
            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());
            try
            {
                MySqlCommand cmd = new MySqlCommand("update usuario set senha_user = @novasenha WHERE email_user = @email;", con);
                cmd.Parameters.AddWithValue("@novasenha", criptografar(novasenha));
                cmd.Parameters.AddWithValue("@email", email);
                con.Open();
                if (cmd.ExecuteNonQuery() != 0)
                {
                    return Ok(new { result = "sucesso", status = 200 });
                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPut]
        [Route("/api/[Controller]/enviar/{email}")]
        public IActionResult enviarEmail(string email)
        {
            Random random = new Random();
            string randomCode = random.Next(10000, 99999).ToString();

            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());

            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                mailMessage.From = new MailAddress("startgrowup.grupo@hotmail.com");
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.Subject = "Token para redefinir sua senha";
                mailMessage.Body = "Seu token para redefinir a senha é: " + randomCode;
                
                smtp.Host = "smtp-mail.outlook.com";
                smtp.EnableSsl = true;
                
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                
                NetworkCred.UserName = "startgrowup.grupo@hotmail.com";
                NetworkCred.Password = "sgu123456";
                
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mailMessage);

                try
                {
                    MySqlCommand cmd = new MySqlCommand("update usuario set codigo_verificacao=@cod_ver WHERE email_user = @email;", con);
                    cmd.Parameters.AddWithValue("@cod_ver", randomCode);
                    cmd.Parameters.AddWithValue("@email", email);
                    con.Open();
                    if (cmd.ExecuteNonQuery() != 0)
                    {
                        return Ok(new { result = "sucesso", status = 200 });
                    }
                    else
                    {
                        return NoContent();
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        private string criptografar(string senha)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }


    }
}
