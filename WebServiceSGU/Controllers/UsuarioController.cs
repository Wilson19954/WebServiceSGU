using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
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
            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
            try {
                MySqlCommand comando = new MySqlCommand("insert into usuario values(@endereco,@nome,@desc,@doc,@telefone,@email,@img,@tipo,@senha)", con);
                comando.Parameters.AddWithValue("@endereco", usuario.endereco);
                comando.Parameters.AddWithValue("@nome", usuario.nome);
                comando.Parameters.AddWithValue("@desc", usuario.desc);
                comando.Parameters.AddWithValue("@doc", usuario.doc);
                comando.Parameters.AddWithValue("@telefone", usuario.telefone);
                comando.Parameters.AddWithValue("@email", usuario.email);
                comando.Parameters.AddWithValue("@img", usuario.img);
                comando.Parameters.AddWithValue("@tipo", usuario.tipo);
                comando.Parameters.AddWithValue("@senha", criptografar(usuario.senha));
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
            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
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

            MySqlConnection con = new MySqlConnection("Server = localhost; Database = sgu; User id = root; Password = 95190529");
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM usuario WHERE doc_user = @doc;", con);
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
                        reader.GetString("senha_user"));
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

            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
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
                        reader.GetString("senha_user"));
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

            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
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
                    reader.GetString("senha_user"));
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
