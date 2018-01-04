using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttackSystem
{
    class Octorok
    {
        public Texture2D texture;
        public Vector2 position;
        Vector2 velocity = Vector2.Zero;
        Vector2 origin;
        List<Vector2> listPosition;
        bool moving = false;
        int waitTime = 100;
        int currentPositionIndex;
        float divisor = 0.3f;
        bool finsishedX = false;
        public float scaleDivisor = 0.05f;
        bool destroyed = false;

        public Octorok(Texture2D _texture, Vector2 _position, List<Vector2> _listPosition)
        {
            this.texture = _texture;
            this.position = _position;
            this.listPosition = _listPosition;
            this.currentPositionIndex = 0;
        }

        public void moveToPoint(Vector2 dest, Vector2 pos)
        {
            velocity.X -= (pos.X - dest.X);
            velocity.Y -= (pos.Y - dest.Y);
            moving = true;
        }

        public void moveToNextPoint()
        {
            if (currentPositionIndex+1 < listPosition.Count)
            {
                currentPositionIndex += 1;
                moveToPoint(listPosition[currentPositionIndex], this.position);
            } else
            {
                currentPositionIndex = 0;
                startWalkLoop();
            }
        }

        public void checkReached()
        {
            if (Vector2.Distance(position,listPosition[currentPositionIndex]) < 0.1)
            {
                Console.WriteLine("Reched Point");
                this.velocity = Vector2.Zero;
                moveToNextPoint();
            }
        }

        public void startWalkLoop()
        {
            moveToPoint(this.listPosition[this.currentPositionIndex], this.position);
        }

        public void updatePosition(GameTime gameTime)
        {
            this.position.X += divisor * this.velocity.X * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            this.position.Y += divisor * this.velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            checkReached();
        }

        public void goToAllPositions()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: this.texture, position: this.position, scale: new Vector2(scaleDivisor,scaleDivisor));
        }
    }
}
