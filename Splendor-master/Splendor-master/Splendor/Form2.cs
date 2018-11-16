using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Splendor
{
    /// <summary>
    /// manages the form that enables to insert player to play the Splendor
    /// </summary>
    public partial class FormAddPlayer : Form
    {
        public string Nom;
        public bool Fermer;
        private ConnectionDB conn;
        private string Pseud = " ";
        private int NbPlayer = 0;
        private frmSplendor f1 = new frmSplendor();

        /// <summary>
        /// contructor
        /// </summary>
        public FormAddPlayer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// loads the form and initialize data in it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
            
            conn = new ConnectionDB();
            for (int i = 0; i <= conn.GetCountPlayer(); i++)
            {
                if(conn.GetPlayerName(i) == "")
                {
                    i++;
                }
                lstPlayer.Items.Add(conn.GetPlayerName(i));
            }
            NbPlayer = conn.GetCountPlayer()+1;
            txtAddPlayer.Select();

        }

        /// <summary>
        /// click on the DeletePlayer button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePlayer_Click(object sender, EventArgs e)
        {
            cmdDeletePlayer.Enabled = false;
            if(lstPlayer.SelectedItem.ToString() != " ")
            {
                Pseud = lstPlayer.SelectedItem.ToString();
                lstPlayer.Items.Remove(lstPlayer.SelectedItem);
                conn.DeletePlayer(Pseud);
                
            }
            if(conn.GetCountPlayer() < 2)
            {
                f1.cmdPlay.Enabled = false;
            }
        }

        /// <summary>
        /// click on the AddPlayer button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPlayer_Click(object sender, EventArgs e)
        {
            AddPlayer();
        }

        /// <summary>
        /// Create Player
        /// </summary>
        private void AddPlayer()
        {
            conn = new ConnectionDB();
            Nom = txtAddPlayer.Text;
            int NbPlayer = conn.GetCountPlayer();
            int id = 0;
            for (int i = 0; i <= NbPlayer; i++)
            {
                id = conn.VerifyPlayer();
            }
            if (NbPlayer <= 3)
            {
                conn.InsertNewPlayer(Nom, id);
                MessageAdd();
                lstPlayer.Items.Add(Nom);
            }
            else
            {
                MessageBox.Show("Il ne peut avoir plus de quatre joueurs");
            }
            if (conn.GetCountPlayer() > 1)
            {
                f1.cmdPlay.Enabled = true;
            }
            txtAddPlayer.Clear();
        }

        /// <summary>
        /// Message when we add a player
        /// </summary>
        private void MessageAdd()
        {

            NbPlayer = conn.GetCountPlayer();
            if (MessageBox.Show("Le joueur " + Nom + " a bien été créé", "Message de confirmation",  MessageBoxButtons.OK) == DialogResult.OK)
            {

                bool Add = true;
                List<string> ListName = new List<string>();
                for (int i = 0; i < lstPlayer.Items.Count; i++)
                {
                    ListName.Add(lstPlayer.Items[i].ToString());
                }
                for (int i = 0; i < lstPlayer.Items.Count; i++)
                {
                    for (int x = 0; x < ListName.Count(); x++) {
                        if (lstPlayer.Items[i].ToString() == ListName[x])
                        {
                            Add = false;
                            MessageBox.Show("Il y a déjà un joueur ayant ce nom");
                        }
                    }
                }
                if (Add)
                {
                    if (NbPlayer == 4)
                    {
                        MessageBox.Show("Il y a 4 joueurs si vous voulez en ajouter veuillez en supprimez");
                    }
                    else
                    {
                        MessageBox.Show("Il y a " + NbPlayer + " vous pouvez en ajouter encore " + (4 - NbPlayer) + "");
                    }
                }
            }
        }

        /// <summary>
        /// Select a player on the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPlayer.SelectedIndex >= 0)
            {
                cmdDeletePlayer.Enabled = true;
            }
        }

        /// <summary>
        /// click on the CloseAddPlayer button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCloseAddPlayer_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstPlayer.Items.Count; i++)
            {
                if(lstPlayer.Items[i] == "")
                {
                    string Vide = lstPlayer.Items[i].ToString();
                    conn.DeletePlayer(Vide);
                }
            }
            if(lstPlayer.Items.Count >= 2)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Il n'y a pas assez de joueurs (il faut qu'il y en ait au moins 2)");
            }
        }

        /// <summary>
        /// Add player when the key Enter are down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddPlayer();
            }
        }
    }
}
