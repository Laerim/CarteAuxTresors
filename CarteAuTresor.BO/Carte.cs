using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor.BO
{
    public class Carte : ICloneable
    {
        public int Largeur { get; set; }
        public int Hauteur { get; set; }
        public List<Montagne> Montagnes { get; set; }
        public List<Aventurier> Aventuriers { get; set; }
        public List<Tresor> Tresors { get; set; }

       
        public Carte()
        {
            Montagnes = new List<Montagne>();
            Aventuriers = new List<Aventurier>();
            Tresors = new List<Tresor>();

        }
        public Tresor GetTresorByPosition(Position position)
        {
            var tresor = Tresors.Find(t => t.Position.Equals(position));
            return tresor;
        }
        public List<Position> GetAllPositions()
        {
            return this.Montagnes.Select(m => m.Position).ToList()
                                            .Concat(this.Aventuriers.Select(m => m.Position).ToList())
                                            .Concat(this.Tresors.Select(t => t.Position).ToList()).ToList();
        }
        public List<Position> GetAllPositionsCollision()
        {
            return this.Montagnes.Select(m => m.Position).ToList()
                                 .Concat(this.Aventuriers.Select(m => m.Position).ToList()).ToList();
        }
        /// <summary>
        /// Renvoie les positions de toutes les montagnes et les tresors. Ne prend pas en compte les aventuriers car un trésor peut être enterré sous ses pieds.
        /// </summary>
        /// <returns></returns>
        public List<Position> GetAllPositionsTresorEtMontagnes()
        {
            return this.Montagnes.Select(m => m.Position).ToList()
                                 .Concat(this.Tresors.Select(m => m.Position).ToList()).ToList();
        }
        
        public object Clone()
        {
            Carte newCarte = new Carte
            {
                Largeur = this.Largeur,
                Hauteur = this.Hauteur,
                Montagnes = new List<Montagne>(),
                Aventuriers = new List<Aventurier>(),
                Tresors = new List<Tresor>()
            };
            foreach (Aventurier aventurier in this.Aventuriers)
            {
                newCarte.Aventuriers.Add(new Aventurier
                {

                    Nom = aventurier.Nom,
                    Orientation = aventurier.Orientation,
                    Deplacements = aventurier.Deplacements,
                    TresorsRamasses = aventurier.TresorsRamasses,
                    Position = new Position(aventurier.Position.GetAxeHorizontale(), aventurier.Position.GetAxeVerticale())
                });
            }
            foreach (Montagne montagne in this.Montagnes)
                newCarte.Montagnes.Add(new Montagne
                {
                    Position = new Position(montagne.Position.GetAxeHorizontale(), montagne.Position.GetAxeVerticale())
                });
            foreach (Tresor tresor in this.Tresors)
                newCarte.Tresors.Add(new Tresor
                {
                    Position = new Position(tresor.Position.GetAxeHorizontale(), tresor.Position.GetAxeVerticale()),
                    NombreDeTresor = tresor.NombreDeTresor
                });
            return newCarte;
        }
    }
}
