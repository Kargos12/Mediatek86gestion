
namespace Mediatek86.metier
{
    /// <summary>
    /// Classe gérant les documents
    /// </summary>
    public class Document
    {

        private readonly string id;
        private readonly string titre;
        private readonly string image;
        private readonly string idGenre;
        private readonly string genre;
        private readonly string idPublic;
        private readonly string lePublic;
        private readonly string idRayon;
        private readonly string rayon;

        /// <summary>
        /// Classe gérant les documents
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
        public Document(string id, string titre, string image, string idGenre, string genre, 
            string idPublic, string lePublic, string idRayon, string rayon)
        {
            this.id = id;
            this.titre = titre;
            this.image = image;
            this.idGenre = idGenre;
            this.genre = genre;
            this.idPublic = idPublic;
            this.lePublic = lePublic;
            this.idRayon = idRayon;
            this.rayon = rayon;
        }
        /// <summary>
        /// id d'un document
        /// </summary>
        public string Id { get => id; }
        /// <summary>
        /// titre d'un document
        /// </summary>
        public string Titre { get => titre; }
        /// <summary>
        /// image d'un document
        /// </summary>
        public string Image { get => image; }
        /// <summary>
        /// id du genre d'un document
        /// </summary>
        public string IdGenre { get => idGenre; }
        /// <summary>
        /// Genre d'un documen
        /// </summary>
        public string Genre { get => genre; }
        /// <summary>
        /// Id du public d'un document
        /// </summary>
        public string IdPublic { get => idPublic; }
        /// <summary>
        /// Public d'un document
        /// </summary>
        public string Public { get => lePublic; }
        /// <summary>
        /// Id du rayon d'un document
        /// </summary>
        public string IdRayon { get => idRayon; }
        /// <summary>
        /// Rayon d'un document
        /// </summary>
        public string Rayon { get => rayon; }

    }


}
