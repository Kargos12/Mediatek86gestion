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
        /// <returns>liste des commandes de documents</returns>
        public static List<CommandeDocument> GetCommandeDocument()
        {
            List<CommandeDocument> lesCommandeDocument = new List<CommandeDocument>();
            string req = "SELECT commandedocument.id, commande.dateCommande, commande.montant,commandedocument.nbExemplaire,suivi.etapes, commandeDocument.idLivreDvd";
            req +=" FROM commandedocument INNER JOIN commande ON commande.id = commandedocument.id";
            req +=" INNER JOIN suivi ON commandedocument.etatsuivi = suivi.id_suivi ";
            req +=" ORDER BY commande.dateCommande DESC";
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);
            while (curs.Read())
            {
                CommandeDocument commandeDocument = new CommandeDocument((string)curs.Field("id"), (int)curs.Field("nbExemplaire"), (double)curs.Field("montant"), (DateTime)curs.Field("dateCommande"), (string)curs.Field("etapes"), (string)curs.Field("idlivredvd"));
                lesCommandeDocument.Add(commandeDocument);
            }
            curs.Close();
            return lesCommandeDocument;
        }

        /// <summary>
        /// Ajoute une commande de livre
        /// </summary>
        /// <param name="commandeDocument"></param>
        public static void AddCommandeDocument(CommandeDocument commandeDocument)
        {
            string req = "INSERT INTO commande (id, montant, dateCommande)";
            req += "VALUES (@id, @montant, @dateCommande);";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", commandeDocument.Id);
            parameters.Add("@montant", commandeDocument.Montant);
            parameters.Add("@dateCommande", commandeDocument.DateCommande);
            BddMySql conn = BddMySql.GetInstance(connectionString);
            conn.ReqUpdate(req, parameters);

            string req2 = "INSERT INTO commandeDocument(id, nbExemplaire, etatsuivi, idLivreDvd)";
            req2 += "VALUES (@id, @nbExemplaire, @etapes, @idLivreDvd);";
            Dictionary<string, object> parameters2 = new Dictionary<string, object>();
            parameters2.Add("@id", commandeDocument.Id);
            parameters2.Add("@nbExemplaire", commandeDocument.NbExemplaire);
            parameters2.Add("@etapes", commandeDocument.Etapes);
            parameters2.Add("@idLivreDvd", commandeDocument.IdLivreDvd);
            conn.ReqUpdate(req2, parameters2);


        }
        /// <summary>
        /// Demande de modification du statut etapes d'une commande de Livre/Dvd
        /// </summary>
        /// <param name="commandeDocument"></param>
        public static void UpdateEtapes(CommandeDocument commandeDocument)
        {
            string req = "update commandedocument set etatsuivi = @etapes ";
            req += "where commandeDocument.id = @id;";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@etapes", commandeDocument.Etapes);
            parameters.Add("@id", commandeDocument.Id);
            BddMySql conn = BddMySql.GetInstance(connectionString);
            conn.ReqUpdate(req, parameters);
        }

        /// <summary>
        /// Suppression d'une commande de livres/Dvd
        /// </summary>
        /// <param name="commandeDocument">objet commande de livre à supprimer</param>
        public static void DelCommandeDocument(CommandeDocument commandeDocument)
        {
            string req ="delete from commandedocument where commandeDocument.id = @id;";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", commandeDocument.Id);
            BddMySql conn = BddMySql.GetInstance(connectionString);
            conn.ReqUpdate(req, parameters);

            string req2 = "delete from commande where commande.id = @id;";
            Dictionary<string, object> parameters2 = new Dictionary<string, object>();
            parameters2.Add("@id", commandeDocument.Id);
            conn.ReqUpdate(req2, parameters2);
        }
    }
}
