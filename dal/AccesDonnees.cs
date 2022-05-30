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
        /// Récupère et retourne les commandes de livres ou de dvds provenant de la BDD
        /// </summary>
        /// <returns>liste des commandes de documents</returns>
        public static List<CommandeDocument> GetCommandeDocument()
        {
            List<CommandeDocument> lesCommandeDocument = new List<CommandeDocument>();
            string req = "SELECT commandedocument.id, commande.dateCommande, commande.montant,commandedocument.nbExemplaire,suivi.etapes, commandeDocument.idLivreDvd";
            req += " FROM commandedocument INNER JOIN commande ON commande.id = commandedocument.id";
            req += " INNER JOIN suivi ON commandedocument.etatsuivi = suivi.id_suivi ";
            req += " ORDER BY commande.dateCommande DESC";
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
            string req = "delete from commandedocument where commandeDocument.id = @id;";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", commandeDocument.Id);
            BddMySql conn = BddMySql.GetInstance(connectionString);
            conn.ReqUpdate(req, parameters);

            string req2 = "delete from commande where commande.id = @id;";
            Dictionary<string, object> parameters2 = new Dictionary<string, object>();
            parameters2.Add("@id", commandeDocument.Id);
            conn.ReqUpdate(req2, parameters2);
        }
        /// <summary>
        /// Ajoute un abonnement à une revue
        /// </summary>
        /// <param name="abonnementRevue"></param>
        public static void AddAbonnementRevue(AbonnementRevue abonnementRevue)
        {
            string req = "INSERT INTO commande (id, montant, dateCommande)";
            req += "VALUES (@id, @montant, @dateCommande);";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", abonnementRevue.Id);
            parameters.Add("@montant", abonnementRevue.Montant);
            parameters.Add("@dateCommande", abonnementRevue.DateCommande);
            BddMySql conn = BddMySql.GetInstance(connectionString);
            conn.ReqUpdate(req, parameters);

            string req2 = "INSERT INTO abonnement(id, dateFinAbonnement, idRevue)";
            req2 += "VALUES (@id, @dateFinAbonnement, @idRevue);";
            Dictionary<string, object> parameters2 = new Dictionary<string, object>();
            parameters2.Add("@id", abonnementRevue.Id);
            parameters2.Add("@dateFinAbonnement", abonnementRevue.DateFinAbonnement);
            parameters2.Add("@idRevue", abonnementRevue.IdRevue);
            conn.ReqUpdate(req2, parameters2);
        }

        /// <summary>
        /// Récupère et retourne les abonnement à une revue provenant de la BDD
        /// </summary>
        /// <param name="refRevue"> référence de la revue </param>
        /// <returns>liste des abonnements à une revue</returns>
        public static List<AbonnementRevue> GetAbonnement(string refRevue)
        {
            List<AbonnementRevue> lesabonnementRevue = new List<AbonnementRevue>();
            string req = "SELECT commande.id, commande.montant, commande.dateCommande,abonnement.dateFinAbonnement,abonnement.idRevue";
            req += " FROM commande INNER JOIN abonnement ON commande.id = abonnement.id";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", refRevue}
                };

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);
            while (curs.Read())
            {
                AbonnementRevue abonnementRevues = new AbonnementRevue((string)curs.Field("id"), (double)curs.Field("montant"), (DateTime)curs.Field("dateCommande"), (DateTime)curs.Field("dateFinAbonnement"), (string)curs.Field("idRevue"));
                lesabonnementRevue.Add(abonnementRevues);
            }
            curs.Close();
            return lesabonnementRevue;
        }


        /// <summary>
        /// Controle si l'utillisateur a le droit de se connecter (identifiant, mot de passe)
        /// </summary>
        /// <param name="Identifiant"></param>
        /// <param name="MotDePasse"></param>
        /// <returns></returns>
        public static Boolean ControleAuthentification(string Identifiant, string MotDePasse)
        {
            string req = "select * from utilisateur u join service s on u.idservice=s.id ";
            req += "where u.identifiant=@identifiant and u.motdepasse=@motdepasse";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Identifiant", Identifiant);
            parameters.Add("@MotDePasse", MotDePasse);

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);
            if (curs.Read())
            {
                curs.Close();
                return true;
            }
            else
            {
                curs.Close();
                return false;
            }
        }

        /// <summary>
        /// Retourne le service d'un utilisateur
        /// </summary>
        /// <param name="identifiant">Le nom d'utilisateur concerné</param>
        /// <param name="motdepasse">Le mot de passe de l'utilisateur</param>
        /// <returns>Le service si l'utilisateur est trouvé dans la bdd, sinon null</returns>
        public static ProfilUtilisateur Authentification(string identifiant, string motdepasse)
        {
            ProfilUtilisateur profilUtilisateur = null;
            string req = "select s.id, s.libelle from utilisateur u join service s on u.idservice = s.id where u.identifiant = @identifiant and u.motdepasse = @motdepasse;";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@identifiant", identifiant},
                    {"@motdepasse", motdepasse}
                };
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);

            while (curs.Read())
            {
                profilUtilisateur = new ProfilUtilisateur((string)curs.Field("id"), (string)curs.Field("libelle"));
            }
            curs.Close();
            return profilUtilisateur;
        }

    }
    
}

        
    
