using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerLibrary
{
    public class Button : Sprite
    {
        private MouseState mouseState;
        private MouseState prevMouseState;

        public delegate void FiredEvent(object sender);
        private FiredEvent OnLeftMousePress;
        public FiredEvent OnLeftMouseClick;
        private FiredEvent OnLeftMouseRelease;

        private Texture2D releasedTexture;
        private Texture2D pressedTexture;

        private string name;
        private bool visible = true;

        public string Name
        {
            get { return name; }
        }
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public Button(Texture2D texture, Texture2D pressedTexture, 
            Vector2 position, string name)
            : base(texture, position)
        {
            this.releasedTexture = texture;
            this.pressedTexture = pressedTexture;
            this.name = name;

            OnLeftMousePress += new FiredEvent(LeftMousePress);
            OnLeftMouseRelease += new FiredEvent(LeftMouseRelease);

            mouseState = Mouse.GetState();
            prevMouseState = mouseState;
        }

        private void LeftMousePress(object sender)
        {
            texture = pressedTexture;
        }

        private void LeftMouseRelease(object sender)
        {
            texture = releasedTexture;
        }

        public override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (visible)
            {
                Rectangle tempRect = new Rectangle(
                    (int)position.X, (int)position.Y,
                    texture.Width, texture.Height);

                if (tempRect.Contains(new Point(mouseState.X, mouseState.Y)))
                {
                    if (mouseState.LeftButton == ButtonState.Released &&
                        prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (OnLeftMouseClick != null)
                            OnLeftMouseClick(this);
                    }

                    else if (mouseState.LeftButton == ButtonState.Pressed &&
                             prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (OnLeftMousePress != null)
                            OnLeftMousePress(this);
                    }

                    else if (mouseState.LeftButton == ButtonState.Released &&
                             prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (OnLeftMouseRelease != null)
                            OnLeftMouseRelease(this);
                    }
                }

                else
                {
                    if (OnLeftMouseRelease != null)
                        OnLeftMouseRelease(this);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
                base.Draw(spriteBatch);
        }
    }
}
