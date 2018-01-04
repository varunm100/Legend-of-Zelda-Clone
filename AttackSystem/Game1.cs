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

        Player zelda;
        Octorok octorok;
        AnimatedSprite animatedSprite;
        public static SpriteFont mainFont;
        
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
            List<Vector2> path = aStar.getPath(new Vector2(0, 0), new Vector2(windowWidth / 2, windowHeight / 2));
            foreach (var i in path)
            {
                Console.WriteLine(i.X.ToString() + ", " + i.Y.ToString());
            }
            octorok = new Octorok(this.Content.Load<Texture2D>("octorok-0"), new Vector2(0,0), path);
            octorok.startWalkLoop();
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

            zelda.Update(gameTime, new Rectangle((int)octorok.position.X, (int)octorok.position.Y, (int)(octorok.texture.Bounds.Width*octorok.scaleDivisor), (int)(octorok.texture.Bounds.Height*octorok.scaleDivisor)));
            octorok.updatePosition(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(new SpriteSortMode(), null, SamplerState.PointClamp, null, null);
            zelda.Draw(spriteBatch);
            octorok.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
