using Mediatek86.controleur;
using Mediatek86.metier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mediatek86.vue
{
    public partial class FrmAlerteAbonnements : Form
    {
        /// <summary>
        /// Classe gérant l'alerte d'un abonnement arrivant à expiration
        /// </summary>
        private Controle controle;

        
        private readonly BindingSource bdgAlerteAbonnements = new BindingSource();

        private readonly List<FinAbonnement> lesFinAbonnement;

        /// <summary>
        /// forme affichant l'alerte sur les abonnments arrivant à expiration
        /// </summary>
        public FrmAlerteAbonnements()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Affiche les abonnements dont l'expiration est à moins de 30 jours
        /// </summary>
        /// <param name="controle"></param>
        internal FrmAlerteAbonnements(Controle controle)
        {
            InitializeComponent();
            lesFinAbonnement = controle.GetFinAbonnement();
            bdgAlerteAbonnements.DataSource = lesFinAbonnement;
            dgvFinAbonnements.DataSource = bdgAlerteAbonnements;
            dgvFinAbonnements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvFinAbonnements.Columns["idRevue"].DisplayIndex = 2;
            dgvFinAbonnements.Columns[0].HeaderCell.Value = "Date fin d'abonnement";
            dgvFinAbonnements.Columns[1].HeaderCell.Value = "Identifiant Revue";
            dgvFinAbonnements.Columns[2].HeaderCell.Value = "Revue";
        }


    }
}
