using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;
using TruckerX.Extensions;
using TruckerX.State;
using TruckerX.Widgets;

namespace TruckerX.Scenes
{
    public class SimulationOverlayScene : BaseScene
    {
        Texture2D background = null;

        public SimulationOverlayScene()
        {
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (!ContentLoader.Done) return;
            if (background == null) background = ContentLoader.GetTexture("overlay-background");

            var rec = TruckerX.TargetRetangle;
            Vector2 size = new Vector2(293, 50) * GetRDMultiplier();
            Vector2 pos = new Vector2(rec.X + rec.Width - size.X, rec.Y);
            batch.Draw(background, new Rectangle((int)pos.X+2, (int)pos.Y, (int)(size.X), (int)(size.Y)), Color.White);

            SpriteFont font = this.GetRDFont("main_font_12");
            string dateFormatted = Currency.USD.Sign + string.Format("{0:0.00}", Simulation.simulation.Money) + " - " + Simulation.simulation.Time.ToString("ddd HH:mm");
            var stringSize = font.MeasureString(dateFormatted);
            batch.DrawString(font, dateFormatted, new Vector2(rec.X + rec.Width - stringSize.X - (10*GetRDMultiplier()), rec.Y + (size.Y/2) - (stringSize.Y/2)), Color.White);

            // 
            base.Draw(batch, gameTime);
        }
    }
}
