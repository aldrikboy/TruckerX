using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.Trucks.Manufacturers;
using TruckerX.Widgets;

namespace TruckerX.Scenes
{
    public class TruckManufacturerSelectionScene : DetailScene
    {
        private List<ManufacturerWidget> manufacturers = new List<ManufacturerWidget>();
        private BasePlace place;

        public TruckManufacturerSelectionScene(BasePlace place) : base("Dealers")
        {
            this.place = place;
            manufacturers.Add(new ManufacturerWidget(new ManManufacturer(), "manufacturer-man-logo"));
            manufacturers.Add(new ManufacturerWidget(new ManManufacturer(), "manufacturer-mercedes-logo"));
        }

        public override void CustomDraw(SpriteBatch batch, GameTime gameTime)
        {
            foreach(var item in manufacturers)
            {
                item.Draw(batch, gameTime);
            }
        }

        public override void CustomUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.SwitchSceneTo(new PlaceDetailScene(place));
            }

            var rec = TruckerX.TargetRetangle;
            var startLeft = (Padding * 2 * rec.Width);
            var startTop = rec.Y + (Padding * 2 * rec.Height) + (int)(70.0f * GetRDMultiplier());

            float x = startLeft;
            foreach (var item in manufacturers)
            {
                item.Position = new Vector2(x, startTop);
                item.Update(this, gameTime);
                x += item.Size.X + 1;
            }
        }
    }
}
