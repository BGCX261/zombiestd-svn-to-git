using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace TowerPipeline
{
    [ContentTypeWriter]

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Level writer. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class LevelWriter : ContentTypeWriter<LevelContent>
    {
        protected override void Write(ContentWriter output, LevelContent value)
        {
            output.Write(value.Layout.GetLength(0));
            output.Write(value.Layout.GetLength(1));

            for (int y = 0; y < value.Layout.GetLength(0); y++)
            {
                for (int x = 0; x < value.Layout.GetLength(1); x++)
                {
                    output.Write(value.Layout[y, x]);
                }
            }

            output.Write(value.Textures.Count);

            foreach(ExternalReference<TextureContent> textContent in value.Textures)
            {
                output.WriteExternalReference<TextureContent>(textContent);
            }

            output.Write(value.Waypoints.Count);

            foreach (Vector2 waypoint in value.Waypoints)
            {
                output.Write(waypoint.X);
                output.Write(waypoint.Y);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "TowerLibrary.LevelReader, TowerLibrary";
        }
    }
}
