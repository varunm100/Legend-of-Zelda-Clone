using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackSystem
{
    class AnimatedSprite
    {
        public List<Texture2D> textureList;
        private int currentFrame;
        private int totalFrames;
        public int currentTextureIndex = 0;
        public long countTill = 2;
        public bool playing = false;
        public bool keyPressed = false;
        public int count = -1;
        public Vector2 renderScale;

        public AnimatedSprite(List <Texture2D> inputList, Vector2 _renderScale, long _countTill = 12)
        {
            this.textureList = inputList;
            this.currentFrame = 0;
            this.totalFrames = inputList.Count;
            this.countTill = _countTill;
            this.renderScale = _renderScale;
        }

        public AnimatedSprite(List<Texture2D> inputList, Vector2 _renderScale, int _count, long _countTill = 12)
        {
            this.textureList = inputList;
            this.currentFrame = 0;
            this.totalFrames = inputList.Count;
            this.countTill = _countTill;
            this.renderScale = _renderScale;
            this.count = _count;
        }

        public void Update()
        {
            if (playing)
            {
                currentFrame++;
                if (currentFrame == countTill)
                    currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color tint)
        {
            if (playing)
            {
                if (currentFrame % countTill == 0)
                {
                    currentTextureIndex += 1;
                }
                if (!(currentTextureIndex > (textureList.Count - 1)))
                {
                    spriteBatch.Draw(textureList[currentTextureIndex], position: location, scale: renderScale, color: tint);
                }
                else
                {
                    if (count <= 0)
                    {
                        currentTextureIndex = 0;
                    } else
                    {
                        this.playing = false;
                    }
                }
            }
        }
    }
}
