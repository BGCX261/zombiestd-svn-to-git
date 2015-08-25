using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Xml;

namespace TowerPipeline
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Level importer. </summary>
    ///
    /// <remarks>   Frost, 16.11.2010. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [ContentImporter(".level", DisplayName = "Level Importer", 
        DefaultProcessor = "LevelProcessor")]
    public class LevelImporter : ContentImporter<XmlDocument>
    {
        public override XmlDocument Import(string filename, ContentImporterContext context)
        {
           XmlDocument doc = new XmlDocument();
           doc.Load(filename);
           return doc;
        }
    }
}
