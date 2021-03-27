using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Audio;

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
        public static Dictionary<string, AssetDefinition<Texture2D>> Textures { get; internal set; } = new Dictionary<string, AssetDefinition<Texture2D>>()
        {
            { "sloth", new AssetDefinition<Texture2D>("Textures/sloth-drawing") },
            { "trees", new AssetDefinition<Texture2D>("Textures/trees") },
            { "loading-background", new AssetDefinition<Texture2D>("Textures/loading-background") },
            { "loading-bg", new AssetDefinition<Texture2D>("Textures/loading-bg") },
            { "star", new AssetDefinition<Texture2D>("Textures/star") },
            { "leaf", new AssetDefinition<Texture2D>("Textures/leaf") },
            { "black", new AssetDefinition<Texture2D>("Textures/black") },
            { "menu-button", new AssetDefinition<Texture2D>("Textures/menu-button") },
            { "rope", new AssetDefinition<Texture2D>("Textures/rope") },
            { "white", new AssetDefinition<Texture2D>("Textures/white") },
            { "world", new AssetDefinition<Texture2D>("Textures/world") },
            { "sign", new AssetDefinition<Texture2D>("Textures/sign") },
            { "list", new AssetDefinition<Texture2D>("Textures/list") },
            { "overlay-background", new AssetDefinition<Texture2D>("Textures/overlay-background") },
            { "overlay-button", new AssetDefinition<Texture2D>("Textures/overlay-button") },
            { "tab-background", new AssetDefinition<Texture2D>("Textures/tab-background") },
            { "padlock", new AssetDefinition<Texture2D>("Textures/padlock") },
            { "detail-button", new AssetDefinition<Texture2D>("Textures/detailbutton") },
            { "detail-view", new AssetDefinition<Texture2D>("Textures/detailview") },
            { "portrait", new AssetDefinition<Texture2D>("Textures/portrait") },
            { "logo-placeholder", new AssetDefinition<Texture2D>("Textures/logo-placeholder") },
            { "search", new AssetDefinition<Texture2D>("Textures/search") },
            { "error", new AssetDefinition<Texture2D>("Textures/error") },
            { "edit", new AssetDefinition<Texture2D>("Textures/edit") },
            { "popup-background", new AssetDefinition<Texture2D>("Textures/popup-background") },
        };

        public static Dictionary<string, AssetDefinition<Song>> Songs { get; internal set; } = new Dictionary<string, AssetDefinition<Song>>()
        {
            { "background-song", new AssetDefinition<Song>("Sounds/background-ambience") },
        };

        public static Dictionary<string, AssetDefinition<SoundEffect>> Samples { get; internal set; } = new Dictionary<string, AssetDefinition<SoundEffect>>()
        {
            { "pop1", new AssetDefinition<SoundEffect>("Sounds/pop1") },
            { "pop2", new AssetDefinition<SoundEffect>("Sounds/pop2") },
            { "rope-pull", new AssetDefinition<SoundEffect>("Sounds/rope-pull") },
        };

        public static Dictionary<string, AssetDefinition<SpriteFont>> Fonts = new Dictionary<string, AssetDefinition<SpriteFont>> {
            { "main_font_3", new AssetDefinition<SpriteFont>("Fonts/main_font_3") },
            { "main_font_6", new AssetDefinition<SpriteFont>("Fonts/main_font_6") },
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

    public static class ContentLoader
    {
        public static bool Done { get; set; } = false;

        public static void LoadContent(ContentManager content)
        {
            Thread thread = new Thread(() =>
            {
                foreach (var item in ContentDefinition.Textures)
                {
                    item.Value.Load(content);
                }
                foreach (var item in ContentDefinition.Fonts)
                {
                    item.Value.Load(content);
                }
                foreach (var item in ContentDefinition.Songs)
                {
                    item.Value.Load(content);
                }
                foreach (var item in ContentDefinition.Samples)
                {
                    item.Value.Load(content);
                }
                Done = true;
            });
            thread.Start();
        }

        public static Texture2D GetTexture(string identifier)
        {
            foreach (var item in ContentDefinition.Textures)
            {
                if (item.Key == identifier)
                {
                    return item.Value.Get();
                }
            }
            throw new Exception("Texture does not exist");
        }

        public static SpriteFont GetFont(string identifier)
        {
            foreach (var item in ContentDefinition.Fonts)
            {
                if (item.Key == identifier)
                {
                    return item.Value.Get();
                }
            }
            throw new Exception("Font does not exist");
        }

        public static Song GetSong(string identifier)
        {
            foreach (var item in ContentDefinition.Songs)
            {
                if (item.Key == identifier)
                {
                    return item.Value.Get();
                }
            }
            throw new Exception("Song does not exist");
        }

        public static SoundEffect GetSample(string identifier)
        {
            foreach (var item in ContentDefinition.Samples)
            {
                if (item.Key == identifier)
                {
                    return item.Value.Get();
                }
            }
            throw new Exception("Song does not exist");
        }

        public static float GetRDMultiplier()
        {
            return (TruckerX.TargetRetangle.Width / 1280.0f);
        }

        public static SpriteFont GetRDFont(string identifier)
        {
            float ratio = TruckerX.TargetRetangle.Width / 1280.0f;
            var current = ContentLoader.GetFont(identifier);
            var lookingFor = current.LineSpacing * ratio;
            foreach (var font in ContentDefinition.Fonts)
            {
                if (font.Value.Asset.LineSpacing >= lookingFor) return font.Value.Asset;
            }
            return null;
        }
    }
}
