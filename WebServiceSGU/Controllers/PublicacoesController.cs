using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace WebServiceSGU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacoesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Publicar([FromBody] Publicacoes publicacoes)
        {
            
            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
            try
            {
                publicacoes.like = 0;
                var dataAtual = DateTime.Now;
                var data_Atual = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dataAtual);

                MySqlCommand comando = new MySqlCommand("insert into publicacoes(img_pub,desc_pub,like_pub,tag_pub,doc_user,data_pub) values(@img,@desc,@like,@tag,@doc_user,@data)", con);
                comando.Parameters.AddWithValue("@img", publicacoes.img);
                comando.Parameters.AddWithValue("@desc", publicacoes.desc);
                comando.Parameters.AddWithValue("@tag", publicacoes.tag);
                comando.Parameters.AddWithValue("@like", publicacoes.like);
                comando.Parameters.AddWithValue("@doc_user", publicacoes.doc_user);
                comando.Parameters.AddWithValue("@data", data_Atual);
                con.Open();
                if (comando.ExecuteNonQuery() != 0)
                {
                    return Ok(new { result = "sucesso", status = 200 });
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        [HttpGet]
        public IActionResult buscarTodos()
        {
            List<Publi> listaPubli = new List<Publi>();

            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
            try
            {
                MySqlCommand cmd = new MySqlCommand("select publicacoes.img_pub, publicacoes.desc_pub, publicacoes.like_pub, publicacoes.tag_pub, publicacoes.data_pub, usuario.nome_user, usuario.doc_user, usuario.img_user, usuario.tipo_user from publicacoes , usuario WHERE publicacoes.doc_user = usuario.doc_user;", con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Publi publi = new Publi(reader.GetString("img_pub"),
                        reader.GetString("desc_pub"),
                        reader.GetInt32("like_pub"),
                        reader.GetString("tag_pub"),
                        reader.GetDateTime("data_pub"),
                        reader.GetString("nome_user"),
                        reader.GetString("doc_user"),
                        reader.GetString("img_user"),
                        reader.GetString("tipo_user"));
                    listaPubli.Add(publi);
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
            return listaPubli.Count == 0 ? NoContent() : Ok(listaPubli);
        }

        [HttpGet]
        [Route("/api/[Controller]/search/{doc}")]
        public IActionResult buscarEspecifico(string doc)
        {
            List<Publi> listaPubli = new List<Publi>();

            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
            try
            {
                MySqlCommand cmd = new MySqlCommand("select publicacoes.img_pub, publicacoes.desc_pub, publicacoes.like_pub, publicacoes.tag_pub, publicacoes.data_pub, usuario.nome_user, usuario.doc_user, usuario.img_user, usuario.tipo_user from publicacoes , usuario WHERE  usuario.doc_user = @doc AND publicacoes.doc_user = @doc;", con);
                cmd.Parameters.AddWithValue("@doc", doc);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Publi publi = new Publi(reader.GetString("img_pub"),
                         reader.GetString("desc_pub"),
                         reader.GetInt32("like_pub"),
                         reader.GetString("tag_pub"),
                         reader.GetDateTime("data_pub"),
                         reader.GetString("nome_user"),
                         reader.GetString("doc_user"),
                         reader.GetString("img_user"),
                         reader.GetString("tipo_user"));
                    listaPubli.Add(publi);
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
            return listaPubli.Count == 0 ? NoContent() : Ok(listaPubli);
        }

        [HttpPost()]
        [Route("/api/[Controller]/like/{cod_pub}")]
        public static string AdicionarLike(string cod_pub)
        {
            int like = 0;
            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
            MySqlCommand comando = new MySqlCommand();
            try
            {
                con.Open();
                comando.Connection = con;
                comando.CommandText = "select like_pub from publicacoes where cod_pub = @cod";
                comando.Parameters.AddWithValue("@cod", cod_pub);   
                MySqlDataReader leitor = comando.ExecuteReader(); 
                while (leitor.Read())
                {
                    like = (int)leitor["like_pub"] + 1;
                }
                con.Close();
                con.Open();
                comando.CommandText = "update publicacoes set like_pub = @like where cod_pub = @cod ";
                comando.Parameters.AddWithValue("@like", like);

                comando.ExecuteNonQuery();
                con.Close();
                return ";)";
            }
            catch
            {
                return "Algo Deu Errado :(";
            }

        }

    }
}
