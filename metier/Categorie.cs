

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe gérant les différentes catégories
    /// </summary>
    public abstract class Categorie
    {
        private readonly string id;
        private readonly string libelle;

        /// <summary>
        /// définition de catégorie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        protected Categorie(string id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;
        }
        /// <summary>
        /// Id de la catégorie
        /// </summary>
        public string Id { get => id; }
        /// <summary>
        /// libellé de la catégorie
        /// </summary>
        public string Libelle { get => libelle; }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.libelle;
        }

    }
}
