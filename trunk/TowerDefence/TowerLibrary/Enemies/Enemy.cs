using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerLibrary
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Enemy. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Enemy : Sprite
    {
        private Queue<Vector2> waypoints = new Queue<Vector2>();

        private float startHealth;
        private float health;
        private bool alive = true;
        private float speed = 0.5f;
        private int bountyGiven;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the speed. </summary>
        ///
        /// <value> The speed. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets a value indicating whether this object is slow. </summary>
        ///
        /// <value> true if this object is slow, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool isSlow
        {
            get
            {
                if (this.Speed < speed) return true;
                else return false;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets a value indicating whether at end. </summary>
        ///
        /// <value> true if at end, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool AtEnd
        {
            get { return waypoints.Count == 0; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets a value indicating whether this object is dead. </summary>
        ///
        /// <value> true if this object is dead, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool IsDead
        {
            get { return health <= 0; }
            set { this.health = 0; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the bounty given. </summary>
        ///
        /// <value> The bounty given. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int BountyGiven
        {
            get { return bountyGiven; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Query if 'bullet' is hit. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="bullet">   The bullet. </param>
        ///
        /// <returns>   true if hit, false if not. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool IsHit(SplashBullet bullet)
        {
            if (Vector2.Distance(center, bullet.Center) <= 12)
                return true;
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="texture">      The texture. </param>
        /// <param name="position">     The position. </param>
        /// <param name="waypoints">    The waypoints. </param>
        /// <param name="health">       The health. </param>
        /// <param name="bountyGiven">  The bounty given. </param>
        /// <param name="speed">        The speed. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Enemy(Texture2D texture, Vector2 position, 
            Queue<Vector2> waypoints, float health, int bountyGiven, float speed)
            : base(texture, position)
        {
            this.waypoints = waypoints;
            this.startHealth = health;
            this.health = startHealth;
            this.bountyGiven = bountyGiven;
            this.speed = speed;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the distance to destination. </summary>
        ///
        /// <value> The distance to destination. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public float DistanceToDestination
        {
            get { return Vector2.Distance(center, waypoints.Peek()); }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets the waypoints. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="waypoints">    The waypoints. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void SetWaypoints(Queue<Vector2> waypoints)
        {
            this.waypoints = waypoints;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Intersects. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="bullet">   The bullet. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Intersects(Bullet bullet)
        {
            if (Vector2.Distance(center, bullet.Center) <= 12)
            {
                health -= (float)bullet.Damage;
                bullet.Kill();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Intersects. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="bullet">   The bullet. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Intersects(SplashBullet bullet)
        {
            health -= (float)bullet.Damage;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (waypoints.Count > 0)
            {
                if (DistanceToDestination < 1f)
                {
                    float distanceX = MathHelper.Distance(center.X, waypoints.Peek().X);
                    float distanceY = MathHelper.Distance(center.Y, waypoints.Peek().Y);

                    position = Vector2.Add(position, new Vector2(distanceX, distanceY));
                    waypoints.Dequeue();
                }

                else
                {
                    Vector2 direction = -(center - waypoints.Peek());
                    direction.Normalize();
                    velocity = Vector2.Multiply(direction, speed);

                    position += velocity;
                }
            }

            if (health <= 0 || AtEnd)
                alive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                float healthPercentage = (float)health / (float)startHealth;
                Color color = new Color(new Vector3(1 - healthPercentage, 
                    1 - healthPercentage, 1 - healthPercentage));

                base.Draw(spriteBatch, color);
            }
        }
    }
}
