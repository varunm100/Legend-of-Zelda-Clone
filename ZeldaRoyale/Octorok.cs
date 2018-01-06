using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#pragma warning disable CS0618
namespace ZeldaRoyale
{
    class Octorok
    {
        public Texture2D texture;
        public Vector2 position;
        Vector2 velocity = Vector2.Zero;
        public List<Vector2> listPosition;
        public bool moving = true;
        public int currentPositionIndex;
        public float scaleDivisor = 0.05f;
        float StartrangeRadius = 200;
        float EndrangeRadius = 500;
        public bool startAStar = false;
        public bool alive = true;
        public Rectangle mainCollider;
        public bool playerBlink = false;
        public int health = 50;
        public int damage = 5; 

        public Octorok(Texture2D _texture, Vector2 _position, List<Vector2> _listPosition)
        {
            this.texture = _texture;
            this.position = _position;
            this.listPosition = _listPosition;
            this.currentPositionIndex = 0;
        }

        public void goToAllPositions()
        {
            if (this.startAStar)
            {
                if (this.currentPositionIndex < listPosition.Count && moving && !playerBlink)
                {
                    position = listPosition[this.currentPositionIndex];
                    this.currentPositionIndex += 2;
                }
                else
                {
                    this.moving = false;
                    this.currentPositionIndex = 0;
                }
            }
        }

        public void startPathFinding(Vector2 _position)
        {
            if (Vector2.Distance(_position, this.position) <= StartrangeRadius)
            {
                startAStar = true;
            } else if (Vector2.Distance(_position, this.position) >= EndrangeRadius)
            {
                startAStar = false;
            } else
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: this.texture, position: this.position, scale: new Vector2(scaleDivisor,scaleDivisor));
        }
    }
}
