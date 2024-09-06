using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AudioExample
{
    /// <summary>
    /// A game demostrating the use of audio content
    /// </summary>
    public class AudioExampleGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        private CoinSprite[] coins;
        private SlimeGhostSprite slimeGhost;
        private SpriteFont spriteFont;
        private int coinsLeft;

        private Texture2D ball;
        private SoundEffect coinPickup;
        private Song backgroundMusic; 

        /// <summary>
        /// A game demonstrating collision detection
        /// </summary>
        public AudioExampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game 
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            System.Random rand = new System.Random();
            coins = new CoinSprite[]
            {
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height))
            };
            coinsLeft = coins.Length;
            slimeGhost = new SlimeGhostSprite();

            base.Initialize();
        }

        /// <summary>
        /// Loads content for the game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var coin in coins) coin.LoadContent(Content);
            slimeGhost.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("arial");
            ball = Content.Load<Texture2D>("ball");
            coinPickup = Content.Load<SoundEffect>("Pickup_Coin15");
            backgroundMusic = Content.Load<Song>("DeeYan-Key-TheGame");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f; // added for headphone users
            MediaPlayer.Play(backgroundMusic);
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            slimeGhost.Update(gameTime);

            // Detect and process collisions
            slimeGhost.Color = Color.White;
            foreach (var coin in coins)
            {
                if(!coin.Collected && coin.Bounds.CollidesWith(slimeGhost.Bounds))
                {
                    slimeGhost.Color = Color.Red;
                    coin.Collected = true;
                    coinsLeft--;
                    coinPickup.Play(0.2f, 0, 0); // added for headphone users
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var coin in coins)
            {
                coin.Draw(gameTime, spriteBatch);
                /*
                var rect = new Rectangle((int)(coin.Bounds.Center.X - coin.Bounds.Radius),
                                         (int)(coin.Bounds.Center.Y - coin.Bounds.Radius),
                                         (int)(2*coin.Bounds.Radius), (int)(2*coin.Bounds.Radius));
                spriteBatch.Draw(ball, rect, Color.White);
                */
            }
            /*
            var rectG = new Rectangle((int)(slimeGhost.Bounds.Center.X - slimeGhost.Bounds.Radius),
                                         (int)(slimeGhost.Bounds.Center.Y - slimeGhost.Bounds.Radius),
                                         (int)(2 * slimeGhost.Bounds.Radius), (int)(2 * slimeGhost.Bounds.Radius));
            spriteBatch.Draw(ball, rectG, Color.White);
            */
            slimeGhost.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(spriteFont, $"Coins left: {coinsLeft}", new Vector2(2,2), Color.Gold);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
