using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarteAuTresor.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor.BO.Tests
{
    [TestClass()]
    public class CarteTests
    {
        [TestMethod()]
        public void GetTresorByPositionTest()
        {
            Carte carte = new Carte();
            Tresor t1 = new Tresor();
            Tresor t2 = new Tresor();
            Position p1 = new Position(0, 0);
            t1.Position = new Position(0, 0);
            t2.Position = new Position(1, 0); ;
            List<Tresor> tresors = new List<Tresor>() { t1, t2 };
            carte.Tresors = tresors;

            Tresor value = carte.GetTresorByPosition(p1);
            Assert.AreEqual(value, t1);
        }

        [TestMethod()]
        public void GetAllPositionsTest()
        {
            Carte carte = new Carte();
            carte.Tresors.Add(new Tresor { Position = new Position(0, 0) });
            carte.Tresors.Add(new Tresor { Position = new Position(1, 0) });
            carte.Montagnes.Add(new Montagne { Position = new Position(2, 0) });
            carte.Montagnes.Add(new Montagne { Position = new Position(2, 1) });
            carte.Aventuriers.Add(new Aventurier { Position = new Position(2, 2) });
            carte.Aventuriers.Add(new Aventurier { Position = new Position(2, 3) });

            int count = carte.GetAllPositions().Count;
            Assert.AreEqual(6, count);
        }

        [TestMethod()]
        public void GetAllPositionsTresorEtMontagnesTest()
        {
            Carte carte = new Carte();
            carte.Tresors.Add(new Tresor { Position = new Position(0, 0) });
            carte.Tresors.Add(new Tresor { Position = new Position(1, 0) });
            carte.Montagnes.Add(new Montagne { Position = new Position(2, 0) });

            carte.Aventuriers.Add(new Aventurier { Position = new Position(2, 2) });
            carte.Aventuriers.Add(new Aventurier { Position = new Position(2, 3) });

            int count = carte.GetAllPositionsTresorEtMontagnes().Count;
            Assert.AreEqual(3, count);
        }

        [TestMethod()]
        public void GetAllPositionsCollisionTest()
        {
            Carte carte = new Carte();
            carte.Tresors.Add(new Tresor { Position = new Position(0, 0) });
            carte.Tresors.Add(new Tresor { Position = new Position(1, 0) });
            carte.Montagnes.Add(new Montagne { Position = new Position(2, 0) });

            carte.Aventuriers.Add(new Aventurier { Position = new Position(2, 3) });

            int count = carte.GetAllPositionsCollision().Count;
            Assert.AreEqual(2, count);
        }
    }
}