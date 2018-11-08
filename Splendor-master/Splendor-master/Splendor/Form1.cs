/**
 * \file      frmAddVideoGames.cs
 * \author    F. Andolfatto
 * \version   1.0
 * \date      August 22. 2018
 * \brief     Form to play.
 *
 * \details   This form enables to choose coins or cards to get ressources (precious stones) and prestige points 
 * to add and to play with other players
 */


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
    /// manages the form that enables to play with the Splendor
    /// </summary>
    public partial class frmSplendor : Form
    {
        //used to store the number of coins selected for the current round of game
        private int nbRubis;
        private int nbOnyx;
        private int nbEmeraude;
        private int nbDiamand;
        private int nbSaphir;
        private int nbTotal;

        private const int maxSameCoin = 2;
        private int[] CoinsPlayer = new int[5];

        //id of the player that is playing
        private int currentPlayerId;
        //boolean to enable us to know if the user can click on a coin or a card
        private bool enableClicLabel;
        //connection to the database
        private ConnectionDB conn;

        /// <summary>
        /// constructor
        /// </summary>
        public frmSplendor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// loads the form and initialize data in it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSplendor_Load(object sender, EventArgs e)
        {
            lblGoldCoin.Text = "5";

            lblDiamandCoin.Text = "7";
            lblEmeraudeCoin.Text = "7";
            lblOnyxCoin.Text = "7";
            lblRubisCoin.Text = "7";
            lblSaphirCoin.Text = "7";

            conn = new ConnectionDB();

            //load cards from the database
            //they are not hard coded any more
            //TO DO

            Card card11 = new Card();
            card11.Level = 1;
            card11.PrestigePt = 1;
            card11.Cout = new int[] { 1, 0, 2, 0, 2 };
            card11.Ress = Ressources.Rubis;

            Card card12 = new Card();
            card12.Level = 1;
            card12.PrestigePt = 0;
            card12.Cout = new int[] { 0, 1, 2, 1, 0 };
            card12.Ress = Ressources.Saphir;

            txtLevel11.Text = card11.ToString();
            txtLevel12.Text = card12.ToString();

            //load cards from the database
            Stack<Card> listCardOne = conn.GetListCardAccordingToLevel(1);
            //Go through the results
            //Don't forget to check when you are at the end of the stack

            //fin TO DO

            this.Width = 680;
            this.Height = 540;

            enableClicLabel = false;

            lblChoiceDiamand.Visible = false;
            lblChoiceOnyx.Visible = false;
            lblChoiceRubis.Visible = false;
            lblChoiceSaphir.Visible = false;
            lblChoiceEmeraude.Visible = false;
            cmdValidateChoice.Visible = false;
            cmdNextPlayer.Visible = false;

            //we wire the click on all cards to the same event
            //TO DO for all cards
            txtLevel11.Click += ClickOnCard;
        }

        private void ClickOnCard(object sender, EventArgs e)
        {
            //We get the value on the card and we split it to get all the values we need (number of prestige points and ressource)
            //Enable the button "Validate"
            //TO DO
        }

        /// <summary>
        /// click on the play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdPlay_Click(object sender, EventArgs e)
        {
            this.Width = 680;
            this.Height = 780;

            int id = 0;

            LoadPlayer(id);

        }


        /// <summary>
        /// load data about the current player
        /// </summary>
        /// <param name="id">identifier of the player</param>
        private void LoadPlayer(int id)
        {

            enableClicLabel = true;

            string name = conn.GetPlayerName(currentPlayerId);

            //no coins or card selected yet, labels are empty
            lblChoiceDiamand.Text = "";
            lblChoiceOnyx.Text = "";
            lblChoiceRubis.Text = "";
            lblChoiceSaphir.Text = "";
            lblChoiceEmeraude.Text = "";

            lblChoiceCard.Text = "";

            //no coins selected
            nbDiamand = 0;
            nbOnyx = 0;
            nbRubis = 0;
            nbSaphir = 0;
            nbEmeraude = 0;

            Player player = new Player();
            player.Name = name;
            player.Id = id;
            player.Ressources = new int[] { 2, 0, 1, 1, 1 };
            player.Coins = new int[] { 0, 0, 0, 0, 0 };

            lblPlayerDiamandCoin.Text = player.Coins[0].ToString();
            lblPlayerOnyxCoin.Text = player.Coins[1].ToString();
            lblPlayerRubisCoin.Text = player.Coins[2].ToString();
            lblPlayerSaphirCoin.Text = player.Coins[3].ToString();
            lblPlayerEmeraudeCoin.Text = player.Coins[4].ToString();
            currentPlayerId = id;

            lblPlayer.Text = "Jeu de " + name;

            cmdPlay.Enabled = false;
        }

        void TestJetons(Label LabelChoix, Label LabelJeton, int nbJeton, string TypeJeton) //method test des jetons
        {
            int jeton = Convert.ToInt32(LabelJeton.Text);
            const int maxSameCoin = 2;

            if (jeton == 2 && nbJeton == 1)
            {
                MessageBox.Show("Vous ne pouvez pas prendre deux jetons de cette couleur");
            }
            else
            {
                if (nbRubis == maxSameCoin || nbSaphir == maxSameCoin || nbOnyx == maxSameCoin || nbEmeraude == maxSameCoin || nbDiamand == maxSameCoin)
                {
                    MessageBox.Show("Nombre max de pièces de la même couleur = 2");
                }
                else
                {
                    if ((nbJeton == 1 && nbSaphir == 1) || (nbJeton == 1 && nbOnyx == 1) || (nbJeton == 1 && nbEmeraude == 1) || (nbJeton == 1 && nbDiamand == 1))
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre un deuxiéme jeton de la même couleur si vous avez déjà choisi un jeton de couleur différente");
                    }
                    else
                    {
                        nbTotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;

                        if (nbTotal >= 3)
                        {
                            MessageBox.Show("Vous ne pouvez pas prendre plus de jetons");
                        }
                        else
                        {
                            nbJeton++;
                            int nbJetonsDispo = jeton - 1;
                            LabelJeton.Text = nbJetonsDispo.ToString();
                            LabelChoix.Text = nbJeton + "\r\n";

                            switch (TypeJeton)
                            {
                                case "Rubis": nbRubis++; CoinsPlayer[0]++; break;

                                case "Saphir": nbSaphir++; CoinsPlayer[1]++; break;

                                case "Emeraude": nbEmeraude++; CoinsPlayer[2]++; break;

                                case "Onyx": nbOnyx++; CoinsPlayer[3]++; break;

                                case "Diamand": nbDiamand++; CoinsPlayer[4]++; break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// click on the red coin (rubis) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblRubisCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceRubis.Visible = true;
                TestJetons(lblChoiceRubis, lblRubisCoin, nbRubis, "Rubis");
            }
            //TO DO check if possible to choose a coin, update the number of available coin  
        }


        /// <summary>
        /// click on the blue coin (saphir) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSaphirCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceSaphir.Visible = true;
                TestJetons(lblChoiceSaphir, lblSaphirCoin, nbSaphir, "Saphir");
            }
        }

        /// <summary>
        /// click on the black coin (onyx) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblOnyxCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceOnyx.Visible = true;
                TestJetons(lblChoiceOnyx, lblOnyxCoin, nbOnyx, "Onyx");
            }
        }

        /// <summary>
        /// click on the green coin (emeraude) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblEmeraudeCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                int var = Convert.ToInt32(lblEmeraudeCoin.Text);
                cmdValidateChoice.Visible = true;
                lblChoiceEmeraude.Visible = true;
                TestJetons(lblChoiceEmeraude, lblEmeraudeCoin, nbEmeraude, "Emeraude");
            }
        }

        /// <summary>
        /// click on the white coin (diamand) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDiamandCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                int var = Convert.ToInt32(lblDiamandCoin.Text);
                cmdValidateChoice.Visible = true;
                lblChoiceDiamand.Visible = true;
                TestJetons(lblChoiceDiamand, lblDiamandCoin, nbDiamand, "Diamand");
            }
        }

        private void VerifValidateChoice(Label LabelChoice, Label LabelPlayerCoin)
        {
            int Jetons = Convert.ToInt32(LabelChoice.Text);
            int JetonsPlayer = Convert.ToInt32(lblPlayerDiamandCoin.Text);
            int Result = Jetons + JetonsPlayer;

            LabelPlayerCoin.Text = Result.ToString();
            LabelChoice.Text = "";
            LabelChoice.Visible = false;
        }

        /// <summary>
        /// click on the validate button to approve the selection of coins or card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdValidateChoice_Click(object sender, EventArgs e)
        {
            cmdNextPlayer.Visible = true;
            //TO DO Check if card or coins are selected, impossible to do both at the same time

            cmdNextPlayer.Enabled = true;

            if (lblChoiceRubis.Visible == true)
            {
                VerifValidateChoice(lblChoiceRubis, lblPlayerRubisCoin);                
            }

            if (lblChoiceSaphir.Visible == true)
            {
                VerifValidateChoice(lblChoiceSaphir, lblPlayerSaphirCoin);
            }

            if (lblChoiceEmeraude.Visible == true)
            {
                VerifValidateChoice(lblChoiceEmeraude, lblPlayerEmeraudeCoin);
            }

            if (lblChoiceOnyx.Visible == true)
            {
                VerifValidateChoice(lblChoiceOnyx, lblPlayerOnyxCoin);
            }

            if (lblChoiceDiamand.Visible == true)
            {
                VerifValidateChoice(lblChoiceDiamand, lblPlayerDiamandCoin);
            }
        }

        /// <summary>
        /// click on the insert button to insert player in the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdInsertPlayer_Click(object sender, EventArgs e)
        {
            txtNewPlayer.Visible = true;
            cmdValiderNewPlayer.Visible = true;
        }

        /// <summary>
        /// click on the next player to tell him it is his turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNextPlayer_Click(object sender, EventArgs e)
        {
            //TO DO in release 1.0 : 3 is hard coded (number of players for the game), it shouldn't. 
            //TO DO Get the id of the player : in release 0.1 there are only 3 players
            //Reload the data of the player
            //We are not allowed to click on the next button
        }

        private void JetonsBack(Label LabelChoice, Label LabelJeton, int NbJeton)
        {
            int JetonsPlayer = Convert.ToInt32(LabelChoice.Text) - 1;
            int JetonsDispo = Convert.ToInt32(LabelJeton.Text) + 1;

            if (JetonsPlayer>=0)
            {
                NbJeton--;
                LabelChoice.Text = JetonsPlayer.ToString();
                LabelJeton.Text = JetonsDispo.ToString();

                if (JetonsPlayer == 0)
                {
                    LabelChoice.Visible = false;
                }
            }
            if (lblChoiceDiamand.Visible == false && lblChoiceEmeraude.Visible == false && lblChoiceOnyx.Visible == false && lblChoiceRubis.Visible == false && lblChoiceSaphir.Visible == false)
            {
                cmdValidateChoice.Visible = false;
            }
        }
        private void lblChoiceRubis_Click(object sender, EventArgs e)
        {
            JetonsBack(lblChoiceRubis, lblRubisCoin, nbRubis);
        }

        private void lblChoiceSaphir_Click(object sender, EventArgs e)
        {
            JetonsBack(lblChoiceSaphir, lblSaphirCoin, nbSaphir);
        }

        private void lblChoiceOnyx_Click(object sender, EventArgs e)
        {
            JetonsBack(lblChoiceOnyx, lblOnyxCoin, nbOnyx);
        }

        private void lblChoiceEmeraude_Click(object sender, EventArgs e)
        {
            JetonsBack(lblChoiceEmeraude, lblEmeraudeCoin, nbEmeraude);
        }

        private void lblChoiceDiamand_Click(object sender, EventArgs e)
        {
            JetonsBack(lblChoiceDiamand, lblDiamandCoin, nbDiamand);
        }

        private void cmdValiderNewPlayer_Click(object sender, EventArgs e)
        {
            string name = txtNewPlayer.Text;

            if (txtNewPlayer.Text == null)
            {
                MessageBox.Show("Veuillez entrer le pseudo du joueur à ajouter");
            }
            else
            {
                conn.CreateNewPlayer(name);
                txtNewPlayer.Visible = false;
                cmdValiderNewPlayer.Visible = false;
            }
            
        }
    }
}
