using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TruckerX
{
    public class AssetDefinition<T>
    {
        public string ContentName { get; set; }
        public T Asset { get; set; }

        public void Load(ContentManager content)
        {
            this.Asset = content.Load<T>(this.ContentName);
        }

        public T Get()
        {
            return this.Asset;
        }

        public AssetDefinition(string name)
        {
            this.ContentName = name;
        }
    }

    public static class ContentDefinition
    {
        public static Dictionary<string, AssetDefinition<SpriteFont>> AllFonts = new Dictionary<string, AssetDefinition<SpriteFont>> {
            { "main_font_9", new AssetDefinition<SpriteFont>("Fonts/main_font_9") },
            { "main_font_12", new AssetDefinition<SpriteFont>("Fonts/main_font_12") },
            { "main_font_15", new AssetDefinition<SpriteFont>("Fonts/main_font_15") },
            { "main_font_18", new AssetDefinition<SpriteFont>("Fonts/main_font_18") },
            { "main_font_21", new AssetDefinition<SpriteFont>("Fonts/main_font_21") },
            { "main_font_24", new AssetDefinition<SpriteFont>("Fonts/main_font_24") },
            { "main_font_27", new AssetDefinition<SpriteFont>("Fonts/main_font_27") },
            { "main_font_30", new AssetDefinition<SpriteFont>("Fonts/main_font_30") },
            { "main_font_33", new AssetDefinition<SpriteFont>("Fonts/main_font_33") },
            { "main_font_36", new AssetDefinition<SpriteFont>("Fonts/main_font_36") },
            { "main_font_39", new AssetDefinition<SpriteFont>("Fonts/main_font_39") },
            { "main_font_42", new AssetDefinition<SpriteFont>("Fonts/main_font_42") },
            { "main_font_45", new AssetDefinition<SpriteFont>("Fonts/main_font_45") },
            { "main_font_48", new AssetDefinition<SpriteFont>("Fonts/main_font_48") },
        };
    }

    public class ContentLoader
    {
        public bool Done { get; set; } = false;

        public event EventHandler OnLoaded;

        public void LoadContent(ContentManager content, BaseScene scene)
        {
            Thread thread = new Thread(() => {
                foreach (var item in scene.Textures)
                {
                    item.Value.Load(content);
                }
                foreach (var item in scene.Fonts)
                {
                    item.Value.Load(content);
                }
                foreach (var item in scene.Songs)
                {
                    item.Value.Load(content);
                }
                foreach (var item in scene.Samples)
                {
                    item.Value.Load(content);
                }
                OnLoaded?.Invoke(this, null);
                Done = true;
            });
            thread.Start();
        }
    }
}
