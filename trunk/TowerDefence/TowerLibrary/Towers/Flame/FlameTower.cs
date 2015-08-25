using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TowerLibrary
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Flame tower. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class SpikeTower : Tower
    {
        private Vector2[] directions = new Vector2[8];
        private List<Enemy> targets = new List<Enemy>();

        public List<Enemy> Targets
        {
            get { return targets; }
        }
        public List<Enemy> GetEnemies(List<Enemy> enemies)
        {
            List<Enemy> tempList = new List<Enemy>();

            foreach (Enemy enemy in enemies)
            {
                if (IsInRange(enemy.Center) && !enemy.AtEnd)
                {
                    tempList.Add(enemy);
                }
            }

            return tempList;
        }

        public SpikeTower(Texture2D texture, Vector2 position, Texture2D bulletTexture, int tier, SoundBank foobar)
            : base(texture, position, bulletTexture, tier)
        {
            Vector2 upLeft = new Vector2(-1, -1);
            directions[0] = upLeft;

            Vector2 up = new Vector2(0, -1);
            directions[1] = up;

            Vector2 upRight = new Vector2(1, -1);
            directions[2] = upRight;

            Vector2 left = new Vector2(-1, 0);
            directions[3] = left;

            Vector2 right = new Vector2(1, 0);
            directions[4] = right;

            Vector2 downLeft = new Vector2(-1, 1);
            directions[5] = downLeft;

            Vector2 down = new Vector2(0, 1);
            directions[6] = down;

            Vector2 downRight = new Vector2(1, 1);
            directions[7] = downRight;

            this.baseDamage = 20;
            this.range = 48;
            this.baseCost = 40;
            this.damage = baseDamage;
            this.cost = baseCost;
            this.costNextLevel = 2 * baseCost;
            //Flammentextur hier einfügen
            //this.bulletTexture = FlameTexture;
            this.soundBank = foobar;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Attacks this object. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Attack()
        {
            attacking = true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="gameTime"> Time of the game. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (attacking && bulletTimer >= 3)
            {
                if (targets.Count == 0)
                    attacking = false;
                else
                {
                    for (int i = 0; i < directions.Length; i++)
                    {
                        Bullet bullet = new Bullet(bulletTexture, Vector2.Subtract(center,
                            new Vector2(bulletTexture.Width / 2)), directions[i], damage);

                        bulletList.Add(bullet);
                    }
                    soundBank.PlayCue("flame2");
                    bulletTimer = 0;
                }
            }

            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet b = bulletList[i];
                b.Update(gameTime);

                for (int t = 0; t < targets.Count; t++)
                    if (targets[t] != null || !targets[t].IsDead)
                        targets[t].Intersects(b); 

                if (!IsInRange(b.Center))
                    b.Kill();

                if (b.IsDead() || targets.Count == 0)
                    bulletList.Remove(b);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Draws tower. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="spriteBatch">  The sprite batch. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Bullet bullet in bulletList)
                bullet.Draw(spriteBatch);
        }
    }
}
