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
            lblEmeraudeCoin.Text = "7" ;
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
        private void LoadPlayer(int id) { 

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

        /// <summary>
        /// click on the red coin (rubis) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblRubisCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                int var = Convert.ToInt32(lblRubisCoin.Text);
                cmdValidateChoice.Visible = true;
                lblChoiceRubis.Visible = true;

                if (var == 2 && nbRubis == 1 )
                {
                    MessageBox.Show("Vous pouvez prendre 2 jetons de la même couleur uniquement à condition qu'il en reste au moin 4 dans la pile");
                }

                if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                {
                    MessageBox.Show("Vous ne pouvez pas prendre ce jeton car vous avez déja pris 2 mêmes pierres précieuse");
                }
                else
                {
                    if ((nbRubis == 1 && nbSaphir == 1) || (nbRubis == 1 && nbOnyx == 1) || (nbRubis == 1 && nbEmeraude == 1) || (nbRubis == 1 && nbDiamand == 1))
                    {
                        MessageBox.Show("Vous ne pouvez par prendre 2 même jetons ainsi qu'un jeton différent");
                    }
                    else
                    {
                        nbTotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                        if (nbTotal >= 3)
                        {
                            MessageBox.Show("Vous avez pris le nombre de jetons maximum");
                        }
                        else
                        {
                            nbRubis++;
                            var--;
                            lblRubisCoin.Text = var.ToString();
                            lblChoiceRubis.Text = nbRubis + "\r\n";
                        }
                    }
                }
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
                int var = Convert.ToInt32(lblSaphirCoin.Text);
                cmdValidateChoice.Visible = true;
                lblChoiceSaphir.Visible = true;

                if (var < 4)
                {
                    if (nbSaphir == 2)
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre 2 de ces jetons car il en reste moin de 4");
                    }
                }

                if (nbSaphir == 2 || nbRubis == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre ce jeton car vous en avez pris 2 d'une autre pierre précieuse");
                    }
                    else
                    {
                        if ((nbSaphir == 1 && nbRubis == 1) || (nbSaphir == 1 && nbOnyx == 1) || (nbSaphir == 1 && nbEmeraude == 1) || (nbSaphir == 1 && nbDiamand == 1))
                        {
                            MessageBox.Show("Vous ne pouvez pas faire ceci");
                        }
                        else
                        {
                            nbTotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                            if (nbTotal >= 3)
                            {
                                MessageBox.Show("Vous avez pris le nombre de jetons maximum");
                            }
                            else
                            {
                                nbSaphir++;
                                var--; 
                                lblSaphirCoin.Text = var.ToString();
                                lblChoiceSaphir.Text = nbSaphir + "\r\n";
                            }

                        }
                    }
                
                //TO DO check if possible to choose a coin, update the number of available coin

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
                int var = Convert.ToInt32(lblOnyxCoin.Text);
                cmdValidateChoice.Visible = true;
                lblChoiceOnyx.Visible = true;

                if (var < 4)
                {
                    if (nbOnyx == 2)
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre 2 de ces jetons car il en reste moin de 4");
                    }
                }

                if (nbOnyx == 2 || nbRubis == 2 || nbSaphir == 2 || nbEmeraude == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre ce jeton car vous en avez pris 2 d'une autre pierre précieuse");
                    }
                    else
                    {
                        if ((nbOnyx == 1 && nbRubis == 1) || (nbOnyx == 1 && nbSaphir == 1) || (nbOnyx == 1 && nbEmeraude == 1) || (nbOnyx == 1 && nbDiamand == 1))
                        {
                            MessageBox.Show("Vous ne pouvez pas faire ceci");
                        }
                        else
                        {
                            nbTotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                            if (nbTotal >= 3)
                            {
                                MessageBox.Show("Vous avez pris le nombre de jetons maximum");
                            }
                            else
                            {
                                nbOnyx++;
                                var--;
                                lblOnyxCoin.Text = var.ToString();
                                lblChoiceOnyx.Text = nbOnyx + "\r\n";
                            }

                        }
                    }
                
                //TO DO check if possible to choose a coin, update the number of available coin

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

                if (var < 4)
                {
                    if (nbEmeraude == 2)
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre 2 de ces jetons car il en reste moin de 4");
                    }
                }

                if (nbEmeraude == 2 || nbRubis == 2 || nbOnyx == 2 || nbSaphir == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre ce jeton car vous en avez pris 2 d'une autre pierre précieuse");
                    }
                    else
                    {
                        if ((nbEmeraude == 1 && nbRubis == 1) || (nbEmeraude == 1 && nbOnyx == 1) || (nbEmeraude == 1 && nbSaphir == 1) || (nbEmeraude == 1 && nbDiamand == 1))
                        {
                            MessageBox.Show("Vous ne pouvez pas faire ceci");
                        }
                        else
                        {
                            nbTotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                            if (nbTotal >= 3)
                            {
                                MessageBox.Show("Vous avez pris le nombre de jetons maximum");
                            }
                            else
                            {
                                nbEmeraude++;
                                var--;
                                lblEmeraudeCoin.Text = var.ToString();
                                lblChoiceEmeraude.Text = nbEmeraude + "\r\n";
                            }

                        }
                    }
                
                //TO DO check if possible to choose a coin, update the number of available coin

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

                if (var < 4)
                {
                    if (nbDiamand == 2)
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre 2 de ces jetons car il en reste moin de 4");
                    }
                }

                if (nbDiamand == 2 || nbRubis == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbSaphir == 2)
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre ce jeton car vous en avez pris 2 d'une autre pierre précieuse");
                    }
                    else
                    {
                        if ((nbDiamand == 1 && nbRubis == 1) || (nbDiamand == 1 && nbOnyx == 1) || (nbDiamand == 1 && nbEmeraude == 1) || (nbDiamand == 1 && nbSaphir == 1))
                        {
                            MessageBox.Show("Vous ne pouvez pas faire ceci");
                        }
                        else
                        {
                            nbTotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                            if (nbTotal >= 3)
                            {
                                MessageBox.Show("Vous avez pris le nombre de jetons maximum");
                            }
                            else
                            {
                                nbDiamand++;
                                var--;
                                lblDiamandCoin.Text = var.ToString();
                                lblChoiceDiamand.Text = nbDiamand + "\r\n";
                            }

                        }
                    }
                
                //TO DO check if possible to choose a coin, update the number of available coin

            }
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
                int var = Convert.ToInt32(lblChoiceRubis.Text);
                int var2 = Convert.ToInt32(lblPlayerRubisCoin.Text);
                int Result = var + var2;

                lblPlayerRubisCoin.Text = Result.ToString();
                lblChoiceRubis.Text = "";
                lblChoiceRubis.Visible = false;
            }

            if (lblChoiceSaphir.Visible == true)
            {
                int var = 0;
                if (lblChoiceSaphir.Text != "")
                {
                    var = Convert.ToInt32(lblChoiceSaphir.Text);
                }
                
                int var2 = Convert.ToInt32(lblPlayerSaphirCoin.Text);
                int Result = var + var2;

                lblPlayerSaphirCoin.Text = Result.ToString();
                lblChoiceSaphir.Text = "";
                lblChoiceSaphir.Visible = false;
            }

            if (lblChoiceEmeraude.Visible == true)
            {
                int var = Convert.ToInt32(lblChoiceEmeraude.Text);
                int var2 = Convert.ToInt32(lblPlayerEmeraudeCoin.Text);
                int Result = var + var2;

                lblPlayerEmeraudeCoin.Text = Result.ToString();
                lblChoiceEmeraude.Text = "";
                lblChoiceEmeraude.Visible = false;
            }

            if (lblChoiceOnyx.Visible == true)
            {
                int var = Convert.ToInt32(lblChoiceOnyx.Text);
                int var2 = Convert.ToInt32(lblPlayerOnyxCoin.Text);
                int Result = var + var2;

                lblPlayerOnyxCoin.Text = Result.ToString();
                lblChoiceOnyx.Text = "";
                lblChoiceOnyx.Visible = false;
            }

            if (lblChoiceDiamand.Visible == true)
            {
                int var = Convert.ToInt32(lblChoiceDiamand.Text);
                int var2 = Convert.ToInt32(lblPlayerDiamandCoin.Text);
                int Result = var + var2;

                lblPlayerDiamandCoin.Text = Result.ToString();
                lblChoiceDiamand.Text = "";
                lblChoiceDiamand.Visible = false;
            }
        }

        /// <summary>
        /// click on the insert button to insert player in the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdInsertPlayer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A implémenter");
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

        private void lblChoiceRubis_Click(object sender, EventArgs e)
        {
            int var = Convert.ToInt32(lblChoiceRubis.Text) - 1;
            int var2 = Convert.ToInt32(lblRubisCoin.Text) + 1;
            if (var >= 0 )
            {
                nbRubis--;
                lblChoiceRubis.Text = var.ToString();
                lblRubisCoin.Text = var2.ToString();

                if (var == 0)
                {
                    lblChoiceRubis.Visible = false;
                }
            }
            if (nbTotal == 0;)
            {
                cmdValidateChoice.Visible = false;
            }
        }

        private void lblChoiceSaphir_Click(object sender, EventArgs e)
        {
            int var = Convert.ToInt32(lblChoiceSaphir.Text) - 1;
            int var2 = Convert.ToInt32(lblSaphirCoin.Text) + 1;
            if (var >= 0)
            {
                nbSaphir--;
                lblChoiceSaphir.Text = var.ToString();
                lblSaphirCoin.Text = var2.ToString();

                if (var == 0)
                {
                    lblChoiceSaphir.Visible = false;
                }
            }
            if (lblChoiceRubis.Visible == false && lblChoiceSaphir.Visible == false && lblChoiceOnyx.Visible == false && lblChoiceEmeraude.Visible == false && lblChoiceDiamand.Visible == false)
            {
                cmdValidateChoice.Visible = false;
            }
        }

        private void lblChoiceOnyx_Click(object sender, EventArgs e)
        {
            int var = Convert.ToInt32(lblChoiceOnyx.Text) - 1;
            int var2 = Convert.ToInt32(lblOnyxCoin.Text) + 1;
            if (var >= 0)
            {
                nbOnyx--;
                lblChoiceOnyx.Text = var.ToString();
                lblOnyxCoin.Text = var2.ToString();

                if (var == 0)
                {
                    lblChoiceOnyx.Visible = false;
                }
            }
            if (lblChoiceRubis.Visible == false && lblChoiceSaphir.Visible == false && lblChoiceOnyx.Visible == false && lblChoiceEmeraude.Visible == false && lblChoiceDiamand.Visible == false)
            {
                cmdValidateChoice.Visible = false;
            }
        }

        private void lblChoiceEmeraude_Click(object sender, EventArgs e)
        {
            int var = Convert.ToInt32(lblChoiceEmeraude.Text) - 1;
            int var2 = Convert.ToInt32(lblEmeraudeCoin.Text) + 1;
            if (var >= 0)
            {
                nbEmeraude--;
                lblChoiceEmeraude.Text = var.ToString();
                lblEmeraudeCoin.Text = var2.ToString();

                if (var == 0)
                {
                    lblChoiceEmeraude.Visible = false;
                }
            }
            if (lblChoiceRubis.Visible == false && lblChoiceSaphir.Visible == false && lblChoiceOnyx.Visible == false && lblChoiceEmeraude.Visible == false && lblChoiceDiamand.Visible == false)
            {
                cmdValidateChoice.Visible = false;
            }
        }

        private void lblChoiceDiamand_Click(object sender, EventArgs e)
        {
            int var = Convert.ToInt32(lblChoiceDiamand.Text) - 1;
            int var2 = Convert.ToInt32(lblDiamandCoin.Text) + 1;
            if (var >= 0)
            {
                nbDiamand--;
                lblChoiceDiamand.Text = var.ToString();
                lblDiamandCoin.Text = var2.ToString();

                if (var == 0)
                {
                    lblChoiceDiamand.Visible = false;
                }
            }
            if (lblChoiceRubis.Visible == false && lblChoiceSaphir.Visible == false && lblChoiceOnyx.Visible == false && lblChoiceEmeraude.Visible == false && lblChoiceDiamand.Visible == false)
            {
                cmdValidateChoice.Visible = false;
            }
        }
    }
}
