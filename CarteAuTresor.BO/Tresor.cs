using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor.BO
{
    public class Tresor : IObjetCarte
    {
        private Position position;
        public int NombreDeTresor { get; set; }

        
        public Position Position
        {
            get { return position; }
            set { position = value; }
        }

   

       
        
        public bool RamasserTresor()
        {
            if (NombreDeTresor > 0)
            {
                this.NombreDeTresor--;
                return true;
            }
            else
                return false;
        }
    }
}
