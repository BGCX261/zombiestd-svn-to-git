using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerLibrary
{
    public class Window : Sprite
    {
        protected MouseState mouseState;
        protected MouseState prevMouseState;

        protected Rectangle windowRectangle;

        protected bool visible = false;

        public Window(Texture2D texture, Vector2 position, int width, int height)
            : base(texture, position)
        {
            this.texture = texture;

            windowRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            mouseState = Mouse.GetState();
            prevMouseState = mouseState;
        }

        public override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
                base.Draw(spriteBatch);
        }
    }
}
