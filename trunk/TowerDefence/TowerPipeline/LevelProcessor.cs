using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Xml;
using System.ComponentModel;

namespace TowerPipeline
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Level processor. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [ContentProcessor(DisplayName = "LevelProcessor")]
    public class LevelProcessor : ContentProcessor<XmlDocument, LevelContent>
    {
        private string grassTextureFilename = "grass.png";
        [DisplayName("Grass Texture")]
        [DefaultValue("grass.png")]
        [Description("The name of the grass texture.")]
        public string GrassTextureFilename
        {
            get { return grassTextureFilename; }
            set { grassTextureFilename = value; }
        }

        private string pathTextureFilename = "path.png";
        [DisplayName("Path Texture")]
        [DefaultValue("path.png")]
        [Description("The name of the path texture.")]
        public string PathTextureFilename
        {
            get { return pathTextureFilename; }
            set { pathTextureFilename = value; }
        }

        public override LevelContent Process(XmlDocument input, ContentProcessorContext context)
        {
            LevelContent level = new LevelContent();

            foreach (XmlNode node in input.DocumentElement.ChildNodes)
            {
                if (node.Name == "Layout")
                {
                    int width = int.Parse(node.Attributes["Width"].Value);
                    int height = int.Parse(node.Attributes["Height"].Value);

                    level.Layout = new int[height, width];
                    
                    string layout = node.InnerText;

                    string[] lines = layout.Split('\r', '\n');

                    int row = 0;

                    foreach (string line in lines)
                    {
                        string realLine = line.Trim();

                        if (string.IsNullOrEmpty(realLine))
                            continue;

                        string[] cells = realLine.Split(' ');

                        for (int x = 0; x < width; x++)
                        {
                            int cellIndex = int.Parse(cells[x]);

                            level.Layout[row, x] = cellIndex;
                        }

                        row++;
                    }
                }

                if (node.Name == "Waypoints")
                {
                    XmlNodeList points = node.ChildNodes;

                    foreach (XmlNode point in points)
                    {
                        float x = int.Parse(point.Attributes["X"].Value);
                        float y = int.Parse(point.Attributes["Y"].Value);
                        level.Waypoints.Add(new Vector2(x, y));
                    }
                }

                ExternalReference<TextureContent> grass = 
                    context.BuildAsset<TextureContent, TextureContent>(
                    new ExternalReference<TextureContent>(grassTextureFilename),
                    "TextureProcessor");
                level.Textures.Add(grass);

                ExternalReference<TextureContent> path =
                    context.BuildAsset<TextureContent, TextureContent>(
                    new ExternalReference<TextureContent>(pathTextureFilename),
                    "TextureProcessor");
                level.Textures.Add(path);
            }

            return level;
        }
    }
}