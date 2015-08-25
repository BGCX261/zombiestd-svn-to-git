using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TowerLibrary
{
    public class Camera
    {        
        public Vector2 position = Vector2.Zero;

        public Matrix TransformMatrix
        {
            get 
            {
                return Matrix.CreateTranslation(new Vector3(-position, 0));
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clamp to area. </summary>
        ///
        /// <remarks>   Frost, 16.11.2010. </remarks>
        ///
        /// <param name="width">    The width. </param>
        /// <param name="height">   The height. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ClampToArea(int width, int height)
        {
            if (position.X > width)
                position.X = width;
            if (position.Y > height)
                position.Y = height;

            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
        }
    }
}
