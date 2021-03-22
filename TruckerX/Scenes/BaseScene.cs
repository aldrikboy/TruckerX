using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Extensions;

namespace TruckerX.Scenes
{
    public abstract class BaseScene : IScene
    {
        protected ContentLoader ContentLoader { get; }
        public bool DoneLoading { get { return ContentLoader.Done; } }

        public Dictionary<string, AssetDefinition<Texture2D>> Textures { get; internal set; } = new Dictionary<string, AssetDefinition<Texture2D>>();
        public Dictionary<string, AssetDefinition<SpriteFont>> Fonts { get; internal set; } = new Dictionary<string, AssetDefinition<SpriteFont>>();
        public Dictionary<string, AssetDefinition<Song>> Songs { get; internal set; } = new Dictionary<string, AssetDefinition<Song>>();
        public Dictionary<string, AssetDefinition<SoundEffect>> Samples { get; internal set; } = new Dictionary<string, AssetDefinition<SoundEffect>>();

        private TimeSpan fadeInDuration = TimeSpan.FromMilliseconds(200);
        protected TimeSpan elapsedTime = TimeSpan.Zero;
        private BaseScene loadingScene = null;
        private TimeSpan loadingSceneSwapDelay = TimeSpan.Zero;
        public bool ChangingScene { get { return this.loadingScene != null; } }

        protected event EventHandler OnDisplay;

        public BaseScene()
        {
            this.elapsedTime = new TimeSpan(0);

            DeclareAssets();
            Fonts.AddRange(ContentDefinition.AllFonts);
            Textures.Add("black", new AssetDefinition<Texture2D>("Textures/black"));
            ContentLoader = new ContentLoader();
            ContentLoader.LoadContent(TruckerX.Game.Content, this);
        }

        public abstract void DeclareAssets();

        public virtual void Draw(SpriteBatch batch, GameTime gameTime)
        {
            var black = this.GetTexture("black");
            float opacity = 1 - (float)elapsedTime.TotalMilliseconds / (float)fadeInDuration.TotalMilliseconds;

            if (this.loadingScene != null && elapsedTime >= fadeInDuration) 
                opacity = ((float)loadingSceneSwapDelay.TotalMilliseconds / (float)fadeInDuration.TotalMilliseconds);

            batch.Draw(black, new Rectangle(0, 0, TruckerX.WindowWidth,
                    TruckerX.WindowHeight), Color.White*opacity);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.elapsedTime == TimeSpan.Zero) OnDisplay?.Invoke(this, null);
            this.elapsedTime += gameTime.ElapsedGameTime;

            if (this.loadingScene != null && elapsedTime >= fadeInDuration)
                this.loadingSceneSwapDelay += gameTime.ElapsedGameTime;

            if (this.loadingSceneSwapDelay.TotalMilliseconds >= fadeInDuration.TotalMilliseconds)
            {
                TruckerX.Game.activeScene = loadingScene;
                loadingScene.Update(gameTime);
            }
        }

        public float GetRDMultiplier()
        {
            return (TruckerX.TargetRetangle.Width / 1280.0f);
        }

        public SpriteFont GetRDFont(string identifier)
        {
            float ratio = TruckerX.TargetRetangle.Width / 1280.0f;
            var current = GetFont(identifier);
            var lookingFor = current.LineSpacing * ratio;
            foreach(var font in Fonts)
            {
                if (font.Value.Asset.LineSpacing >= lookingFor) return font.Value.Asset;
            }    
            return null;
        }

        public Texture2D GetTexture(string identifier)
        {
            foreach (var item in Textures)
            {
                if (item.Key == identifier)
                {
                    return item.Value.Get();
                }
            }
            throw new Exception("Texture does not exist");
        }

        public SpriteFont GetFont(string identifier)
        {
            foreach (var item in Fonts)
            {
                if (item.Key == identifier)
                {
                    return item.Value.Get();
                }
            }
            throw new Exception("Font does not exist");
        }

        public Song GetSong(string identifier)
        {
            foreach (var item in Songs)
            {
                if (item.Key == identifier)
                {
                    return item.Value.Get();
                }
            }
            throw new Exception("Song does not exist");
        }

        public SoundEffect GetSample(string identifier)
        {
            foreach (var item in Samples)
            {
                if (item.Key == identifier)
                {
                    return item.Value.Get();
                }
            }
            throw new Exception("Song does not exist");
        }

        protected void SwitchSceneTo(BaseScene scene)
        {
            loadingScene = scene;
        }
    }
}
