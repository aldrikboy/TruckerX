using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TruckerX.Extensions;
using TruckerX.Particles;
using TruckerX.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using MonoGame;

namespace TruckerX.Scenes
{
    public class WorldMapScene : BaseScene
    {
        public List<WorldLocationWidget> Locations { get; } = new List<WorldLocationWidget>();
        int placeDotSize = 10;

        public WorldMapScene()
        {
            this.ContentLoader.OnLoaded += ContentLoader_OnLoaded;
        }

        private void ContentLoader_OnLoaded(object sender, EventArgs e)
        {
            foreach(var country in WorldData.Countries)
            {
                foreach (var place in country.Places)
                {
                    var btn = new WorldLocationWidget(this, place);
                    btn.OnClick += Btn_OnClick;
                    Locations.Add(btn);
                }
            }
        }

        private void Btn_OnClick(object sender, EventArgs e)
        {
            var btn = sender as WorldLocationWidget;
            this.SwitchSceneTo(new PlaceDetailScene(btn.Place));
        }

        public override void DeclareAssets()
        {
            Textures = new Dictionary<string, AssetDefinition<Texture2D>>()
            {
                // Images
                { "trees", new AssetDefinition<Texture2D>("Textures/trees") },
                { "world", new AssetDefinition<Texture2D>("Textures/world") },
                { "menu-button", new AssetDefinition<Texture2D>("Textures/menu-button") },
                { "leaf", new AssetDefinition<Texture2D>("Textures/leaf") },
                { "white", new AssetDefinition<Texture2D>("Textures/white") },
                { "sign", new AssetDefinition<Texture2D>("Textures/sign") },
            };

            Songs = new Dictionary<string, AssetDefinition<Song>>()
            {
                // Songs
            };

            Samples = new Dictionary<string, AssetDefinition<SoundEffect>>()
            {
                // Songs
                { "pop2", new AssetDefinition<SoundEffect>("Sounds/pop2") },
            };
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            {
                var bg = this.GetTexture("world");
                batch.Draw(bg, TruckerX.TargetRetangle, Color.White);
            }

            int dotCenter = placeDotSize / 2;
            foreach (var item in Locations)
            {
                foreach (var connection in item.Place.Connections)
                {
                    var rec = TruckerX.TargetRetangle;
                    Vector2 start = new Vector2(rec.X + (int)(item.Place.MapX * rec.Width) + dotCenter, rec.Y + (int)(item.Place.MapY * rec.Height) + dotCenter);
                    Vector2 end = new Vector2(rec.X + (int)(connection.MapX * rec.Width) + dotCenter, rec.Y + (int)(connection.MapY * rec.Height) + dotCenter);
                    Primitives2D.DrawLine(batch, start, end, Color.Red);
                }
            }

            foreach (var item in Locations)
            {
                item.Draw(batch, gameTime);
            }

            base.Draw(batch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            var rec = TruckerX.TargetRetangle;
            placeDotSize = (int)(10.0f / (1280.0f / rec.Width));
            foreach (var item in Locations)
            {
                item.Position = new Vector2(rec.X + (int)(item.Place.MapX * rec.Width), rec.Y + (int)(item.Place.MapY * rec.Height));
                int size = (int)(10.0f / (1280.0f / rec.Width));
                item.Size = new Vector2(placeDotSize, placeDotSize);
                item.Update(gameTime);
            }

            base.Update(gameTime);
        }
    }
}
