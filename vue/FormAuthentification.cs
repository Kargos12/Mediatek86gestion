using Mediatek86.controleur;
using Mediatek86.dal;
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
    
    /// <summary>
    /// Fenêtre d'authentification
    /// </summary>
    public partial class FormAuthentification : Form
    {

        /// <summary>
        /// instance du controleur
        /// </summary>
        private Controle controle;

        public FormAuthentification(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
        }

        /// <summary>
        /// Booléen, a vrai si l'authentification a réussie
        /// </summary>
        public bool AuthentificationOk { get; private set; }

        /// <summary>
        /// Evénement sur le bouton se connecter
        /// Vérification que le login et mot de passe sont bien renseignés.
        /// Envoi les informations de connexion au controleur pour accorder l'accès
        /// Si profil culture : pas d'accès
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnection_Click(object sender, EventArgs e)
        {
            string identifiant = txtIdentifiant.Text.Trim();
            string motdepasse = txtMotDePasse.Text.Trim();
            ProfilUtilisateur profilUtilisateur = controle.Authentification(identifiant, motdepasse);

            if (profilUtilisateur != null)
            {
                if (profilUtilisateur.Libelle == "culture")
                {
                    MessageBox.Show("Votre service est identifié comme relevant de la Culture, vos droits ne sont pas suffisants pour accéder à cette application", "Information");
                    Application.Exit();
                }
                else
                {
                    AuthentificationOk = true;
                    Close();
                }
            }
            else
            {
                if (txtIdentifiant.Text == "")
                {
                    MessageBox.Show("Authentification impossible, veuillez renseigner votre identifiant", "Alerte");
                    txtMotDePasse.Text = "";
                    txtIdentifiant.Focus();
                }
                else if (txtMotDePasse.Text == "")
                {
                    MessageBox.Show("Authentification impossible, veuillez renseigner votre mot de passe", "Alerte");
                    txtMotDePasse.Focus();
                }
                else if (!controle.ControleAuthentification(txtIdentifiant.Text, txtMotDePasse.Text))
                {
                    MessageBox.Show("Authentification incorrecte", "Alerte");
                    txtIdentifiant.Text = "";
                    txtMotDePasse.Text = "";
                    txtIdentifiant.Focus();
                }
            }

        }
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
