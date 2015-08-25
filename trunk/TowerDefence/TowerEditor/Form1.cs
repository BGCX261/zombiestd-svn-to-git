using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TowerLibrary;

namespace TileEditor
{
    using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

	public partial class Form1 : Form
	{
		SpriteBatch spriteBatch;
        MouseState mouseState;
        MouseState prevMouseState;

		Texture2D grass;
        Texture2D path;
        Texture2D cursor;
        Texture2D waypoint;

        Level level = new Level(24, 18);
		Camera camera = new Camera();

        List<Vector2> tempWaypoints = new List<Vector2>();

		int cellX, cellY;

        bool focused;

		public GraphicsDevice GraphicsDevice
		{
			get { return tileDisplay1.GraphicsDevice; }
		}

		public Form1()
		{
			InitializeComponent();
            
			tileDisplay1.OnInitialize += new EventHandler(tileDisplay1_OnInitialize);
			tileDisplay1.OnDraw += new EventHandler(tileDisplay1_OnDraw);

			Application.Idle += delegate { tileDisplay1.Invalidate(); };

			Mouse.WindowHandle = tileDisplay1.Handle;

            string filePath = "C:/Users/Si/Documents/Visual Studio 2005/Projects/TowerDefence/TowerDefence/Content/";

            openFileDialog1.Filter = "Level File|*.level";
            openFileDialog1.InitialDirectory = filePath;
            openFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Filter = "Level File|*.level";
            saveFileDialog1.InitialDirectory = filePath;
            saveFileDialog1.RestoreDirectory = true;
		}

		void tileDisplay1_OnInitialize(object sender, EventArgs e)
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

            string filePath = Environment.CurrentDirectory;
            string contentPath = Path.GetFullPath(Path.Combine(filePath, "../../Content\\"));

            grass = Texture2D.FromFile(GraphicsDevice, contentPath + "grass.png");
            path = Texture2D.FromFile(GraphicsDevice, contentPath + "path.png");
            cursor = Texture2D.FromFile(GraphicsDevice, contentPath + "cursor.png");
            waypoint = Texture2D.FromFile(GraphicsDevice, contentPath + "waypoint.png");

            level.AddTexture(grass);
            level.AddTexture(path);
		}

		void tileDisplay1_OnDraw(object sender, EventArgs e)
		{
			Logic();
			Render();
		}

        private bool IsInBounds(int cellX, int cellY)
        {
            if (cellX >= 0 && cellY >= 0 && cellX < level.Width && cellY < level.Height && focused)
                return true;
            return false;
        }

        private void Logic()
        {
            mouseState = Mouse.GetState();

            //camera.position.X = hScrollBar1.Value * Engine.TileWidth;
            //camera.position.Y = vScrollBar1.Value * Engine.TileHeight;

            cellX = (int)(mouseState.X / Engine.TileWidth);
            cellY = (int)(mouseState.Y / Engine.TileHeight);

            if (IsInBounds(cellX, cellY) && tileDisplay1.Focused)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (tsDraw.Checked)
                        level.SetCellIndex(cellX, cellY, 1);

                    if (tsErase.Checked)
                        level.SetCellIndex(cellX, cellY, 0);

                    if (prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (tsAdd.Checked)
                        {
                            tempWaypoints.Add(Vector2.Add(
                                new Vector2(cellX * Engine.TileWidth,
                                cellY * Engine.TileHeight), new Vector2(Engine.TileWidth / 2)));
                        }

                        if (tsRemove.Checked)
                        {
                            tempWaypoints.Remove(Vector2.Add(
                                new Vector2(cellX * Engine.TileWidth,
                                cellY * Engine.TileHeight), new Vector2(Engine.TileWidth / 2)));

                        }
                    }
                }
            }

            this.Text = tempWaypoints.Count.ToString();

            prevMouseState = mouseState;
        }

		private void Render()
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, camera.TransformMatrix);

            level.Draw(spriteBatch);

            foreach (Vector2 w in tempWaypoints)
            {
                Vector2 cell = Vector2.Subtract(w, new Vector2(16, 16));
                Rectangle rect = new Rectangle((int)cell.X, (int)cell.Y, 
                    Engine.TileWidth, Engine.TileHeight);
                spriteBatch.Draw(waypoint, rect, new Color(new Vector4(1, 0, 0, 0.4f)));
            }

            if (IsInBounds(cellX, cellY))
            {
                Rectangle rect = Engine.CreateRectForCell(new Point(cellX, cellY));

                if (level.GetCellIndex(cellX, cellY) != 0)
                    spriteBatch.Draw(cursor, rect, Color.Red);
                else
                    spriteBatch.Draw(cursor, rect, Color.White);
            }

            spriteBatch.End();
		}

        private void tileDisplay1_MouseEnter(object sender, EventArgs e)
        {
            focused = true;
        }

        private void tileDisplay1_MouseLeave(object sender, EventArgs e)
        {
            focused = false;
        }

        private void tsOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                level = Level.Open(openFileDialog1.FileName);
                level.AddTexture(grass);
                level.AddTexture(path);
                tempWaypoints.Clear();
                tempWaypoints.AddRange(level.Waypoints);
            }
        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                level.Waypoints.Clear();

                for (int i = 0; i < tempWaypoints.Count; i++)
                    level.Waypoints.Enqueue(tempWaypoints[i]);

                level.Save(saveFileDialog1.FileName);
            }
        }

        private void tsNewLayer_Click(object sender, EventArgs e)
        {
            NewLayerForm form = new NewLayerForm();

            level.map = new int[19, 19];

            //form.ShowDialog();

            /*if (form.OKPressed)
            {
                level.map = new int[int.Parse(form.height.Text), int.Parse(form.width.Text)];

                for (int x = 0; x < level.Width; x++)
                {
                    for (int y = 0; y < level.Height; y++)
                    {
                        level.SetCellIndex(x, y, 0);
                    }
                }
            }*/
        }

        private void tsDraw_Click(object sender, EventArgs e)
        {
            if (tsDraw.Checked == false)
                tsDraw.Checked = true;
            else if (tsDraw.Checked == true)
                tsDraw.Checked = false;

            tsErase.Checked = false;
            tsAdd.Checked = false;
            tsRemove.Checked = false;
        }

        private void tsErase_Click(object sender, EventArgs e)
        {
            if (tsErase.Checked == false)
                tsErase.Checked = true;
            else if (tsErase.Checked == true)
                tsErase.Checked = false;

            tsDraw.Checked = false;
            tsAdd.Checked = false;
            tsRemove.Checked = false;
        }

        private void tsAdd_Click(object sender, EventArgs e)
        {
            if (tsAdd.Checked == false)
                tsAdd.Checked = true;
            else if (tsAdd.Checked == true)
                tsAdd.Checked = false;

            tsDraw.Checked = false;
            tsErase.Checked = false;
            tsRemove.Checked = false;
        }

        private void tsRemove_Click(object sender, EventArgs e)
        {
            if (tsRemove.Checked == false)
                tsRemove.Checked = true;
            else if (tsRemove.Checked == true)
                tsRemove.Checked = false;

            tsDraw.Checked = false;
            tsAdd.Checked = false;
            tsErase.Checked = false;
        }
	}
}