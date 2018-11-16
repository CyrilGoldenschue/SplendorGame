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
using System.IO;
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
        //connection to the database
        private ConnectionDB conn;
        //used to store the number of coins selected for the current round of game
        private int nbRubis = 0;
        private int nbOnyx = 0;
        private int nbEmeraude = 0;
        private int nbDiamand = 0;
        private int nbSaphir = 0;
        private int nbTotal = 0;
        private Stack<Card> listCardOne = new Stack<Card>();
        private Stack<Card> listCardTwo = new Stack<Card>();
        private Stack<Card> listCardThree = new Stack<Card>();
        private Stack<Card> listCardNoble = new Stack<Card>();
        private int nbCardRubis = 0;
        private int nbCardEmeraude = 0;
        private int nbCardOnyx = 0;
        private int nbCardSaphire = 0;
        private int nbCardDiamant = 0;
        private int[] NbPtPrestige;
        private char[] MyChar = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private int id = 0;                                                                 //Variable contenant l'id du joueur
        private bool Reid = true;                                                           
        private int[,] Coin;                                                                //Tableau des jetons de ressources pour chaque joueur
        private int[,] Ressource;                                                           //Tableau des cartes de ressources pour chaque joueur
        private string NameControlAll = "";                                                 //Nom du controleur qui va être désactivé
        private FormAddPlayer form2;                                                        //Accès au form2
        private string NameCard;                                                            //Variable contenant le nom d'une carte
        private Control DisableControler = new Control();                                   //Variable pour désactiver les cartes de nobles
        private string[] Booked = new string[4];                                            //Initialisation de la variable Booked (1 résérvation max par personne)
        private int[] NbTour = new int[4];                                                  //Nombre de joueur max
        private int NbJeton = 0;                                                            //Nombre de ressource à l'affichage
        private Random rnd = new Random();                                                  //Récuperation d'un nombre aléatoire
        private string Nom;                                                                 //Récuperation du nom du joueur
        //Nombre de carte de ressource
        private int NbCardRubis;
        private int NbCardSaphire;
        private int NbCardOnyx;
        private int NbCardEmeraude;
        private int NbCardDiamant;
        //Nombre de ressources en stock
        private int NbRubisPoss;
        private int NbSaphirePoss;
        private int NbEmeraudePoss;
        private int NbOnyxPoss;
        private int NbDiamantPoss;
        private int NbGoldPoss;
        //Nombre de ressources restantes
        private int BanqueRubis;
        private int BanqueSaphire;
        private int BanqueEmeraude;
        private int BanqueOnyx;
        private int BanqueDiamant;
        private int BanqueGold;

        private const int maxSameCoin = 2;
        private int[] CoinsPlayer = new int[5];
        //id of the player that is playing
        private int currentPlayerId;
        //boolean to enable us to know if the user can click on a coin or a card
        private bool enableClicLabel;
        
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
        public void frmSplendor_Load(object sender, EventArgs e)
        {
            conn = new ConnectionDB();
            int NbPlayer = conn.GetCountPlayer();                   //get the number of players that are in the database

            ActiveButtonPlay();

            lblGoldCoin.Text = "5";                                 //gives basic values
            lblPlayerGoldCoin.Text = "0";
            lblDiamandCoin.Text = "7";
            lblEmeraudeCoin.Text = "7";
            lblOnyxCoin.Text = "7";
            lblRubisCoin.Text = "7";
            lblSaphirCoin.Text = "7";

            Coin = new int[conn.NumberPlayer, 5];
            Ressource = new int[conn.NumberPlayer, 5];

            //load cards from the database
            listCardOne = conn.GetListCardAccordingToLevel(1);
            listCardTwo = conn.GetListCardAccordingToLevel(2);
            listCardThree = conn.GetListCardAccordingToLevel(3);
            listCardNoble = conn.GetListCardAccordingToLevel(4);

            Shuffle(listCardOne);                                   //Shuffle stack of card
            Shuffle(listCardTwo);
            Shuffle(listCardThree);
            Shuffle(listCardNoble);

            // Card level one
            txtLevel11.Text = listCardOne.Pop().ToString();         //Insert card in a textbox
            txtLevel12.Text = listCardOne.Pop().ToString();
            txtLevel13.Text = listCardOne.Pop().ToString();
            txtLevel14.Text = listCardOne.Pop().ToString();
            // Card level two
            txtLevel21.Text = listCardTwo.Pop().ToString();         //Insert card in a textbox
            txtLevel22.Text = listCardTwo.Pop().ToString();
            txtLevel23.Text = listCardTwo.Pop().ToString();
            txtLevel24.Text = listCardTwo.Pop().ToString();
            // Card level three
            txtLevel31.Text = listCardThree.Pop().ToString();       //Insert card in a textbox
            txtLevel32.Text = listCardThree.Pop().ToString();             
            txtLevel33.Text = listCardThree.Pop().ToString();
            txtLevel34.Text = listCardThree.Pop().ToString();
            // Card level Noble
            txtNoble1.Text = listCardNoble.Pop().ToString();        //Insert card in a textbox
            txtNoble2.Text = listCardNoble.Pop().ToString();
            txtNoble3.Text = listCardNoble.Pop().ToString();
            txtNoble4.Text = listCardNoble.Pop().ToString();

            this.Width = 680;                                       //Change the resolution of frmSplendor
            this.Height = 540;

            enableClicLabel = false;

            lblChoiceDiamant.Visible = false;                       //erases the labels of choice
            lblChoiceOnyx.Visible = false;
            lblChoiceRubis.Visible = false;
            lblChoiceSaphire.Visible = false;
            lblChoiceEmeraude.Visible = false;
            cmdValidateChoice.Visible = false;                      //erases the buttons of validate choice
            cmdNextPlayer.Visible = false;                          //erases the buttons of next player

            //we wire the click on all cards to the same event

            // Click card level one
            txtLevel11.Click += ClickOnCard;
            txtLevel12.Click += ClickOnCard;
            txtLevel13.Click += ClickOnCard;
            txtLevel14.Click += ClickOnCard;

            // Click card level two
            txtLevel21.Click += ClickOnCard;                        //When we click it buys the card
            txtLevel22.Click += ClickOnCard;
            txtLevel23.Click += ClickOnCard;
            txtLevel24.Click += ClickOnCard;

            // Click card level three
            txtLevel31.Click += ClickOnCard;                         //When we click it buys the card
            txtLevel32.Click += ClickOnCard;
            txtLevel33.Click += ClickOnCard;
            txtLevel34.Click += ClickOnCard;

            // Click card level Noble
            txtNoble1.Click += ClickOnCard;                        //When we click it buys the card
            txtNoble2.Click += ClickOnCard;
            txtNoble3.Click += ClickOnCard;
            txtNoble4.Click += ClickOnCard;

            // Click card booked
            txtPlayerBookedCard.Click += ClickOnBooked;             //When we click it buys the booked card
        }
        /// <summary>
        /// see if there is more than one player and less than five before activating the play button
        /// </summary>
        private void ActiveButtonPlay()
        {
            if(conn.GetCountPlayer() < 2)
            {
                cmdPlay.Enabled = false;
            }
            else
            {
                cmdPlay.Enabled = true;
            }
        }
        
        /// <summary>
        /// mix of stacks of cards
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        private void Shuffle<T>(Stack<T> stack)
        {
            var values = stack.ToArray();                           //Stock value of stack
            stack.Clear();                                          //Clear value of stack
            foreach (var value in values.OrderBy(x => rnd.Next()))  //swap the space between two cards
            {
                stack.Push(value);                                  //Send the stored card instead of another
            }   

        }

        /// <summary>
        /// Purchase card booked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickOnBooked(object sender, EventArgs e)
        {
            TextBox CardBooked = (TextBox)sender;
            
            //Nombre de carte de ressource
            NbCardRubis = Convert.ToInt16(txtPlayerRubisCard.Text);
            NbCardSaphire = Convert.ToInt16(txtPlayerSaphireCard.Text);
            NbCardOnyx = Convert.ToInt16(txtPlayerOnyxCard.Text);
            NbCardEmeraude = Convert.ToInt16(txtPlayerEmeraudeCard.Text);
            NbCardDiamant = Convert.ToInt16(txtPlayerDiamantCard.Text);
            //Nombre de ressources en stock
            NbRubisPoss = Convert.ToInt16(lblPlayerRubisCoin.Text);
            NbSaphirePoss = Convert.ToInt16(lblPlayerSaphireCoin.Text);
            NbEmeraudePoss = Convert.ToInt16(lblPlayerEmeraudeCoin.Text);
            NbOnyxPoss = Convert.ToInt16(lblPlayerOnyxCoin.Text);
            NbDiamantPoss = Convert.ToInt16(lblPlayerDiamantCoin.Text);
            NbGoldPoss = Convert.ToInt16(lblPlayerGoldCoin.Text);
            //Nombre de ressources restantes
            BanqueRubis = Convert.ToInt16(lblRubisCoin.Text);
            BanqueSaphire = Convert.ToInt16(lblSaphirCoin.Text);
            BanqueEmeraude = Convert.ToInt16(lblEmeraudeCoin.Text);
            BanqueOnyx = Convert.ToInt16(lblOnyxCoin.Text);
            BanqueDiamant = Convert.ToInt16(lblDiamandCoin.Text);
            BanqueGold = Convert.ToInt16(lblGoldCoin.Text);

            int NbRessource = 2;
            int NbRessourceMiss = 0;
            string[] TabComparatif;
            TabComparatif = CardBooked.Lines;
            int NbLines = CardBooked.Lines.Count() - 1;
            bool Achat = true;

            while (NbRessource != NbLines)
            {
                if ((TabComparatif[NbRessource].Substring(4)).StartsWith("Rubis"))
                {
                    if (NbRubisPoss + NbCardRubis + NbGoldPoss >= Convert.ToInt16(TabComparatif[NbRessource].Substring(10)))
                    {
                        if (NbRubisPoss + NbCardRubis >= Convert.ToInt16(TabComparatif[NbRessource].Substring(10)))
                        {
                            NbRubisPoss = NbRubisPoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(10)) + NbCardRubis;
                            BanqueRubis = BanqueRubis + Convert.ToInt16(TabComparatif[NbRessource].Substring(10)) - NbCardRubis;
                        }
                        else
                        {
                            NbRubisPoss = NbRubisPoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(10)) + NbCardRubis + NbGoldPoss;
                            BanqueGold = BanqueGold + 1;
                            NbGoldPoss = NbGoldPoss - 1;

                        }
                        lblRubisCoin.Text = BanqueRubis.ToString();
                        lblGoldCoin.Text = BanqueGold.ToString();

                    }
                    else
                    {
                        int NbRubisMiss = Convert.ToInt16(TabComparatif[NbRessource].Substring(10)) - NbRubisPoss;
                        if (NbRubisMiss == 1)
                        {
                            NbRessourceMiss++;
                        }
                    }
                }
                else if ((TabComparatif[NbRessource].Substring(4)).StartsWith("Saphire"))
                {

                    if (NbSaphirePoss + NbCardSaphire + NbGoldPoss >= Convert.ToInt16(TabComparatif[NbRessource].Substring(12)))
                    {
                        if (NbSaphirePoss + NbCardSaphire >= Convert.ToInt16(TabComparatif[NbRessource].Substring(12)))
                        {
                            NbSaphirePoss = NbSaphirePoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(12)) + NbCardSaphire;
                            BanqueSaphire = BanqueSaphire + Convert.ToInt16(TabComparatif[NbRessource].Substring(12)) - NbCardSaphire;
                        }
                        else
                        {
                            NbSaphirePoss = NbSaphirePoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(12)) + NbCardSaphire + NbGoldPoss;
                            BanqueGold = BanqueGold + 1;
                            NbGoldPoss = NbGoldPoss - 1;

                        }

                        lblSaphirCoin.Text = BanqueSaphire.ToString();
                        lblGoldCoin.Text = BanqueGold.ToString();

                    }
                    else
                    {
                        int NbSaphireMiss = Convert.ToInt16(TabComparatif[NbRessource].Substring(12)) - NbSaphirePoss;
                        if (NbSaphireMiss == 1)
                        {
                            NbRessourceMiss++;
                        }
                    }
                }
                else if ((TabComparatif[NbRessource].Substring(4)).StartsWith("Emeraude"))
                {
                    if (NbEmeraudePoss + NbCardEmeraude + NbGoldPoss >= Convert.ToInt16(TabComparatif[NbRessource].Substring(13)))
                    {
                        if (NbEmeraudePoss + NbCardEmeraude >= Convert.ToInt16(TabComparatif[NbRessource].Substring(13)))
                        {
                            NbEmeraudePoss = NbEmeraudePoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(13)) + NbCardEmeraude;
                            BanqueEmeraude = BanqueEmeraude + Convert.ToInt16(TabComparatif[NbRessource].Substring(13)) - NbCardEmeraude;
                        }
                        else
                        {
                            NbEmeraudePoss = NbEmeraudePoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(13)) + NbCardEmeraude + NbGoldPoss;
                            BanqueGold = BanqueGold + 1;
                            NbGoldPoss = NbGoldPoss - 1;

                        }

                        lblEmeraudeCoin.Text = BanqueEmeraude.ToString();
                        lblGoldCoin.Text = BanqueGold.ToString();

                    }
                    else
                    {
                        int NbEmeraudeMiss = Convert.ToInt16(TabComparatif[NbRessource].Substring(13)) - NbEmeraudePoss;
                        if (NbEmeraudeMiss == 1)
                        {
                            NbRessourceMiss++;
                        }
                    }
                }
                else if ((TabComparatif[NbRessource].Substring(4)).StartsWith("Onyx"))
                {
                    if (NbOnyxPoss + NbCardOnyx + NbGoldPoss >= Convert.ToInt16(TabComparatif[NbRessource].Substring(9)))
                    {
                        if (NbRubisPoss + NbCardOnyx >= Convert.ToInt16(TabComparatif[NbRessource].Substring(9)))
                        {
                            NbOnyxPoss = NbOnyxPoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(9)) + NbCardOnyx;
                            BanqueOnyx = BanqueOnyx + Convert.ToInt16(TabComparatif[NbRessource].Substring(9)) - NbCardOnyx;
                        }
                        else
                        {
                            NbOnyxPoss = NbOnyxPoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(9)) + NbCardOnyx + NbGoldPoss;
                            BanqueGold = BanqueGold + 1;
                            NbGoldPoss = NbGoldPoss - 1;

                        }

                        lblOnyxCoin.Text = BanqueOnyx.ToString();
                        lblGoldCoin.Text = BanqueGold.ToString();

                    }
                    else
                    {
                        int NbOnyxMiss = Convert.ToInt16(TabComparatif[NbRessource].Substring(9)) - NbOnyxPoss;
                        if (NbOnyxMiss == 1)
                        {
                            NbRessourceMiss++;
                        }
                    }
                }
                else if ((TabComparatif[NbRessource].Substring(4)).StartsWith("Diamant"))
                {
                    if (NbDiamantPoss + NbCardDiamant + NbGoldPoss >= Convert.ToInt16(TabComparatif[NbRessource].Substring(12)))
                    {
                        if (NbDiamantPoss + NbCardOnyx >= Convert.ToInt16(TabComparatif[NbRessource].Substring(12)))
                        {
                            NbDiamantPoss = NbDiamantPoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(12)) + NbCardDiamant;
                            BanqueDiamant = BanqueDiamant + Convert.ToInt16(TabComparatif[NbRessource].Substring(12)) - NbCardDiamant;
                        }
                        else
                        {
                            NbDiamantPoss = NbDiamantPoss - Convert.ToInt16(TabComparatif[NbRessource].Substring(12)) + NbCardDiamant + NbGoldPoss;
                            BanqueGold = BanqueGold + 1;
                            NbGoldPoss = NbGoldPoss - 1;
                        }

                        lblDiamandCoin.Text = BanqueDiamant.ToString();
                        lblGoldCoin.Text = BanqueGold.ToString();
                    }
                    else
                    {
                        int NbDiamantMiss = Convert.ToInt16(TabComparatif[NbRessource].Substring(12)) - NbDiamantPoss;
                        if (NbDiamantMiss == 1)
                        {
                            NbRessourceMiss++;
                        }
                    }
                }

                if (NbRessource + 1 != NbLines)
                {
                    NbRessource++;
                }
                else
                {
                    break;
                }
            }
            if(NbRessourceMiss > 1)
            {
                Achat = false;
            }
            if (Achat)
            {
                CardBooked.Clear();

                lblPlayerRubisCoin.Text = NbRubisPoss.ToString();
                lblPlayerSaphireCoin.Text = NbSaphirePoss.ToString();
                lblPlayerOnyxCoin.Text = NbOnyxPoss.ToString();
                lblPlayerEmeraudeCoin.Text = NbEmeraudePoss.ToString();
                lblPlayerDiamantCoin.Text = NbDiamantPoss.ToString();
                lblPlayerGoldCoin.Text = NbGoldPoss.ToString();
                Booked[id] = null;
                
                if (TabComparatif[0].StartsWith("Rubis"))
                {
                    nbCardRubis++;
                    txtPlayerRubisCard.Text = nbCardRubis.ToString();

                    if (TabComparatif[0].Substring(7) != " ")
                    {
                        if (TabComparatif[0].Substring(9) != "\t")
                        {
                            NbPtPrestige[id] += Convert.ToInt16(TabComparatif[0].Substring(7));
                        }
                    }
                }
                else if (TabComparatif[0].StartsWith("Emeraude"))
                {
                    NbCardEmeraude++;
                    txtPlayerEmeraudeCard.Text = NbCardEmeraude.ToString();
                    if (TabComparatif[0].Substring(10) != " ")
                    {
                        if (TabComparatif[0].Substring(12) != "\t")
                        {
                            NbPtPrestige[id] += Convert.ToInt16(TabComparatif[0].Substring(8));
                        }

                    }
                }
                else if (TabComparatif[0].StartsWith("Onyx"))
                {
                    nbCardOnyx++;
                    txtPlayerOnyxCard.Text = nbCardOnyx.ToString();
                    if (TabComparatif[0].Substring(6) != " ")
                    {
                        if (TabComparatif[0].Substring(8) != "\t")
                        {
                            NbPtPrestige[id] += Convert.ToInt16(TabComparatif[0].Substring(6));
                        }
                    }
                }
                else if (TabComparatif[0].StartsWith("Saphire"))
                {
                    nbCardSaphire++;
                    txtPlayerSaphireCard.Text = nbCardSaphire.ToString();
                    if (TabComparatif[0].Substring(9) != " ")
                    {
                        if (TabComparatif[0].Substring(11) != "\t")
                        {
                            NbPtPrestige[id] += Convert.ToInt16(TabComparatif[0].Substring(9));
                        }
                    }
                }
                else if (TabComparatif[0].StartsWith("Diamant"))
                {
                    nbCardDiamant++;
                    txtPlayerDiamantCard.Text = nbCardDiamant.ToString();
                    if (TabComparatif[0].Substring(9) != " ")
                    {
                        if (TabComparatif[0].Substring(11) != "\t")
                        {
                            NbPtPrestige[id] += Convert.ToInt16(TabComparatif[0].Substring(9));
                        }
                    }
                }
                
                lblNbPtPrestige.Text = "Nb pt prestige : " + NbPtPrestige[id];
                cmdNextPlayer.Visible = true;
                cmdNextPlayer.Enabled = true;
                cmdValidateChoice.Enabled = false;
                // disable card Level one
                txtLevel11.Enabled = false;
                txtLevel12.Enabled = false;
                txtLevel13.Enabled = false;
                txtLevel14.Enabled = false;
                // disable card Level two
                txtLevel21.Enabled = false;
                txtLevel22.Enabled = false;
                txtLevel23.Enabled = false;
                txtLevel24.Enabled = false;
                // disable card Level three
                txtLevel31.Enabled = false;
                txtLevel32.Enabled = false;
                txtLevel33.Enabled = false;
                txtLevel34.Enabled = false;
                // disable lblCoin
                lblRubisCoin.Enabled = false;
                lblSaphirCoin.Enabled = false;
                lblEmeraudeCoin.Enabled = false;
                lblOnyxCoin.Enabled = false;
                lblDiamandCoin.Enabled = false;

            }

            txtPlayerBookedCard.Enabled = false;
        }
        /// <summary>
        /// click on card to purchase this card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickOnCard(object sender, EventArgs e)
        {
            //We get the value on the card and we split it to get all the values we need (number of prestige points and ressource)
            //Enable the button "Validate"
            if (enableClicLabel)
            {

                TextBox cardOnBoard = (TextBox)sender;
                NameControlAll = cardOnBoard.Name;
                NameCard = cardOnBoard.Name.ToString();
                string[] TableauComparatif; 
                TableauComparatif = cardOnBoard.Lines;
                int NbLines = cardOnBoard.Lines.Count()-1;
                //Nombre de carte de ressource
                int NbCardRubis = Convert.ToInt16(txtPlayerRubisCard.Text);
                int NbCardSaphire = Convert.ToInt16(txtPlayerSaphireCard.Text);
                int NbCardOnyx = Convert.ToInt16(txtPlayerOnyxCard.Text);
                int NbCardEmeraude = Convert.ToInt16(txtPlayerEmeraudeCard.Text);
                int NbCardDiamant = Convert.ToInt16(txtPlayerDiamantCard.Text);
                //Nombre de ressources en stock
                int NbRubisPoss = Convert.ToInt16(lblPlayerRubisCoin.Text);
                int NbSaphirePoss = Convert.ToInt16(lblPlayerSaphireCoin.Text);
                int NbEmeraudePoss = Convert.ToInt16(lblPlayerEmeraudeCoin.Text);
                int NbOnyxPoss = Convert.ToInt16(lblPlayerOnyxCoin.Text);
                int NbDiamantPoss = Convert.ToInt16(lblPlayerDiamantCoin.Text);
                int NbGoldPoss = Convert.ToInt16(lblPlayerGoldCoin.Text);
                //Nombre de ressources restantes
                int BanqueRubis = Convert.ToInt16(lblRubisCoin.Text);
                int BanqueSaphire = Convert.ToInt16(lblSaphirCoin.Text);
                int BanqueEmeraude = Convert.ToInt16(lblEmeraudeCoin.Text);
                int BanqueOnyx = Convert.ToInt16(lblOnyxCoin.Text);
                int BanqueDiamant = Convert.ToInt16(lblDiamandCoin.Text);
                int BanqueGold = Convert.ToInt16(lblGoldCoin.Text);
                //Nombre de cartes restantes
                int NbCardResidualOne = listCardOne.Count();
                int NbCardResidualTwo = listCardTwo.Count();
                int NbCardResidualThree = listCardThree.Count();
                int NbCardResidualNoble = listCardNoble.Count();

                string SorteRessourceMiss = "Vous n'avez pas assez de ";
                int NbRessource = 2;
                int NbRessourceMiss = 0;
                //Boolean pour savoir si le joueur achète la carte ou non
                bool Achat = true;
                if (NameCard.Substring(0, 8) != "txtNoble")
                {
                    while (NbRessource != NbLines)
                    {
                        if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Rubis"))
                        {
                            if (NbRubisPoss + NbCardRubis >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(10)))
                            {
                                NbRubisPoss = NbRubisPoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(10)) + NbCardRubis;
                                lblRubisCoin.Text = BanqueRubis.ToString();
                            }
                            else
                            {
                                int NbRubisMiss = NbRubisPoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(10));
                                SorteRessourceMiss += "Rubis ";
                                Achat = false;
                                NbRessourceMiss++;
                            }
                        }
                        else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Saphire"))
                        {

                            if (NbSaphirePoss + NbCardSaphire >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(12)))
                            {
                                NbSaphirePoss = NbSaphirePoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(12)) + NbCardSaphire;
                                lblSaphirCoin.Text = BanqueSaphire.ToString();
                            }
                            else
                            {
                                int NbSaphireMiss = NbSaphirePoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(12));
                                SorteRessourceMiss += "Saphire ";
                                Achat = false;
                                NbRessourceMiss++;
                            }
                        }
                        else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Emeraude"))
                        {
                            if (NbEmeraudePoss + NbCardEmeraude >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(13)))
                            {
                                NbEmeraudePoss = NbEmeraudePoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(13)) + NbCardEmeraude;
                                lblEmeraudeCoin.Text = BanqueEmeraude.ToString();
                            }
                            else
                            {
                                int NbEmeraudeMiss = NbEmeraudePoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(13));
                                    SorteRessourceMiss += "Emeraude ";
                                Achat = false;
                                NbRessourceMiss++;
                            }
                        }
                        else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Onyx"))
                        {
                            if (NbOnyxPoss + NbCardOnyx >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(9)))
                            {
                                NbOnyxPoss = NbOnyxPoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(9)) + NbCardOnyx;
                                lblOnyxCoin.Text = BanqueOnyx.ToString();
                            }
                            else
                            {
                                int NbOnyxMiss = NbOnyxPoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(9));
                                SorteRessourceMiss += "Onyx ";
                                Achat = false;
                                NbRessourceMiss++;
                            }
                        }
                        else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Diamant"))
                        {
                            if (NbDiamantPoss + NbCardDiamant >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(12)))
                            {
                                NbDiamantPoss = NbDiamantPoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(12)) + NbCardDiamant;
                                lblDiamandCoin.Text = BanqueDiamant.ToString();
                            }
                            else
                            {
                                int NbDiamantMiss = NbDiamantPoss - Convert.ToInt16(TableauComparatif[NbRessource].Substring(12));
                                SorteRessourceMiss += "Diamant ";
                                Achat = false;
                                if (NbDiamantMiss <= 1)
                                NbRessourceMiss++;
                            }
                        }
                        
                        if (NbRessource + 1 != NbLines)
                        {
                            NbRessource++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (NbRessourceMiss == 1)
                    {
                        if(MessageBox.Show("il vous manque une ressource. Voulez-vous réserver la carte ?", "Message de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            lblGoldCoin.Text = "" + (BanqueGold - 1) + "";
                            lblPlayerGoldCoin.Text = "" + (NbGoldPoss + 1) + "";
                            Booked[id] = cardOnBoard.Text;
                            NbTour[id] = 1;
                            txtPlayerBookedCard.Text = cardOnBoard.Text;
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
                            cmdNextPlayer.Visible = true;
                            cmdNextPlayer.Enabled = true;
                            cmdValidateChoice.Enabled = false;
                            // disable card Level one
                            txtLevel11.Enabled = false;
                            txtLevel12.Enabled = false;
                            txtLevel13.Enabled = false;
                            txtLevel14.Enabled = false;
                            // disable card Level two
                            txtLevel21.Enabled = false;
                            txtLevel22.Enabled = false;
                            txtLevel23.Enabled = false;
                            txtLevel24.Enabled = false;
                            // disable card Level three
                            txtLevel31.Enabled = false;
                            txtLevel32.Enabled = false;
                            txtLevel33.Enabled = false;
                            txtLevel34.Enabled = false;
                            txtPlayerBookedCard.Enabled = false;
                            // disable lblCoin
                            lblRubisCoin.Enabled = false;
                            lblSaphirCoin.Enabled = false;
                            lblEmeraudeCoin.Enabled = false;
                            lblOnyxCoin.Enabled = false;
                            lblDiamandCoin.Enabled = false;
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


                        if (TableauComparatif[NbRessource].Substring(4).StartsWith("Rubis"))
                        {
                            nbCardRubis++;
                            BanqueRubis = BanqueRubis + Convert.ToInt16(TableauComparatif[NbRessource].Substring(10));
                            txtPlayerRubisCard.Text = nbCardRubis.ToString();

                            if (TableauComparatif[0].Substring(7) != " ")
                            {
                                if (TableauComparatif[0].Substring(7) != "\t")
                                {
                                    NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(7));
                                }
                            }
                        }
                        else if (TableauComparatif[NbRessource].Substring(4).StartsWith("Emeraude"))
                        {
                            nbCardEmeraude++;
                            BanqueEmeraude = BanqueEmeraude + Convert.ToInt16(TableauComparatif[NbRessource].Substring(13));
                            txtPlayerEmeraudeCard.Text = nbCardEmeraude.ToString();
                            if (TableauComparatif[0].Substring(10) != " ")
                            {
                                if (TableauComparatif[0].Substring(10) != "\t")
                                {
                                    NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(8));
                                }

                            }
                        }
                        else if (TableauComparatif[NbRessource].Substring(4).StartsWith("Onyx"))
                        {
                            nbCardOnyx++;
                            txtPlayerOnyxCard.Text = nbCardOnyx.ToString();
                            BanqueOnyx = BanqueOnyx + Convert.ToInt16(TableauComparatif[NbRessource].Substring(9));
                            if (TableauComparatif[0].Substring(6) != " ")
                            {
                                if (TableauComparatif[0].Substring(6) != "\t")
                                {
                                    NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(6));
                                }
                            }
                        }
                        else if (TableauComparatif[NbRessource].Substring(4).StartsWith("Saphire"))
                        {
                            nbCardSaphire++;
                            BanqueSaphire = BanqueSaphire + Convert.ToInt16(TableauComparatif[NbRessource].Substring(12));
                            txtPlayerSaphireCard.Text = nbCardSaphire.ToString();
                            if (TableauComparatif[0].Substring(9) != " ")
                            {
                                if (TableauComparatif[0].Substring(9) != "\t")
                                {
                                    NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(9));
                                }
                            }
                        }
                        else if (TableauComparatif[NbRessource].Substring(4).StartsWith("Diamant"))
                        {
                            nbCardDiamant++;
                            BanqueDiamant = BanqueDiamant + Convert.ToInt16(TableauComparatif[NbRessource].Substring(12));
                            txtPlayerDiamantCard.Text = nbCardDiamant.ToString();
                            if (TableauComparatif[0].Substring(9) != " ")
                            {
                                if (TableauComparatif[0].Substring(9) != "\t")
                                {
                                    NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(9));
                                }
                            }
                        }
                        else if (TableauComparatif[0].StartsWith(" "))
                        {
                            NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(2));
                        }

                        lblNbPtPrestige.Text = "Nb pt prestige : " + NbPtPrestige[id];
                        cmdNextPlayer.Visible = true;
                        cmdNextPlayer.Enabled = true;
                        cmdValidateChoice.Enabled = false;
                        // disable card Level one
                        txtLevel11.Enabled = false;
                        txtLevel12.Enabled = false;
                        txtLevel13.Enabled = false;
                        txtLevel14.Enabled = false;
                        // disable card Level two
                        txtLevel21.Enabled = false;
                        txtLevel22.Enabled = false;
                        txtLevel23.Enabled = false;
                        txtLevel24.Enabled = false;
                        // disable card Level three
                        txtLevel31.Enabled = false;
                        txtLevel32.Enabled = false;
                        txtLevel33.Enabled = false;
                        txtLevel34.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(SorteRessourceMiss);
                    }
                }
                else
                {
                    //Purchase of noble card
                    while (NbRessource != NbLines)
                    {
                        if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Rubis"))
                        {
                            if (NbCardRubis <= Convert.ToInt16(TableauComparatif[NbRessource].Substring(10)))
                            {
                                SorteRessourceMiss += "Rubis ";
                                Achat = false;
                            }
                        }
                        else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Saphire"))
                        {
                            if (NbCardSaphire <= Convert.ToInt16(TableauComparatif[NbRessource].Substring(12)))
                            {
                                SorteRessourceMiss += "Saphire ";
                                Achat = false;
                            }
                        }
                        else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Emeraude"))
                        {
                            if (NbCardEmeraude <= Convert.ToInt16(TableauComparatif[NbRessource].Substring(13)))
                            {
                                SorteRessourceMiss += "Emeraude ";
                                Achat = false;
                            }
                        }
                        else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Onyx"))
                        {
                            if (NbCardOnyx <= Convert.ToInt16(TableauComparatif[NbRessource].Substring(9)))
                            {
                                SorteRessourceMiss += "Onyx ";
                                Achat = false;
                            }
                        }
                        else if ((TableauComparatif[NbRessource].Substring(4)).StartsWith("Diamant"))
                        {
                            if (NbCardDiamant >= Convert.ToInt16(TableauComparatif[NbRessource].Substring(12)))
                            {
                                SorteRessourceMiss += "Diamant ";
                                Achat = false;
                            }
                        }

                        if (NbRessource + 1 != NbLines)
                        {
                            NbRessource++;
                        }
                        else
                        {
                            break;
                        }
                        if (Achat)
                        {
                            cardOnBoard.Clear();

                            if (TableauComparatif[NbRessource].StartsWith("Rubis"))
                            {
                                nbCardRubis++;
                                txtPlayerRubisCard.Text = nbCardRubis.ToString();

                                if (TableauComparatif[0].Substring(7) != " ")
                                {
                                    if (TableauComparatif[0].Substring(9) != "\t")
                                    {
                                        NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(7));
                                    }
                                }
                            }
                            else if (TableauComparatif[NbRessource].StartsWith("Emeraude"))
                            {
                                nbCardEmeraude++;
                                txtPlayerEmeraudeCard.Text = nbCardEmeraude.ToString();
                                if (TableauComparatif[0].Substring(10) != " ")
                                {
                                    if (TableauComparatif[0].Substring(12) != "\t")
                                    {
                                        NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(8));
                                    }

                                }
                            }
                            else if (TableauComparatif[NbRessource].StartsWith("Onyx"))
                            {
                                nbCardOnyx++;
                                txtPlayerOnyxCard.Text = nbCardOnyx.ToString();
                                if (TableauComparatif[0].Substring(6) != " ")
                                {
                                    if (TableauComparatif[0].Substring(8) != "\t")
                                    {
                                        NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(6));
                                    }
                                }
                            }
                            else if (TableauComparatif[NbRessource].StartsWith("Saphire"))
                            {
                                nbCardSaphire++;
                                txtPlayerSaphireCard.Text = nbCardSaphire.ToString();
                                if (TableauComparatif[0].Substring(9) != " ")
                                {
                                    if (TableauComparatif[0].Substring(11) != "\t")
                                    {
                                        NbPtPrestige[id] += Convert.ToInt16(TableauComparatif[0].Substring(9));
                                    }
                                }
                            }
                            else if (TableauComparatif[NbRessource].StartsWith("Diamant"))
                            {
                                nbCardDiamant++;
                                txtPlayerDiamantCard.Text = nbCardDiamant.ToString();
                                if (TableauComparatif[0].Substring(9) != " ")
                                {
                                    if (TableauComparatif[0].Substring(11) != "\t")
                                    {
                                        NbPtPrestige[id] = Convert.ToInt16(TableauComparatif[0].Substring(9));
                                    }
                                }
                            }
                            else if (TableauComparatif[0].StartsWith(" "))
                            {
                                NbPtPrestige[id] = Convert.ToInt16(TableauComparatif[0].Substring(2));
                            }

                            lblNbPtPrestige.Text = "Nb pt prestige : " + NbPtPrestige[id];
                            cmdNextPlayer.Visible = true;
                            cmdNextPlayer.Enabled = true;
                            cmdValidateChoice.Enabled = false;
                            // disable card Level one
                            txtLevel11.Enabled = false;
                            txtLevel12.Enabled = false;
                            txtLevel13.Enabled = false;
                            txtLevel14.Enabled = false;
                            // disable card Level two
                            txtLevel21.Enabled = false;
                            txtLevel22.Enabled = false;
                            txtLevel23.Enabled = false;
                            txtLevel24.Enabled = false;
                            // disable card Level three
                            txtLevel31.Enabled = false;
                            txtLevel32.Enabled = false;
                            txtLevel33.Enabled = false;
                            txtLevel34.Enabled = false;
                            // disable card Noble
                            txtNoble1.Enabled = false;
                            txtNoble2.Enabled = false;
                            txtNoble3.Enabled = false;
                            txtNoble4.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show(SorteRessourceMiss);
                        }
                    }
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
            int NbPlayer = conn.GetCountPlayer();
            switch (NbPlayer)
            {
                case 2:
                    NbJeton = 4;
                    break;
                case 3:
                    NbJeton = 5;
                    break;

                case 4:
                    NbJeton = 7;
                    break;
            }

            lblDiamandCoin.Text = NbJeton.ToString();
            lblEmeraudeCoin.Text = NbJeton.ToString();
            lblOnyxCoin.Text = NbJeton.ToString();
            lblRubisCoin.Text = NbJeton.ToString();
            lblSaphirCoin.Text = NbJeton.ToString();

            this.Width = 680;
            this.Height = 780;

            cmdInsertPlayer.Enabled = false;

            conn.NumberPlayer = conn.GetCountPlayer();

            Coin = new int[conn.NumberPlayer, 6];
            Coin[id, 0] = 0;
            Coin[id, 1] = 0;
            Coin[id, 2] = 0;
            Coin[id, 3] = 0;
            Coin[id, 4] = 0;
            Coin[id, 5] = 0;

            NbPtPrestige = new int[conn.GetCountPlayer()];
            NbPtPrestige[0] = 0;
            NbPtPrestige[1] = 0;
            if (conn.GetCountPlayer()-1 > 1)
            {
                NbPtPrestige[2] = 0;
                if (conn.GetCountPlayer()-1 > 2)
                {
                    NbPtPrestige[3] = 0;
                }
            }

            Ressource = new int[conn.NumberPlayer, 5];
            Ressource[id, 0] = 0;
            Ressource[id, 1] = 0;
            Ressource[id, 2] = 0;
            Ressource[id, 3] = 0;
            Ressource[id, 4] = 0;
            //Ressource[id, 5] = 0;

            LoadPlayer(id);

        }


        /// <summary>
        /// load data about the current player
        /// </summary>
        /// <param name="id">identifier of the player</param>
        private void LoadPlayer(int id) {
            txtPlayerBookedCard.Clear();
            currentPlayerId = id;
            enableClicLabel = true;
            if (conn.GetPlayerName(id) == "")
            {
                currentPlayerId++;
            }
            Nom = conn.GetPlayerName(currentPlayerId);

            //no coins or card selected yet, labels are empty
            lblChoiceDiamant.Text = "";
            lblChoiceOnyx.Text = "";
            lblChoiceRubis.Text = "";
            lblChoiceSaphire.Text = "";
            lblChoiceEmeraude.Text = "";

            lblChoiceCard.Text = "";

            //no coins selected
            nbDiamand = 0;
            nbOnyx = 0;
            nbRubis = 0;
            nbSaphir = 0;
            nbEmeraude = 0;

            //Initialisation du joueur
            Player player = new Player();
            player.Name = Nom;
            player.Id = id;
            player.Ressources = new int[] { Ressource[id, 0], Ressource[id, 1], Ressource[id, 2], Ressource[id, 3], Ressource[id, 4]};
            player.Coins = new int[] { Coin[id, 0], Coin[id, 1], Coin[id, 2], Coin[id, 3], Coin[id, 4], Coin[id, 5] };

            if (Booked[id] != null)
            {
                //enable card booked
                txtPlayerBookedCard.Enabled = true;
                txtPlayerBookedCard.Text = Booked[id];
            }

            lblPlayerRubisCoin.Text = player.Coins[0].ToString();
            lblPlayerSaphireCoin.Text = player.Coins[1].ToString();
            lblPlayerOnyxCoin.Text = player.Coins[2].ToString();
            lblPlayerEmeraudeCoin.Text = player.Coins[3].ToString();
            lblPlayerDiamantCoin.Text = player.Coins[4].ToString();
            //Gold Coin
            lblPlayerGoldCoin.Text = player.Coins[5].ToString();
            lblNbPtPrestige.Text = NbPtPrestige[id].ToString();

            txtPlayerRubisCard.Text = player.Ressources[0].ToString();
            txtPlayerSaphireCard.Text = player.Ressources[1].ToString();
            txtPlayerOnyxCard.Text = player.Ressources[2].ToString();
            txtPlayerEmeraudeCard.Text = player.Ressources[3].ToString();
            txtPlayerDiamantCard.Text = player.Ressources[4].ToString();
            //Gold coin
            //lxtPlayerDiamantCard.Text = player.Ressources[4].ToString();


            lblPlayer.Text = "Plateau de jeu de " + Nom;

            cmdPlay.Enabled = false;
        }
        /// <summary>
        /// Test number of coins 
        /// </summary>
        /// <param name="LabelChoix"></param>
        /// <param name="LabelJeton"></param>
        /// <param name="nbJeton"></param>
        /// <param name="TypeJeton"></param>
        void TestJetons(Label LabelChoix, Label LabelJeton, int nbJeton, string TypeJeton) //méthode test des jetons
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
                    LabelChoix.Enabled = false;
                }
                else
                {
                    if ((jeton == 1 && nbSaphir == 1) || (jeton == 1 && nbOnyx == 1) || (jeton == 1 && nbEmeraude == 1) || (jeton == 1 && nbDiamand == 1) || (jeton == 1 && nbRubis == 1))
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre un deuxième jeton de la même couleur si vous avez déjà choisi un jeton de couleur différente");
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

                                case "Saphire": nbSaphir++; CoinsPlayer[1]++; break;

                                case "Emeraude": nbEmeraude++; CoinsPlayer[2]++; break;

                                case "Onyx": nbOnyx++; CoinsPlayer[3]++; break;

                                case "Diamant": nbDiamand++; CoinsPlayer[4]++; break;
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
                lblChoiceSaphire.Visible = true;
                TestJetons(lblChoiceSaphire, lblSaphirCoin, nbSaphir, "Saphire");
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
                cmdValidateChoice.Visible = true;
                lblChoiceDiamant.Visible = true;
                TestJetons(lblChoiceDiamant, lblDiamandCoin, nbDiamand, "Diamant");
            }
        }

        /// <summary>
        /// Verif number of coins when we click on validate button
        /// </summary>
        /// <param name="LabelChoice"></param>
        /// <param name="LabelCoin"></param>
        /// <param name="LabelPlayerCoin"></param>
        /// <param name="NbJetonsActif"></param>
        private void VerifValidateChoice(Label LabelChoice, Label LabelCoin, Label LabelPlayerCoin, int NbJetonsActif)
        {
            int Jetons = Convert.ToInt32(LabelChoice.Text);
            int JetonsPlayerCoins = Convert.ToInt32(LabelPlayerCoin.Text);
            int JetonCoins = Convert.ToInt32(LabelCoin.Text);
            int Result = Jetons + JetonsPlayerCoins;

            if (JetonCoins < 4)
            {
                if (Jetons == 2)
                {
                    MessageBox.Show("Vous ne pouvez pas prendre 2 de ces jetons car il en reste moins de 4");
                }
            }
            nbTotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;

            if ((nbDiamand == 2 || nbRubis == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbSaphir == 2) && nbTotal == 3)
            {
                MessageBox.Show("Vous ne pouvez pas prendre ce jeton car vous en avez pris 2 d'une autre pierre précieuse");
            }
            else
            {
                if(NbJetonsActif != 3 && NbJetonsActif != 1)
                {
                    if (((Jetons == 1 && nbRubis == 1) || (Jetons == 1 && nbOnyx == 1) || (Jetons == 1 && nbEmeraude == 1) || (Jetons == 1 && nbSaphir == 1) || (Jetons == 1 && nbDiamand == 1)) || (nbRubis == 1 || nbSaphir == 1 || nbEmeraude == 1 || nbOnyx == 1 || nbDiamand == 1))
                    {
                        MessageBox.Show("Vous ne pouvez pas faire ceci");
                    }
                }
                else
                {
                    nbTotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                    if (nbTotal >= 3 || nbTotal == 2)
                    {
                        //MessageBox.Show("Vous avez pris le nombre de jetons maximum");
                        //LabelCoin.Text = Result.ToString();
                        LabelChoice.Text = "";
                        LabelChoice.Visible = false;
                        JetonsPlayerCoins += Jetons;
                        LabelPlayerCoin.Text = JetonsPlayerCoins.ToString();
                        cmdNextPlayer.Visible = true;
                        cmdNextPlayer.Enabled = true;
                        cmdValidateChoice.Visible = false;
                        // disable card Level one
                        txtLevel11.Enabled = false;
                        txtLevel12.Enabled = false;
                        txtLevel13.Enabled = false;
                        txtLevel14.Enabled = false;
                        // disable card Level two
                        txtLevel21.Enabled = false;
                        txtLevel22.Enabled = false;
                        txtLevel23.Enabled = false;
                        txtLevel24.Enabled = false;
                        // disable card Level three
                        txtLevel31.Enabled = false;
                        txtLevel32.Enabled = false;
                        txtLevel33.Enabled = false;
                        txtLevel34.Enabled = false;
                        // disable lblCoin
                        lblRubisCoin.Enabled = false;
                        lblSaphirCoin.Enabled = false;
                        lblEmeraudeCoin.Enabled = false;
                        lblOnyxCoin.Enabled = false;
                        lblDiamandCoin.Enabled = false;
                    }
                    else
                    {
                        JetonCoins++;
                        Jetons--;
                        LabelCoin.Text = JetonCoins.ToString();
                        LabelChoice.Text = "";
                        LabelChoice.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// click on the validate button to approve the selection of coins or card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdValidateChoice_Click(object sender, EventArgs e)
        {
            int NbLblVisible = 0;
            
            if(lblChoiceRubis.Text != "")
            {
                NbLblVisible++;
            }
            if (lblChoiceSaphire.Text != "")
            {
                NbLblVisible++;
            }
            if (lblChoiceOnyx.Text != "")
            {
                NbLblVisible++;
            }
            if (lblChoiceEmeraude.Text != "")
            {
                NbLblVisible++;
            }
            if (lblChoiceDiamant.Text != "")
            {
                NbLblVisible++;
            }

            for(int i = 0; i< NbLblVisible; i++)
            {
                if (lblChoiceRubis.Text != "")
                {
                    VerifValidateChoice(lblChoiceRubis, lblRubisCoin, lblPlayerRubisCoin, NbLblVisible);
                }
                if (lblChoiceSaphire.Text != "")
                {
                    VerifValidateChoice(lblChoiceSaphire, lblSaphirCoin, lblPlayerSaphireCoin, NbLblVisible);
                }
                if (lblChoiceOnyx.Text != "")
                {
                    VerifValidateChoice(lblChoiceOnyx, lblOnyxCoin, lblPlayerOnyxCoin, NbLblVisible);
                }
                if (lblChoiceEmeraude.Text != "")
                {
                    VerifValidateChoice(lblChoiceEmeraude, lblEmeraudeCoin, lblPlayerEmeraudeCoin, NbLblVisible);
                }
                if (lblChoiceDiamant.Text != "")
                {
                    VerifValidateChoice(lblChoiceDiamant, lblDiamandCoin, lblPlayerDiamantCoin, NbLblVisible);
                }
            }
        }

        /// <summary>
        /// click on the insert button to insert player in the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdInsertPlayer_Click(object sender, EventArgs e)
        {

            form2 = new FormAddPlayer();
            form2.Show();

           // MessageBox.Show("A implémenter");
        }

        /// <summary>
        /// Verif if the player actual click on NextPlayer button are 15 Point of Prestige
        /// </summary>
        /// <returns></returns>
        private bool PlayerFinish()
        {
            bool Finish = true;
            if(NbPtPrestige[id] <= 15)
            {
                Finish = true;
            }
            else
            {
                Finish = false;
            }
           
            return Finish;
        }

        /// <summary>
        /// click on the next player to tell him it is his turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNextPlayer_Click(object sender, EventArgs e)
        {
            if (PlayerFinish())
            {
                cmdValidateChoice.Visible = false;
                cmdValidateChoice.Enabled = true;
                // enable card Level one
                txtLevel11.Enabled = true;
                txtLevel12.Enabled = true;
                txtLevel13.Enabled = true;
                txtLevel14.Enabled = true;
                // enable card Level two
                txtLevel21.Enabled = true;
                txtLevel22.Enabled = true;
                txtLevel23.Enabled = true;
                txtLevel24.Enabled = true;
                // enable card Level three
                txtLevel31.Enabled = true;
                txtLevel32.Enabled = true;
                txtLevel33.Enabled = true;
                txtLevel34.Enabled = true;
                //enable card Noble except cards already purchased
                txtNoble1.Enabled = true;
                txtNoble2.Enabled = true;
                txtNoble3.Enabled = true;
                txtNoble4.Enabled = true;
                // disable lblCoin
                lblRubisCoin.Enabled = true;
                lblSaphirCoin.Enabled = true;
                lblEmeraudeCoin.Enabled = true;
                lblOnyxCoin.Enabled = true;
                lblDiamandCoin.Enabled = true;

                Coin[id, 0] = Convert.ToInt16(lblPlayerRubisCoin.Text);
                Coin[id, 1] = Convert.ToInt16(lblPlayerSaphireCoin.Text);
                Coin[id, 2] = Convert.ToInt16(lblPlayerOnyxCoin.Text);
                Coin[id, 3] = Convert.ToInt16(lblPlayerEmeraudeCoin.Text);
                Coin[id, 4] = Convert.ToInt16(lblPlayerDiamantCoin.Text);
                //Coin[id, 5] = Convert.ToInt16(lblPlayerGoldCoin.Text);

                Ressource[id, 0] = Convert.ToInt16(txtPlayerRubisCard.Text);
                Ressource[id, 1] = Convert.ToInt16(txtPlayerSaphireCard.Text);
                Ressource[id, 2] = Convert.ToInt16(txtPlayerOnyxCard.Text);
                Ressource[id, 3] = Convert.ToInt16(txtPlayerEmeraudeCard.Text);
                Ressource[id, 4] = Convert.ToInt16(txtPlayerDiamantCard.Text);

                if (id + 1 < conn.NumberPlayer)
                {
                    if (id + 1 != conn.NumberPlayer && Reid == true)
                    {
                        id++;
                    }
                    Reid = true;
                    cmdNextPlayer.Visible = false;
                    LoadPlayer(id);
                }
                else if (id == conn.NumberPlayer - 1)
                {
                    Reid = true;
                    id = -1;
                    id++;
                    cmdNextPlayer.Visible = false;
                    LoadPlayer(id);
                }
            }
            else
            {
                if(MessageBox.Show("Bravo " + Nom + " tu as gagné. ", "Message de fermeture", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    this.Close();
                }
            }
        }
        /// <summary>
        /// Back coins when we click on label choice
        /// </summary>
        /// <param name="LabelChoice"></param>
        /// <param name="LabelJeton"></param>
        /// <param name="NbJeton"></param>
        /// <param name="TypeCoin"></param>
        private void JetonsBack(Label LabelChoice, Label LabelJeton, int NbJeton, string TypeCoin)
        {
            int JetonsChoice = Convert.ToInt32(LabelChoice.Text) - 1;
            int JetonsDispo = Convert.ToInt32(LabelJeton.Text) + 1;

            if (JetonsChoice >= 0)
            {
                NbJeton--;
                LabelChoice.Text = JetonsChoice.ToString();
                LabelJeton.Text = JetonsDispo.ToString();

                if (JetonsChoice == 0)
                {
                    LabelChoice.Visible = false;
                }
            }
            if (lblChoiceDiamant.Visible == false && lblChoiceEmeraude.Visible == false && lblChoiceOnyx.Visible == false && lblChoiceRubis.Visible == false && lblChoiceSaphire.Visible == false)
            {
                cmdValidateChoice.Visible = false;
            }

            switch (TypeCoin)
            {
                case "Rubis": nbRubis--; break;
                case "Saphire": nbSaphir--; break;
                case "Onyx": nbOnyx--; break;
                case "Emeraude": nbEmeraude--; break;
                case "Diamant": nbDiamand--; break;

            }
        }
        /// <summary>
        /// Label (Rubis) click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceRubis_Click(object sender, EventArgs e)
        {
            JetonsBack(lblChoiceRubis, lblRubisCoin, nbRubis, "Rubis");
        }
        /// <summary>
        /// Label (Saphire) click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceSaphir_Click(object sender, EventArgs e)
        {
            JetonsBack(lblChoiceSaphire, lblSaphirCoin, nbSaphir, "Saphire");
        }
        /// <summary>
        /// Label (Onyx) click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceOnyx_Click(object sender, EventArgs e)
        {
            JetonsBack(lblChoiceOnyx, lblOnyxCoin, nbOnyx, "Onyx");
        }
        /// <summary>
        /// Label (Emeraude) click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceEmeraude_Click(object sender, EventArgs e)
        {
            JetonsBack(lblChoiceEmeraude, lblEmeraudeCoin, nbEmeraude, "Emeraude");
        }
        /// <summary>
        /// Label (Diamant) click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceDiamand_Click(object sender, EventArgs e)
        { 
            JetonsBack(lblChoiceDiamant, lblDiamandCoin, nbDiamand, "Diamant");
        }
    }
}