using System;

namespace Mediatek86.dal
{
    public class CommandeDocument
    {
        private string id;
        private int nbExemplaire;
        private double montant;
        private DateTime dateCommande;
        private string etapes;
        private string idLivreDvd;

        public string Id { get => id; set => id = value; }
        public int NbExemplaire { get => nbExemplaire; }
        public double Montant { get => montant; }
        public DateTime DateCommande { get => dateCommande; }
        public string Etapes { get => etapes; set => etapes = value; }
        public string IdLivreDvd { get => idLivreDvd; set => idLivreDvd = value; }


        /// <summary>
        /// Constructeur : valorise les propriétés
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nbExemplaire"></param>
        /// <param name="montant"></param>
        /// <param name="dateCommande"></param>
        /// <param name="etapes"></param>
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