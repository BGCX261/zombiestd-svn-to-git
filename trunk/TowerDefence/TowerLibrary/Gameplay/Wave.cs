using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerLibrary
{
    public class Wave
    {
        private int numOfEnemies;
        private int waveNumber;
        private float timer = 0;
        private int enemiesSpawned = 0;
        private bool enemyReachedEnd;

        private bool finished = false;

        private Queue<Vector2> waypoints = new Queue<Vector2>();

        private bool started = false;

        public List<Enemy> enemies = new List<Enemy>();
        private Vector2 startLocation;

        private Texture2D enemyTexture;

        public int EnemyCount
        {
            get { return numOfEnemies; }
        }
        public bool Finished
        {
            get { return finished; }
        }
        public int RoundNumber
        {
            get { return waveNumber; }
        }
        public bool EnemyReachedEnd
        {
            get { return enemyReachedEnd; }
            set { enemyReachedEnd = value; }
        }
        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        public Wave(Texture2D enemyTexture, int numOfEnemies, int waveNumber,
            Vector2 startLocation, Queue<Vector2> waypoints)
        {
            this.enemyTexture = enemyTexture;
            this.numOfEnemies = numOfEnemies;
            this.waveNumber = waveNumber;
            this.startLocation = startLocation;
            this.waypoints = waypoints;
        }

        public void Start()
        {
            started = true;
            finished = false;
        }

        public void Update(GameTime gameTime)
        {
            timer += 0.1f;

            if (started)
            {
                SetUpEnemies();
            }

            if (enemiesSpawned == numOfEnemies)
                started = false;

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];

                enemy.Update(gameTime);

                if (enemy.AtEnd || enemy.IsDead)
                {
                    if (enemy.AtEnd)
                        enemyReachedEnd = true;
                    enemies.Remove(enemy);
                }
            }

            if (enemies.Count == 0 && enemiesSpawned > 0 && !started)
                finished = true;
        }

        public void SetWaypoints(Queue<Vector2> waypoints)
        {
            this.waypoints = waypoints;
        }

        private void SetUpEnemies()
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                if (timer >= 10)
                {
                    float currentHealth = 0;
                    int bountyGiven = 0;
                    float speed = 0.5f;

                    if ((waveNumber + 1) % 5 != 0 && (waveNumber + 1) % 6 != 0) // Normal
                    {
                        currentHealth = 50 * (waveNumber + 1);

                        if ((waveNumber + 1) < 10)
                            bountyGiven = 1;
                        else if ((waveNumber + 1) < 20)
                            bountyGiven = 2;
                        else if ((waveNumber + 1) < 30)
                            bountyGiven = 3;
                        else if ((waveNumber + 1) < 40)
                            bountyGiven = 4;
                        else if ((waveNumber + 1) < 50)
                            bountyGiven = 5;
                        else if ((waveNumber + 1) < 60)
                            bountyGiven = 6;
                        else if ((waveNumber + 1) < 70)
                            bountyGiven = 7;

                        speed = 0.5f;
                    }

                    else if ((waveNumber + 1) % 5 == 0 && (waveNumber + 1) % 6 != 0) // Boss
                    {
                        currentHealth = 80 * (waveNumber + 1);
                        bountyGiven = 25 * ((waveNumber + 1) / 5);
                        speed = 0.25f;
                    }

                    else if ((waveNumber + 1) % 5 != 0 && (waveNumber + 1) % 6 == 0) // Fast
                    {
                        currentHealth = 45 * (waveNumber + 1);
                        bountyGiven = 2;
                        speed = 1f;                        
                    }

                    Enemy enemy = new Enemy(enemyTexture, startLocation,
                        new Queue<Vector2>(waypoints), currentHealth, bountyGiven,speed);

                    enemies.Add(enemy);
                    timer = 0;
                    enemiesSpawned++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
                enemy.Draw(spriteBatch);
        }
    }
}
