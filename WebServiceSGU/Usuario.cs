using MySql.Data.MySqlClient;

namespace WebServiceSGU
{
    public class Usuario
    {
       
        public int? id { get; set; } 
        public String? endereco { get; set; }
        public String? nome { get; set; }
        public String? desc { get; set; }
        public String doc { get; set; }
        public String? telefone { get; set; }
        public String? email { get; set; }
        public String? img { get; set; }
        public String senha { get; set; }
        public String? tipo { get; set; }
        
        public Usuario() { }

        public Usuario(string endereco, string nome, string desc, string doc, string telefone, string email, string img, string senha, string tipo)
        {
            this.endereco = endereco;
            this.nome = nome;
            this.desc = desc;
            this.doc = doc;
            this.telefone = telefone;
            this.email = email;
            this.img = img;
            this.senha = senha;
            this.tipo = tipo;
        }

        public Usuario(int id, string endereco, string nome, string desc, string doc, string telefone, string email, string img, string senha, string tipo)
        {
            this.id = id;
            this.endereco = endereco;
            this.nome = nome;
            this.desc = desc;
            this.doc = doc;
            this.telefone = telefone;
            this.email = email;
            this.img = img;
            this.senha = senha;
            this.tipo = tipo;
        }
    }
}
