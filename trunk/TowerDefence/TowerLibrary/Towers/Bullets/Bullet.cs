using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerLibrary
{
    public class Bullet : Sprite
    {
        private int damage;
        private int age;

        public int Damage
        {
            get { return damage; }
        }
        public bool IsDead()
        {
            if (age > 100)
                return true;
            return false;
        }


        public Bullet(Texture2D texture, Vector2 position, float rotation, int damage)
            : base(texture, position)
        {
            this.rotation = rotation;
            this.damage = damage;

            velocity = new Vector2(0, -6);
            velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(rotation));
        }

        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity, int damage)
            : base(texture, position)
        {
            this.rotation = rotation;
            this.damage = damage;

            this.velocity = Vector2.Multiply(velocity, 5);
        }

        public void Kill()
        {
            this.age = 200;
        }

        public void SetRotation(float value)
        {
            rotation = value;
            velocity = new Vector2(0, -6);
            velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(rotation));
        }

        public override void Update(GameTime gameTime)
        {
            age++;
            position += velocity;

            base.Update(gameTime);
        }
    }
}
