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
        List<Octorok> octorokList = new List<Octorok>();
        int numOctorok = 8;
        AnimatedSprite animatedSprite;
        int enemyAStarCount = 0;
        public static SpriteFont mainFont;
        const float unitToPixel = 100.0f;
        const float pixelToUnit = 1 / unitToPixel;

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

            zelda = new Player(this.Content.Load<Texture2D>("mr.square"), new Vector2(windowWidth/2, windowHeight/2), 0, 500, 500);
            zelda.loadContent(upTexture: upTex, downTexture: downTex, leftTexture: leftTex, rightTexture: rightTex, swordDown: _downSword, swordUp: _upSword, swordLeft: _leftSword, swordRight: _rightSword, _upProjectile: _upProjectile, _downProjectile: _downProjectile, _leftProjectile: _leftProjectile, _rightProjectile: _rightProjectile);

            //List<Vector2> positionList = new List<Vector2>();
            //positionList.Add(new Vector2(20,20));
            //positionList.Add(new Vector2(100, windowHeight));
            //positionList.Add(new Vector2(windowWidth/2, windowHeight/2));
            List<Vector2> path = aStar.getPath(new Vector2(0, 0), zelda.position);

            for (int i = 0; i < numOctorok; i++)
            {
                octorokList.Add(new Octorok(this.Content.Load<Texture2D>("octorok-0"), new Vector2(i * (windowWidth / numOctorok), 0), path));
            }
            whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });
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

            for (int i = 0; i < octorokList.Count; i++)
            {
                octorokList[i].mainCollider = new Rectangle((int)octorokList[i].position.X, (int)octorokList[i].position.Y, (int)(octorokList[i].texture.Bounds.Width * octorokList[i].scaleDivisor), (int)(octorokList[i].texture.Bounds.Height * octorokList[i].scaleDivisor));
            }
            zelda.Update(gameTime, octorokList);
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(new SpriteSortMode(), null, SamplerState.PointClamp, null, null);
            zelda.Draw(spriteBatch, octorokList, gameTime);
            for (int i = 0; i < octorokList.Count; i++)
            {
                octorokList[i].Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
