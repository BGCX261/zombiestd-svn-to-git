using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TowerLibrary
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Machine Gun tower. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class ArrowTower : Tower
    {
        public ArrowTower(Texture2D texture, Vector2 position, Texture2D bulletTexture, int tier, SoundBank foobar)
            : base(texture, position, bulletTexture, tier)
        {
            this.soundBank = foobar;
            // was 15
            this.baseDamage = 2;
            this.baseCost = 15;
            this.damage = baseDamage;
            this.cost = baseCost;
            this.costNextLevel = 2 * baseCost;
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
            // if (attacking && bulletTimer >= 1.7f)
            if (attacking && bulletTimer >= 0.3f)
            {
                if (target == null || !IsInRange(target.Center))
                    attacking = false;
                else
                {
                    GetTarget();

                    Bullet bullet = new Bullet(bulletTexture, Vector2.Subtract(center,
                        new Vector2(bulletTexture.Width / 2)), rotation, damage);

                    bulletList.Add(bullet);
                    
                    soundBank.PlayCue("91572__steveygos93__layeredgunshot2");

                    bulletTimer = 0;                   
                }
            }

            if (target != null)
                GetTarget();

            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet b = bulletList[i];
                b.SetRotation(rotation);
                b.Update(gameTime);

                if (target != null)
                    target.Intersects(b);

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
