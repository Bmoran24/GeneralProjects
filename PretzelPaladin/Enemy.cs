using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PretzelPaladin
{
    internal class Enemy : Character
    {
        // Fields --

        private Random random;

        // Constructor
        /// <summary>
        /// Creates a new Enemy
        /// </summary>
        /// <param name="characterImage">Texture of the enemy</param>
        /// <param name="name">Enemy's name</param>
        /// <param name="maxHealth">Starting/Max health</param>
        /// <param name="currentHealth">Current health</param>
        /// <param name="attackMultiplier">Multiplier that boosts attack damage</param>
        /// <param name="defenseMultiplier">Multiplier that reduces incoming damage</param>
        public Enemy(Texture2D characterImage, string name, int maxHealth, int attackMultiplier, int defenseMultiplier)
            : base(characterImage, name, maxHealth, attackMultiplier, defenseMultiplier)
        {
            random = new Random();
        }
    }
}
