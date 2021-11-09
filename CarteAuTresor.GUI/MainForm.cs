using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarteAuTresor.BO;
using CarteAuTresor.BLL;
using System.IO;

namespace CarteAuTresor.GUI
{
    public partial class MainForm : Form
    {
        CarteAuTresorManager instance = CarteAuTresorManager.GetInstance;
        private Carte carteEntree = new Carte();
        Montagne montagneEnCours;
        Tresor tresorEnCours;
        Aventurier aventurierEnCours;
        public MainForm()
        {
            InitializeComponent();
        }


        private void LoadCarte()
        {
            lsbCarte.DataSource = null;

            lsbCarte.Items.Clear();
            lsbCarte.DataSource = instance.ReadCarte(carteEntree);

            numHorizontalMontagne.Maximum = carteEntree.Largeur;
            numVerticalMontagne.Maximum = carteEntree.Hauteur;
            numHorizontalAventurier.Maximum = carteEntree.Largeur;
            numVerticalAventurier.Maximum = carteEntree.Hauteur;
            numHorizontalTresor.Maximum = carteEntree.Largeur;
            numVerticalTresor.Maximum = carteEntree.Hauteur;

            NewTresor();
            NewMontagne();
            NewAventurier();
        }
        private void btnChargerCarte_Click(object sender, EventArgs e)
        {
            try
            {
                carteEntree = instance.ChargerCarte(txtFile.Text.Trim());
                LoadCarte();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ".:: ERREUR ::.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private void rdbMontagne_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbMontagne.Checked == true)
            {
                TabControl.SelectedTab = tabPageMontagnes;
                lsbCarte.SelectedIndex = -1;
            }
        }

        private void rdbTresor_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbTresor.Checked == true)
            {
                TabControl.SelectedTab = tabPageTresors;
                lsbCarte.SelectedIndex = -1;
            }

        }

        private void rdbAventurier_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAventurier.Checked == true)
            {
                TabControl.SelectedTab = tabPageAventuriers;
                lsbCarte.SelectedIndex = -1;
            }
        }

        private void lsbCarte_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnTraitement_Click(object sender, EventArgs e)
        {
            try
            {
                Carte sortie = instance.Traitement(carteEntree);
                lsbSortie.DataSource = null;
                lsbSortie.Items.Clear();
                lsbSortie.DataSource = instance.ReadSortie(sortie);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Impossible de traiter la carte :\n" + ex.Message, ".:: ERREUR ::.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Fichiers texte (*.txt)|*.txt";
            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                txtFile.Text = openFileDialog.FileName;
                
            }
        }
        private void NewMontagne()
        {
            numHorizontalMontagne.Value = 0;
            numVerticalMontagne.Value = 0;
            btnSupprimerMontagne.Enabled = false;
            montagneEnCours = null;
        }

        private void btnNouveauMontagne_Click(object sender, EventArgs e)
        {
            NewMontagne();            
        }

        private void btnSaveMontagne_Click(object sender, EventArgs e)
        {
            Position newPosition = new Position(Decimal.ToInt32(numHorizontalMontagne.Value), Decimal.ToInt32(numVerticalMontagne.Value));
            bool bCollision = false;
            if (!newPosition.Equals(montagneEnCours.Position))
                bCollision = instance.CheckCollision(carteEntree.GetAllPositions(), newPosition);
            if (bCollision)
                MessageBox.Show("Une collision va avoir lieu avec ces positions. Impossible d'enregistrer la montagne");
            else
            {
                if (montagneEnCours != null)
                    montagneEnCours.Position = newPosition;

                else
                    carteEntree.Montagnes.Add(new Montagne
                    {
                        Position = newPosition
                    });
                LoadCarte();
            }
        }

        private void btnSupprimerMontagne_Click(object sender, EventArgs e)
        {
            if (montagneEnCours != null)
                carteEntree.Montagnes.Remove(montagneEnCours);

            LoadCarte();
            
        }

        private void NewTresor()
        {
            numHorizontalTresor.Value = 0;
            numVerticalTresor.Value = 0;
            numNombreTresors.Value = 0;
            btnSupprimerTresor.Enabled = false;
            
            tresorEnCours = null;
        }
        private void btnNouveauTresor_Click(object sender, EventArgs e)
        {
            NewTresor();
        }

        private void btnSaveTresor_Click(object sender, EventArgs e)
        {
            Position newPosition = new Position(Decimal.ToInt32(numHorizontalTresor.Value), Decimal.ToInt32(numVerticalTresor.Value));
            bool bCollision = false;
            if (tresorEnCours== null || !newPosition.Equals(tresorEnCours.Position))
                bCollision = instance.CheckCollision(carteEntree.GetAllPositionsTresorEtMontagnes(), newPosition);
            if (bCollision)
                MessageBox.Show("Une collision va avoir lieu avec ces positions. Impossible d'enregistrer le trésor");
            else
            {
                if (tresorEnCours != null)
                {
                    tresorEnCours.Position = newPosition;
                    tresorEnCours.NombreDeTresor = Decimal.ToInt32(numNombreTresors.Value);
                }

                else
                    carteEntree.Tresors.Add(new Tresor
                    {
                        Position = new Position(Decimal.ToInt32(numHorizontalTresor.Value), Decimal.ToInt32(numVerticalTresor.Value)),
                        NombreDeTresor = Decimal.ToInt32(numNombreTresors.Value)
                    });
                LoadCarte();
            }
        }

        private void btnSupprimerTresor_Click(object sender, EventArgs e)
        {
            if (tresorEnCours != null)
                carteEntree.Tresors.Remove(tresorEnCours);
        }

        private void NewAventurier()
        {
            numHorizontalAventurier.Value = 0;
            numVerticalAventurier.Value = 0;
            txtNom.Text = string.Empty;
            txtOrientation.Text = string.Empty;
            txtDeplacements.Text = string.Empty;
            btnSupprimerAventurier.Enabled = false;

            aventurierEnCours = null;
        }
        private void btnNouveauAventurier_Click(object sender, EventArgs e)
        {
            NewAventurier();
        }

        private void btnSaveAventurier_Click(object sender, EventArgs e)
        {
            
            Position newPosition = new Position(Decimal.ToInt32(numHorizontalAventurier.Value), Decimal.ToInt32(numVerticalAventurier.Value));
            bool bCollision = false;
            if(aventurierEnCours == null || !newPosition.Equals(aventurierEnCours.Position))
                bCollision = instance.CheckCollision(carteEntree.GetAllPositionsCollision(), newPosition);
            if (bCollision)
                MessageBox.Show("Une collision va avoir lieu avec ces positions. Impossible d'enregistrer l'aventurier");
            else
            {


                if (aventurierEnCours != null)
                {
                    aventurierEnCours.Position = newPosition;
                    aventurierEnCours.Deplacements = txtDeplacements.Text;
                    aventurierEnCours.Orientation = Convert.ToChar(txtOrientation.Text);
                    aventurierEnCours.Nom = txtNom.Text;
                    // aventurierEnCours.NombreDeTresor = Decimal.ToInt32(numNombreTresors.Value);
                }

                else
                    carteEntree.Aventuriers.Add(new Aventurier
                    {
                        Position = newPosition,
                        Deplacements = txtDeplacements.Text,
                        Orientation = Convert.ToChar(txtOrientation.Text),
                        Nom = txtNom.Text
                    });
                LoadCarte();
            }
        }

        private void btnSupprimerAventurier_Click(object sender, EventArgs e)
        {
            if (aventurierEnCours != null)
                carteEntree.Aventuriers.Remove(aventurierEnCours);
        }

        private void txtDeplacements_KeyPress(object sender, KeyPressEventArgs e)
        {
            List<char> allowed = new List<char>() { 'A', 'G', 'D', '\b' }; // \b est la touche "retour arrière"
            if (!allowed.Contains(Char.ToUpper(e.KeyChar)))
                e.Handled = true;
        }

        private void txtOrientation_KeyPress(object sender, KeyPressEventArgs e)
        {
            List<char> allowed = new List<char>() { 'N', 'S', 'E', 'O', '\b' };
            if (!allowed.Contains(Char.ToUpper(e.KeyChar)))
                e.Handled = true;
        }

        private void lsbCarte_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lsbCarte.SelectedIndex != -1)
            {
                string[] array = lsbCarte.SelectedValue.ToString().Split('-');
                switch (array[0].Trim().ToUpper())
                {
                    case "M":
                        rdbMontagne.Checked = true;
                        TabControl.SelectedTab = tabPageMontagnes;
                        montagneEnCours = carteEntree.Montagnes.Find(m => m.Position.Equals(new Position(int.Parse(array[1].Trim()), int.Parse(array[2].Trim()))));
                        numVerticalMontagne.Value = montagneEnCours.Position.GetAxeVerticale();
                        numHorizontalMontagne.Value = montagneEnCours.Position.GetAxeHorizontale();
                        btnSupprimerMontagne.Enabled = true;
                        break;
                    case "T":
                        rdbTresor.Checked = true;
                        TabControl.SelectedTab = tabPageTresors;
                        tresorEnCours = carteEntree.Tresors.Find(m => m.Position.Equals(new Position(int.Parse(array[1].Trim()), int.Parse(array[2].Trim()))));
                        numVerticalTresor.Value = tresorEnCours.Position.GetAxeVerticale();
                        numHorizontalTresor.Value = tresorEnCours.Position.GetAxeHorizontale();
                        numNombreTresors.Value = tresorEnCours.NombreDeTresor;
                        btnSupprimerTresor.Enabled = true;
                        break;
                    case "A":
                        rdbAventurier.Checked = true;
                        TabControl.SelectedTab = tabPageAventuriers;
                        aventurierEnCours = carteEntree.Aventuriers.Find(m => m.Position.Equals(new Position(int.Parse(array[2].Trim()), int.Parse(array[3].Trim()))));
                        numVerticalAventurier.Value = aventurierEnCours.Position.GetAxeVerticale();
                        numHorizontalAventurier.Value = aventurierEnCours.Position.GetAxeHorizontale();
                        txtNom.Text = aventurierEnCours.Nom;
                        txtOrientation.Text = aventurierEnCours.Orientation.ToString();
                        txtDeplacements.Text = aventurierEnCours.Deplacements;
                        btnSupprimerAventurier.Enabled = true;
                        break;
                }
            }
        }
    }
}
