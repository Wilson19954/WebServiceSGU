using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace WebServiceSGU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GaleriaController : ControllerBase
    {
        public IActionResult cadastro([FromBody] ListaGaleria listaGaleria)
        {
            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());
            bool cadastrado = false;

            Random r = new Random();
            String cod = "1667811372";
            var numero = r.Next().ToString();

            try
            {
                MySqlCommand comando = new MySqlCommand("insert into galeria values(@img,@numero,@cod)", con);
                comando.Parameters.Add("@img", MySqlDbType.String);
                comando.Parameters.AddWithValue("@numero", numero);
                comando.Parameters.AddWithValue("@cod", cod);
                con.Open();

                foreach (Galeria imagem in listaGaleria.galeria)
                {
                    comando.Parameters[0].Value = imagem.foto;

                    if (comando.ExecuteNonQuery() != 0)
                    {
                        cadastrado = true;
                    }
                    else
                    {
                        cadastrado = false;
                        break;
                    }
                }

                if (cadastrado)
                {
                    return Ok(new { result = "sucesso", status = 200 });
                }
                else
                {
                    return NoContent();
                }

                /*con.Open();
                Random r = new Random();*/

                /*MySqlCommand comando2 = new MySqlCommand("insert into galeria values(@img,@numero,@cod)", con);
                bool cadastrou = false;

                foreach (Galeria img in listaGaleria)
                {
                    String cod = "1667811372";
                    var numero = r.Next().ToString();

                    comando2.Parameters.AddWithValue("@img", img);
                    comando2.Parameters.AddWithValue("@numero", numero);
                    comando2.Parameters.AddWithValue("@cod", cod);

                    if (comando2.ExecuteNonQuery() != 0)
                    {
                        cadastrou = true;
                    }
                    else
                    {
                        cadastrou = false;
                        break;
                    }
                }
                if (cadastrou)
                {
                    return Ok(new { result = "sucesso", status = 200 });
                }
                else
                {
                    return NoContent();
                }*/
            }
            catch (MySqlException e) 
            {
                throw;
            } 
            finally 
            {
                con.Close(); 
            }  
           

        }


        [HttpGet]
        [Route("/api/[Controller]/search/{codproj}")]
        public IActionResult buscar(string codproj)
        {
            List<Galeria> galeria = new List<Galeria>();
            MySqlConnection con = new MySqlConnection(ConexaoMysql.conexaoString());

            try {
                MySqlCommand cmd = new MySqlCommand("select img_proj from sgu.galeria where cod_proj = @codproj;", con);
                cmd.Parameters.AddWithValue("@codproj", codproj);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Galeria img = new Galeria(reader.GetString("img_proj")); 
                    galeria.Add(img);
                }

                } 
            catch(Exception e) 
            {
                throw; 
            } 
            finally 
            {
                con.Close(); 
            }
            return galeria.Count == 0 ? NoContent() : Ok(galeria);
        }
    }
}
