using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace WebServiceSGU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetosController : ControllerBase
    {
        [HttpPost]
        public IActionResult CadastrarProjeto([FromBody] Projetos projetos)
        {
            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
            try
            {
                var dataAtual = DateTime.Now;
                var data_Atual = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dataAtual);
                Random r = new Random();
                var cod = r.Next().ToString();

                MySqlCommand comando = new MySqlCommand("insert into projetos values(@cod,@desc,@custo,@tag,@nome,@doc,@data)", con);
                comando.Parameters.AddWithValue("@cod", cod);
                comando.Parameters.AddWithValue("@desc", projetos.desc);
                comando.Parameters.AddWithValue("@custo", projetos.custo);
                comando.Parameters.AddWithValue("@tag", projetos.tag);
                comando.Parameters.AddWithValue("@nome", projetos.nome);
                comando.Parameters.AddWithValue("@doc", projetos.doc_user);
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

        /*[HttpGet]
        public IActionResult buscarTodos()
        {
            List<Projetos> listaProjetos = new List<Projetos>();

            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from projetos where doc_user = 123456;", con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Projetos proj = new Projetos(reader.GetString("cod_proj"),
                    reader.GetString("desc_proj"),
                    reader.GetString("custo_proj"),
                    reader.GetString("tag_proj"),
                    reader.GetString("nome_proj"),
                    reader.GetString("doc_user"),
                    reader.GetDateTime("data_proj"));
                    listaProjetos.Add(proj);
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
            return listaProjetos.Count == 0 ? NoContent() : Ok(listaProjetos);
        }*/

        [HttpGet]
        [Route("/api/[Controller]/search/{doc}")]
        public IActionResult buscarEspecifico(string doc)
        {
            List<Projetos> listaProjetos = new List<Projetos>();

            MySqlConnection con = new MySqlConnection("Server=localhost;Database=sgu;User id=root;Password=95190529");
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from projetos where doc_user = @doc;", con);
                cmd.Parameters.AddWithValue("@doc", doc);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Projetos proj = new Projetos(reader.GetString("cod_proj"),
                    reader.GetString("desc_proj"),
                    reader.GetString("custo_proj"),
                    reader.GetString("tag_proj"),
                    reader.GetString("nome_proj"),
                    reader.GetString("doc_user"),
                    reader.GetDateTime("data_proj"));
                    listaProjetos.Add(proj);
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
            return listaProjetos.Count == 0 ? NoContent() : Ok(listaProjetos);
        }

    }
}
