using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CarteAuTresor.BO;

namespace CarteAuTresor.BLL
{
    public  class CarteAuTresorManager
    {
        #region ATTRIBUTS

        private static CarteAuTresorManager instance = null;
        private static readonly object myLock = new object();



        #endregion

        #region ACCESSEURS


        public static CarteAuTresorManager GetInstance
        {
            get
            {
                lock (myLock)
                {
                    if (instance == null)
                        instance = new CarteAuTresorManager();
                    return instance;
                }

            }
        }

        #endregion

        #region METHODES
        public List<string> ReadFile(string sChemin)
        {
            List<string> lines = new List<string>();
            if (File.Exists(sChemin.Trim()))
            {
                string line;
                using (StreamReader reader = new StreamReader(sChemin.Trim()))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line.Trim());
                    }
                    reader.Dispose();
                    reader.Close();
                }
                return lines;
            }
            else
                throw new Exception("Aucun fichier n'est disponible à ce lien");
        }
        /// <summary>
        /// Créer un objet "Carte" à partir d'un ensemble de lignes
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public Carte ChargerCarte(List<string> lines)
        {
            Carte carte = new Carte();
            List<Position> positions = new List<Position>();
            string lineCarte = lines.Find(l=> l.Substring(0,1)== "C");
            if (lineCarte != null)
            {
                string[] arrayFirstLine = lineCarte.Split('-');
                int largeur = 0;
                int hauteur = 0;
                if (!int.TryParse(arrayFirstLine[1].Trim(), out largeur))
                    throw new Exception("La largeur de la carte n'est pas un entier numérique. Impossible de traiter le fichier.\nErreur à la ligne ");
                if (!int.TryParse(arrayFirstLine[2].Trim(), out hauteur))
                    throw new Exception("La hauteur de la carte n'est pas un entier numérique. Impossible de traiter le fichier.\nErreur à la ligne ");
                carte.Largeur = largeur;
                carte.Hauteur = hauteur;

            }
            else
                throw new Exception("Il n'y a pas de pas de ligne 'Carte' dans le fichier. Impossible de traiter le fichier.\nErreur à la ligne ");
            lines.Remove(lineCarte);

            int indLine = 1;
                
                    foreach (string line in lines)
                    {
                        string[] array = line.Split('-');
                        switch (array[0].Trim())
                        {
                    case "C":
                        throw new Exception("Il y a plusieurs cartes décrites dans le fichier. Impossible de traiter le fichier.\nErreur à la ligne ");
                            case "M":
                                var montagne = new Montagne();
                                try
                                {
                                    montagne.Position = AddPosition(array[1].Trim(), array[2].Trim(), carte);
                                    if (positions.Contains(montagne.Position))
                                        throw new Exception("Deux objets se superposent. Impossible de traiter le fichier.\nErreur à la ligne " + indLine);
                                    positions.Add(montagne.Position);
                                    carte.Montagnes.Add(montagne);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message + indLine.ToString());
                                }
                                
                                break;
                            case "T":
                                var tresor = new Tresor();
                                try
                                {
                                    tresor.Position = AddPosition( array[1].Trim(), array[2].Trim(), carte);
                                    if (positions.Contains(tresor.Position))
                                        throw new Exception("Deux objets se superposent. Impossible de traiter le fichier.\nErreur à la ligne " );
                                    int iNumber = 0;
                                    if (!int.TryParse(array[3].Trim(), out iNumber))
                                        throw new Exception("Le nombre de trésors n'est pas un entier numérique. Impossible de traiter le fichier.\nErreur à la ligne " );
                                    tresor.NombreDeTresor = iNumber;
                                    carte.Tresors.Add(tresor);
                                }
                                catch (Exception ex)
                                {
                                    
                                    throw new Exception(ex.Message+indLine.ToString());
                                }
                                break;
                            case "A":
                                var aventurier = new Aventurier();
                                try
                                {
                                    aventurier.Position = AddPosition(array[2].Trim(), array[3].Trim(), carte);
                                    if (positions.Contains(aventurier.Position))
                                        throw new Exception("Deux objets se superposent. Impossible de traiter le fichier.\nErreur à la ligne ");
                                    int iNumber = 0;
                                    if (!int.TryParse(array[3].Trim(), out iNumber))
                                        throw new Exception("Le nombre de trésors n'est pas un entier numérique. Impossible de traiter le fichier.\nErreur à la ligne " );
                                    aventurier.TresorsRamasses = 0;

                                    char[] orientation = { 'N', 'S', 'O', 'E' };
                                    char[] deplacement = { 'A', 'G', 'D' };
                                    if (!CaracteresAutorises(orientation, array[4].Trim().ToCharArray()))
                                        throw new Exception("L'orientation de l'aventurier est incorrect. Impossible de traiter le fichier.\nErreur à la ligne " );
                                    if (!CaracteresAutorises(deplacement, array[5].Trim().ToCharArray()))
                                        throw new Exception("Des déplacements inexistants de l'aventurier sont inscrits. Impossible de traiter le fichier.\nErreur à la ligne " );
                                    aventurier.Nom = array[1].Trim();
                                    aventurier.Orientation = Convert.ToChar(array[4].Trim());
                                    aventurier.Deplacements = array[5].Trim();
                                    carte.Aventuriers.Add(aventurier);


                                }
                                catch (Exception ex)
                                {

                                    throw new Exception(ex.Message + indLine.ToString());
                                }
                                break;

                        }
                        indLine++;
                    
                }
                return carte;
            }
           
        

        public Carte Traitement(Carte entree)
        {
            Carte sortie = (Carte)entree.Clone() ;

            int iNbTourMax = sortie.Aventuriers.Max(a => a.Deplacements.Length);
            for (int i = 1; i <= iNbTourMax; i++)
            {
                foreach (Aventurier aventurier in sortie.Aventuriers)
                {
                    if (aventurier.Deplacements.Length >= i)
                    {
                        char action = Convert.ToChar(aventurier.Deplacements.Substring(i - 1, 1));
                        if (action == 'A')
                        {
                            Position position = aventurier.NouvellePosition();
                            List<Position> positions = sortie.GetAllPositionsCollision();
                            if (!CheckCollision(positions, position) && !position.IsNegative())
                            {
                                aventurier.Position = position;
                                var tresor = sortie.GetTresorByPosition(position);
                                if (tresor != null)
                                {
                                    bool ramasser = tresor.RamasserTresor();
                                    aventurier.RamasserTresor(ramasser);
                                }
                            }
                        }
                        else
                            aventurier.ChangeDirection(action);
                    }
                }
            }
            return sortie;
        }
        
        
        /// <summary>
        /// Return true si une collision a lieu
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="positionATester"></param>
        /// <returns></returns>
        public bool CheckCollision(List<Position> positions, Position positionATester)
        {
            return positions.Contains(positionATester);
        }
        public Position AddPosition( string sHorizontal, string sVertical, Carte carte)
        {
          
            int iHorizontal = 0;
            int iVertical = 0;
            if (!int.TryParse(sHorizontal.Trim(), out iHorizontal))
                throw new Exception("L'axe horizontale de l'objet n'est pas un entier numérique. Impossible de traiter le fichier.\nErreur à la ligne ");
            if (!int.TryParse(sVertical.Trim(), out iVertical))
                throw new Exception("L'axe verticale de l'objet n'est pas un entier numérique. Impossible de traiter le fichier.\nErreur à la ligne ");
            Position position = new Position(iHorizontal, iVertical);
            if (position.IsNegative())
                throw new Exception("Une position enregistrée est négative. Impossible de traiter le fichier.\nErreur à la ligne");
            if(position.GetAxeVerticale() >= carte.Hauteur || position.GetAxeHorizontale() >= carte.Largeur)
                throw new Exception("Un des axes dépasse les limites de la carte. Impossible de traiter le fichier.\nErreur à la ligne");

            return position;
           
        }

        public bool CaracteresAutorises(char[] arrayAutorise, char[] arrayTest )
        {
            bool ok = true;
            arrayTest = arrayTest.Distinct().ToArray(); //Retire les doublons
            foreach (char value in arrayTest)
            {
                if(!arrayAutorise.Contains(value))
                {
                    ok = false;
                    break;
                }
            }
            return ok;
        }


        #region METHODES POUR LA GUI

        public List<string> ReadCarte(Carte carte)
        {
            List<string> lines = new List<string>();
            lines.Add( String.Join(" - ", "C", carte.Largeur, carte.Hauteur).Trim() );
            foreach (Montagne montagne in carte.Montagnes)
                lines.Add(String.Join(" - ", "M", montagne.Position.ToString()).Trim() );
            foreach (Tresor tresor in carte.Tresors)
                lines.Add(String.Join(" - ", "T", tresor.Position.ToString(), tresor.NombreDeTresor).Trim());
            foreach (Aventurier aventurier in carte.Aventuriers)
                lines.Add(String.Join(" - ", "A", aventurier.Nom, aventurier.Position.ToString(), aventurier.Orientation, aventurier.Deplacements).Trim() );

            return lines;
        }
        public List<string> ReadSortie(Carte carte)
        {
            List<string> lines = new List<string>();
            lines.Add(String.Join(" - ", "C", carte.Largeur, carte.Hauteur).Trim());
            foreach (Montagne montagne in carte.Montagnes)
                lines.Add(String.Join(" - ", "M", montagne.Position.ToString()).Trim());
            foreach (Tresor tresor in carte.Tresors)
            {
                if(tresor.NombreDeTresor >0)
                    lines.Add(String.Join(" - ", "T", tresor.Position.ToString(), tresor.NombreDeTresor).Trim());
            }
            foreach (Aventurier aventurier in carte.Aventuriers)
                lines.Add(String.Join(" - ", "A", aventurier.Nom, aventurier.Position.ToString(), aventurier.Orientation, aventurier.TresorsRamasses).Trim());

            return lines;
        }

        #endregion


        #endregion
    }
}
