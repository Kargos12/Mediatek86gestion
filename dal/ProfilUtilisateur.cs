using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.dal
{
    public class ProfilUtilisateur
    {
        private string id;
        private string libelle;

        public string Id { get => id; }
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
