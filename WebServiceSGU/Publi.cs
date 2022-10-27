namespace WebServiceSGU
{
    public class Publi
    {
        public Publi(string img_pub, string desc_pub, int like, string tag_pub, DateTime data_pub, string nome_user, string doc_user, string img_user, string tipo_user)
        {
            this.img_pub = img_pub;
            this.desc_pub = desc_pub;
            this.like = like;
            this.tag_pub = tag_pub;
            this.data_pub = data_pub;
            this.nome_user = nome_user;
            this.doc_user = doc_user;
            this.img_user = img_user;
            this.tipo_user = tipo_user;
        }

        public String img_pub { get; set; }
        public String desc_pub { get; set; }
        public int like { get; set; }
        public String tag_pub { get; set; }
        public DateTime data_pub { get; set; }
        public String nome_user { get; set; }
        public String doc_user { get; set; }
        public String img_user { get; set; }
        public String tipo_user { get; set; }
        
       

    }
}
