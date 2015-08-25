using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework;

namespace TowerPipeline
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Level content. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class LevelContent
    {
        public Collection<ExternalReference<TextureContent>> Textures
            = new Collection<ExternalReference<TextureContent>>();
        public int[,] Layout;

        public Collection<Vector2> Waypoints = new Collection<Vector2>();
    }
}
