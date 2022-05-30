using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe gérant le public d'un document
    /// </summary>
    public class Public : Categorie
    {
        /// <summary>
        /// Détail du public d'un document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Public(string id, string libelle):base(id, libelle)
        {
        }

    }
}
