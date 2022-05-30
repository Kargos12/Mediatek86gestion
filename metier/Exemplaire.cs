using System;

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe gérant le détail d'un exemplaire
    /// </summary>
    public class Exemplaire
    {
        /// <summary>
        /// structure des exemplaires
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="dateAchat"></param>
        /// <param name="photo"></param>
        /// <param name="idEtat"></param>
        /// <param name="idDocument"></param>
        public Exemplaire(int numero, DateTime dateAchat, string photo,string idEtat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.IdDocument = idDocument;
        }
        /// <summary>
        /// Numéro de l'exemplaire
        /// </summary>
        public int Numero { get; set; }
        /// <summary>
        /// Photo de l'exemplaire
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Date d'achat de l'exemplaire
        /// </summary>
        public DateTime DateAchat { get; set; }
        /// <summary>
        /// Lien vers l'id définissant l'état du document
        /// </summary>
        public string IdEtat { get; set; }
        /// <summary>
        /// Lien vers l'id definissant le document
        /// </summary>
        public string IdDocument { get; set; }
    }
}
