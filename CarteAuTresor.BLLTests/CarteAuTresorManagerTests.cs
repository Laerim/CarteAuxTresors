using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarteAuTresor.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarteAuTresor.BO;
using System.IO;


namespace CarteAuTresor.BLL.Tests
{
    [TestClass()]
    public class CarteAuTresorManagerTests
    {


        [TestMethod()]
        public void CheckCollisionTest_Collision()
        {

            Position p1 = new Position(0, 0);
            List<Position> positions = new List<Position>() { new Position(0, 0), new Position(1, 0) };
            bool test = CarteAuTresorManager.GetInstance.CheckCollision(positions, p1);
            Assert.IsTrue(test);
        }
        [TestMethod()]
        public void CheckCollisionTest_NoCollision()
        {

            Position p1 = new Position(5, 5);
            List<Position> positions = new List<Position>() { new Position(0, 0), new Position(1, 0) };
            bool test = CarteAuTresorManager.GetInstance.CheckCollision(positions, p1);
            Assert.IsFalse(test);
        }

        [TestMethod()]
        public void TraitementTest_Deplacement()
        {
            Carte carte = new Carte();
            carte.Montagnes = new List<Montagne>();
            carte.Tresors = new List<Tresor>();
            Aventurier aventurier = new Aventurier();
            aventurier.Orientation = 'S';
            aventurier.Deplacements = "AA";
            aventurier.Position = new Position(0, 2);
            carte.Aventuriers = new List<Aventurier>() { aventurier };
            Carte sortie = CarteAuTresorManager.GetInstance.Traitement(carte);

            Assert.AreEqual(sortie.Aventuriers[0].Position.GetAxeVerticale(), 4);
            Assert.AreNotEqual(carte.Aventuriers[0].Position.GetAxeVerticale(), sortie.Aventuriers[0].Position.GetAxeVerticale());
        }
        [TestMethod()]
        public void TraitementTest_Collision_Montagne()
        {
            Carte carte = new Carte();
            carte.Montagnes = new List<Montagne>();
            carte.Tresors = new List<Tresor>();

            Aventurier aventurier = new Aventurier();
            aventurier.Orientation = 'S';
            aventurier.Deplacements = "AADA";
            aventurier.Position = new Position(0, 2);
            carte.Aventuriers = new List<Aventurier>() { aventurier };
            carte.Montagnes.Add(new Montagne { Position = new Position(0, 4) });
            Carte sortie = CarteAuTresorManager.GetInstance.Traitement(carte);

            Assert.AreEqual(3, sortie.Aventuriers[0].Position.GetAxeVerticale());
            Assert.AreEqual(0, sortie.Aventuriers[0].Position.GetAxeHorizontale());

        }
        [TestMethod()]
        public void TraitementTest_Ramasser_Tresor()
        {
            Carte carte = new Carte();
            carte.Montagnes = new List<Montagne>();
            carte.Tresors = new List<Tresor>();

            Aventurier aventurier = new Aventurier();
            aventurier.Orientation = 'S';
            aventurier.Deplacements = "AADA";
            aventurier.Position = new Position(0, 2);
            carte.Aventuriers = new List<Aventurier>() { aventurier };
            carte.Tresors.Add(new Tresor { Position = new Position(0, 4), NombreDeTresor = 2 });
            Carte sortie = CarteAuTresorManager.GetInstance.Traitement(carte);

            Assert.AreEqual(4, sortie.Aventuriers[0].Position.GetAxeVerticale());
            Assert.AreEqual(0, sortie.Aventuriers[0].Position.GetAxeHorizontale());
            Assert.AreEqual(1, sortie.Tresors[0].NombreDeTresor);

        }

        [TestMethod()]
        public void TraitementTest_Plusieurs_Aventuriers()
        {
            Carte carte = new Carte();
            carte.Hauteur = 9;
            carte.Largeur = 9;
            carte.Montagnes = new List<Montagne>();
            carte.Tresors = new List<Tresor>();

            Aventurier a1 = new Aventurier();
            a1.Orientation = 'S';
            a1.Deplacements = "A";
            a1.Nom = "A1";
            a1.Position = new Position(0, 2);
            Aventurier a2 = new Aventurier();
            a2.Orientation = 'S';
            a2.Deplacements = "AA";
            a2.Nom = "A2";
            a2.Position = new Position(0, 1);
            carte.Aventuriers = new List<Aventurier>() { a1, a2 };
            Carte sortie = CarteAuTresorManager.GetInstance.Traitement(carte);

            Assert.AreEqual(3, sortie.Aventuriers[0].Position.GetAxeVerticale());
            Assert.AreEqual(0, sortie.Aventuriers[0].Position.GetAxeHorizontale());
            Assert.AreEqual(2, sortie.Aventuriers[1].Position.GetAxeVerticale());
            Assert.AreEqual(0, sortie.Aventuriers[1].Position.GetAxeHorizontale());

        }

        

     

        [TestMethod()]
        public void CheckCollisionTest()
        {
            List<Position> positions = new List<Position>() { new Position(0, 0), new Position(1, 0) };
            bool collision = CarteAuTresorManager.GetInstance.CheckCollision(positions, new Position(0, 0));
            bool no_collision = CarteAuTresorManager.GetInstance.CheckCollision(positions, new Position(9, 9));
            Assert.AreEqual(true, collision);
            Assert.AreEqual(false, no_collision);

        }

        [TestMethod()]
        [ExpectedException(typeof(Exception), "L'axe horizontale de l'objet n'est pas un entier numérique. Impossible de traiter le fichier.\nErreur à la ligne ")]
        public void AddPositionAxeHorizontaleFail()
        {
            Carte carte = new Carte { Largeur = 3, Hauteur = 3 };
            Position position = CarteAuTresorManager.GetInstance.AddPosition("s", "1", carte);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception), "L'axe verticale de l'objet n'est pas un entier numérique. Impossible de traiter le fichier.\nErreur à la ligne ")]
        public void AddPositionAxeVerticaleFail()
        {
            Carte carte = new Carte { Largeur = 3, Hauteur = 3 };
            Position position = CarteAuTresorManager.GetInstance.AddPosition("1", "po", carte);
        }
        [TestMethod()]
        [ExpectedException(typeof(Exception), "Un des axes dépasse les limites de la carte. Impossible de traiter le fichier.\nErreur à la ligne ")]
        public void AddPositionDepasseLaCarte()
        {
            Carte carte = new Carte { Largeur = 3, Hauteur = 3 };
            Position position = CarteAuTresorManager.GetInstance.AddPosition("4", "0", carte);
        }
        [TestMethod()]
        public void AddPosition_OK()
        {
            Carte carte = new Carte { Largeur = 3, Hauteur = 3 };
            Position position = CarteAuTresorManager.GetInstance.AddPosition("2", "0", carte);
        }

        [TestMethod()]
        public void ReadCarteTest()
        {
            Carte carte = new Carte();
            carte.Hauteur = 9;
            carte.Largeur = 9;
            carte.Montagnes.Add(new Montagne { Position = new Position(1, 1) });
            carte.Tresors.Add(new Tresor { Position = new Position(1, 2), NombreDeTresor = 0 });
            carte.Tresors.Add(new Tresor { Position = new Position(1, 3), NombreDeTresor = 1 });
            carte.Aventuriers.Add(new Aventurier
            {
                Position = new Position(0, 0),
                Deplacements = "AAAGA",
                Orientation = 'S',
                Nom = "Lara"
            });
            List<string> lignes = CarteAuTresorManager.GetInstance.ReadCarte(carte);
            Assert.AreEqual(5, lignes.Count);
            Assert.AreEqual("C - 9 - 9", lignes[0].Trim());
            Assert.AreEqual("A - Lara - 0 - 0 - S - AAAGA", lignes.Last().Trim());
        }

        [TestMethod()]
        public void ReadSortieTest()
        {
            Carte carte = new Carte();
            carte.Hauteur = 9;
            carte.Largeur = 9;
            carte.Montagnes.Add(new Montagne { Position = new Position(1, 1) });
            carte.Tresors.Add(new Tresor { Position = new Position(1, 2), NombreDeTresor=0 });
            carte.Tresors.Add(new Tresor { Position = new Position(1, 3), NombreDeTresor = 1 });
            carte.Aventuriers.Add(new Aventurier {
                Position = new Position(0, 0),
                TresorsRamasses = 8,
                Orientation = 'S',
                Nom = "Lara"
            });
            List<string> lignes = CarteAuTresorManager.GetInstance.ReadSortie(carte);
            Assert.AreEqual(4, lignes.Count);
            Assert.AreEqual("C - 9 - 9", lignes[0].Trim());
            Assert.AreEqual("A - Lara - 0 - 0 - S - 8", lignes.Last().Trim());
        }

        [TestMethod()]
        public void CaracteresAutorisesTest_OK()
        {
            char[] allowed = { 'A', 'Z' };
            string test = "AAAAZZZA";
            bool check = CarteAuTresorManager.GetInstance.CaracteresAutorises(allowed, test.ToCharArray());
            Assert.AreEqual(true, check);
        }
        [TestMethod()]
        public void CaracteresAutorisesTest_FAIL()
        {
            char[] allowed = { 'A', 'Z' };
            string test = "AAAAZZDZA";
            bool check = CarteAuTresorManager.GetInstance.CaracteresAutorises(allowed, test.ToCharArray());
            Assert.AreEqual(false, check);
        }

      
    }
}