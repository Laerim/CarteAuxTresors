using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor.BO
{
    public class Aventurier : IObjetCarte
    {
        private Position position;

        
        public string Nom { get; set; }
        public char Orientation { get; set; }
        public string Deplacements { get; set; }
        public int TresorsRamasses { get; set; }

        public Position Position
        {
            get { return position; }
            set { position = value; }
        }

        public Position NouvellePosition()
        {
            Position newPosition = new Position (this.Position.GetAxeHorizontale(), this.Position.GetAxeVerticale());

            switch (this.Orientation)
            {
                case 'N':
                    newPosition.SetAxeVerticale(position.GetAxeVerticale() - 1);
                    break;
                case 'S':
                    newPosition.SetAxeVerticale(position.GetAxeVerticale() + 1);
                    break;
                case 'O':
                    newPosition.SetAxeHorizontale(position.GetAxeHorizontale() - 1);
                    break;
                case 'E':
                    newPosition.SetAxeHorizontale(position.GetAxeHorizontale() + 1);
                    break;

            }
            return newPosition;
        }


        /// <summary>
        /// Change l'orientation du personnage dans le sens donné
        /// </summary>
        /// <param name="cSens">'D' pour droite et 'G' pour gauche</param>
        public void ChangeDirection(char cSens)
        {
            
            switch (Orientation)
            {
                case 'N':
                    if (cSens == 'D')
                        Orientation = 'E';
                    else
                        Orientation = 'O';
                    break;
                case 'S':
                    if (cSens == 'D')
                        Orientation = 'O';
                    else
                        Orientation = 'E';
                    break;
                case 'O':
                    if (cSens == 'D')
                        Orientation = 'N';
                    else
                        Orientation = 'S';
                    break;
                case 'E':
                    if (cSens == 'D')
                        Orientation = 'S';
                    else
                        Orientation = 'N';
                    break;
            }
        }
        public void RamasserTresor(bool bResteTresor)
        {
            if(bResteTresor)
                this.TresorsRamasses++;
        }
    }
}
