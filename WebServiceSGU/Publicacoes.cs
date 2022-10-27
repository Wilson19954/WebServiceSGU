namespace WebServiceSGU
{
    public class Publicacoes
    {    
       
        public String desc { get; set; }
        public int cod { get; set; }
        public String doc_user { get; set; }
        public String tag { get; set; }
        public int like { get; set; }
        public String img { get; set; }
        public DateTime data { get; set; }
        public Publicacoes( string desc, int cod, string doc_user, string tag, int like, string img, DateTime data)
        {
            this.desc = desc;
            this.cod = cod;
            this.doc_user = doc_user;
            this.tag = tag;
            this.like = like;
            this.img = img;
            this.data = data;
        }
        public Publicacoes() { }

    }
}
