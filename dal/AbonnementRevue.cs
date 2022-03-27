using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.dal
{
    public class AbonnementRevue
    {
        private string id;
        private double montant;
        private DateTime dateCommande;
        private DateTime dateFinAbonnement;
        private string idRevue;

        public string Id { get => id; set => id = value; }
        public double Montant { get => montant; }
        public DateTime DateCommande { get => dateCommande; }
        public DateTime DateFinAbonnement { get => dateFinAbonnement; set => dateFinAbonnement = value; }
        public string IdRevue { get => idRevue; set => idRevue = value; }

        /// <summary>
        /// Constructeur : valorise les propriétés
        /// </summary>
        /// <param name="id"></param>
        /// <param name="montant"></param>
        /// <param name="dateCommande"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="idRevue"></param>

        public AbonnementRevue (string id, double montant, DateTime dateCommande, DateTime dateFinAbonnement, string idRevue)
        {
            this.id = id;
            this.montant = montant;
            this.dateCommande = dateCommande;
            this.dateFinAbonnement = dateFinAbonnement;
            this.idRevue = idRevue;
        }
    }
}
