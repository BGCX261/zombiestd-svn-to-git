using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerLibrary
{
    public class SplashBullet : Bullet
    {
        private float range;

        public SplashBullet(Texture2D texture, Vector2 position, 
            float rotation, int damage, float range)
            : base(texture, position, rotation, damage)
        {
            this.range = range;
        }

        protected bool IsInRange(Vector2 position)
        {
            if (Vector2.Distance(center, position) <= range)
                return true;
            return false;
        }

        public List<Enemy> GetEnemiesInSplash(List<Enemy> enemies)
        {
            List<Enemy> tempList = new List<Enemy>();

            foreach (Enemy enemy in enemies)
            {
                if (IsInRange(enemy.Center))
                {
                    tempList.Add(enemy);
                }
            }

            return tempList;
        }
    }
}
