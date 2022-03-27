using Mediatek86.controleur;
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
    public partial class FormAuthentification : Form
    {
        /// <summary>
        /// instance du controleur
        /// </summary>
        private Controle controle;

        /// <summary>
        /// Evénement sur le bouton se connecter
        /// Vérification que le login et mot de passe sont bien renseignés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (txtIdentifiant.Equals(""))
            {
                MessageBox.Show("Authentification impossible, veuillez renseigner votre identifiant", "Alerte");
                txtMotDePasse.Text = "";
                txtIdentifiant.Focus();
            }
            else if (txtMotDePasse.Equals(""))
            {
                MessageBox.Show("Authentification impossible, veuillez renseigner votre mot de passe", "Alerte");
                txtMotDePasse.Focus();
            }
            else
            {
                if (!controle.ControleAuthentification(txtIdentifiant.Text, txtMotDePasse.Text))
                {
                    MessageBox.Show("Authentification incorrecte", "Alerte");
                    txtIdentifiant.Text = "";
                    txtMotDePasse.Text = "";
                    txtIdentifiant.Focus();
                }
            }
        }
    }
}
