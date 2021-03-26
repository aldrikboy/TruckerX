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
        OverlayButtonWidget buttonLog;
        int topRowRefHeight = 50;

        public SimulationOverlayScene()
        {

        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (!ContentLoader.Done) return;
            if (background == null) background = ContentLoader.GetTexture("overlay-background");

            var rec = TruckerX.TargetRetangle;
            Vector2 size = new Vector2(293, 250) * GetRDMultiplier();
            Vector2 pos = new Vector2(rec.X + rec.Width - size.X, rec.Y);
            batch.Draw(background, new Rectangle((int)pos.X+2, (int)pos.Y, (int)(size.X), (int)(size.Y)), Color.White);

            float textY = (topRowRefHeight * GetRDMultiplier()) / 2;
            SpriteFont font = this.GetRDFont("main_font_12");
            string dateFormatted = Currency.USD.Sign + string.Format("{0:0.00}", Simulation.simulation.Money) + " - " + Simulation.simulation.Time.ToString("ddd dd/MM HH:mm");
            var stringSize = font.MeasureString(dateFormatted);
            batch.DrawString(font, dateFormatted, new Vector2(rec.X + rec.Width - stringSize.X - (10*GetRDMultiplier()), rec.Y + textY - (stringSize.Y/2)), Color.White);

            buttonLog.Draw(batch, gameTime);

            base.Draw(batch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!ContentLoader.Done) return;
            base.Update(gameTime);
            var rec = TruckerX.TargetRetangle;

            if (buttonLog == null) buttonLog = new OverlayButtonWidget(this, "list");
            buttonLog.Update(this, gameTime);
            buttonLog.Position = new Vector2(rec.X + rec.Width - buttonLog.Size.X - (6 * GetRDMultiplier()), rec.Y + (topRowRefHeight * GetRDMultiplier()));
        }
    }
}
