using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor.BO
{
    public class Montagne : IObjetCarte
    {
        private Position position;

        public Position Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
