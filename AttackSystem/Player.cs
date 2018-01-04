using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AttackSystem.Content;

namespace AttackSystem
{
    class Player
    {
        Texture2D texture;
        Vector2 position;
        Vector2 origin;
        Vector2 scale;
        float rotation;
        float moveSpeed = 200f;
        float jumpDistance = 25f;
        float projectileSpeed = 250f;
        bool allowShoot = true;
        int coolDownTime = 100;
        int coolDownCount = 0;

        KeyboardState oldState;

        AnimatedSprite upAnimation;
        AnimatedSprite downAnimation;
        AnimatedSprite leftAnimation;
        AnimatedSprite rightAnimation;

        bool usingSword = false;
        Rectangle swordCollider;

        Texture2D swordTexturePlayer;
        Texture2D upSword;
        Texture2D downSword;
        Texture2D leftSword;
        Texture2D rightSword;

        Texture2D upProjectile;
        Texture2D downProjectile;
        Texture2D rightProjectile;
        Texture2D leftProjectile;
        List<SwordProjectile> swordProjectileList = new List<SwordProjectile>();

        float playerVelocity = 0;

        String orientation = "down";

        public Player(Texture2D _texture, Vector2 _position, float _rotation, float targetX, float targetY)
        {
            this.texture = _texture;
            this.position = _position;
            this.rotation = _rotation;
            //this.scale = new Vector2(targetX / texture.Width, targetY / texture.Height);
            this.scale = new Vector2(2, 2);
        }

        public void updateOrigin() { origin = new Vector2(texture.Bounds.Center.X, texture.Bounds.Center.Y); }

        public void loadContent(List<Texture2D> upTexture, List<Texture2D> downTexture, List<Texture2D> rightTexture, List<Texture2D> leftTexture, Texture2D swordUp, Texture2D swordDown, Texture2D swordLeft, Texture2D swordRight, Texture2D _upProjectile, Texture2D _downProjectile, Texture2D _leftProjectile, Texture2D _rightProjectile)
        {
            upAnimation = new AnimatedSprite(upTexture, scale);
            downAnimation = new AnimatedSprite(downTexture, scale);
            leftAnimation = new AnimatedSprite(leftTexture, scale);
            rightAnimation = new AnimatedSprite(rightTexture, scale);
            upSword = swordUp;
            downSword = swordDown;
            leftSword = swordLeft;
            rightSword = swordRight;

            upProjectile = _upProjectile;
            downProjectile = _downProjectile;
            leftProjectile = _leftProjectile;
            rightProjectile = _rightProjectile;
        }

        public void checkBounds ()
        {
            if (position.X < 0)
            {
                position.X = 0;
            }

            if (position.X > Game1.windowWidth)
            {
                position.X = Game1.windowWidth;
            }

            if (position.Y < 0)
            {
                position.Y = 0;
            }

            if (position.Y > Game1.windowHeight)
            {
                position.Y = Game1.windowHeight;
            }
        }

        public void handleInput(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            //if (allowShoot)
            //{
            //    Console.WriteLine("Shoot Cool Down Time Over!");
            //} else
            //{
            //    Console.WriteLine("Cool Down Time Started!");
            //}

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position += new Vector2(moveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                orientation = "right";
                rightAnimation.playing = true;
                playerVelocity = moveSpeed;

                leftAnimation.playing = false; upAnimation.playing = false; downAnimation.playing = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position += new Vector2(-moveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                orientation = "left";
                leftAnimation.playing = true;
                playerVelocity = moveSpeed;

                rightAnimation.playing = false; upAnimation.playing = false; downAnimation.playing = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position += new Vector2(0, -moveSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                orientation = "up";
                upAnimation.playing = true;
                playerVelocity = moveSpeed;

                leftAnimation.playing = false; rightAnimation.playing = false; downAnimation.playing = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position += new Vector2(0, moveSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                orientation = "down";
                downAnimation.playing = true;
                playerVelocity = moveSpeed;

                leftAnimation.playing = false; upAnimation.playing = false; rightAnimation.playing = false;
            } else
            {

            }

            if (currentState.IsKeyUp(Keys.Right) && oldState.IsKeyDown(Keys.Right))
            {
                rightAnimation.playing = false;
                playerVelocity = 0;
            }

            if (currentState.IsKeyUp(Keys.Left) && oldState.IsKeyDown(Keys.Left))
            {
                leftAnimation.playing = false;
                playerVelocity = 0;
            }

            if (currentState.IsKeyUp(Keys.Up) && oldState.IsKeyDown(Keys.Up))
            {
                upAnimation.playing = false;
                playerVelocity = 0;
            }

            if (currentState.IsKeyUp(Keys.Down) && oldState.IsKeyDown(Keys.Down))
            {
                downAnimation.playing = false;
                playerVelocity = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                usingSword = true;
                switch(orientation)
                {
                    case "up":
                        swordTexturePlayer = upSword;
                        break;
                    case "down":
                        swordTexturePlayer = downSword;
                        break;
                    case "right":
                        swordTexturePlayer = rightSword;
                        break;
                    case "left":
                        swordTexturePlayer = leftSword;
                        break;
                }
            }

            if (currentState.IsKeyUp(Keys.Space) && oldState.IsKeyDown(Keys.Space))
            {
                usingSword = false;
            }

            if (currentState.IsKeyDown(Keys.LeftShift) && allowShoot)
            {
                switch (orientation)
                {
                    case "up":
                        swordProjectileList.Add(new SwordProjectile(_texture: upProjectile, _velocity: new Vector2(0, -projectileSpeed - playerVelocity),_position: this.position));
                        break;
                    case "down":
                        swordProjectileList.Add(new SwordProjectile(_texture: downProjectile, _velocity: new Vector2(0, projectileSpeed + playerVelocity), _position: this.position));
                        break;
                    case "right":
                        swordProjectileList.Add(new SwordProjectile(_texture: rightProjectile, _velocity: new Vector2(projectileSpeed + playerVelocity, 0), _position: this.position));
                        break;
                    case "left":
                        swordProjectileList.Add(new SwordProjectile(_texture: leftProjectile, _velocity: new Vector2(-projectileSpeed-playerVelocity, 0), _position: this.position));
                        break;
                }
                allowShoot = false;
            }

            if (currentState.IsKeyUp(Keys.LeftShift) && oldState.IsKeyDown(Keys.LeftShift))
            {
                
            }

            if (coolDownCount >= coolDownTime)
            {
                coolDownCount = 0;
                allowShoot = true;
            }

            for (int i = 0; i < swordProjectileList.Count; i++)
            {
                swordProjectileList[i].updatePosition(gameTime);
                if (swordProjectileList[i].position.X < 0 || swordProjectileList[i].position.X > Game1.windowWidth || swordProjectileList[i].position.Y < 0 || swordProjectileList[i].position.Y > Game1.windowHeight)
                {
                    swordProjectileList.RemoveAt(i);
                }
            }
            coolDownCount += 1;

            oldState = currentState;
        }

        public void Update(GameTime gameTime, Rectangle enemyCollider)
        {
            updateOrigin();
            checkBounds();
            handleInput(gameTime);

            for (int i = 0; i < swordProjectileList.Count; i++)
            {
                if (swordProjectileList[i].checkCollision(enemyCollider))
                {
                    swordProjectileList.RemoveAt(i);
                }
            }

            upAnimation.Update(); downAnimation.Update(); rightAnimation.Update(); leftAnimation.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture: texture, position: position, rotation: rotation, origin: origin, scale : scale);
            showText(spriteBatch);
            if (upAnimation.playing || downAnimation.playing || rightAnimation.playing || leftAnimation.playing)
            {
                upAnimation.Draw(spriteBatch, position); downAnimation.Draw(spriteBatch, position); rightAnimation.Draw(spriteBatch, position); leftAnimation.Draw(spriteBatch, position);
            } else
            {
                if (!usingSword)
                {
                    switch (orientation)
                    {
                        case "up":
                            spriteBatch.Draw(upAnimation.textureList[upAnimation.currentTextureIndex], position: position, scale: scale);
                            break;
                        case "down":
                            spriteBatch.Draw(downAnimation.textureList[downAnimation.currentTextureIndex], position: position, scale: scale);
                            break;
                        case "right":
                            spriteBatch.Draw(rightAnimation.textureList[rightAnimation.currentTextureIndex], position: position, scale: scale);
                            break;
                        case "left":
                            spriteBatch.Draw(leftAnimation.textureList[leftAnimation.currentTextureIndex], position: position, scale: scale);
                            break;
                    }
                } else
                {
                    Vector2 localSpace = Vector2.Zero;
                    switch (orientation)
                    {
                        case "up":
                            localSpace = new Vector2(position.X, position.Y - jumpDistance);
                            break;
                        case "down":
                            localSpace = new Vector2(position.X, position.Y);
                            break;
                        case "right":
                            localSpace = new Vector2(position.X, position.Y);
                            break;
                        case "left":
                            localSpace = new Vector2(position.X - jumpDistance, position.Y);
                            break;
                    }
                    spriteBatch.Draw(swordTexturePlayer, position: localSpace, scale: scale);
                }
            }

            for (int i = 0; i < swordProjectileList.Count; i++)
            {
                swordProjectileList[i].Draw(spriteBatch);
            }
        }

        public void showText(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(Game1.mainFont, "Cool Down Time: " + (coolDownTime - coolDownCount).ToString(), position: new Vector2(20,20), color: Color.Blue);
        }
    }
}
