using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerLibrary
{
    public class GuiManager : Sprite
    {
        private Rectangle guiRectangle;
        private SpriteFont arial;
        private SpriteFont infoFont;

        private List<Window> windows = new List<Window>();

        private List<Button> buttonList = new List<Button>();

        public List<Button> ButtonList
        {
            get { return buttonList; }
            set { buttonList = value; }
        }

        public Button GetButton(string name)
        {
            foreach (Button b in buttonList)
            {
                if (b.Name == name)
                    return b;
            }

            return null;
        }

        public GuiManager(Texture2D texture, Vector2 position, SpriteFont arial, SpriteFont infoFont)
            : base(texture, position)
        {
            this.guiRectangle = new Rectangle(
                (int)position.X, (int)position.Y,
                texture.Width, texture.Height);

            this.arial = arial;
            this.infoFont = infoFont;
        }

        public void AddButton(Texture2D button, Texture2D buttonPressed, 
            Vector2 position, string name)
        {
            Button newButton = new Button(button, buttonPressed, position, name);
            buttonList.Add(newButton);
        }

        public void AddWindow(Texture2D windowTexture, Vector2 windowPosition, string buttonName)
        {
            Button button = null;

            foreach (Button b in buttonList)
            {
                if (b.Name == buttonName)
                    button = b;
            }

            windows.Add(new InfoWindow(windowTexture, windowPosition, button, windowTexture.Width, windowTexture.Height));
        }

        public float CenterText(string text)
        {
            float textLength = arial.MeasureString(text).X;
            float margin = (texture.Width - textLength) / 2;

            return margin;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Button button in buttonList)
                button.Update(gameTime);

            foreach(Window window in windows)
                window.Update(gameTime);

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, int round, int money, int lives)
        {
            // Hintergrundfarbe Seitenleiste
            // Color.White = Transparent
            spriteBatch.Draw(texture, guiRectangle, Color.White);

            float heightOffset = arial.MeasureString(" ").Y;

            DrawString(spriteBatch, "Runde", round, 0, 20);
            DrawString(spriteBatch, "Geld", money, heightOffset, 20);
            DrawString(spriteBatch, "Leben", lives, heightOffset * 2, 20);

            spriteBatch.DrawString(arial, "Towers", new Vector2(
                guiRectangle.X + CenterText("Towers"), 
                (guiRectangle.Y + 20) + (125 - arial.MeasureString(" ").Y)), Color.White);

            foreach (Button button in buttonList)
                button.Draw(spriteBatch);

            foreach (Window window in windows)
            {
                if (window is InfoWindow)
                {
                    InfoWindow info = window as InfoWindow;
                    info.Draw(spriteBatch, infoFont);
                }

                else
                    window.Draw(spriteBatch);
            }
        }

        private void DrawString(SpriteBatch spriteBatch, string key,
            int value, float heightOffset, float margin)
        {
            spriteBatch.DrawString(arial, key + ":", new Vector2(
                guiRectangle.X + margin, (guiRectangle.Y + margin) + heightOffset), Color.White);

            spriteBatch.DrawString(arial, value.ToString(), new Vector2(
                (guiRectangle.Right - margin) - arial.MeasureString(value.ToString()).X,
                (guiRectangle.Y + margin) + heightOffset), Color.White);

        }
    }
}
