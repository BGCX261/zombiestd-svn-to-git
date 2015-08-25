using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace TowerLibrary
{
    public class Level
    {
        private List<Texture2D> tileTextures = new List<Texture2D>();
        private Queue<Vector2> waypoints = new Queue<Vector2>();

        public int[,] map;

        public int WidthInPixels
        {
            get 
            {
                return Width * Engine.TileWidth; 
            }
        }

        public int HeightInPixels
        {
            get
            {
                return Height * Engine.TileHeight;
            }
        }

        public int Width
        {
            get { return map.GetLength(1); }
        }

        public int Height
        {
            get { return map.GetLength(0); }
        }

        public Queue<Vector2> Waypoints
        {
            get { return waypoints; }
        }

        public Level(int width, int height)
        {
            map = new int[height, width];
        }

        public Level(int[,] existingMap)
        {
            map = (int[,])existingMap.Clone();
        }

        public void AddTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        private bool IsInBounds(int cellX, int cellY)
        {
            if (cellX >= 0 && cellY >= 0 && cellX < Width && cellY < Height)
                return true;
            return false;
        }

        public int GetCellIndex(int x, int y)
        {
            return map[y, x];
        }

        public int GetCellIndex(Point point)
        {
            if (IsInBounds(point.X, point.Y))
                return map[point.Y, point.X];
            else
                return -1;
        }

        public void AddWaypoint(Vector2 waypoint)
        {
            waypoints.Enqueue(waypoint);
        }

        public void SetCellIndex(int x, int y, int cellIndex)
        {
            map[y, x] = cellIndex;
        }

        public void SetCellIndex(Point point, int cellIndex)
        {
            map[point.Y, point.X] = cellIndex;
        }

        public void LoadTileTexture(ContentManager content, params string[] textureNames)
        {
            Texture2D texture;

            foreach (string textureName in textureNames)
            {
                texture = content.Load<Texture2D>(textureName);
                tileTextures.Add(texture);
            }
        }

        public void Save(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            XmlTextWriter xw = new XmlTextWriter(sw);
            XmlDocument levelDoc = new XmlDocument();
            levelDoc.Save(xw);

            xw.WriteStartElement("Level");

                xw.Formatting = Formatting.Indented;

                xw.WriteStartElement("Layout");
                xw.WriteAttributeString("Width", Width.ToString());
                xw.WriteAttributeString("Height", Height.ToString());

                    xw.Formatting = Formatting.Indented;
                    for (int y = 0; y < Height; y++)
                    {
                        string line = string.Empty;

                        for (int x = 0; x < Width; x++)
                        {
                            line += map[y, x].ToString() + " ";
                        }
                        xw.WriteString(sw.NewLine);
                        xw.WriteString(line);
                    }

                xw.WriteString(sw.NewLine);
                xw.WriteEndElement();

                xw.WriteString(sw.NewLine);
                xw.WriteStartElement("Waypoints");
                    xw.Formatting = Formatting.Indented;
                    foreach (Vector2 waypoint in waypoints)
                    {
                        xw.WriteStartElement("Waypoint");
                        xw.WriteAttributeString("X", waypoint.X.ToString());
                        xw.WriteAttributeString("Y", waypoint.Y.ToString());
                        xw.WriteEndElement();
                        xw.WriteString(sw.NewLine);
                    }
                xw.WriteEndElement();

            xw.WriteString(sw.NewLine);
            xw.WriteEndElement();

            xw.Close();
            sw.Close();
        }

        public static Level Open(string filename)
        {
            Level level = new Level(24, 18);

            List<List<int>> tempLayout = new List<List<int>>();

            StreamReader sr = new StreamReader(filename);
            XmlTextReader xr = new XmlTextReader(sr);
            XmlDocument levelDoc = new XmlDocument();
            levelDoc.Load(xr);

            foreach (XmlNode node in levelDoc.FirstChild.ChildNodes)
            {
                if (node.Name == "Layout")
                {
                    int width = int.Parse(node.Attributes["Width"].Value);
                    int height = int.Parse(node.Attributes["Height"].Value);

                    level = new Level(width, height);

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

                            level.SetCellIndex(x, row, cellIndex);
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
                        level.AddWaypoint(new Vector2(x, y));
                    }
                }
            }

            return level;
        }

        public void Draw(SpriteBatch batch)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];

                    if (textureIndex == -1)
                        continue;

                    Texture2D texture = tileTextures[textureIndex];

                    batch.Draw(texture, new Rectangle(x * Engine.TileWidth, y * Engine.TileHeight,
                        Engine.TileWidth, Engine.TileHeight), Color.White);
                }
            }
        }
    }

    public class LevelReader : ContentTypeReader<Level>
    {
        protected override Level Read(ContentReader input, Level existingInstance)
        {
            int height = input.ReadInt32();
            int width = input.ReadInt32();

            Level level = new Level(width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    level.SetCellIndex(x, y, input.ReadInt32());

            List<Texture2D> textures = new List<Texture2D>();

            int numTexture = input.ReadInt32();

            for (int i = 0; i < numTexture; i++)
            {
                Texture2D t = input.ReadExternalReference<Texture2D>();

                textures.Add(t);
            }

            foreach (Texture2D t in textures)
                level.AddTexture(t);

            int numPoints = input.ReadInt32();

            for (int i = 0; i < numPoints; i++)
            {
                float x = input.ReadSingle();
                float y = input.ReadSingle();

                level.AddWaypoint(new Vector2(x, y));
            }

            return level;
        }
    }
}
