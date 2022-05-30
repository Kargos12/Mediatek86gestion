using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.dal
{
    /// <summary>
    /// Classe gérant l'abonnement à une revue
    /// </summary>
    public class AbonnementRevue
    {
        private string id;
        private readonly double montant;
        private readonly DateTime dateCommande;
        private DateTime dateFinAbonnement;
        private string idRevue;

        /// <summary>
        /// Id de l'abonnement à une revue
        /// </summary>
        public string Id { get => id; set => id = value; }
        /// <summary>
        /// Montant de l'abonnement à une revue
        /// </summary>
        public double Montant { get => montant; }
        /// <summary>
        /// Date de début d'un abonnement à une revue
        /// </summary>
        public DateTime DateCommande { get => dateCommande; }
        /// <summary>
        /// Date de fin d'abonnement à une revue
        /// </summary>
        public DateTime DateFinAbonnement { get => dateFinAbonnement; set => dateFinAbonnement = value; }
        /// <summary>
        /// Id de la revue concerné par l'abonnement
        /// </summary>
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
