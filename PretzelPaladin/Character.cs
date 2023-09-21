using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace PretzelPaladin
{
    internal class Character
    {
        // Fields --
        private string name;
        private int maxHealth; 
        private int currentHealth;
        private int attackMultiplier;
        private int defenseMultiplier;
        private string damageDealt;

        private List<Move> moves;  
        private Texture2D characterImage;

        // Properties --

        /// <summary>
        /// Name of the character
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Character's Max/Starting health
        /// </summary>
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        /// <summary>
        /// Current health of the character
        /// </summary>
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        /// <summary>
        /// Texture of the Character
        /// </summary>
        public Texture2D CharacterImage { get { return characterImage; } }

        /// <summary>
        /// Attack Multiplier that increases damage
        /// </summary>
        public int AttackMultiplier { get { return attackMultiplier; } set { attackMultiplier = value; } }

        /// <summary>
        /// Defense Multiplier that reduces incoming damage
        /// </summary>
        public int DefenseMultiplier { get { return defenseMultiplier; } set { defenseMultiplier = value; } }

        /// <summary>
        /// Get's the amount of damage the move did
        /// </summary>
        public string DamageDealt
        {
            get
            {
                return damageDealt;
            }
        }
        // Constructor --

        /// <summary>
        /// Creates a new Character
        /// </summary>
        /// <param name="characterImage">Texture of the character</param>
        /// <param name="name">Character's name</param>
        /// <param name="maxHealth">Character's Max/Start health</param>
        /// <param name="currentHealth">Character's current health</param>
        /// <param name="attackMultiplier">Attack multiplier that boosts or reduces damage</param>
        /// <param name="defenseMultiplier">Defense multiplier that reduces or boosts incoming damage</param>
        public Character(Texture2D characterImage, string name, int maxHealth, int attackMultiplier, int defenseMultiplier)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            this.currentHealth = maxHealth;
            this.attackMultiplier = attackMultiplier;
            this.defenseMultiplier = defenseMultiplier;
            this.characterImage = characterImage;
            damageDealt = "";
            StreamReader file = new StreamReader("../../../Content/MoveList.txt");
            moves = new List<Move>();

            string currentLine = file.ReadLine();

            while(currentLine!=null)
            {
                string[] components = currentLine.Split(",");

                Move move = new Move(components[0], int.Parse(components[1]), components[2]);

                moves.Add(move);

                currentLine = file.ReadLine();
            }

            file.Close();
        }

        // Methods --

        /// <summary>
        /// Character takes damage from another, the amount of which could be 
        /// possibly reduced by their defense multiplier
        /// </summary>
        /// <param name="amtDamage">Amount of damage inflicted</param>
        public  void TakeDamage(int amtDamage)
        {
            Random rng = new Random();
            int damageTaken = rng.Next(amtDamage/2, amtDamage);

            currentHealth -= damageTaken;
            // Set our string variable to how much damage was taken so we can print it out on screen how 
            // much damage the move did
            damageDealt = damageTaken.ToString();
        }

        

    }
}

