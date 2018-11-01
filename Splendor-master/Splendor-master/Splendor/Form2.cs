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
    public partial class Form2 : Form
    {


        public string Nom;
        public bool Fermer;
        private ConnectionDB conn;
        private string Pseud = " ";
        private int NbPlayer = 0;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new ConnectionDB();
            for (int i = 0; i < conn.GetCountPlayer(); i++)
            {
                lstPlayer.Items.Add(conn.GetPlayerName(i));
            }
            NbPlayer = conn.GetCountPlayer()+1;


        }

        private void cmdDeletePlayer_Click(object sender, EventArgs e)
        {
            cmdDeletePlayer.Enabled = false;
            if(lstPlayer.SelectedItem.ToString() != " ")
            {
                Pseud = lstPlayer.SelectedItem.ToString();
                lstPlayer.Items.Remove(lstPlayer.SelectedItem);
                conn.DeletePlayer(Pseud);
            }
        }

        private void cmdAddPlayer_Click(object sender, EventArgs e)
        {
            conn = new ConnectionDB();
            Nom = txtAddPlayer.Text;
            int NbPlayer = conn.GetCountPlayer();
            int id = 0;
            for(int i = 0; i<= NbPlayer; i++)
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
                //this.Close();
            }
        }

        private void MessageAdd()
        {

            NbPlayer = conn.GetCountPlayer();
            if (MessageBox.Show("Le joueur " + Nom + " a bien été créé", "Message de confirmation",  MessageBoxButtons.OK) == DialogResult.OK)
            {
                if(NbPlayer == 4)
                {
                    MessageBox.Show("Il y a 4 joueurs si vous voulez en ajouter veuillez en supprimez");
                }
                else
                {
                    MessageBox.Show("Il y a "+ NbPlayer+" vous pouvez en ajouter encore "+ (4-NbPlayer) +"");
                }

                //this.Close();
            }

        }

        private void lstPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPlayer.SelectedIndex >= 0)
            {
                cmdDeletePlayer.Enabled = true;
            }
        }
    }
}
