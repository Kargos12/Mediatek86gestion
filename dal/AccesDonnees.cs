using Mediatek86.bdd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.dal
{
    /// <summary>
    /// Classe permettant de gérer les demandes concernant les données distantes
    /// </summary>
    class AccesDonnees
    {

        /// <summary>
        /// chaine de connexion à la bdd
        /// </summary>
        private static string connectionString = "server=localhost;user id=root;persistsecurityinfo=True;database=mediatek86";

        /// <summary>
        /// Récupère et retourne les commandes de livres provenant de la BDD
        /// </summary>
        /// <returns>liste des commandes de livres</returns>
        public static List<CommandeLivres> GetCommandeLivres()
        {
            List<CommandeLivres> lesCommandeLivres = new List<CommandeLivres>();
            string req = "SELECT commandedocument.id, commande.dateCommande, commande.montant,commandedocument.nbExemplaire,suivi.etapes";
            req +=" FROM commandedocument INNER JOIN commande ON commande.id = commandedocument.id";
            req +=" INNER JOIN suivi ON commandedocument.etatsuivi = suivi.id_suivi ";
            req +=" ORDER BY commande.dateCommande DESC";
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);
            while (curs.Read())
            {
                CommandeLivres commandeLivres = new CommandeLivres((string)curs.Field("id"), (int)curs.Field("nbExemplaire"), (double)curs.Field("montant"), (DateTime)curs.Field("dateCommande"), (string)curs.Field("etapes"));
                lesCommandeLivres.Add(commandeLivres);
            }
            curs.Close();
            return lesCommandeLivres;
        }

        /// <summary>
        /// Ajoute une commande de livre
        /// </summary>
        /// <param name="commandeLivres"></param>
        public static void AddCommandeLivres(CommandeLivres commandeLivres)
        {
            string req = "INSERT INTO commandedocument (cd.id, cd.nbExemplaire, c.montant, c.dateCommade, s.etapes)";
            req += "SELECT cd.id, cd.nbExemplaire, c.montant, c.dateCommande, s.etapes";
            req += "FROM commandedocument cd";
            req += "INNER JOIN commande c ON cd.id = c.id";
            req += "INNER JOIN suivi s ON cd.etatsuivi = s.id_suivi";
            req += "VALUES (@id, @nbExemplaire, @montant, @dateCommande, @etapes);";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", commandeLivres.Id);
            parameters.Add("@nbExemplaire", commandeLivres.NbExemplaire);
            parameters.Add("@montant", commandeLivres.Montant);
            parameters.Add("@dateCommande", commandeLivres.DateCommande);
            parameters.Add("@etapes", commandeLivres.Etapes);
            BddMySql conn = BddMySql.GetInstance(connectionString);
            conn.ReqUpdate(req, parameters);
        }
        /// <summary>
        /// Demande de modification du statut etapes d'une commande
        /// </summary>
        /// <param name="commandeLivres"></param>
        public static void UpdateEtapes(CommandeLivres commandeLivres)
        {
            string req = "update commandedocument set etatsuivi = @etapes ";
            req += "where commandeDocument.id = @id;";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@etapes", commandeLivres.Etapes);
            parameters.Add("@id", commandeLivres.Id);
            BddMySql conn = BddMySql.GetInstance(connectionString);
            conn.ReqUpdate(req, parameters);
        }

        /// <summary>
        /// Suppression d'une commande de livres
        /// </summary>
        /// <param name="commandeLivres">objet commande de livre à supprimer</param>
        public static void DelCommandeLivres(CommandeLivres commandeLivres)
        {
            string req = "delete from commande where commande.id = @id;";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", commandeLivres.Id);
            BddMySql conn = BddMySql.GetInstance(connectionString);
            conn.ReqUpdate(req, parameters);
        }
    }
}
