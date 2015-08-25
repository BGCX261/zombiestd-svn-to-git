using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerLibrary
{
    public class WaveManager
    {
        private int numOfWaves;

        private Queue<Vector2> waypoints = new Queue<Vector2>();
        private Queue<Wave> waves = new Queue<Wave>();

        private Texture2D enemyTexture;

        public bool waveFinished = false;

        public Wave CurrentWave
        {
            get { return waves.Peek(); }
        }
        public List<Enemy> GetEnemies()
        {
            return CurrentWave.enemies;
        }
        public int Round
        {
            get { return CurrentWave.RoundNumber + 1; }
        }

        public WaveManager(Texture2D enemyTexture, int numOfWaves, Queue<Vector2> waypoints)
        {
        
            this.numOfWaves = numOfWaves;
            this.enemyTexture = enemyTexture;

            this.waypoints = waypoints;

            Vector2 startLocation = new Vector2(this.waypoints.Peek().X - 
                Engine.TileWidth / 2, this.waypoints.Peek().Y - Engine.TileHeight / 2);

            for (int i = 0; i < numOfWaves; i++)
            {
                int numOfEnemies = 12;

                int test = (i + 1) % 5;

                if ((i + 1) % 5 != 0 && (i + 1) % 6 != 0) // Normal
                {
                    numOfEnemies = 12;
                }

                else if ((i + 1) % 5 == 0 && (i + 1) % 6 != 0) // Boss
                {
                    numOfEnemies = 1;
                }

                else if ((i + 1) % 5 != 0 && (i + 1) % 6 == 0) // Fast
                {
                    numOfEnemies = 10;
                }

                Wave wave = new Wave(this.enemyTexture, numOfEnemies, i,
                    startLocation, this.waypoints);

                waves.Enqueue(wave);
            }
        }

        public void StartNextWave()
        {
            if ((waves.Count - 1) > 0)
            {
                if (waveFinished)
                    waves.Dequeue();
                waves.Peek().Start();
                waveFinished = false;
            }

            else waveFinished = false;
        }

        public void SetWaypoints(Vector2[] waypoints)
        {
            this.waypoints.Clear();
            foreach (Vector2 waypoint in waypoints)
                this.waypoints.Enqueue(Vector2.Add(Vector2.Multiply(waypoint, 32), new Vector2(16)));

            foreach (Wave wave in waves)
                wave.SetWaypoints(this.waypoints);
        }

        public void Update(GameTime gameTime)
        {
            Wave currentWave = waves.Peek();
            currentWave.Update(gameTime);

            if (currentWave.Finished)
            {
                waveFinished = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Wave currentWave = waves.Peek();
            currentWave.Draw(spriteBatch);
        }
    }
}
