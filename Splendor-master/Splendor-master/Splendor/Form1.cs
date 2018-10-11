/**
 * \file      frmAddVideoGames.cs
 * \author    C.Goldenschue & J.Alipio-Penedo
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
        Stack<Card> listCardOne;
        Stack<Card> listCardTwo;
        Stack<Card> listCardThree;
        Stack<Card> listCardNoble;
        int nbCardRubis = 0;
        int nbCardEmeraude = 0;
        int nbCardOnyx = 0;
        int nbCardSaphire = 0;
        int nbCardDiamant = 0;
        int NbPtPrestige;
        char[] MyChar = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        int id = 0;
        bool Reid;
        int[,] Coin;
        int[,] Ressource;
        string namePlayer;
        Form2 form2;


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



            /*Card card11 = new Card();
            card11.Level = 1;
            card11.PrestigePt = 1;
            card11.Cout = new int[] { 1, 0, 2, 0, 2 };
            card11.Ress = Ressources.Rubis;

            Card card12 = new Card();
            card12.Level = 1;
            card12.PrestigePt = 0;
            card12.Cout = new int[] { 0, 1, 2, 1, 0 };
            card12.Ress = Ressources.Saphir;*/

            //txtNoble1.Text = card11.ToString();
            //txtNoble3.Text = card12.ToString();


            //load cards from the database
            listCardOne = conn.GetListCardAccordingToLevel(1);
            listCardTwo = conn.GetListCardAccordingToLevel(2);
            listCardThree = conn.GetListCardAccordingToLevel(3);
            listCardNoble = conn.GetListCardAccordingToLevel(4);
            //Go through the results
            //Don't forget to check when you are at the end of the stack


            // Card level one
            txtLevel11.Text = listCardOne.Pop().ToString();
            txtLevel12.Text = listCardOne.Pop().ToString();
            txtLevel13.Text = listCardOne.Pop().ToString();
            txtLevel14.Text = listCardOne.Pop().ToString();

            // Card level two
            
            txtLevel21.Text = listCardTwo.Pop().ToString();
            txtLevel22.Text = listCardTwo.Pop().ToString();
            txtLevel23.Text = listCardTwo.Pop().ToString();
            txtLevel24.Text = listCardTwo.Pop().ToString();

            // Card level three
            txtLevel31.Text = listCardThree.Pop().ToString();
            txtLevel32.Text = listCardThree.Pop().ToString();
            txtLevel33.Text = listCardThree.Pop().ToString();
            txtLevel34.Text = listCardThree.Pop().ToString();

            // Card level Noble
            txtNoble1.Text = listCardNoble.Pop().ToString();
            txtNoble2.Text = listCardNoble.Pop().ToString();
            txtNoble3.Text = listCardNoble.Pop().ToString();
            txtNoble4.Text = listCardNoble.Pop().ToString();

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

            // Click card level one
            txtLevel11.Click += ClickOnCard;
            txtLevel12.Click += ClickOnCard;
            txtLevel13.Click += ClickOnCard;
            txtLevel14.Click += ClickOnCard;

            // Click card level two
            txtLevel21.Click += ClickOnCard;
            txtLevel22.Click += ClickOnCard;
            txtLevel23.Click += ClickOnCard;
            txtLevel24.Click += ClickOnCard;

            // Click card level three
            txtLevel31.Click += ClickOnCard;
            txtLevel32.Click += ClickOnCard;
            txtLevel33.Click += ClickOnCard;
            txtLevel34.Click += ClickOnCard;

            // Click card level one
            txtNoble1.Click += ClickOnCard;
            txtNoble2.Click += ClickOnCard;
            txtNoble3.Click += ClickOnCard;
            txtNoble4.Click += ClickOnCard;
        }

        private void ClickOnCard(object sender, EventArgs e)
        {
            //We get the value on the card and we split it to get all the values we need (number of prestige points and ressource)
            //Enable the button "Validate"
            //TO DO
            if (enableClicLabel)
            {

                TextBox cardOnBoard = (TextBox)sender;

                String[] TableauComparatif = cardOnBoard.Lines;

                int NbLines = cardOnBoard.Lines.Count()-1;
                int NbRubisPoss = Convert.ToInt16(lblPlayerRubisCoin.Text);
                int NbSaphirePoss = Convert.ToInt16(lblPlayerSaphireCoin.Text);
                int NbEmeraudePoss = Convert.ToInt16(lblPlayerEmeraudeCoin.Text);
                int NbOnyxPoss = Convert.ToInt16(lblPlayerOnyxCoin.Text);
                int NbDiamantPoss = Convert.ToInt16(lblPlayerDiamantCoin.Text);
                int NbCardResidualOne = listCardOne.Count();
                int NbCardResidualTwo = listCardOne.Count();
                int NbCardResidualThree = listCardOne.Count();
                int NbCardResidualNoble = listCardOne.Count();
                int NbRessource = 2;
                bool Achat = true;

                while (NbRessource != NbLines)
                {
                    if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Rubis"))
                    {
                        if(NbRubisPoss >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(10)))
                        {
                            //MessageBox.Show("C'est du Rubis");
                            NbRubisPoss = NbRubisPoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(10));

                        }
                        else
                        {
                            MessageBox.Show("Vous n'avez pas assez de Rubis");
                            Achat = false;
                        }
                    }
                    else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Saphire"))
                    {
                        
                        if (NbSaphirePoss >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(12)))
                        {
                            //MessageBox.Show("C'est du Saphire");
                            NbSaphirePoss = NbSaphirePoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(12));
                        }
                        else
                        {
                            MessageBox.Show("Vous n'avez pas assez de Saphire");
                            Achat = false;
                        }
                    }
                    else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Emeraude"))
                    {
                        if (NbEmeraudePoss >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(13)))
                        {
                            //MessageBox.Show("C'est du Emeraude");
                            NbEmeraudePoss = NbEmeraudePoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(13));
                        }
                        else
                        {
                            MessageBox.Show("Vous n'avez pas assez de Emeraude");
                            Achat = false;
                        }
                    }
                    else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Onyx"))
                    {
                        if (NbOnyxPoss >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(9)))
                        {
                            //MessageBox.Show("C'est du Onyx");
                            NbOnyxPoss = NbOnyxPoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(9));
                        }
                        else
                        {
                            MessageBox.Show("Vous n'avez pas assez de Onyx");
                            Achat = false;
                        }
                    }
                    else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Diamant"))
                    {
                        if (NbDiamantPoss >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(12)))
                        {
                            //MessageBox.Show("C'est du Diamant");
                            NbDiamantPoss = NbDiamantPoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(12));
                        }
                        else
                        {
                            MessageBox.Show("Vous n'avez pas assez de Diamant");
                            Achat = false;
                        }
                    }

                    if (NbRessource+1 != NbLines) { 
                        NbRessource++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (Achat)
                {
                    cardOnBoard.Clear();

                    if (cardOnBoard.Name == "txtLevel11" | cardOnBoard.Name == "txtLevel12" | cardOnBoard.Name == "txtLevel13" | cardOnBoard.Name == "txtLevel14")
                    {
                        cardOnBoard.Text = listCardOne.Pop().ToString();
                        NbCardResidualOne--;
                    }
                    else if (cardOnBoard.Name == "txtLevel21" | cardOnBoard.Name == "txtLevel22" | cardOnBoard.Name == "txtLevel23" | cardOnBoard.Name == "txtLevel24")
                    {
                        cardOnBoard.Text = listCardTwo.Pop().ToString();
                        NbCardResidualTwo--;
                    }
                    else if (cardOnBoard.Name == "txtLevel31" | cardOnBoard.Name == "txtLevel32" | cardOnBoard.Name == "txtLevel33" | cardOnBoard.Name == "txtLevel34")
                    {
                        cardOnBoard.Text = listCardThree.Pop().ToString();
                        NbCardResidualThree--;
                    }

                    lblPlayerRubisCoin.Text = NbRubisPoss.ToString();
                    lblPlayerSaphireCoin.Text = NbSaphirePoss.ToString();
                    lblPlayerOnyxCoin.Text = NbOnyxPoss.ToString();
                    lblPlayerEmeraudeCoin.Text = NbEmeraudePoss.ToString();
                    lblPlayerDiamantCoin.Text = NbDiamantPoss.ToString();


                    if (TableauComparatif[0].StartsWith("Rubis"))
                    {
                        nbCardRubis++;
                        txtPlayerRubisCard.Text = nbCardRubis.ToString();
                        
                        if (TableauComparatif[0].Substring(7) != " ")
                        {
                            if (TableauComparatif[0].Substring(9) != "\t")
                            {
                                NbPtPrestige += Convert.ToInt16(TableauComparatif[0].Substring(7));
                            }
                        }
                    }
                    else if (TableauComparatif[0].StartsWith("Emeraude"))
                    {
                        nbCardEmeraude++;
                        txtPlayerEmeraudeCard.Text = nbCardEmeraude.ToString();
                        if (TableauComparatif[0].Substring(10) != " ")
                        {
                            if (TableauComparatif[0].Substring(12) != "\t")
                            {
                                NbPtPrestige += Convert.ToInt16(TableauComparatif[0].Substring(8));
                            }
                            
                        }
                    }
                    else if (TableauComparatif[0].StartsWith("Onyx"))
                    {
                        nbCardOnyx++;
                        txtPlayerOnyxCard.Text = nbCardOnyx.ToString();
                        if (TableauComparatif[0].Substring(6) != " ")
                        {
                            if (TableauComparatif[0].Substring(8) != "\t")
                            {
                                NbPtPrestige += Convert.ToInt16(TableauComparatif[0].Substring(6));
                            }
                        }
                    }
                    else if (TableauComparatif[0].StartsWith("Saphire"))
                    {
                        nbCardSaphire++;
                        txtPlayerSaphireCard.Text = nbCardSaphire.ToString();
                        if (TableauComparatif[0].Substring(9) != " ")
                        {
                            if (TableauComparatif[0].Substring(11) != "\t")
                            {
                                NbPtPrestige += Convert.ToInt16(TableauComparatif[0].Substring(9));
                            }
                        }
                    }
                    else if (TableauComparatif[0].StartsWith("Diamant"))
                    {
                        nbCardDiamant++;
                        txtPlayerDiamantCard.Text = nbCardDiamant.ToString();
                        if (TableauComparatif[0].Substring(9) != " ")
                        {
                            if (TableauComparatif[0].Substring(11) != "\t")
                            {
                                NbPtPrestige = NbPtPrestige + Convert.ToInt16(TableauComparatif[0].Substring(9));
                            }
                        }
                    }
                    else if (TableauComparatif[0].StartsWith(" "))
                    {
                        NbPtPrestige = NbPtPrestige + Convert.ToInt16(TableauComparatif[0].Substring(2));
                    }

                    lblNbPtPrestige.Text = "Nb pt prestige : " + NbPtPrestige;
                }
            }

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

            cmdInsertPlayer.Enabled = false;

            conn.NumberPlayer = conn.GetCountPlayer();

            Coin = new int[conn.NumberPlayer, 5];
            Coin[id, 0] = 0;
            Coin[id, 1] = 0;
            Coin[id, 2] = 0;
            Coin[id, 3] = 0;
            Coin[id, 4] = 0;

            Ressource = new int[conn.NumberPlayer, 5];
            Ressource[id, 0] = 0;
            Ressource[id, 1] = 0;
            Ressource[id, 2] = 0;
            Ressource[id, 3] = 0;
            Ressource[id, 4] = 0;

            LoadPlayer(id);

        }


        /// <summary>
        /// load data about the current player
        /// </summary>
        /// <param name="id">identifier of the player</param>
        private void LoadPlayer(int id) {

            currentPlayerId = id;
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
            player.Ressources = new int[] { Ressource[id, 0], Ressource[id, 1], Ressource[id, 2], Ressource[id, 3], Ressource[id, 4] };
            player.Coins = new int[] { Coin[id, 0], Coin[id, 1], Coin[id, 2], Coin[id, 3], Coin[id, 4] };
            
            lblPlayerRubisCoin.Text = player.Coins[0].ToString();
            lblPlayerSaphireCoin.Text = player.Coins[1].ToString();
            lblPlayerOnyxCoin.Text = player.Coins[2].ToString();
            lblPlayerEmeraudeCoin.Text = player.Coins[3].ToString();
            lblPlayerDiamantCoin.Text = player.Coins[4].ToString();

            txtPlayerRubisCard.Text = player.Ressources[0].ToString();
            txtPlayerSaphireCard.Text = player.Ressources[1].ToString();
            txtPlayerOnyxCard.Text = player.Ressources[2].ToString();
            txtPlayerEmeraudeCard.Text = player.Ressources[3].ToString();
            txtPlayerDiamantCard.Text = player.Ressources[4].ToString();

            

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
                cmdValidateChoice.Visible = true;
                lblChoiceRubis.Visible = true;
                //TO DO check if possible to choose a coin, update the number of available coin
                nbRubis++;
                lblChoiceRubis.Text = nbRubis + "\r\n";
            }
        }

        /// <summary>
        /// click on the blue coin (saphir) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSaphirCoin_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// click on the black coin (onyx) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblOnyxCoin_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// click on the green coin (emeraude) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblEmeraudeCoin_Click(object sender, EventArgs e)
        {

            
        }

        /// <summary>
        /// click on the white coin (diamand) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDiamandCoin_Click(object sender, EventArgs e)
        {
            
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
        }

        /// <summary>
        /// click on the insert button to insert player in the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdInsertPlayer_Click(object sender, EventArgs e)
        {

            form2 = new Form2();
            form2.ShowDialog();

           // MessageBox.Show("A implémenter");
        }

        /// <summary>
        /// click on the next player to tell him it is his turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNextPlayer_Click(object sender, EventArgs e)
        {


            Coin = new int[conn.NumberPlayer, 5];
            Coin[id, 0] = Convert.ToInt16(lblPlayerRubisCoin.Text);
            Coin[id, 1] = Convert.ToInt16(lblPlayerSaphireCoin.Text);
            Coin[id, 2] = Convert.ToInt16(lblPlayerOnyxCoin.Text);
            Coin[id, 3] = Convert.ToInt16(lblPlayerEmeraudeCoin.Text);
            Coin[id, 4] = Convert.ToInt16(lblPlayerDiamantCoin.Text);

            Ressource = new int[conn.NumberPlayer, 5];
            Ressource[id, 0] = Convert.ToInt16(txtPlayerRubisCard.Text);
            Ressource[id, 1] = Convert.ToInt16(txtPlayerSaphireCard.Text);
            Ressource[id, 2] = Convert.ToInt16(txtPlayerOnyxCard.Text);
            Ressource[id, 3] = Convert.ToInt16(txtPlayerEmeraudeCard.Text);
            Ressource[id, 4] = Convert.ToInt16(txtPlayerDiamantCard.Text);

            if (id+1 < conn.NumberPlayer)
            {
                if (id+1 != conn.NumberPlayer && Reid == true)
                {
                    id++;
                }
                Reid = true;
                LoadPlayer(id);
                
            }
            else if(id == conn.NumberPlayer-1)
            {
                Reid = false;
                id = -1;
                id++;
                LoadPlayer(id);

            }
            



            //TO DO in release 1.0 : 3 is hard coded (number of players for the game), it shouldn't. 
            //TO DO Get the id of the player : in release 0.1 there are only 3 players
            //Reload the data of the player
            //We are not allowed to click on the next button

        }

    }
}
