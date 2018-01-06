using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#pragma warning disable CS0618
namespace ZeldaRoyale.Content
{
    class SwordProjectile
    {
        public Texture2D texture;
        public Vector2 velocity;
        public Vector2 position;
        public Vector2 scale;
        public String orientation;

        public SwordProjectile(Texture2D _texture, Vector2 _velocity, Vector2 _position, String _orientation,float targetX=10, float targetY=10)
        {
            this.texture = _texture;
            this.velocity = _velocity;
            this.position = _position;
            this.scale = new Vector2(2.5f, 2.5f);
            this.orientation = _orientation;
        }

        public void updatePosition(GameTime gameTime)
        {
            this.position += this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool checkCollision(Rectangle inCollider)
        {
            return (new Rectangle((int)this.position.X,(int) this.position.Y, (int)(this.texture.Width*this.scale.X), (int)(this.texture.Height*this.scale.Y))).Intersects(inCollider);
        }

        public void Draw(SpriteBatch spriteBatch, Color tint)
        {
            spriteBatch.Draw(texture: this.texture, position: this.position, scale: this.scale, color: tint);
        }
    }
}
