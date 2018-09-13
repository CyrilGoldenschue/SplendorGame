﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Splendor
{
    /// <summary>
    /// contains methods and attributes to connect and deal with the database
    /// TO DO : le modèle de données n'est pas super, à revoir!!!!
    /// </summary>
    class ConnectionDB
    {
        //connection to the database
        private SQLiteConnection m_dbConnection;

        /// <summary>
        /// constructor : creates the connection to the database SQLite
        /// </summary>
        public ConnectionDB()
        {
            if (File.Exists("Splendor.sqlite"))
            {
                m_dbConnection = new SQLiteConnection("Data Source=Splendor.sqlite;Version=3;");
                m_dbConnection.Open();
            }
            else
            {
                SQLiteConnection.CreateFile("Splendor.sqlite");
                m_dbConnection = new SQLiteConnection("Data Source=Splendor.sqlite;Version=3;");
                m_dbConnection.Open();
                //create and insert players
                CreateInsertPlayer();
                //Create and insert cards
                //TO DO
                CreateInsertCards();
                //Create and insert ressources
                //TO DO
                CreateInsertRessources();
            }

            

            
        }


        /// <summary>
        /// get the list of cards according to the level
        /// </summary>
        /// <returns>cards stack</returns>
        public Stack<Card> GetListCardAccordingToLevel(int level)
        {
            //Get all the data from card table selecting them according to the data
            //TO DO
            //Create an object "Stack of Card"
            Stack<Card> listCard = new Stack<Card>();

            string sql = "select level, fkRessource, nbPtPrestige, idCard from card where level = " + level;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            
            while (reader.Read())
            {
                string ressourceCard = "select nbRessource, fkRessource from cost where fkCard = " + reader["idCard"];
                SQLiteCommand commandCard = new SQLiteCommand(ressourceCard, m_dbConnection);
                SQLiteDataReader readerCard = commandCard.ExecuteReader();

                Card carte = new Card();
                carte.Level = (int)reader["level"];          
                carte.Ress = (Ressources)reader["fkRessource"];
                carte.PrestigePt = (int)reader["nbPtPrestige"];
                while (readerCard.Read())
                {
                    int rubis;
                    int emeraude;
                    int onyx;
                    int saphire;
                    int diamant;

                    if ((int)readerCard["fkRessource"] == 1)
                    {
                        rubis = (int)readerCard["nbRessource"];
                    }
                    else
                    {
                        rubis = 0;
                    }
                    if ((int)readerCard["fkRessource"] == 2)
                    {
                        emeraude = (int)readerCard["nbRessource"];
                    }
                    else
                    {
                        emeraude = 0;
                    }
                    if ((int)readerCard["fkRessource"] == 3)
                    {
                        onyx = (int)readerCard["nbRessource"];
                    }
                    else
                    {
                        onyx = 0;
                    }
                    if ((int)readerCard["fkRessource"] == 4)
                    {
                        saphire = (int)readerCard["nbRessource"];
                    }
                    else
                    {
                        saphire = 0;
                    }
                    if ((int)readerCard["fkRessource"] == 5)
                    {
                        diamant = (int)readerCard["nbRessource"];
                    }
                    else
                    {
                        diamant = 0;
                    }
                        carte.Cout = new int[] { rubis, emeraude, onyx, saphire, diamant};
                }
                listCard.Push(carte);
            }


            //do while to go to every record of the card table
            //while (....)
            //{
            //Get the ressourceid and the number of prestige points
            //Create a card object

            //select the cost of the card : look at the cost table (and other)

            //do while to go to every record of the card table
            //while (....)
            //{
            //get the nbRessource of the cost
            //}
            //push card into the stack

            //}
            return listCard;


            
        }


        /// <summary>
        /// create the "player" table and insert data
        /// </summary>
        private void CreateInsertPlayer()
        {
            string sql = "CREATE TABLE player (idPlayer INT PRIMARY KEY, pseudo VARCHAR(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into player (idPlayer, pseudo) values (0, 'Fred')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into player (idPlayer, pseudo) values (1, 'Harry')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into player (idPlayer, pseudo) values (2, 'Sam')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        
        /// <summary>
        /// get the name of the player according to his id
        /// </summary>
        /// <param name="id">id of the player</param>
        /// <returns></returns>
        public string GetPlayerName(int id)
        {
            string sql = "select pseudo from player where idPlayer = " + id;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string name = "";
            while (reader.Read())
            {
                name = reader["pseudo"].ToString();
            }
            return name;
        }

        /// <summary>
        /// create the table "ressources" and insert data
        /// </summary>
        private void CreateInsertRessources()
        {
            string sql = "CREATE TABLE ressource (idRessource INT PRIMARY KEY, NameRessource VARCHAR(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into ressource(idRessource, NameRessource) values(1, 'Rubis')";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into ressource(idRessource, NameRessource) values(2, 'Emeraude')";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into ressource(idRessource, NameRessource) values(3, 'Onyx')";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into ressource(idRessource, NameRessource) values(4, 'Saphire')";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into ressource(idRessource, NameRessource) values(5, 'Diamant')";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
        }

        /// <summary>
        ///  create tables "cards", "cost" and insert data
        /// </summary>
        private void CreateInsertCards()
        {
            string sql = "CREATE TABLE card (idCard INT PRIMARY KEY, fkRessource INT, level INT, nbPtPrestige INT)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(2, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(3, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(4, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(5, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(6, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(7, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(8, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(9, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(10, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(11, 0, 4, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(12, 4, 3, 5)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(13, 3, 3, 5)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(14, 2, 3, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(15, 5, 3, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(16, 1, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(17, 2, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(18, 5, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(19, 5, 3, 5)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(20, 1, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(21, 4, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(22, 2, 3, 5)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(23, 3, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(24, 1, 3, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(25, 4, 3, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(26, 2, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(27, 3, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(28, 4, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(29, 1, 3, 5)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(30, 5, 3, 4)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(31, 3, 3, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(32, 5, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(33, 1, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(34, 5, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(35, 5, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(36, 5, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(37, 2, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(38, 4, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(39, 4, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(40, 2, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(41, 2, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(42, 3, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(43, 1, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(44, 5, 2, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(45, 4, 2, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(46, 2, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(47, 3, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(48, 1, 2, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(49, 4, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(50, 3, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(51, 2, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(52, 4, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(53, 1, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(54, 1, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(55, 3, 2, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(56, 4, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(57, 3, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(58, 1, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(59, 5, 2, 2)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(60, 2, 2, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(61, 3, 2, 3)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(62, 3, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(63, 2, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(64, 1, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(65, 5, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(66, 4, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(67, 5, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(68, 5, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(69, 5, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(70, 5, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(71, 5, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(72, 5, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(73, 5, 1, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(74, 1, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(75, 1, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(76, 1, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(77, 1, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(78, 1, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(79, 1, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(80, 1, 1, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(81, 3, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(82, 3, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(83, 3, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(84, 3, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(85, 3, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(86, 3, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(87, 3, 1, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(88, 4, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(89, 4, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(90, 4, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(91, 4, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(92, 4, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(93, 4, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(94, 4, 1, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(95, 2, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(96, 2, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(97, 2, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(98, 2, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(99, 2, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(100, 2, 1, 0)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into card(idcard, fkRessource, level, nbPtPrestige) values(101, 2, 1, 1)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();


            sql = "CREATE TABLE cost (idCost INT PRIMARY KEY, fkCard INT, fkRessource INT, nbRessource INT)";
            command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();

            // Rubis

            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (3, 0,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (6, 0,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (7, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (9, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (11, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (13, 0,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (14, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (15, 0,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (16, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (23, 0,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (25, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (27, 0,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (29, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (30, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (31, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (32, 0,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (33, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (34, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (35, 0,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (36, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (38, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (39, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (42, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (48, 0,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (51, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (53, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (57, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (59, 0,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (62, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (63, 0,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (64, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (66, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (67, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (70, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (72, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (76, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (81, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (84, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (85, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (86, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (88, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (91, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (93, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (94, 0,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (96, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (97, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (98, 0,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (100, 0,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();

            // Emeraude


            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (2, 1,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (3, 1,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (8, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (9, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (11, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (15, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (16, 1,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (17, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (20, 1,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (22, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (24, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (25, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (27, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (29, 1,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (31, 1,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (34, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (35, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (37, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (39, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (41, 1,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (42, 1,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (47, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (49, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (51, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (55, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (57, 1,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (58, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (60, 1,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (62, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (66, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (70, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (71, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (72, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (73, 1,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (74, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (77, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (78, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (79, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (82, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (83, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (84, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (85, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (86, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (88, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (91, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (92, 1,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (93, 1,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (95, 1,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();

            // Onyx

            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (4, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (5, 2,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (6, 2,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (7, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (11, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (13, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (14, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (15, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (18, 2,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (19, 2,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (21, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (24, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (25, 2,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (27, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (30, 2,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (33, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (34, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (35, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (38, 2,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (40, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (43, 2,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (46, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (47, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (49, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (53, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (54, 2,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (59, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (61, 2,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (62, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (64, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (65, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (66, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (67, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (68, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (70, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (71, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (72, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (74, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (78, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (79, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (88, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (89, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (90, 2,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (92, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (96, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (97, 2,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (100, 2,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();

            // Saphire

            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (2, 3,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (4, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (8, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (9, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (10, 3,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (12, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (14, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (15, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (16, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (17, 3,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (21, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (22, 3,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (24, 3,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (26, 3,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (31, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (33, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (36, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (37, 3,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (39, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (40, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (45, 3,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (46, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (49, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (52, 3,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (55, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (56, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (57, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (58, 3,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (65, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (68, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (69, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (70, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (71, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (72, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (74, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (77, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (79, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (81, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (85, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (86, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (87, 3,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (93, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (95, 3,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (96, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (97, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (98, 3,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (99, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (100, 3,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (101, 3,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();

            // Diamant

            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (4, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (5, 4,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (7, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (8, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (10, 4,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (12, 4,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (14, 4,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (17, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (19, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (21, 4,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (24, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (25, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (28, 4,7)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (30, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (31, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (36, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (38, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (40, 4,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (43, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (44, 4,6)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (46, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (47, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (50, 4,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (51, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (53, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (55, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (56, 4,5)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (58, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (64, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (65, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (66, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (74, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (75, 4,3)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (76, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();  
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (78, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (79, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (80, 4,4)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (81, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (82, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (85, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (86, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (88, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (89, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (91, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (95, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (97, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (99, 4,2)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();
            sql = "insert into cost(fkCard, fkRessource, nbRessource) values (100, 4,1)"; command = new SQLiteCommand(sql, m_dbConnection); command.ExecuteNonQuery();

        }

    }
}
