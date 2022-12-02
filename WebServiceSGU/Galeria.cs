namespace WebServiceSGU
{
    public class Galeria
    {
        public Galeria(string foto)
        {
            this.foto = foto;
        }

        public string foto { get; set; }
    }
    public class ListaGaleria
    {
        public List<Galeria> galeria { get; set; }
    }
}
