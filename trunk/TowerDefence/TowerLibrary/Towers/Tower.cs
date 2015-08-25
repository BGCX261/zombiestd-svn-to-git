using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TowerLibrary
{
    public class Tower : Sprite
    {

        /// <summary> The sound bank </summary>
        protected SoundBank soundBank;
        
        protected int range = 80;

        protected float bulletTimer;
        protected float bulletSpeed;

        protected int tier;

        protected int damage;
        protected int cost;

        protected int baseDamage;
        protected int baseCost;

        protected int costNextLevel;

        protected bool attacking = false;
        protected Enemy target;

        protected Texture2D bulletTexture;
        protected List<Bullet> bulletList = new List<Bullet>();

        protected bool selected = false;

        protected float currentSpeed;
        protected float newSpeed;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets target for the Enemies. </summary>
        ///
        /// <value> The target. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Enemy Target
        {
            get { return target; }
            set { target = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the range. </summary>
        ///
        /// <value> The range. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Range
        {
            get { return range; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the cost. </summary>
        ///
        /// <value> The cost. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Cost
        {
            get { return cost; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the cost next level. </summary>
        ///
        /// <value> The cost next level. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int CostNextLevel
        {
            get { return costNextLevel; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the tier. </summary>
        ///
        /// <value> The tier. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int Tier
        {
            get { return tier; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets a value indicating whether the selected. </summary>
        ///
        /// <value> true if selected, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Query if 'position' is in range. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="position"> The position. </param>
        ///
        /// <returns>   true if in range, false if not. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool IsInRange(Vector2 position)
        {
            if (Vector2.Distance(center, position) <= range)
                return true;
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets a closest enemy. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="enemies">  The enemies. </param>
        ///
        /// <returns>   The closest enemy. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Enemy GetClosestEnemy(List<Enemy> enemies)
        {
            Enemy closest = null;
            float smallestRange = range;

            foreach (Enemy enemy in enemies)
            {
                if (IsInRange(enemy.Center))
                {
                    if (Vector2.Distance(center, enemy.Center) < smallestRange)
                    {
                        smallestRange = Vector2.Distance(center, enemy.Center);
                        closest = enemy;
                    }
                }
            }

            return closest;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="texture">          The texture. </param>
        /// <param name="position">         The position. </param>
        /// <param name="bulletTexture">    The bullet texture. </param>
        /// <param name="tier">             The tier. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Tower(Texture2D texture, Vector2 position, Texture2D bulletTexture, int tier)
            : base(texture, position)
        {
            this.bulletTexture = bulletTexture;
            this.tier = tier;
            this.costNextLevel = 2 * baseCost;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Upgrades this object. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Upgrade()
        {
            tier++;

            damage = (int)Math.Round((decimal)(baseDamage * Math.Pow(1.43f, tier)));
            costNextLevel = (tier + 1) * baseCost;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Slows enemy. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="enemy">    The enemy. </param>
        /// <param name="slowness"> The slowness. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void slowEnemy(Enemy enemy, float slowness)
        {
            currentSpeed = enemy.Speed;
            if (currentSpeed >= 0.35f)
            {
                newSpeed = currentSpeed * slowness;
                enemy.Speed = newSpeed;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Attacks enemies. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="enemy">    The enemy. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Attack(Enemy enemy)
        {
            if (IsInRange(enemy.Center))
            {
                attacking = true;
                target = enemy;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the target. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        protected void GetTarget()
        {
            Vector2 direction = center - target.Center;
            direction.Normalize();

            rotation = (float)Math.Atan2(-direction.X, direction.Y);
        }

        public override void Update(GameTime gameTime)
        {
            if (target != null && !IsInRange(target.Center))
                target = null;
            bulletTimer += 0.05f;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (tier == 1)
                base.Draw(spriteBatch, Color.Blue);
            else if (tier == 2)
                base.Draw(spriteBatch, Color.Green);
            else if (tier == 3)
                base.Draw(spriteBatch, Color.Red);
        }
    }
}
