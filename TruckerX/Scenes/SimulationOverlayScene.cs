using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Extensions;
using TruckerX.State;
using TruckerX.Widgets;

namespace TruckerX.Scenes
{
    public class SimulationOverlayScene : BaseScene
    {
        public SimulationOverlayScene()
        {
 
        }

        public override void DeclareAssets()
        {
            Textures.AddRange(new Dictionary<string, AssetDefinition<Texture2D>>()
            {
                // Images
                { "tab-background", new AssetDefinition<Texture2D>("Textures/tab-background") },
                { "padlock", new AssetDefinition<Texture2D>("Textures/padlock") },
                { "detail-button", new AssetDefinition<Texture2D>("Textures/detailbutton") },
            });
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (!DoneLoading) return;
            SpriteFont font = this.GetFont("main_font_24");
            var rec = TruckerX.TargetRetangle;
            batch.DrawString(font, Simulation.simulation.Time.ToString("ddd HH:mm"), new Vector2(rec.X, rec.Y), Color.Red);

            base.Draw(batch, gameTime);
        }
    }
}
