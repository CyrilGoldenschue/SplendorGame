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

        public Form2()
        {
            InitializeComponent();
        }

        private void cmdAddPlayer_Click(object sender, EventArgs e)
        {
            conn = new ConnectionDB();
            Nom = txtAddPlayer.Text;
            int NbPlayer = conn.GetCountPlayer();
            if (NbPlayer <= 3)
            {
                conn.InsertNewPlayer(Nom, NbPlayer);
                Message();
            }
            else
            {
                MessageBox.Show("Il y a trop de joueur veuillez en supprimer ou ne pas en ajouter");
                this.Close();
            }
        }

        private void Message()
        {

            
            if (MessageBox.Show("Le joueur " + Nom + " a bien été créé", "Message de confirmation",  MessageBoxButtons.OK) == DialogResult.OK)
            {
                this.Close();
            }

        }
    }
}
