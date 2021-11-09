using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor.BO
{
    public interface IObjetCarte
    {

        Position Position { get; set; }
       
    }
    public class Position
    {
        private int iAxeHorizontale;
        private int iAxeVerticale;

        public int GetAxeHorizontale()
        {
            return iAxeHorizontale;
        }
        public int GetAxeVerticale()
        {
            return iAxeVerticale;
        }
        public void SetAxeHorizontale(int iAxe)
        {
            this.iAxeHorizontale = iAxe;
        }
        public void SetAxeVerticale(int iAxe)
        {
            this.iAxeVerticale = iAxe;
        }

        public Position(int iHorizontale, int iVerticale)
        {
            iAxeHorizontale = iHorizontale;
            iAxeVerticale = iVerticale;
        }

        public bool IsNegative()
        {
            if (iAxeHorizontale < 0 || iAxeVerticale < 0)
                return true;
            else
                return false;
        }
        public override bool Equals(object obj)
        {
            var other = obj as Position;

            if (other == null)
                return false;

            if (iAxeHorizontale != other.iAxeHorizontale || iAxeVerticale != other.iAxeVerticale)
                return false;

            return true;
        }
        public override string ToString()
        {
            return iAxeHorizontale + " - " + iAxeVerticale;
        }
    }
}
