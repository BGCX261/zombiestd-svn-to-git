using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TowerLibrary
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Rocket tower. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class BombTower : Tower
    {
        public BombTower(Texture2D texture, Vector2 position, Texture2D bulletTexture, int tier, SoundBank foobar)
            : base(texture, position, bulletTexture, tier)
        {
            this.baseDamage = 50;
            this.baseCost = 75;
            this.damage = baseDamage;
            this.cost = baseCost;
            this.costNextLevel = 2 * baseCost;
            this.soundBank = foobar;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (attacking && bulletTimer >= 10)
            {
                if (target == null || !IsInRange(target.Center))
                    attacking = false;
                else
                {
                    GetTarget();

                    SplashBullet bullet = new SplashBullet(bulletTexture, 
                        Vector2.Subtract(center, new Vector2(bulletTexture.Width / 2)), 
                        rotation, damage, 40);

                    bulletList.Add(bullet);
                    soundBank.PlayCue("100772__CGEffex__Huge_rocket_launcher");
                    bulletTimer = 0;
                }
            }

            if (target != null)
                GetTarget();
        }

        public void UpdateSplashDamage(List<Enemy> enemies, GameTime gameTime)
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                SplashBullet b = bulletList[i] as SplashBullet;
                b.SetRotation(rotation);
                b.Update(gameTime);

                if (target != null)
                {
                    if (target.IsHit(b))
                    {
                        List<Enemy> tempList = b.GetEnemiesInSplash(enemies);
                        foreach (Enemy enemy in tempList)
                        {
                            enemy.Intersects(b);
                        }

                        b.Kill();
                    }
                }


                if (!IsInRange(b.Center))
                    b.Kill();

                if (b.IsDead() || target == null)
                    bulletList.Remove(b);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Bullet bullet in bulletList)
                bullet.Draw(spriteBatch);
        }
    }
}
