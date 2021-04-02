using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.State;
using TruckerX.Trucks;

namespace TruckerX.Widgets
{
    public class TruckBannerWidget : BannerWidget
    {
        private Texture2D truckicon;
        private SpriteFont font;

        public BaseTruck truck { get; set; }

        public TruckBannerWidget(BaseTruck truck) : base()
        {
            font = ContentLoader.GetRDFont("main_font_15");
            this.truck = truck;
            truckicon = ContentLoader.GetTexture("truck-icon");
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            base.Draw(batch, gameTime);

            int portraitSize = (int)(this.Size.Y * 0.8f);
            int padding = (int)((this.Size.Y - portraitSize) / 2);
            {
                // Portrait
                MonoGame.Primitives2D.FillRectangle(batch, 
                    new Rectangle((int)this.Position.X + padding, (int)this.Position.Y + padding, portraitSize, portraitSize),
                    Color.FromNonPremultiplied(60, 60, 60, 255));

                batch.Draw(truckicon, new Rectangle((int)this.Position.X + padding+8, (int)this.Position.Y + padding+8, portraitSize-16, portraitSize-16), Color.White);
            }

            font = ContentLoader.GetRDFont("main_font_18");
            int nameHeight = 0;
            {
                // Name
                var str = truck.Name;
                var strSize = font.MeasureString(str);
                int offsetx = (int)this.Position.X + padding + portraitSize + padding;
                int offsety = (int)this.Position.Y + padding;
                nameHeight = (int)strSize.Y;
                batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(60, 60, 60, 255));
            }

            font = ContentLoader.GetRDFont("main_font_12");
            {
                // id
                var str = "Id: " + truck.Id;
                var strSize = font.MeasureString(str);
                int offsetx = (int)this.Position.X + padding + portraitSize + padding;
                int offsety = (int)this.Position.Y + padding + nameHeight;
                nameHeight += (int)strSize.Y;
                batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(80, 80, 80, 255));
            }

            {
                // Driving to
                string str = truck.Assignee == null ? "" : "Assigned to " + truck.Assignee.Id;
                int offsetx = (int)this.Position.X + padding + portraitSize + padding;
                int offsety = (int)this.Position.Y + padding + nameHeight;
                batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(80, 80, 80, 255));
            }
        }
    }
}
