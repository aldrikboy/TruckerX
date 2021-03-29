using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TruckerX.Extensions;
using TruckerX.Locations;
using TruckerX.Messaging;
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

        public BaseScene activeScene { get; private set; } = null;
        private SimulationOverlayScene overlayScene = null;
        RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
        Simulation simulation;

        public TruckerX()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferMultiSampling = true;
            _graphics.ApplyChanges();
            Game = this;
            Window.AllowAltF4 = true;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            

            WorldData.CreateData();
            WorldState.CreateDefault();
        }

        public void SetScene(BaseScene scene)
        {
            if (this.activeScene != null) this.activeScene.Dispose();
            this.activeScene = scene;
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            // Minimum window size
            if (Window.ClientBounds.Width < 768)
            {
                _graphics.PreferredBackBufferWidth = 768;
                _graphics.ApplyChanges();
            }
            if (Window.ClientBounds.Height < 432)
            {
                _graphics.PreferredBackBufferHeight = 432;
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
            ContentLoader.LoadContent(Content);

            this.activeScene = new LoadingScene();
            this.overlayScene = new SimulationOverlayScene();
            TargetRetangle = GetRenderRectangle();
            simulation = new Simulation();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            Helper.PrevCursorToSet = Helper.CursorToSet;
            Helper.CursorToSet = MouseCursor.Arrow;

            KeyboardExtensions.Update();

            overlayScene.Update(gameTime);
            simulation.Update(gameTime);
            activeScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            TruckerX.Game.GraphicsDevice.ScissorRectangle = TargetRetangle;

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                       null, null, _rasterizerState);
            activeScene.Draw(_spriteBatch, gameTime);
            overlayScene.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();

            if (Helper.PrevCursorToSet != Helper.CursorToSet)
                Mouse.SetCursor(Helper.CursorToSet);
            MouseStateExtensions.MouseUsedThisFrame = false;

            base.Draw(gameTime);
        }
    }
}
