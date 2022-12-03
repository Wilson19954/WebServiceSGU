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
            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());
            try
            {
                var dataAtual = DateTime.Now;
                var data_Atual = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dataAtual);
                Random r = new Random();
                var cod = r.Next().ToString();
                var numero = r.Next().ToString();

                MySqlCommand comando = new MySqlCommand("insert into projetos values(@cod,@desc,@custo,@tag,@nome,@doc,@data, @img)", con);
                comando.Parameters.AddWithValue("@cod", cod);
                comando.Parameters.AddWithValue("@desc", projetos.desc);
                comando.Parameters.AddWithValue("@custo", projetos.custo);
                comando.Parameters.AddWithValue("@tag", projetos.tag);
                comando.Parameters.AddWithValue("@nome", projetos.nome);
                comando.Parameters.AddWithValue("@doc", projetos.doc_user);
                comando.Parameters.AddWithValue("@data", data_Atual);
                comando.Parameters.AddWithValue("@img", projetos.img);

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
        [Route("/api/[Controller]/search/{doc}")]
        public IActionResult buscarEspecifico(string doc)
        {
            List<Projetos> listaProjetos = new List<Projetos>();

            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());
            //MySqlConnection con = new MySqlConnection("Server=ESN509VMYSQL;Database=sgu;User id=aluno;Password=Senai1234");

            try
            {
                //MySqlCommand cmd = new MySqlCommand("select projetos.cod_proj, projetos.desc_proj, projetos.custo_proj, projetos.tag_proj, projetos.nome_proj, projetos.doc_user, projetos.data_proj, galeria.img_proj from sgu.projetos, sgu.galeria where doc_user = @doc AND projetos.cod_proj = galeria.cod_proj;", con);
                MySqlCommand cmd = new MySqlCommand("select * from projetos where doc_user = @doc ", con);
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
                    reader.GetDateTime("data_proj"),
                    reader.GetString("img_principal_proj"));
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
