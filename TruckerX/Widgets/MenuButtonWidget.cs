using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Widgets
{
    public class MenuButtonWidget : BaseWidget
    {
        private Texture2D bg;
        private string text;
        private SpriteFont font;

        public MenuButtonWidget(BaseScene scene, string text, Vector2 position, Vector2 size) : base()
        {
            bg = ContentLoader.GetTexture("menu-button");
            this.text = text;
            font = ContentLoader.GetFont("main_font_24");
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (this.State == WidgetState.MouseHover)
            {
                batch.Draw(bg, new Rectangle((this.Position - new Vector2(0, 2)).ToPoint(), (this.Size).ToPoint()), Color.White);
            }
            else if (this.State == WidgetState.MouseDown)
            {
                batch.Draw(bg, new Rectangle((this.Position + new Vector2(0, 2)).ToPoint(), (this.Size).ToPoint()), Color.White);
            }
            else
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.White);
            }

       
            var strSize = font.MeasureString(text);
            float angle = (float)Math.PI * 0.0f / 180.0f;
            int offsetx = (int)this.Position.X - (int)strSize.X / 2 + (int)this.Size.X / 2;
            int offsety = (int)this.Position.Y - (int)strSize.Y / 2 + (int)this.Size.Y / 2;
            if (this.State == WidgetState.MouseHover) offsety -= 2;
            if (this.State == WidgetState.MouseDown) offsety += 2;

            batch.DrawString(font, text, new Vector2(offsetx, offsety+2), Color.Black, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
            batch.DrawString(font, text, new Vector2(offsetx, offsety), Color.White, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
