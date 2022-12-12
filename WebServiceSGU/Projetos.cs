namespace WebServiceSGU
{
    public class Projetos
    {

        public String cod { get; set; }
        public String desc { get; set; }
        public String custo { get; set; }
        public String tag { get; set; }
        public String nome { get; set; }
        public String doc_user { get; set; }
        public DateTime data { get; set; }
        public string img { get; set; }

        public Projetos(string cod, string desc, string custo, string tag, string nome, string doc_user, DateTime data, string img )
        {
            this.cod = cod;
            this.desc = desc;
            this.custo = custo;
            this.tag = tag;
            this.nome = nome;
            this.doc_user = doc_user;
            this.data = data;
            this.img = img;
          
        }
        public Projetos() { }
    } 
}
