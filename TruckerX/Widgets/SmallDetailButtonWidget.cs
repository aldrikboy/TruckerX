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
    public class SmallDetailButtonWidget : BaseWidget
    {
        private Texture2D bg;
        private Texture2D padlock;
        private SpriteFont font;
        private BaseScene scene;
        private bool flipped = false;

        public string Text { get; set; }

        public SmallDetailButtonWidget(BaseScene scene, bool flipped = false) : base()
        {
            bg = ContentLoader.GetTexture("detail-button");
            padlock = ContentLoader.GetTexture("padlock");
            font = scene.GetRDFont("main_font_15");
            this.scene = scene;
            this.flipped = flipped;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            font = scene.GetRDFont("main_font_12");
            batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), null, Color.White, 0.0f, Vector2.Zero, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
            Color textColor = Color.FromNonPremultiplied(60, 60, 60, 255);
            if (this.State == WidgetState.MouseHover)
            {
                Mouse.SetCursor(MouseCursor.Hand);
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), null, Color.FromNonPremultiplied(0, 0, 0, 50), 0.0f, Vector2.Zero, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
            }
            else if (this.State == WidgetState.MouseDown)
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), null, Color.FromNonPremultiplied(0, 0, 0, 150), 0.0f, Vector2.Zero, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
            }
            else if (this.State == WidgetState.Disabled)
            {
                textColor = Color.FromNonPremultiplied(255,255,255,150);
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), null, Color.FromNonPremultiplied(0, 0, 0, 230), 0.0f, Vector2.Zero, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
                float lockW = padlock.Width / 12 * scene.GetRDMultiplier();
                float lockH = padlock.Height / 12 * scene.GetRDMultiplier();
                batch.Draw(padlock, new Rectangle((int)(this.Position.X + (this.Size.X / 2) - (lockW / 2)), (int)(this.Position.Y + (this.Size.Y / 2) - (lockH / 2)), (int)lockW, (int)lockH), Color.White);
            }

            var str = Text;
            var strSize = font.MeasureString(str);
            float angle = (float)Math.PI * 0.0f / 180.0f;
            int offsetx = (int)this.Position.X - (int)strSize.X / 2 + (int)this.Size.X / 2;
            int offsety = (int)this.Position.Y - (int)strSize.Y / 2 + (int)this.Size.Y / 2;
            batch.DrawString(font, str, new Vector2(offsetx, offsety), textColor, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            Size = new Vector2(150, 35) * scene.GetRDMultiplier();
            base.Update(scene, gameTime);
        }
    }
}
