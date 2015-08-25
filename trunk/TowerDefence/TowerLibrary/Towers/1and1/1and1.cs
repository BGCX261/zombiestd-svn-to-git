using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TowerLibrary
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   On eand one tower. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class OneandOneTower : Tower
    {
        public OneandOneTower(Texture2D texture, Vector2 position, Texture2D bulletTexture, int tier, SoundBank foobar)
            : base(texture, position, bulletTexture, tier)
        {
            this.soundBank = foobar;
            this.baseDamage = 0;
            this.baseCost = 30;
            this.damage = baseDamage;
            this.cost = baseCost;
            this.costNextLevel = 2 * baseCost;           
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the 1and1 Tower. The Tower slows Enemies. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="gameTime"> Time of the game. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (attacking && bulletTimer >= 5)
            {
                if (target == null || !IsInRange(target.Center))
                    attacking = false;
                else
                {
                    GetTarget();

                    SplashBullet bullet = new SplashBullet(bulletTexture,
                        Vector2.Subtract(center, new Vector2(bulletTexture.Width / 2)),
                        rotation, damage, 40);
                    //letzter zahl oben ist range

                    bulletList.Add(bullet);
                    bulletTimer = 0;
                }
            }

            if (target != null)
                GetTarget();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the splash damage. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="enemies">  The enemies. </param>
        /// <param name="gameTime"> Time of the game. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void UpdateSplashDamage(List<Enemy> enemies, GameTime gameTime)
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                SplashBullet b = bulletList[i] as SplashBullet;
                b.SetRotation(rotation);
                b.Update(gameTime);

                if (target != null && ! target.isSlow)
                {
                    if (target.IsHit(b))
                    {
                        List<Enemy> tempList = b.GetEnemiesInSplash(enemies);
                        foreach (Enemy enemy in tempList)
                        {
                            enemy.Intersects(b);
                            // slowenemy
                            slowEnemy(enemy, 0.8f);
                        }

                        b.Kill();
                        soundBank.PlayCue("30935__aust_paul__possiblelazer");
                    }
                }


                if (!IsInRange(b.Center))
                    b.Kill();

                if (b.IsDead() || target == null)
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
