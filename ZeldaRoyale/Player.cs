using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeldaRoyale.Content;

#pragma warning disable CS0618
namespace ZeldaRoyale
{
    class Player
    {
        Texture2D texture;
        public Vector2 position;
        Vector2 origin;
        Vector2 scale;
        float rotation;
        float moveSpeed = 200f;
        float jumpDistance = 25f;
        float projectileSpeed = 250f;
        float wandSpeed = 500f;
        public bool allowShoot = true;
        public bool allowWand = true;
        public int coolDownTimeWand = 80;
        public double coolDownCountWand = 0;
        public double coolDownTime = 100;
        public double coolDownCount = 0;
        public int playerBlinkIndex;

        KeyboardState oldState;

        AnimatedSprite upAnimation;
        AnimatedSprite downAnimation;
        AnimatedSprite leftAnimation;
        AnimatedSprite rightAnimation;

        bool usingSword = false;
        bool blink = false;
        bool usingWand = false;
        public Rectangle swordCollider;
        public Rectangle playerCollider;

        Texture2D swordTexturePlayer;
        Texture2D upSword;
        Texture2D downSword;
        Texture2D leftSword;
        Texture2D rightSword;

        Texture2D wandTex;
        Texture2D upWand;
        Texture2D downWand;
        Texture2D leftWand;
        Texture2D rightWand;

        Texture2D upWave;
        Texture2D downWave;
        Texture2D rightWave;
        Texture2D leftWave;

        Texture2D upProjectile;
        Texture2D downProjectile;
        Texture2D rightProjectile;
        Texture2D leftProjectile;

        public int totalHealth = 100;
        public int health = 100;
        public int swordDamage = 14;
        public int projectileDamage = 24;
        public int wandDamge = 4;
        public bool dead;
        bool swordJabbed = false;

        public Keys up;
        public Keys down;
        public Keys right;
        public Keys left;
        public Keys swordStrike;
        public Keys swordBeamKey;
        public Keys wandKey;

        public List<Color> loopColorBlink = new List<Color>();
        public int blinkColorIndex;

        public String hitOrientation;

        List<SwordProjectile> swordProjectileList = new List<SwordProjectile>();
        const int initialCountBlink = 120;
        public int countBlink = initialCountBlink;

        float playerVelocity = 0;

        public String orientation = "down";
        public List<int> removeIndex = new List<int>();

        public Player(Texture2D _texture, Vector2 _position, float _rotation, float targetX, float targetY)
        {
            this.texture = _texture;
            this.position = _position;
            this.rotation = _rotation;
            this.scale = new Vector2(2, 2);
            this.dead = false;
            this.blinkColorIndex = 0;
        }

        public void updateOrigin() { origin = new Vector2(texture.Bounds.Center.X, texture.Bounds.Center.Y); }

        public void loadContent(List<Texture2D> upTexture, List<Texture2D> downTexture, List<Texture2D> rightTexture, List<Texture2D> leftTexture, Texture2D swordUp, Texture2D swordDown, Texture2D swordLeft, Texture2D swordRight, Texture2D _upProjectile, Texture2D _downProjectile, Texture2D _leftProjectile, Texture2D _rightProjectile, Texture2D _upWand, Texture2D _downWand, Texture2D _leftWand, Texture2D _rightWand, Texture2D _upWave, Texture2D _downWave, Texture2D _leftWave, Texture2D _rightWave)
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

            upWand = _upWand;
            downWand = _downWand;
            leftWand = _leftWand;
            rightWand = _rightWand;

            upWave = _upWave;
            downWave = _downWave;
            leftWave = _leftWave;
            rightWave = _rightWave;
        }

        public void checkBounds ()
        {
            if (position.X < 0)
            {
                position.X = 0;
            }

            if (position.X > Game1.windowWidth - 30)
            {
                position.X = Game1.windowWidth - 30;
            }

            if (position.Y < 0)
            {
                position.Y = 0;
            }

            if (position.Y > Game1.windowHeight - 30)
            {
                position.Y = Game1.windowHeight - 30;
            }
        }

        public void handleInput(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(right))
            {
                position += new Vector2(moveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                orientation = "right";
                rightAnimation.playing = true;
                playerVelocity = moveSpeed;

                leftAnimation.playing = false; upAnimation.playing = false; downAnimation.playing = false;
            }
            else if (Keyboard.GetState().IsKeyDown(left))
            {
                position += new Vector2(-moveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                orientation = "left";
                leftAnimation.playing = true;
                playerVelocity = moveSpeed;

                rightAnimation.playing = false; upAnimation.playing = false; downAnimation.playing = false;
            }
            else if (Keyboard.GetState().IsKeyDown(up))
            {
                position += new Vector2(0, -moveSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                orientation = "up";
                upAnimation.playing = true;
                playerVelocity = moveSpeed;

                leftAnimation.playing = false; rightAnimation.playing = false; downAnimation.playing = false;
            }
            else if (Keyboard.GetState().IsKeyDown(down))
            {
                position += new Vector2(0, moveSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                orientation = "down";
                downAnimation.playing = true;
                playerVelocity = moveSpeed;

                leftAnimation.playing = false; upAnimation.playing = false; rightAnimation.playing = false;
            } else if (Keyboard.GetState().IsKeyDown(swordStrike))
            {
                usingSword = true;
                switch (orientation)
                {
                    case "up":
                        swordTexturePlayer = upSword;
                        swordCollider = new Rectangle((int)(this.position.X + 16.5), (int)this.position.Y - 25, upSword.Bounds.Width - 10, upSword.Bounds.Height - 5);
                        break;
                    case "down":
                        swordTexturePlayer = downSword;
                        swordCollider = new Rectangle((int)(this.position.X + 14.5), (int)this.position.Y + 32, upSword.Bounds.Width - 10, upSword.Bounds.Height - 6);
                        break;
                    case "right":
                        swordTexturePlayer = rightSword;
                        swordCollider = new Rectangle((int)(this.position.X + 31.5), (int)(this.position.Y + 13.5), upSword.Bounds.Width + 7, (upSword.Bounds.Height / 4));
                        break;
                    case "left":
                        swordTexturePlayer = leftSword;
                        swordCollider = new Rectangle((int)(this.position.X - 24.5), (int)(this.position.Y + 13.5), upSword.Bounds.Width + 7, (upSword.Bounds.Height / 4));
                        break;
                }
                leftAnimation.playing = false; rightAnimation.playing = false; downAnimation.playing = false; upAnimation.playing = false;
            } else if (currentState.IsKeyDown(wandKey))
            {
                usingWand = true;
                switch (orientation)
                {
                    case "up":
                        wandTex = upWand;
                        break;
                    case "down":
                        wandTex = downWand;
                        break;
                    case "right":
                        wandTex = rightWand;
                        break;
                    case "left":
                        wandTex = leftWand;
                        break;
                }
            } else
            {

            }

            if (usingWand && allowWand)
            {
                switch (orientation)
                {
                    case "up":
                        swordProjectileList.Add(new SwordProjectile(_texture: upWave, _velocity: new Vector2(0, -wandSpeed - playerVelocity), _position: this.position, _orientation: this.orientation, _damage: this.wandDamge));
                        break;
                    case "down":
                        swordProjectileList.Add(new SwordProjectile(_texture: downWave, _velocity: new Vector2(0, wandSpeed + playerVelocity), _position: this.position, _orientation: this.orientation, _damage: this.wandDamge));
                        break;
                    case "right":
                        swordProjectileList.Add(new SwordProjectile(_texture: rightWave, _velocity: new Vector2(wandSpeed + playerVelocity, 0), _position: this.position, _orientation: this.orientation, _damage: this.wandDamge));
                        break;
                    case "left":
                        swordProjectileList.Add(new SwordProjectile(_texture: leftWave, _velocity: new Vector2(-wandSpeed - playerVelocity, 0), _position: this.position, _orientation: this.orientation, _damage: this.wandDamge));
                        break;
                }
                swordProjectileList[swordProjectileList.Count - 1].orientation = this.orientation.ToString().ToLower();
                allowWand = false;
            }

            if (currentState.IsKeyDown(swordBeamKey) && allowShoot)
            {
                switch (orientation)
                {
                    case "up":
                        swordProjectileList.Add(new SwordProjectile(_texture: upProjectile, _velocity: new Vector2(0, -projectileSpeed - playerVelocity), _position: this.position, _orientation: this.orientation, _damage: this.projectileDamage));
                        break;
                    case "down":
                        swordProjectileList.Add(new SwordProjectile(_texture: downProjectile, _velocity: new Vector2(0, projectileSpeed + playerVelocity), _position: this.position, _orientation: this.orientation, _damage: this.projectileDamage));
                        break;
                    case "right":
                        swordProjectileList.Add(new SwordProjectile(_texture: rightProjectile, _velocity: new Vector2(projectileSpeed + playerVelocity, 0), _position: this.position, _orientation: this.orientation, _damage: this.projectileDamage));
                        break;
                    case "left":
                        swordProjectileList.Add(new SwordProjectile(_texture: leftProjectile, _velocity: new Vector2(-projectileSpeed - playerVelocity, 0), _position: this.position, _orientation: this.orientation, _damage: this.projectileDamage));
                        break;
                }
                swordProjectileList[swordProjectileList.Count - 1].orientation = this.orientation.ToString().ToLower();
                allowShoot = false;
            }

            if (currentState.IsKeyUp(wandKey) && oldState.IsKeyDown(wandKey))
            {
                usingWand = false;
            }

            if (currentState.IsKeyUp(swordBeamKey) && oldState.IsKeyDown(swordBeamKey))
            {

            }

            if (currentState.IsKeyUp(right) && oldState.IsKeyDown(right))
            {
                rightAnimation.playing = false;
                playerVelocity = 0;
            }

            if (currentState.IsKeyUp(left) && oldState.IsKeyDown(left))
            {
                leftAnimation.playing = false;
                playerVelocity = 0;
            }

            if (currentState.IsKeyUp(up) && oldState.IsKeyDown(up))
            {
                upAnimation.playing = false;
                playerVelocity = 0;
            }

            if (currentState.IsKeyUp(down) && oldState.IsKeyDown(down))
            {
                downAnimation.playing = false;
                playerVelocity = 0;
            }

            if (currentState.IsKeyUp(swordStrike) && oldState.IsKeyDown(swordStrike))
            {
                usingSword = false;
                swordJabbed = false;
            }

            if (coolDownCount >= coolDownTime)
            {
                coolDownCount = 0;
                allowShoot = true;
            }

            if (coolDownCountWand >= coolDownTimeWand)
            {
                coolDownCountWand = 0;
                allowWand = true;
            }

            for (int i = 0; i < swordProjectileList.Count; i++)
            {
                swordProjectileList[i].updatePosition(gameTime);
                if (swordProjectileList[i].position.X < 0 || swordProjectileList[i].position.X > Game1.windowWidth || swordProjectileList[i].position.Y < 0 || swordProjectileList[i].position.Y > Game1.windowHeight)
                {
                    swordProjectileList.RemoveAt(i);
                }
            }

            coolDownCount += gameTime.ElapsedGameTime.TotalSeconds * 50;
            coolDownCountWand += gameTime.ElapsedGameTime.TotalSeconds * 50;

            oldState = currentState;
        }

        public void Update(GameTime gameTime, List<Octorok> enemyCollider, List<Player> enemyPlayer, bool handleInputbool)
        {
            playerCollider = new Rectangle((int)this.position.X + 5, (int)this.position.Y + 1, (int)(upAnimation.textureList[0].Width * 1.9f), (int)(upAnimation.textureList[0].Height * 1.9f));
            updateOrigin();
            checkBounds();
            if (!blink)
            {
                if (handleInputbool)
                {
                    handleInput(gameTime);
                }
            } else
            {
                rightAnimation.playing = false; leftAnimation.playing = false; upAnimation.playing = false; downAnimation.playing = false;
            }

            if (enemyCollider.Any())
            {
                removeIndex.Clear();
                for (int i = 0; i < swordProjectileList.Count; i++)
                {
                    for (int j = 0; j < enemyCollider.Count; j++)
                    {
                        try
                        {
                            if (swordProjectileList[i].checkCollision(enemyCollider[j].mainCollider))
                            {
                                swordProjectileList.RemoveAt(i);
                                enemyCollider[j].health -= this.swordDamage;
                                if (enemyCollider[j].health <= 0)
                                {
                                    enemyCollider[j].alive = false;
                                }
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {

                        }
                        finally
                        {

                        }
                    }
                }

                if (usingSword && !blink && !swordJabbed)
                {
                    for (int j = 0; j < enemyCollider.Count; j++)
                    {
                        if (swordCollider.Intersects(enemyCollider[j].mainCollider))
                        {
                            if (!(enemyCollider[j].health - swordDamage <= 0))
                            {
                                enemyCollider[j].health -= swordDamage;
                            }
                            else
                            {
                                enemyCollider[j].alive = false;
                            }
                            swordJabbed = true;
                        }
                    }
                }

                if (!blink)
                {
                    for (int i = 0; i < enemyCollider.Count; i++)
                    {
                        if (enemyCollider[i].mainCollider.Intersects(playerCollider))
                        {
                            blink = true;
                            upAnimation.playing = false; downAnimation.playing = false; rightAnimation.playing = false; leftAnimation.playing = false;
                            enemyCollider[i].playerBlink = true;
                            playerBlinkIndex = i;
                            if (!(health - enemyCollider[i].damage <= 0))
                            {
                                health -= enemyCollider[i].damage;
                            }
                            else
                            {
                                this.dead = true;
                            }
                        }
                    }
                }
            }

            if (enemyPlayer.Any())
            {
                removeIndex.Clear();
                for (int i = 0; i < swordProjectileList.Count; i++)
                {
                    for (int j = 0; j < enemyPlayer.Count; j++)
                    {
                        if (this.swordProjectileList[i].checkCollision(enemyPlayer[j].playerCollider))
                        {
                            enemyPlayer[j].health -= (int)swordProjectileList[i].damage;
                            if (enemyPlayer[j].health <= 0)
                            {
                                enemyPlayer[j].dead = true;
                            }
                            enemyPlayer[j].blink = true;
                            enemyPlayer[j].hitOrientation = this.swordProjectileList[i].orientation.ToLower();
                            removeIndex.Add(i);
                        }
                    }
                }

                foreach (var i in removeIndex)
                {
                    swordProjectileList.RemoveAt(i);
                }

                if (usingSword && !blink && !swordJabbed)
                {
                    for (int j = 0; j < enemyPlayer.Count; j++)
                    {
                        if (swordCollider.Intersects(enemyPlayer[j].playerCollider))
                        {
                            enemyPlayer[j].health -= swordDamage;
                            if (enemyPlayer[j].health <= 0)
                            {
                                enemyPlayer[j].dead = true;
                            }
                            swordJabbed = true;
                            enemyPlayer[j].blink = true;
                            enemyPlayer[j].hitOrientation = this.orientation;
                        }
                    }
                }
            }

            upAnimation.Update(); downAnimation.Update(); rightAnimation.Update(); leftAnimation.Update();
        }

        public void Draw(SpriteBatch spriteBatch, List<Octorok> listOctorok, List<Player> enemyPlayer,GameTime gameTime, Color tint)
        {
            if (!blink)
            {
                if (upAnimation.playing || downAnimation.playing || rightAnimation.playing || leftAnimation.playing)
                {
                    upAnimation.Draw(spriteBatch, position, tint); downAnimation.Draw(spriteBatch, position, tint); rightAnimation.Draw(spriteBatch, position, tint); leftAnimation.Draw(spriteBatch, position, tint);
                }
                else
                {
                    if (!usingSword && !usingWand)
                    {
                        switch (orientation)
                        {
                            case "up":
                                spriteBatch.Draw(upAnimation.textureList[upAnimation.currentTextureIndex], position: position, scale: scale, color: tint);
                                break;
                            case "down":
                                spriteBatch.Draw(downAnimation.textureList[downAnimation.currentTextureIndex], position: position, scale: scale, color: tint);
                                break;
                            case "right":
                                spriteBatch.Draw(rightAnimation.textureList[rightAnimation.currentTextureIndex], position: position, scale: scale, color: tint);
                                break;
                            case "left":
                                spriteBatch.Draw(leftAnimation.textureList[leftAnimation.currentTextureIndex], position: position, scale: scale, color: tint);
                                break;
                        }
                    }
                    else if (usingSword)
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
                        spriteBatch.Draw(swordTexturePlayer, position: localSpace, scale: scale, color: tint);
                    } else if (usingWand)
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
                        spriteBatch.Draw(wandTex, position: localSpace, scale: this.scale, color: tint);
                    } else
                    {

                    }
                }

                for (int i = 0; i < swordProjectileList.Count; i++)
                {
                    swordProjectileList[i].Draw(spriteBatch, tint);
                }
            } else
            {
                if (countBlink <= 0)
                {
                    blink = false;
                    try
                    {
                        listOctorok[playerBlinkIndex].playerBlink = false;
                    } catch (ArgumentException)
                    {

                    } finally
                    {

                    }
                    countBlink = initialCountBlink;
                }
                if (countBlink % 10 == 0)
                {
                    switch (this.hitOrientation.ToString().ToLower())
                    {
                        case "up":
                            position += new Vector2(0, -1) * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 100) * 100;
                            spriteBatch.Draw(upAnimation.textureList[upAnimation.currentTextureIndex], position: position, scale: scale, color: loopColorBlink[blinkColorIndex]);
                            break;
                        case "down":
                            position += new Vector2(0, 1) * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 100) * 100;
                            spriteBatch.Draw(downAnimation.textureList[downAnimation.currentTextureIndex], position: position, scale: scale, color: loopColorBlink[blinkColorIndex]);
                            break;
                        case "right":
                            position += new Vector2(1, 0) * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 100) * 100;
                            spriteBatch.Draw(rightAnimation.textureList[rightAnimation.currentTextureIndex], position: position, scale: scale, color: loopColorBlink[blinkColorIndex]);
                            break;
                        case "left":
                            position += new Vector2(-1, 0) * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 100) * 100;
                            spriteBatch.Draw(leftAnimation.textureList[leftAnimation.currentTextureIndex], position: position, scale: scale, color: loopColorBlink[blinkColorIndex]);
                            break;
                    }
                    blinkColorIndex += 1;
                    if (blinkColorIndex >= loopColorBlink.Count) { blinkColorIndex = 0; }
                }

                countBlink -= 1;
            }
        }

        public static double degreesToRadians(double i)
        {
            return i * (Math.PI / 180);
        }
    }
}