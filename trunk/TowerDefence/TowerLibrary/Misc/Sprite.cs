using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerLibrary
{
    public class Sprite
    {
        protected Texture2D texture;

        protected Vector2 position;
        protected Vector2 velocity;

        protected Vector2 center;
        protected Vector2 origin;

        protected float rotation;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the position. </summary>
        ///
        /// <value> The position. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Vector2 Position
        {
            get { return position; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the center. </summary>
        ///
        /// <value> The center. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Vector2 Center
        {
            get { return center; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the collision rectangle. </summary>
        ///
        /// <value> The collision rectangle. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Rectangle CollisionRect
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="texture">  The texture. </param>
        /// <param name="position"> The position. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;

            this.position = position;
            this.velocity = Vector2.Zero;
            if (texture != null)
            {
                this.center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
                this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            }

            this.rotation = MathHelper.ToRadians(0);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="texture">  The texture. </param>
        /// <param name="position"> The position. </param>
        /// <param name="velocity"> The velocity. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Sprite(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture = texture;

            this.position = position;
            this.velocity = velocity;
            this.center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);

            this.rotation = MathHelper.ToRadians(0);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, center, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, center, null, color, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
