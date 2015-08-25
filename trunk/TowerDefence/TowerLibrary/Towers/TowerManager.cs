using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerLibrary
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Manager for towers. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class TowerManager
    {
        private List<Tower> towers = new List<Tower>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public TowerManager()
        {

        }

        public void AddTower(Tower tower)
        {
            towers.Add(tower);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Removes the last tower. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void RemoveLastTower()
        {
            towers.RemoveAt(towers.Count - 1);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Query if 'cellX' is on tower. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="cellX">    The cell x coordinate. </param>
        /// <param name="cellY">    The cell y coordinate. </param>
        ///
        /// <returns>   true if on tower, false if not. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool IsOnTower(int cellX, int cellY)
        {
            foreach (Tower tower in towers)
                if (tower.Position == new Vector2(cellX, cellY))
                    return true;

            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Is tower selected. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="cellX">    The cell x coordinate. </param>
        /// <param name="cellY">    The cell y coordinate. </param>
        ///
        /// <returns>   . </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Tower IsTowerSelected(int cellX, int cellY)
        {
            foreach (Tower tower in towers)
                if (tower.Position == new Vector2(cellX, cellY))
                    return tower;

            return null;
        }

        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            foreach(Tower tower in towers)
            {
                tower.Update(gameTime);

                if (tower is ArrowTower)
                {
                    if (tower.Target == null || tower.Target.IsDead)
                        tower.Target = tower.GetClosestEnemy(enemies);
                    if (tower.Target != null)
                    {
                        tower.Attack(tower.Target);
                        if (tower.Target.AtEnd)
                            tower.Target = null;
                    }
                }

                else if (tower is SpikeTower)
                {
                    SpikeTower spike = tower as SpikeTower;

                    spike.Targets.Clear();
                    spike.Targets.AddRange(spike.GetEnemies(enemies));

                    spike.Attack(); 
                }

                else if (tower is OneandOneTower)
                {
                    OneandOneTower oneandone = tower as OneandOneTower;
                    oneandone.UpdateSplashDamage(enemies, gameTime);
                    if (tower.Target == null || tower.Target.IsDead)
                        tower.Target = tower.GetClosestEnemy(enemies);
                    if (tower.Target != null)
                    {
                        tower.Attack(tower.Target);
                        if (tower.Target.AtEnd)
                            tower.Target = null;
                    }
                }

                else if (tower is ofLoveTower)
                {
                    break;
                    /*
                    if (tower.Target == null || tower.Target.IsDead)
                        tower.Target = tower.GetClosestEnemy(enemies);
                    if (tower.Target != null)
                    {
                        tower.Attack(tower.Target);
                        if (tower.Target.AtEnd)
                            tower.Target = null;
                    }*/
                }

                else
                {
                    BombTower bomb = tower as BombTower;
                    bomb.UpdateSplashDamage(enemies, gameTime);
                    if (tower.Target == null || tower.Target.IsDead)
                        tower.Target = tower.GetClosestEnemy(enemies);
                    if (tower.Target != null)
                    {
                        tower.Attack(tower.Target);
                        if (tower.Target.AtEnd)
                            tower.Target = null;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tower tower in towers)
                tower.Draw(spriteBatch);
        }
    }
}
