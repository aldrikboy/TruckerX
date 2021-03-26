using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.State;
using Microsoft.Xna.Framework.Input;

namespace TruckerX.Widgets
{
    public class OverlayButtonWidget : BaseWidget
    {
        private Texture2D icon;
        private Texture2D bg;
        private BaseScene scene;
        private Vector2 iconSize;

        public string Text { get; set; }

        public OverlayButtonWidget(BaseScene scene, string iconStr) : base()
        {
            bg = ContentLoader.GetTexture("overlay-button");
            icon = ContentLoader.GetTexture(iconStr);
            this.scene = scene;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.White);
            batch.Draw(icon, new Rectangle((this.Position + (this.Size/2)-(iconSize/2)).ToPoint(), this.iconSize.ToPoint()), Color.White);
            if (this.State == WidgetState.MouseHover)
            {
                Helper.CursorToSet = MouseCursor.Hand;
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(0, 0, 0, 50));
            }
            else if (this.State == WidgetState.MouseDown)
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(0, 0, 0, 150));
            }
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            Size = new Vector2(30, 30) * scene.GetRDMultiplier();
            iconSize = Size / 2;
            base.Update(scene, gameTime);
        }
    }
}
