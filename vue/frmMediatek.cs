using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mediatek86.metier;
using Mediatek86.controleur;
using System.Drawing;
using System.Linq;
using System.Data;
using MySql.Data.MySqlClient;
using Mediatek86.dal;

namespace Mediatek86.vue
{
    public partial class FrmMediatek : Form
    {

        #region Variables globales



        private readonly Controle controle;
        const string ETATNEUF = "00001";


        private readonly BindingSource bdgLivresListe = new BindingSource();
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private readonly BindingSource bdgListeCommandeRevues = new BindingSource();
        /// <summary>
        /// Objet pour gérer la liste des commandes de dvds
        /// </summary>
        private readonly BindingSource bdgListeCommandeDvds = new BindingSource();
        /// <summary>
        /// Objet pour gérer la liste des commandes de livres
        /// </summary>
        private readonly BindingSource bdgLivresListeCommandeLivres = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();
        private List<Dvd> lesDvd = new List<Dvd>();
        private List<Revue> lesRevues = new List<Revue>();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        private List<CommandeDocument> lescommandeDocument = new List<CommandeDocument>();
        private List<AbonnementRevue> lesabonnementRevue = new List<AbonnementRevue>();
        private List<ProfilUtilisateur> lesprofilUtilisateurs = new List<ProfilUtilisateur>();
        private string etapescmd;
        private string idlivredvd;
        private string numCommandeLivre;
        private int nbCommandeLivre;
        private double montantCommandeLivre;
        private DateTime dtCommandeLivre;
        private string numCommandeDvd;
        private int nbCommandeDvd;
        private double montantCommandeDvd;
        private DateTime dtCommandeDvd;
        private string numCommandeRevue;
        private double montantCommandeRevue;
        private DateTime dtCommandeRevue;
        private DateTime dtFinCommandeRevue;
        private string refRevue;

        /// <summary>
        /// Gestion de l'ouverture de l'application
        /// Si l'utilisateur est du service de prets : caches les onglets de gestion de commandes
        /// Si l'utilisateur est du service administratif : affiche l'alerte sur les abonnements
        /// </summary>
        /// <param name="controle"></param>

        internal FrmMediatek(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
            if (controle.lesprofilUtilisateurs.Libelle == "prets")
            {
                tabOngletsApplication.TabPages.Remove(tabCommandeLivres);
                tabOngletsApplication.TabPages.Remove(tabCommandeDvds);
                tabOngletsApplication.TabPages.Remove(tabCommandeRevues);
            }
            if (controle.lesprofilUtilisateurs.Libelle == "administratif")
            {
                FrmAlerteAbonnements alerteFinAbonnements = new FrmAlerteAbonnements(controle)
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                alerteFinAbonnements.ShowDialog();
            }
        }
        #endregion

        #region modules communs

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        #endregion


        #region Revues
        //-----------------------------------------------------------
        // ONGLET "Revues"
        //------------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["empruntable"].Visible = false;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>();
                    revues.Add(revue);
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            chkRevuesEmpruntable.Checked = revue.Empruntable;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            chkRevuesEmpruntable.Checked = false;
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        #endregion


        #region Livres

        //-----------------------------------------------------------
        // ONGLET "LIVRES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controle.GetAllLivres();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>();
                    livres.Add(livre);
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        #endregion


        #region Dvd
        //-----------------------------------------------------------
        // ONGLET "DVD"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controle.GetAllDvd();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>();
                    Dvd.Add(dvd);
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd"></param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        #endregion


        #region Réception Exemplaire de presse
        //-----------------------------------------------------------
        // ONGLET "RECEPTION DE REVUES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet : blocage en saisie des champs de saisie des infos de l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            bdgExemplairesListe.DataSource = exemplaires;
            dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
            dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
            dgvReceptionExemplairesListe.Columns["idDocument"].Visible = false;
            dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
            dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    VideReceptionRevueInfos();
                }
            }
            else
            {
                VideReceptionRevueInfos();
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            accesReceptionExemplaireGroupBox(false);
            VideReceptionRevueInfos();
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            chkReceptionRevueEmpruntable.Checked = revue.Empruntable;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            afficheReceptionExemplairesRevue();
            // accès à la zone d'ajout d'un exemplaire
            accesReceptionExemplaireGroupBox(true);
        }

        private void afficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controle.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
        }

        /// <summary>
        /// Vide les zones d'affchage des informations de la revue
        /// </summary>
        private void VideReceptionRevueInfos()
        {
            txbReceptionRevuePeriodicite.Text = "";
            chkReceptionRevueEmpruntable.Checked = false;
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            lesExemplaires = new List<Exemplaire>();
            RemplirReceptionExemplairesListe(lesExemplaires);
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de l'exemplaire
        /// </summary>
        private void VideReceptionExemplaireInfos()
        {
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces"></param>
        private void accesReceptionExemplaireGroupBox(bool acces)
        {
            VideReceptionExemplaireInfos();
            grpReceptionExemplaire.Enabled = acces;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controle.CreerExemplaire(exemplaire))
                    {
                        VideReceptionExemplaireInfos();
                        afficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// Sélection d'une ligne complète et affichage de l'image sz l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        #endregion

        #region Commande de livres
        //-----------------------------------------------------------
        // ONGLET "Commande de livres"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Commande de livres : 
        /// initialise le champ de date de commande à la date du jour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeLivres_Enter(object sender, EventArgs e)
        {
            dtpCommandeLivres.Value = DateTime.Today;
        }


        /// <summary>
        /// Affiche tout les livres
        /// </summary>
        public void RemplirListeCommandeLivres()
        {
            List<CommandeDocument> lesCommandeLivres = controle.GetLesCommandeDocument();

            bdgLivresListeCommandeLivres.DataSource = lesCommandeLivres;
            dgvLivresListeCommandeLivres.DataSource = bdgLivresListeCommandeLivres;
            dgvLivresListeCommandeLivres.Columns["id"].HeaderText = "Ref commande";
            dgvLivresListeCommandeLivres.Columns["nbExemplaire"].HeaderText = "Nbre d'exemplaire";
            dgvLivresListeCommandeLivres.Columns["DateCommande"].HeaderText = "Date commande";
            dgvLivresListeCommandeLivres.Columns["idLivreDvd"].HeaderText = "Ref livre";
            dgvLivresListeCommandeLivres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        /// <summary>
        /// Recherche les informations du livre dont le numéro à été renseigné
        /// Si le numéro n'est pas trouvé : message d'erreur
        /// Si le numéro est trouver : affiche les infos du livre et affiche le panneau de commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheLivreCommandeLivres_Click(object sender, EventArgs e)
        {
            if (!txtNumeroCommandeLivres.Text.Equals(""))
            {
                CommandeDocument commandeDocument = lescommandeDocument.Find(x => x.IdLivreDvd.Equals(txtNumeroCommandeLivres.Text.Trim()));
                Livre livre = lesLivres.Find(x => x.Id.Equals(txtNumeroCommandeLivres.Text.Trim()));
                if (livre != null)
                {
                    AfficheInfosLivreCommandeLivres(livre);
                    RemplirListeCommandeLivres();
                    List<Livre> livres = new List<Livre>();
                    gpbNouvelleCommandeCommandeLivres.Visible = true;
                    grbModificationCommandeLivres.Visible = true;
                }
                else
                {
                    MessageBox.Show("Aucun livre portant le numéro " + txtNumeroCommandeLivres.Text + " n'a été trouvé");
                    txtNumeroCommandeLivres.Text = "";
                    txtNumeroCommandeLivres.Focus();
                    gpbNouvelleCommandeCommandeLivres.Visible = false;
                    VideInfoLivre();
                }
            }
            else
            {
                VideInfoLivre();
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheInfosLivreCommandeLivres(Livre livre)
        {
            // informations sur le livre
            txtTitreCommandeLivres.Text = livre.Titre;
            txtAuteurCommandeLivres.Text = livre.Auteur;
            txtCollectionCommandeLivres.Text = livre.Collection;
            txtGenreCommandeLivres.Text = livre.Genre;
            txtPublicCommandeLivres.Text = livre.Public;
            txtRayonCommandeLivres.Text = livre.Rayon;
            txtCheminImgCommandeLivres.Text = livre.Image;
            txtISBNCommandeLivres.Text = livre.Isbn;
        }

        /// <summary>
        /// Réinitialise les info de commande de livre
        /// </summary>
        private void VideInfoCommandeLivre()
        {
            txtbrefCommandeLivres.Text = "";
            nudNbCommandeLivres.Value = 0;
            nudMontantCommandeLivres.Value = 0;
        }
        /// <summary>
        /// Réinitialise les info du livre
        /// </summary>
        private void VideInfoLivre()
        {
            txtTitreCommandeLivres.Text = "";
            txtAuteurCommandeLivres.Text = "";
            txtCollectionCommandeLivres.Text = "";
            txtGenreCommandeLivres.Text = "";
            txtPublicCommandeLivres.Text = "";
            txtRayonCommandeLivres.Text = "";
            txtCheminImgCommandeLivres.Text = "";
            txtISBNCommandeLivres.Text = "";
        }

        /// <summary>
        /// Clique sur le bouton pour annuler la commande de livre en cours :
        /// Vide les champs et cache la date de commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerCommandeLivres_Click(object sender, EventArgs e)
        {
            dtpCommandeLivres.Value = DateTime.Today;
            nudNbCommandeLivres.Value = 0;
            nudMontantCommandeLivres.Value = 0;
            txtbrefCommandeLivres.Text = "";
            lblDateCommandeLivre.Visible = false;
            dtpCommandeLivres.Visible = false;
        }

        /// <summary>
        /// Clique sur le bouton de validation de commande de livre :
        /// vérifie si le nombre d'exemplaires et le montant ne sont pas égaux à 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderCommandeLivres_Click(object sender, EventArgs e)
        {
            if (txtbrefCommandeLivres.Text == "")
            {
                MessageBox.Show("La commande doit avoir une référence");
                txtbrefCommandeLivres.Focus();
            }
            else if (nudNbCommandeLivres.Value == 0)
            {
                MessageBox.Show("La commande doit contenir au moins un exemplaire");
                nudNbCommandeLivres.Focus();
            }
            else if (nudMontantCommandeLivres.Value == 0)
            {
                MessageBox.Show("Le montant ne peux être égal à 0");
                nudMontantCommandeLivres.Focus();
            }
            else
            {
                lblDateCommandeLivre.Visible = true;
                dtpCommandeLivres.Visible = true;
                numCommandeLivre = txtbrefCommandeLivres.Text;
                nbCommandeLivre = (int)nudNbCommandeLivres.Value;
                montantCommandeLivre = (double)nudMontantCommandeLivres.Value;
                dtCommandeLivre = dtpCommandeLivres.Value;
                etapescmd = "1";
                idlivredvd = txtNumeroCommandeLivres.Text;
                CommandeDocument commandeDocument = new CommandeDocument(numCommandeLivre, nbCommandeLivre, montantCommandeLivre, dtCommandeLivre, etapescmd, idlivredvd);
                controle.AddCommandeDocument(commandeDocument);
                RemplirListeCommandeLivres();
                VideInfoCommandeLivre();
                MessageBox.Show("Commande réalisée");
            }

        }
        /// <summary>
        /// Clique sur le bouton pour mettre la commande de livre selectionnée au statut en cours
        /// la commande ne peux être relancée que si elle est en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatutCommandeLivresRelance_Click(object sender, EventArgs e)
        {
            if (dgvLivresListeCommandeLivres.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgLivresListeCommandeLivres.List[bdgLivresListeCommandeLivres.Position];
                if (commandeDocument.Etapes == "en cours")
                {
                    commandeDocument.Etapes = "4";
                    commandeDocument.Id = (string)(dgvLivresListeCommandeLivres.SelectedRows[0].Cells["id"].Value);
                    controle.UpdateEtapes(commandeDocument);
                    RemplirListeCommandeLivres();
                    MessageBox.Show("La commande a bien été relancée");
                }
                else
                {
                    if (commandeDocument.Etapes == "relancée")
                    {
                        MessageBox.Show("La commande a déjà été relancée", "Information");
                    }
                    else if (commandeDocument.Etapes == "livrée")
                    {
                        MessageBox.Show("La commande ne peux pas être relancée, celle-ci a été livrée", "Information");
                    }
                    else if (commandeDocument.Etapes == "réglée")
                    {
                        MessageBox.Show("Le statut de la commande ne peux pas être changé, celle-ci a été livrée et réglée", "Information");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une ligne", "Information");
            }
        }
        /// <summary>
        /// Clique sur le bouton pour mettre la commande selectionnée au statut livrée
        /// Possible que si la commande est en cours ou relancée
        /// ajoute les exemplaires dans la table corresponsante
        /// la commande peux être mise au statut livrée que si elle est en cours ou relancée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatutCommandeLivresLivree_Click(object sender, EventArgs e)
        {
            if (dgvLivresListeCommandeLivres.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgLivresListeCommandeLivres.List[bdgLivresListeCommandeLivres.Position];
                if (commandeDocument.Etapes == "en cours" || commandeDocument.Etapes == "relancée")
                {
                    commandeDocument.Etapes = "2";
                    commandeDocument.Id = (string)(dgvLivresListeCommandeLivres.SelectedRows[0].Cells["id"].Value);
                    controle.UpdateEtapes(commandeDocument);

                    for (int k = 1; k <= commandeDocument.NbExemplaire; k++)
                    {
                        int numero = k;
                        DateTime dateAchat = commandeDocument.DateCommande;
                        string photo = "";
                        string idEtat = ETATNEUF;
                        string idDocument = commandeDocument.IdLivreDvd;
                        Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                        controle.CreerExemplaire(exemplaire);
                        RemplirListeCommandeLivres();               
                    }
                    MessageBox.Show("La commande a bien été notée comme livrée", "Information");
                }
                else
                {
                    if (commandeDocument.Etapes == "livrée")
                    {
                        MessageBox.Show("La commande est déjà notée comme étant livrée", "Information");
                    }
                    else if (commandeDocument.Etapes == "réglée")
                    {
                        MessageBox.Show("Le statut de la commande ne peux pas être changé, celle-ci a été livrée et réglée", "Information");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une ligne", "Information");
            }
        }
        /// <summary>
        /// Clique sur le bouton pour mettre la commande selectionnée au statut réglée
        /// la commande ne peux être notée réglée que si elle a été livrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatutCommandeLivresReglee_Click(object sender, EventArgs e)
        {
            if (dgvLivresListeCommandeLivres.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgLivresListeCommandeLivres.List[bdgLivresListeCommandeLivres.Position];

                if (commandeDocument.Etapes == "livrée")
                {
                    commandeDocument.Etapes = "3";
                    commandeDocument.Id = (string)(dgvLivresListeCommandeLivres.SelectedRows[0].Cells["id"].Value);
                    controle.UpdateEtapes(commandeDocument);
                    RemplirListeCommandeLivres();
                    MessageBox.Show("La commande a bien été notée comme reglée", "Information");
                }
                else
                {
                    if (commandeDocument.Etapes == "réglée")
                    {
                        MessageBox.Show("La commande est déjà notée comme étant réglée", "Information");
                    }
                    else if (commandeDocument.Etapes == "en cours" || commandeDocument.Etapes == "relancée")
                    {
                        MessageBox.Show("La commande ne peux pas être notée réglée : elle n'a pas été livrée", "Information");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une ligne", "Information");
            }
        }
        /// <summary>
        /// Clique sur le bouton pour supprimer la commande selectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatutCommandeLivresSupprimer_Click(object sender, EventArgs e)
        {
            CommandeDocument commandeDocument = (CommandeDocument)bdgLivresListeCommandeLivres.List[bdgLivresListeCommandeLivres.Position];
            if (dgvLivresListeCommandeLivres.SelectedRows.Count > 0)
            {
                if (commandeDocument.Etapes == "livrée" || commandeDocument.Etapes == "reglée")
                {
                    MessageBox.Show("Il n'est pas possible de supprimer une commande livée", "Information");
                }
                else
                {
                    if (MessageBox.Show("Voulez-vous vraiment supprimer la commande du livre ayant pour référence " + commandeDocument.Id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        controle.DelCommandeDocument(commandeDocument);
                        RemplirListeCommandeLivres();
                    }
                    
                }
            }
            else
                    {
                        MessageBox.Show("Veuillez selectionner une ligne", "Information");
                    }
        }
        #endregion

        #region Commande de Dvds
        //-----------------------------------------------------------
        // ONGLET "Commande de Dvds"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Commande de livres : 
        /// initialise le champ de date de commande à la date du jour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeDvds_Enter(object sender, EventArgs e)
        {
            dtpCommandeDvds.Value = DateTime.Today;
        }

        /// <summary>
        /// Affiche tout les Dvds
        /// </summary>
        public void RemplirListeCommandeDvds()
        {
            List<CommandeDocument> lesCommandeDvds = controle.GetLesCommandeDocument();

            bdgListeCommandeDvds.DataSource = lesCommandeDvds;
            dgvListeCommandeDvds.DataSource = bdgListeCommandeDvds;
            dgvListeCommandeDvds.Columns["id"].HeaderText = "Ref commande";
            dgvListeCommandeDvds.Columns["nbExemplaire"].HeaderText = "Nbre d'exemplaire";
            dgvListeCommandeDvds.Columns["DateCommande"].HeaderText = "Date commande";
            dgvListeCommandeDvds.Columns["idLivreDvd"].HeaderText = "Ref Dvd";
            dgvListeCommandeDvds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        /// <summary>
        /// Recherche les informations du livre dont le numéro à été renseigné
        /// Si le numéro n'est pas trouvé : message d'erreur
        /// Si le numéro est trouver : affiche les infos du livre et affiche le panneau de commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void btnRechercheCommandeDvds_Click(object sender, EventArgs e)
        {
            if (!txtNumeroCommandeDvds.Text.Equals(""))
            {
                CommandeDocument commandeDocument = lescommandeDocument.Find(x => x.IdLivreDvd.Equals(txtNumeroCommandeDvds.Text.Trim()));
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txtNumeroCommandeDvds.Text.Trim()));
                if (dvd != null)
                {
                    AfficheInfosCommandeDvds(dvd);
                    RemplirListeCommandeDvds();
                    List<Dvd> dvds = new List<Dvd>();
                    gpbNouvelleCommandeCommandeDvds.Visible = true;
                    grbModificationCommandeDvds.Visible = true;
                }
                else
                {
                    MessageBox.Show("Aucun dvd portant le numéro " + txtNumeroCommandeLivres.Text + " n'a été trouvé");
                    txtNumeroCommandeDvds.Text = "";
                    txtNumeroCommandeDvds.Focus();
                    gpbNouvelleCommandeCommandeDvds.Visible = false;
                    VideInfoDvd();
                }
            }
            else
            {
                VideInfoDvd();
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd"></param>
        private void AfficheInfosCommandeDvds(Dvd dvd)
        {
            // informations sur le dvd
            txtTitreCommandeDvds.Text = dvd.Titre;
            txtDureeCommandeDvds.Text = dvd.Duree.ToString();
            txtRealCommandeDvds.Text = dvd.Realisateur;
            txtSynopsisCommandeDvds.Text = dvd.Synopsis;
            txtGenreCommandeDvds.Text = dvd.Genre;
            txtPublicCommandeDvds.Text = dvd.Public;
            txtRayonCommandeDvds.Text = dvd.Rayon;
            txtCheminCommandeDvds.Text = dvd.Image;
        }

        /// <summary>
        /// Réinitialise les info de commande de dvd
        /// </summary>
        private void VideInfoCommandeDvd()
        {
            txtbrefCommandeDvds.Text = "";
            nudNbCommandeDvds.Value = 0;
            nudMontantCommandeDvds.Value = 0;
        }
        /// <summary>
        /// Réinitialise les info du dvd
        /// </summary>
        private void VideInfoDvd()
        {
            txtTitreCommandeDvds.Text = "";
            txtDureeCommandeDvds.Text = "";
            txtRealCommandeDvds.Text = "";
            txtSynopsisCommandeDvds.Text = "";
            txtGenreCommandeDvds.Text = "";
            txtPublicCommandeDvds.Text = "";
            txtRayonCommandeDvds.Text = "";
            txtCheminCommandeDvds.Text = "";
        }
        /// <summary>
        /// Clique sur le bouton pour annuler la commande de dvd en cours :
        /// Vide les champs et cache la date de commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerCommandeDvds_Click(object sender, EventArgs e)
        {
            dtpCommandeDvds.Value = DateTime.Today;
            nudNbCommandeDvds.Value = 0;
            nudMontantCommandeDvds.Value = 0;
            txtbrefCommandeDvds.Text = "";
            lblDateCommandeDvd.Visible = false;
            dtpCommandeDvds.Visible = false;
        }
        /// <summary>
        /// Clique sur le bouton de validation de commande de dvd :
        /// vérifie si le nombre d'exemplaires et le montant ne sont pas égaux à 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void btnValiderCommandeDvds_Click(object sender, EventArgs e)
        {
            if (txtbrefCommandeDvds.Text == "")
            {
                MessageBox.Show("La commande doit avoir une référence");
                txtbrefCommandeDvds.Focus();
            }
            else if (nudNbCommandeDvds.Value == 0)
            {
                MessageBox.Show("La commande doit contenir au moins un exemplaire");
                nudNbCommandeDvds.Focus();
            }
            else if (nudMontantCommandeDvds.Value == 0)
            {
                MessageBox.Show("Le montant ne peux être égal à 0");
                nudMontantCommandeDvds.Focus();
            }
            else
            {
                lblDateCommandeDvd.Visible = true;
                dtpCommandeDvds.Visible = true;
                numCommandeDvd = txtbrefCommandeDvds.Text;
                nbCommandeDvd = (int)nudNbCommandeDvds.Value;
                montantCommandeDvd = (double)nudMontantCommandeDvds.Value;
                dtCommandeDvd = dtpCommandeDvds.Value;
                etapescmd = "1";
                idlivredvd = txtNumeroCommandeDvds.Text;
                CommandeDocument commandeDocument = new CommandeDocument(numCommandeDvd, nbCommandeDvd, montantCommandeDvd, dtCommandeDvd, etapescmd, idlivredvd);
                controle.AddCommandeDocument(commandeDocument);
                RemplirListeCommandeDvds();
                VideInfoCommandeDvd();
                MessageBox.Show("Commande réalisée");
            }
        }
        /// <summary>
        /// Clique sur le bouton pour mettre la commande de dvd selectionnée au statut en cours
        /// la commande ne peux être relancée que si elle est en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatutCommandeDvdsRelance_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandeDvds.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgListeCommandeDvds.List[bdgListeCommandeDvds.Position];
                
                if (commandeDocument.Etapes == "en cours")
                {
                    commandeDocument.Etapes = "4";
                    commandeDocument.Id = (string)(dgvListeCommandeDvds.SelectedRows[0].Cells["id"].Value);
                    controle.UpdateEtapes(commandeDocument);
                    RemplirListeCommandeDvds();
                    MessageBox.Show("La commande a bien été relancée");
                }
                else
                {
                    if (commandeDocument.Etapes == "relancée")
                    {
                        MessageBox.Show("La commande a déjà été relancée", "Information");
                    }
                    else if (commandeDocument.Etapes == "livrée")
                    {
                        MessageBox.Show("La commande ne peux pas être relancée, celle-ci a été livrée", "Information");
                    }
                    else if (commandeDocument.Etapes == "réglée")
                    {
                        MessageBox.Show("Le statut de la commande ne peux pas être changé, celle-ci a été livrée et réglée", "Information");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une ligne", "Information");
            }
        }
        /// <summary>
        /// Clique sur le bouton pour mettre la commande de dvd selectionnée au statut livrée
        /// Possible que si la commande est en cours ou relancée
        /// ajoute les exemplaires dans la table corresponsante
        /// la commande peux être mise au statut livrée que si elle est en cours ou relancée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatutCommandeDvdsLivree_Click(object sender, EventArgs e)
            {
            if (dgvListeCommandeDvds.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgListeCommandeDvds.List[bdgListeCommandeDvds.Position];
                if (commandeDocument.Etapes == "en cours" || commandeDocument.Etapes == "relancée")
                {
                    commandeDocument.Etapes = "2";
                    commandeDocument.Id = (string)(dgvListeCommandeDvds.SelectedRows[0].Cells["id"].Value);
                    controle.UpdateEtapes(commandeDocument);

                    for (int k = 1; k <= commandeDocument.NbExemplaire; k++)
                    {
                        int numero = k;
                        DateTime dateAchat = commandeDocument.DateCommande;
                        string photo = "";
                        string idEtat = ETATNEUF;
                        string idDocument = commandeDocument.IdLivreDvd;
                        Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                        controle.CreerExemplaire(exemplaire);
                        RemplirListeCommandeLivres();
                    }
                    MessageBox.Show("La commande a bien été notée comme livrée", "Information");
                }
                else
                {
                    if (commandeDocument.Etapes == "livrée")
                    {
                        MessageBox.Show("La commande est déjà notée comme étant livrée", "Information");
                    }
                    else if (commandeDocument.Etapes == "réglée")
                    {
                        MessageBox.Show("Le statut de la commande ne peux pas être changé, celle-ci a été livrée et réglée", "Information");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une ligne", "Information");
            }
        }
        /// <summary>
        /// Clique sur le bouton pour mettre la commande de dvd selectionnée au statut réglée
        /// la commande ne peux être notée réglée que si elle a été livrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatutCommandeDvdsReglee_Click(object sender, EventArgs e)
        {
            if (dgvListeCommandeDvds.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgListeCommandeDvds.List[bdgListeCommandeDvds.Position];
                if (commandeDocument.Etapes == "livrée")
                {
                    commandeDocument.Etapes = "3";
                    commandeDocument.Id = (string)(dgvListeCommandeDvds.SelectedRows[0].Cells["id"].Value);
                    controle.UpdateEtapes(commandeDocument);
                    RemplirListeCommandeDvds();
                    MessageBox.Show("La commande a bien été notée comme reglée", "Information");
                }
                else
                {
                    if (commandeDocument.Etapes == "réglée")
                    {
                        MessageBox.Show("La commande est déjà notée comme étant réglée", "Information");
                    }
                    else if (commandeDocument.Etapes == "en cours" || commandeDocument.Etapes == "relancée")
                    {
                        MessageBox.Show("La commande ne peux pas être notée réglée : elle n'a pas été livrée", "Information");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une ligne", "Information");
            }
        }

        /// <summary>
        /// Clique sur le bouton pour supprimer la commande de dvd selectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatutCommandeDvdsSupprimer_Click(object sender, EventArgs e)
        {
            CommandeDocument commandeDocument = (CommandeDocument)bdgListeCommandeDvds.List[bdgListeCommandeDvds.Position];
            if (dgvListeCommandeDvds.SelectedRows.Count > 0)
            {
                if (commandeDocument.Etapes == "livrée" || commandeDocument.Etapes == "reglée")
                {
                    MessageBox.Show("Il n'est pas possible de supprimer une commande livée", "Information");
                }
                else
                {
                    if (MessageBox.Show("Voulez-vous vraiment supprimer la commande du dvd ayant pour référence " + commandeDocument.Id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        controle.DelCommandeDocument(commandeDocument);
                        RemplirListeCommandeDvds();
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une ligne", "Information");
            }
        }
        #endregion

        #region Commande de Revues
        //-----------------------------------------------------------
        // ONGLET "Commande de Revues"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet de commande de Revues : 
        /// Initialise le champ de date de commande des revues à la date du jour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeRevues_Enter(object sender, EventArgs e)
        {
            dtpCommandeAboRevues.Value = DateTime.Today;
        }


        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvée, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheCommandeRevues_Click(object sender, EventArgs e)
            {
                if (!txtNumeroCommandeRevues.Text.Equals(""))
                {
                    Revue revue = lesRevues.Find(x => x.Id.Equals(txtNumeroCommandeRevues.Text));
                    if (revue != null)
                    {
                        List<Revue> revues = new List<Revue>();
                        revues.Add(revue);
                    AfficheInfosRevues(revue);
                    }
                    else
                    {
                    MessageBox.Show("Numéro introuvable");
                    txtNumeroCommandeRevues.Text = "";
                    txtNumeroCommandeRevues.Focus();
                    }
                }
        }

        /// <summary>
        /// Affichage des informations de la revue cherchée
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheInfosRevues(Revue revue)
        {
            // informations sur la revue
            txtTitreCommandeRevues.Text = revue.Titre;
            txtPeriodeCommandeRevues.Text = revue.Periodicite;
            txtDelaiCommandeRevues.Text = revue.DelaiMiseADispo.ToString();
            txtGenreCommandeRevues.Text = revue.Genre;
            txtPublicCommandeRevues.Text = revue.Public;
            txtRayonCommandeRevues.Text = revue.Rayon;
            txtCheminCommandeRevues.Text = revue.Image;
        }


        /// <summary>
        /// Remplit le dategrid de revue de l'onglet commande de revues avec la liste reçue en paramètre
        /// </summary>
        private void RemplirRevuesListeONCommande(List<AbonnementRevue> lesabonnementRevues)
        {
            
            bdgListeCommandeRevues.DataSource = lesabonnementRevues;
            dgvListeCommandeRevues.DataSource = bdgListeCommandeRevues;
            dgvListeCommandeRevues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Evénement sur le clique de validation de l'abonnement à une revue
        /// Vérification si tous les champs sont renseignés
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderCommandeRevues_Click(object sender, EventArgs e)
        {
            if (txtbrefCommandeRevues.Text == "")
            {
                MessageBox.Show("La commande doit avoir une référence");
                txtbrefCommandeRevues.Focus();
            }
            else if (nudMontantCommandeRevues.Value == 0)
            {
                MessageBox.Show("Le montant ne peux être égal à 0");
                nudMontantCommandeRevues.Focus();
            }
            else
            {
                numCommandeRevue = txtbrefCommandeRevues.Text;
                montantCommandeRevue = (double)nudMontantCommandeRevues.Value;
                dtCommandeRevue = dtpCommandeAboRevues.Value;
                dtFinCommandeRevue = dtpCommandeFinAboRevues.Value;
                refRevue = txtNumeroCommandeRevues.Text;
                AbonnementRevue abonnementRevue = new AbonnementRevue (numCommandeRevue, montantCommandeRevue, dtCommandeRevue, dtFinCommandeRevue, refRevue);
                controle.AddAbonnementRevue(abonnementRevue);

                List<AbonnementRevue> lesabonnementRevue = controle.GetLesAbonnement(refRevue);
                lesabonnementRevue.Add(abonnementRevue);
                RemplirRevuesListeONCommande(lesabonnementRevue);
                    
                MessageBox.Show("Abonnement effectif jusqu'au " +dtFinCommandeRevue);
                VideRevuesCommandeZones();
              
                }
        }
            /// <summary>
            /// Evènement lors du clique sur l'annulation de demande d'abonnement à une revue
            /// Vide le champ référence de la commande
            /// réinitialise la date de fin d'abonnement à la date du jour
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnAnnulerCommandeRevues_Click(object sender, EventArgs e)
        {
            VideRevuesCommandeZones();
        }
        /// <summary>
        /// Vide les informations de la partie commandes des revues
        /// </summary>
        private void VideRevuesCommandeZones()
        {
            txtbrefCommandeRevues.Text = "";
            dtpCommandeFinAboRevues.Value = DateTime.Today;
            nudMontantCommandeRevues.Value = 0;
        }

        private void btnSupprimerCommandeRevues_Click(object sender, EventArgs e)
        {
            AbonnementRevue abonnementrevue = (AbonnementRevue)bdgListeCommandeRevues.List[bdgListeCommandeRevues.Position];
            if (controle.CheckSupprimerAbonnement(abonnementrevue))
            {
                if (MessageBox.Show("Voulez-vous vraiment supprimer cet anonnement ?", "Suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controle.SupprimerAbonnement(abonnementrevue.Id))
                    {
                        RemplirRevuesListeONCommande(lesabonnementRevue);
                    }
                    else
                    {
                        MessageBox.Show("Une erreur s'est produite.", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Impossible de supprimer cet abonnement car il est lié à des exemplaires.", "Information");
            }
        }

        #endregion

    }
}