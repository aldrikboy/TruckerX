using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TruckerX.Locations;
using TruckerX.Scenes;
using TruckerX.State;

namespace TruckerX
{
    public class TruckerX : Game
    {
        public static TruckerX Game;

        public static int WindowWidth { get { return Game.Window.ClientBounds.Width; } }
        public static int WindowHeight { get { return Game.Window.ClientBounds.Height; } }

        public static Rectangle TargetRetangle { get; private set; }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public BaseScene activeScene = null;
        RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };

        public TruckerX()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            Game = this;
            Window.AllowAltF4 = true;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            

            WorldData.CreateData();
            WorldState.CreateDefault();
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            // Minimum window size
            if (Window.ClientBounds.Width < 640)
            {
                _graphics.PreferredBackBufferWidth = 640;
                _graphics.ApplyChanges();
            }
            if (Window.ClientBounds.Height < 360)
            {
                _graphics.PreferredBackBufferHeight = 360;
                _graphics.ApplyChanges();
            }

            TargetRetangle = GetRenderRectangle();
        }

        private static Rectangle GetRenderRectangle()
        {
            int w = WindowWidth;
            int h = WindowHeight;

            double targetRatio = 16.0 / 9.0;
            double ratio = (double)w / h;
            if (ratio < targetRatio) // Too much height
            {
                int newHeight = (int)(w / targetRatio);
                int newY = (h - newHeight) / 2;
                return new Rectangle(0, newY, w, newHeight);
            }
            else if (ratio > targetRatio) // Too much width
            {
                int newWidth = (int)(h * targetRatio);
                int newX = (w - newWidth) / 2;
                return new Rectangle(newX, 0, newWidth, h);
            }

            return new Rectangle(0,0,w,h);
        }

        protected override void Initialize()
        {
            this.activeScene = new LoadingScene();
            TargetRetangle = GetRenderRectangle();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            activeScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                       null, null, _rasterizerState);
            activeScene.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
