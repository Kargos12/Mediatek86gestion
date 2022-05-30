using System;

namespace Mediatek86.dal
{
    /// <summary>
    /// classe gérant la commande d'un document
    /// </summary>
    public class CommandeDocument
    {
        private string id;
        private readonly int nbExemplaire;
        private readonly double montant;
        private readonly DateTime dateCommande;
        private string etapes;
        private string idLivreDvd;

        /// <summary>
        /// id de la commande d'un document
        /// </summary>
        public string Id { get => id; set => id = value; }
        /// <summary>
        /// Nombre d'exemplaire d'une commande d'un document
        /// </summary>
        public int NbExemplaire { get => nbExemplaire; }
        /// <summary>
        /// Montant d'une commande de document
        /// </summary>
        public double Montant { get => montant; }
        /// <summary>
        /// Date d'une commande de document
        /// </summary>
        public DateTime DateCommande { get => dateCommande; }
        /// <summary>
        /// Etape d'une commande de document
        /// </summary>
        public string Etapes { get => etapes; set => etapes = value; }
        /// <summary>
        /// id du document associé à la commande
        /// </summary>
        public string IdLivreDvd { get => idLivreDvd; set => idLivreDvd = value; }


        /// <summary>
        /// Constructeur : valorise les propriétés
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nbExemplaire"></param>
        /// <param name="montant"></param>
        /// <param name="dateCommande"></param>
        /// <param name="etapes"></param>
        /// <param name="idlivreDvd"></param>
        public CommandeDocument(string id, int nbExemplaire, double montant, DateTime dateCommande, string etapes, string idlivreDvd)
        {
            this.id = id;
            this.nbExemplaire = nbExemplaire;
            this.montant = montant;
            this.dateCommande = dateCommande;
            this.etapes = etapes;
            this.idLivreDvd = idlivreDvd;

        }
    }
}