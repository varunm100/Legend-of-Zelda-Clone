using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AttackSystem
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static float windowWidth;
        public static float windowHeight;

        public static Texture2D whiteTexture;
        Player zelda;
        Player zelda1;
        List<Octorok> octorokList = new List<Octorok>();
        int numOctorok = 8;
        AnimatedSprite animatedSprite;
        int enemyAStarCount = 0;
        public static SpriteFont mainFont;
        public bool spawnOctorok = false;
        public bool gameOver = false;
        public int gameOverRotationInt = 0;
        public float gameOverScaleInt = 1;
        public String winningString = "";
        public int timePerScale = 0;
        public int totalTime = 20; 
        Texture2D background;
        Vector2 backgroundScale;
        Texture2D contrastHearts;
        Vector2 contrastHeartScale;

        List<Player> zeldaEnemy = new List<Player>();
        List<Player> zelda1Enemy = new List<Player>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            windowWidth = GraphicsDevice.Viewport.Width;
            windowHeight = GraphicsDevice.Viewport.Height;

            mainFont = this.Content.Load<SpriteFont>("mainFont");

            List<Texture2D> upTex = new List<Texture2D>();
            List<Texture2D> downTex = new List<Texture2D>();
            List<Texture2D> leftTex = new List<Texture2D>();
            List<Texture2D> rightTex = new List<Texture2D>();

            Texture2D _downSword = this.Content.Load<Texture2D>("sword/down/sword-down-left");
            Texture2D _upSword = this.Content.Load<Texture2D>("sword/up/sword-up-left");
            Texture2D _leftSword = this.Content.Load<Texture2D>("sword/left/sword-left");
            Texture2D _rightSword = this.Content.Load<Texture2D>("sword/right/sword-right");

            Texture2D _upProjectile = this.Content.Load<Texture2D>("projectiles/sword/sword-projectile-up");
            Texture2D _downProjectile = this.Content.Load<Texture2D>("projectiles/sword/sword-projectile-down");
            Texture2D _leftProjectile = this.Content.Load<Texture2D>("projectiles/sword/sword-projectile-right");
            Texture2D _rightProjectile = this.Content.Load<Texture2D>("projectiles/sword/sword-projectile-left");

            upTex.Add(this.Content.Load<Texture2D>("movement/up/up-1"));
            upTex.Add(this.Content.Load<Texture2D>("movement/up/up-2"));
            downTex.Add(this.Content.Load<Texture2D>("movement/down/down-1"));
            downTex.Add(this.Content.Load<Texture2D>("movement/down/down-2"));
            leftTex.Add(this.Content.Load<Texture2D>("movement/left/left-1"));
            leftTex.Add(this.Content.Load<Texture2D>("movement/left/left-2"));
            rightTex.Add(this.Content.Load<Texture2D>("movement/right/right-1"));
            rightTex.Add(this.Content.Load<Texture2D>("movement/right/right-2"));

            zelda = new Player(this.Content.Load<Texture2D>("mr.square"), new Vector2(50, windowHeight/2), 0, 500, 500);
            zelda.loadContent(upTexture: upTex, downTexture: downTex, leftTexture: leftTex, rightTexture: rightTex, swordDown: _downSword, swordUp: _upSword, swordLeft: _leftSword, swordRight: _rightSword, _upProjectile: _upProjectile, _downProjectile: _downProjectile, _leftProjectile: _leftProjectile, _rightProjectile: _rightProjectile);
            zelda1 = new Player(this.Content.Load<Texture2D>("mr.square"), new Vector2(windowWidth - 50, windowHeight / 2), 0, 500, 500);
            zelda1.loadContent(upTexture: upTex, downTexture: downTex, leftTexture: leftTex, rightTexture: rightTex, swordDown: _downSword, swordUp: _upSword, swordLeft: _leftSword, swordRight: _rightSword, _upProjectile: _upProjectile, _downProjectile: _downProjectile, _leftProjectile: _leftProjectile, _rightProjectile: _rightProjectile);


            zelda.up = Keys.W;
            zelda.down = Keys.S;
            zelda.left = Keys.A;
            zelda.right = Keys.D;
            zelda.lshift = Keys.LeftShift;
            zelda.space = Keys.LeftAlt;

            zelda1.up = Keys.P;
            zelda1.down = Keys.OemSemicolon;
            zelda1.left = Keys.L;
            zelda1.right = Keys.OemQuotes;
            zelda1.lshift = Keys.RightShift;
            zelda1.space = Keys.RightAlt;

            zeldaEnemy.Add(zelda1);
            zelda1Enemy.Add(zelda);

            List<Vector2> path = aStar.getPath(new Vector2(0, 0), zelda.position);

            if (!spawnOctorok)
            {
                numOctorok = 0;
            }
            for (int i = 0; i < numOctorok; i++)
            {
                octorokList.Add(new Octorok(this.Content.Load<Texture2D>("octorok-0"), new Vector2(i * (windowWidth / numOctorok), 0), path));
            }
            whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });
            background = this.Content.Load<Texture2D>("zelda-arena-cropped");
            contrastHearts = this.Content.Load<Texture2D>("contrast-hearts");
            backgroundScale = new Vector2((windowWidth / background.Width), (windowHeight / background.Height));
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            windowWidth = GraphicsDevice.Viewport.Bounds.Width;
            windowHeight = GraphicsDevice.Viewport.Bounds.Height;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            backgroundScale = new Vector2((windowWidth / background.Width), (windowHeight / background.Height));
            if (spawnOctorok)
            {
                for (int i = 0; i < octorokList.Count; i++)
                {
                    octorokList[i].mainCollider = new Rectangle((int)octorokList[i].position.X, (int)octorokList[i].position.Y, (int)(octorokList[i].texture.Bounds.Width * octorokList[i].scaleDivisor), (int)(octorokList[i].texture.Bounds.Height * octorokList[i].scaleDivisor));
                }
            }
            if (spawnOctorok)
            {
                for (int i = 0; i < octorokList.Count; i++)
                {
                    octorokList[i].goToAllPositions();
                    octorokList[i].startPathFinding(zelda.position);
                    enemyAStarCount += 1;
                    if (enemyAStarCount % 2 == 0)
                    {
                        octorokList[i].listPosition = aStar.getPath(octorokList[i].position, zelda.position);
                        octorokList[i].moving = true;
                        octorokList[i].currentPositionIndex = 0;
                        enemyAStarCount = 0;
                    }
                    if (!octorokList[i].alive)
                    {
                        octorokList.RemoveAt(i);
                    }
                }
            }

            Console.WriteLine("Player 1 Health: " + zelda.health);
            Console.WriteLine("Player 2 Health: " + zelda1.health);
            if (zelda.health <= 0 && !gameOver) { Console.WriteLine("Player 2 Wins!"); gameOver = true; winningString = "BOOM! P2 RECKS P1!"; }
            if (zelda1.health <= 0 && !gameOver) { Console.WriteLine("Player 1 Wins!"); gameOver = true; winningString = "BOOM! P1 RECKS P2"; }

            zelda.Update(gameTime, octorokList, zeldaEnemy, true);
            zelda1.Update(gameTime, octorokList, zelda1Enemy, true);

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                zelda.health = zelda.totalHealth;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                zelda1.health = zelda1.totalHealth;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(new SpriteSortMode(), null, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(texture: background, position: new Vector2(windowWidth / 2, windowHeight / 2), origin: new Vector2(background.Bounds.Center.X, background.Bounds.Center.Y), scale: backgroundScale);
            if (!gameOver)
            {
                zelda.Draw(spriteBatch, octorokList, zeldaEnemy, gameTime, Color.White);
                zelda1.Draw(spriteBatch, octorokList, zelda1Enemy, gameTime, Color.White);
                for (int i = 0; i < octorokList.Count; i++)
                {
                    octorokList[i].Draw(spriteBatch);
                }
                double zeldaWidth = ((double)zelda.health / (double)zelda.totalHealth) * ((double)2 * (double)contrastHearts.Width);
                double zelda1Width = ((double)zelda1.health / (double)zelda1.totalHealth) * ((double)2 * (double)contrastHearts.Width);
                if (zeldaWidth < 0) { zeldaWidth = 0; }
                if (zelda1Width < 0) { zelda1Width = 0; }
                Rectangle ZeldaheartsRectangle = new Rectangle(0, 0, (int)zeldaWidth, 2 * contrastHearts.Height);
                Rectangle Zelda1heartsRectangle = new Rectangle((int)(windowWidth - (2 * contrastHearts.Width)), 0, (int)zelda1Width, 2 * contrastHearts.Height);
                spriteBatch.Draw(whiteTexture, ZeldaheartsRectangle, Color.Red);
                spriteBatch.Draw(whiteTexture, Zelda1heartsRectangle, Color.Red);
                spriteBatch.Draw(texture: contrastHearts, position: new Vector2(0,0), scale: new Vector2(2,2));
                spriteBatch.Draw(texture: contrastHearts, position: new Vector2(windowWidth-(2*contrastHearts.Width), 0), scale: new Vector2(2, 2));
            } else
            {
                spriteBatch.DrawString(spriteFont: Game1.mainFont, text: winningString, position: new Vector2((windowWidth / 2)-250, windowHeight / 2), color: Color.Red, rotation: (float)Player.degreesToRadians(1), origin: new Vector2(0, 0), scale: gameOverScaleInt, effects: new SpriteEffects(), layerDepth: 1);
                timePerScale += 1;
                if (timePerScale % totalTime == 0)
                {
                    gameOverScaleInt += 0.1f;
                    timePerScale = 1;
                }
                if (gameOverScaleInt >= 2)
                {
                    gameOverScaleInt = 1;
                }
                gameOverRotationInt += 1;
                if (gameOverRotationInt >= 360)
                {
                    gameOverRotationInt = 0;
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
