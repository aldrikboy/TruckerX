using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.State;

namespace TruckerX.Scenes
{
    public class OfferDetailScene : DetailScene
    {
        JobOffer offer;
        double distance = 0;
        double travelHours = 0;

        public OfferDetailScene(JobOffer offer) : base("Company Name")
        {
            this.offer = offer;
            this.ContentLoader.OnLoaded += ContentLoader_OnLoaded;
            distance = this.offer.GetDistanceInKm();
            travelHours = this.offer.GetTravelTime(distance);
        }

        private void ContentLoader_OnLoaded(object sender, EventArgs e)
        {
            
        }

        private int AddLine(SpriteBatch batch, string text, SpriteFont font, float x, float y)
        {
            var strSize = font.MeasureString(text);
            batch.DrawString(font, text, new Vector2(x + 2, y + 2), Color.Gray);
            batch.DrawString(font, text, new Vector2(x, y), Color.White);
            return (int)strSize.Y;
        }

        public override void CustomDraw(SpriteBatch batch, GameTime gameTime)
        {
            var rec = TruckerX.TargetRetangle;
            var font = this.GetRDFont("main_font_18");
            var startLeft = rec.X + (Padding * 2 * rec.Width);
            var startTop = rec.Y + (Padding * 2 * rec.Height);
            int textY = (int)(70.0f * GetRDMultiplier());


            textY += AddLine(batch, "From: " + offer.From.Name, font, startLeft, startTop + textY);
            textY += AddLine(batch, "To: " + offer.To.Name, font, startLeft, startTop + textY);

            int hrs = (int)Math.Floor(travelHours);
            double min = travelHours - hrs;
            min = 60 * min;
            min = Math.Round(min);
            textY += AddLine(batch, "Distance: " + distance + "KM / " + hrs + "H" + min + "M", font, startLeft, startTop + textY);

        }

        public override void CustomUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.SwitchSceneTo(new PlaceDetailScene(offer.From));
            }
        }
    }
}
