using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Widgets
{
    public class CampaignLevelButtonWidget : BaseWidget
    {
        private Texture2D bg;
        private string text;
        private SpriteFont font;
        private bool enabled;

        public CampaignLevelButtonWidget(BaseScene scene, string text, Vector2 position, Vector2 size, bool enabled) : base(position, size)
        {
            bg = scene.GetTexture("menu-button");
            this.text = text;
            font = scene.GetFont("main_font_18");
            this.enabled = enabled;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.White);

            var strSize = font.MeasureString(text);
            float angle = (float)Math.PI * 0.0f / 180.0f;
            int offsetx = (int)this.Position.X + 20;
            int offsety = (int)this.Position.Y - (int)strSize.Y / 2 + (int)this.Size.Y / 2;

            Color c = Color.White;
            if (this.State == WidgetState.MouseHover) offsetx += 5;
            batch.DrawString(font, text, new Vector2(offsetx, offsety + 2), Color.Black, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
            batch.DrawString(font, text, new Vector2(offsetx, offsety), c, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
