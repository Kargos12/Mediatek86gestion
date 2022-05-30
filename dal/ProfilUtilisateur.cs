using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.dal
{
    /// <summary>
    /// Classe gérant le profil des utilisateurs
    /// </summary>
    public class ProfilUtilisateur
    {
        private readonly string id;
        private readonly string libelle;

        /// <summary>
        /// Id d'un profil utilisateur
        /// </summary>
        public string Id { get => id; }
        /// <summary>
        /// Libellé d'un profil utilisateur
        /// </summary>
        public string Libelle { get => libelle; }

        /// <summary>
        /// Constructeur : valorise les propriétés
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public ProfilUtilisateur(string id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;
        }
    }
}
