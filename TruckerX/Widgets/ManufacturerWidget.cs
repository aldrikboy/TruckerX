using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Extensions;
using TruckerX.Scenes;
using TruckerX.Trucks;

namespace TruckerX.Widgets
{
    public class ManufacturerWidget : BaseWidget
    {
        public BaseTruckManufacturer Manufacturer { get; }
        Texture2D icon;

        public ManufacturerWidget(BaseTruckManufacturer manufacturer, string iconName)
        {
            this.Manufacturer = manufacturer;
            this.icon = ContentLoader.GetTexture(iconName);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            var iconSize = icon.ScaleToHeight(this.Size.Y, 0.8f);
            float offsetx = (this.Size.X - iconSize.X) / 2;
            float offsety = (this.Size.Y - iconSize.Y) / 2;

            Color c = Color.FromNonPremultiplied(255, 255, 255, 255);
            if (this.State == WidgetState.MouseHover) c = Color.FromNonPremultiplied(210, 210, 210, 255);
            if (this.State == WidgetState.MouseDown) c = Color.FromNonPremultiplied(190, 190, 190, 255);
            Primitives2D.FillRectangle(batch, new Rectangle((this.Position).ToPoint(), (this.Size).ToPoint()), c);

            batch.Draw(icon, new Rectangle((int)(this.Position.X + offsetx), (int)(this.Position.Y + offsety), (int)(iconSize.X), (int)(iconSize.Y)), Color.White);
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            float targetHeight = 120;
            var size = icon.ScaleToHeight(targetHeight, 1.0f);
            this.Size = new Vector2(size.X, size.Y) * ContentLoader.GetRDMultiplier();
            base.Update(scene, gameTime);
        }
    }
}
