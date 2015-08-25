using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerLibrary
{
    public static class Engine
    {
        public const int TileWidth = 32;
        public const int TileHeight = 32;

        public static Vector2 Vector2FromMapPosition(int x, int y)
        {
            Vector2 vec = new Vector2(x * TileWidth, y * TileHeight);

            return vec;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Creates a rectangle for cell. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="point">    The point. </param>
        ///
        /// <returns>   . </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Rectangle CreateRectForCell(Point point)
        {
            return new Rectangle(
                point.X * TileWidth,
                point.Y * TileHeight,
                TileWidth,
                TileHeight);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Wrap text. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="font">                 The font. </param>
        /// <param name="text">                 The text. </param>
        /// <param name="maximumLineLength">    Length of the maximum line. </param>
        ///
        /// <returns>   . </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string WrapText(SpriteFont font, string text, float maximumLineLength)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float spaceLength = font.MeasureString(" ").X;

            float lineLength = 0;

            foreach (string word in words)
            {
                float wordLength = font.MeasureString(word).X;
                if (wordLength + lineLength < maximumLineLength)
                {
                    sb.Append(word + " ");
                    lineLength += wordLength + spaceLength;
                }

                else
                {
                    sb.Append("\n" + word + " ");
                    lineLength = wordLength + spaceLength;
                }
            }

            return sb.ToString();
        }
    }
}
