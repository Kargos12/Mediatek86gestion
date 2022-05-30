

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe gérant l'état des documents
    /// </summary>
    public class Etat
    {
        /// <summary>
        /// Etat des documents
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Etat(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }
        /// <summary>
        /// Id de l'état du document
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Libellé de l'état du document
        /// </summary>
        public string Libelle { get; set; }
    }
}
