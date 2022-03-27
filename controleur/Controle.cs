using System.Collections.Generic;
using Mediatek86.modele;
using Mediatek86.metier;
using Mediatek86.vue;
using Mediatek86.dal;
using System;

namespace Mediatek86.controleur
{
    internal class Controle
    {
        private readonly List<Livre> lesLivres;
        private readonly List<Dvd> lesDvd;
        private readonly List<Revue> lesRevues;
        private readonly List<Categorie> lesRayons;
        private readonly List<Categorie> lesPublics;
        private readonly List<Categorie> lesGenres;
        /// <summary>
        /// fenêtre d'authentification
        /// </summary>
        private FormAuthentification formAuthentification;

        /// <summary>
        /// Ouverture de la fenêtre
        /// </summary>
        public Controle()
        {
            lesLivres = Dao.GetAllLivres();
            lesDvd = Dao.GetAllDvd();
            lesRevues = Dao.GetAllRevues();
            lesGenres = Dao.GetAllGenres();
            lesRayons = Dao.GetAllRayons();
            lesPublics = Dao.GetAllPublics();
            FrmMediatek frmMediatek = new FrmMediatek(this);
            frmMediatek.ShowDialog();
            formAuthentification = new FormAuthentification();
            formAuthentification.ShowDialog();
        }
        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Collection d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return lesGenres;
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Collection d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return lesLivres;
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Collection d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return lesDvd;
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Collection d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return lesRevues;
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return lesRayons;
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return lesPublics;
        }

        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <returns>Collection d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return Dao.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return Dao.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Récupère et retourne les infos des commandes de livres provenant de la BDD
        /// </summary>
        /// <returns>liste des commandes de livres</returns>
        public List<CommandeDocument> GetLesCommandeDocument()
        {
            return AccesDonnees.GetCommandeDocument();
        }

        /// <summary>
        /// Demande d'ajout d'une commande de livres/dvds
        /// </summary>
        /// <param name="commandeDocument"></param>
        public void AddCommandeDocument(CommandeDocument commandeDocument)
        {
            AccesDonnees.AddCommandeDocument(commandeDocument);
        }
        /// <summary>
        /// Demande de changement du statut etapes d'une commande de livres/dvds
        /// </summary>
        /// <param name="commandeDocument"></param>
        public void UpdateEtapes(CommandeDocument commandeDocument)
        {
            AccesDonnees.UpdateEtapes(commandeDocument);
        }
        /// <summary>
        /// Demande de suppression d'une commande de livres/dvds
        /// </summary>
        /// <param name="commandeDocument">objet developpeur à supprimer</param>
        public void DelCommandeDocument(CommandeDocument commandeDocument)
        {
            AccesDonnees.DelCommandeDocument(commandeDocument);
        }
        /// <summary>
        /// Demande d'ajout d'abonnement à une revue
        /// </summary>
        /// <param name="abonnementRevue"></param>
        public void AddAbonnementRevue(AbonnementRevue abonnementRevue)
        {
            AccesDonnees.AddAbonnementRevue(abonnementRevue);
        }

        /// <summary>
        /// Demande la vérification de l'authentification
        /// Si correct, alors ouvre la fenêtre principale
        /// </summary>
        /// <param name="Identifiant"></param>
        /// <param name="MotDePasse"></param>
        /// <returns></returns>
        public Boolean ControleAuthentification(string Identifiant, string MotDePasse)
        {
            if (AccesDonnees.ControleAuthentification(Identifiant,MotDePasse))
            {
                formAuthentification.Hide();
                (new FrmMediatek(this)).ShowDialog();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}

