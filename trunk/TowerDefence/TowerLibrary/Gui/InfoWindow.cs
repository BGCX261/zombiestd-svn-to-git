using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerLibrary
{
    class InfoWindow : Window
    {
        public delegate void FiredEvent();
        private FiredEvent OnMouseOver;
        private FiredEvent OnMouseLeave;

        private Button parentButton;

        private string title;
        private string speed;
        private string cost;
        private string description;

        private float CenterText(string text, SpriteFont arial)
        {
            float textLength = arial.MeasureString(text).X;
            float margin = (texture.Width - textLength) / 2;

            return margin;
        }

        public InfoWindow(Texture2D texture, Vector2 position, Button parentButton, int width, int height)
            : base(texture, position, width, height)
        {
            this.parentButton = parentButton;

            OnMouseOver += new FiredEvent(MouseOver);
            OnMouseLeave += new FiredEvent(MouseLeave);

            switch (this.parentButton.Name)
            {
                case "arrow" :
                    title = "Machine Gun Tower";
                    speed = "Schnell";
                    description = "Beschreibung: Projektilwaffe mit hoher Schussrate aber geringem Schaden.";
                    cost = "15";
                    break;

                case "spike" :
                    title = "Flame Thrower";
                    speed = "Langsam";
                    description = "Beschreibung: Erhitzt Zombies. Geringe Reichweite, hoher Schaden";
                    cost = "40";
                    break;

                case "bomb" :
                    title = "Rocket Tower";
                    speed = "Mittel";
                    description = "Beschreibung: Verarbeitet Zombies zu Mus.";
                    cost = "75";
                    break;

                case "oneandone":
                    title = "1und1 Tower";
                    speed = "Langsam";
                    description = "Beschreibung: Macht Zombies langsam.";
                    cost = "30";
                    break;
                
                case "oflove":
                    title = "Tower of <3";
                    speed = "-";
                    description = "Beschreibung: <3<3<3 X0X0";
                    cost = "50";
                    break;

                case "nuclear":
                    title = "Nuclear Strike";
                    speed = "ZAP!";
                    description = "Nukular, das Wort heisst Nukular. 3x Benutzbar!";
                    cost = "100";
                    break;
            }
        }

        private void MouseOver()
        {
            this.visible = true;
        }

        private void MouseLeave()
        {
            this.visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (parentButton != null)
            {
                Rectangle tempRect = parentButton.CollisionRect;

                if (tempRect.Contains(new Point(mouseState.X, mouseState.Y)))
                {
                    if (OnMouseOver != null)
                        OnMouseOver();
                }

                else
                {
                    if (OnMouseLeave != null)
                        OnMouseLeave();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont arial)
        {
            if (visible)
            {
                spriteBatch.Draw(texture, windowRectangle, Color.White);

                spriteBatch.DrawString(arial, title, new Vector2(
                    windowRectangle.X + CenterText(title, arial), windowRectangle.Top + 10), Color.White);
                string descriptionSplit = Engine.WrapText(arial, description, windowRectangle.Width - 30);

                DrawString(spriteBatch, arial, "Kosten", cost, 30, 15);
                DrawString(spriteBatch, arial, "Geschw.", speed, 50, 15);
                spriteBatch.DrawString(arial, descriptionSplit, new Vector2(
                    windowRectangle.X + 15, windowRectangle.Y + 85), Color.White);
            }
        }

        private void DrawString(SpriteBatch spriteBatch, SpriteFont arial, string key,
            string value, float heightOffset, float margin)
        {
            spriteBatch.DrawString(arial, key + ":", new Vector2(
                windowRectangle.X + margin, (windowRectangle.Y + margin) + heightOffset), Color.White);

            spriteBatch.DrawString(arial, value, new Vector2(
                (windowRectangle.Right - margin) - arial.MeasureString(value).X,
                (windowRectangle.Y + margin) + heightOffset), Color.White);

        }
    }
}
