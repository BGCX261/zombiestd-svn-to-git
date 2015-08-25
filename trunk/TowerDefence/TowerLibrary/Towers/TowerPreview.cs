using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerLibrary
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Tower preview. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class TowerPreview : Sprite
    {
        private Texture2D rangeTexture;

        protected int range = 80;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="texture">      The texture. </param>
        /// <param name="position">     The position. </param>
        /// <param name="rangeTexture"> The range texture. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public TowerPreview(Texture2D texture, Vector2 position, Texture2D rangeTexture)
            : base(texture, position)
        {
            this.rangeTexture = rangeTexture;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Change tower texture. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="texture">  The texture. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ChangeTowerTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Change range texture. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="texture">  The texture. </param>
        /// <param name="newRange"> The new range. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ChangeRangeTexture(Texture2D texture, int newRange)
        {
            this.rangeTexture = texture;
            range = newRange;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="gameTime"> Time of the game. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, int cellX, int cellY)
        {
            Rectangle rect = Engine.CreateRectForCell(new Point(cellX, cellY));

            spriteBatch.Draw(texture, rect, new Color(new Vector4(0f, 0f, 1f, 0.6f)));

            Vector2 center = new Vector2(rect.X + rect.Width / 2,
                                         rect.Y + rect.Height / 2);

            spriteBatch.Draw(rangeTexture, Vector2.Subtract(center, new Vector2(range)),
                new Color(new Vector4(0, 0, 0, 0.5f)));
        }
    }
}
