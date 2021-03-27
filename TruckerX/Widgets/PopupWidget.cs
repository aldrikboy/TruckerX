using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Scenes;

namespace TruckerX.Widgets
{
    public class PopupWidget : BaseWidget
    {
        Texture2D bg;
        Vector2 popupSize;

        public bool Visible { get; set; } = false;
        private string[] textLines = { };

        SmallDetailButtonWidget buttonAccept;
        SmallDetailButtonWidget buttonCancel;

        public event EventHandler OnAccept;
        public event EventHandler OnDecline;

        private object data;

        public PopupWidget() : base()
        {
            bg = ContentLoader.GetTexture("popup-background");
            buttonAccept = new SmallDetailButtonWidget();
            buttonAccept.Text = "Purchase";
            buttonAccept.OnClick += ButtonAccept_OnClick;

            buttonCancel = new SmallDetailButtonWidget();
            buttonCancel.Text = "Cancel";
            buttonCancel.OnClick += ButtonCancel_OnClick;
        }

        private void ButtonAccept_OnClick(object sender, EventArgs e)
        {
            OnAccept?.Invoke(data, null);
        }

        private void ButtonCancel_OnClick(object sender, EventArgs e)
        {
            OnDecline?.Invoke(data, null);
        }

        public void Show(string text, object data)
        {
            this.data = data;
            this.Visible = true;
            this.textLines = text.Split("\n");
        }

        public void Hide()
        {
            this.Visible = false;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (!Visible) return;
            var rec = TruckerX.TargetRetangle;
            this.Position = new Vector2(rec.X, rec.Y);
            this.Size = new Vector2(rec.Width, rec.Height);
            popupSize = new Vector2(533, 300) * ContentLoader.GetRDMultiplier();

            int x = (int)(this.Position.X + (this.Size.X / 2) - (popupSize.X / 2));
            int y = (int)(this.Position.Y + (this.Size.Y / 2) - (popupSize.Y / 2));
            int w = (int)popupSize.X;
            int h = (int)popupSize.Y;
            float pad = (20 * ContentLoader.GetRDMultiplier());

            batch.Draw(bg, new Rectangle(x, y, w, h), Color.White);

            float textY = y + pad;
            foreach (var text in textLines)
            {
                var font = ContentLoader.GetRDFont("main_font_21");
                var size = font.MeasureString(text);
                float angle = (float)Math.PI * 0.0f / 180.0f;
                float offsetx = x + pad;
                float offsety = textY;
                batch.DrawString(font, text, new Vector2(offsetx, offsety), Color.White, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
                textY += size.Y;
            }

            buttonAccept.Position = new Vector2(x + (3 * ContentLoader.GetRDMultiplier()), y + h - ((buttonAccept.Size.Y + (pad/1.5f))*2));
            buttonAccept.Draw(batch, gameTime);

            buttonCancel.Position = new Vector2(x + (3 * ContentLoader.GetRDMultiplier()), y + h - buttonAccept.Size.Y - pad);
            buttonCancel.Draw(batch, gameTime);
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            if (!Visible) return;
            buttonAccept.Update(scene, gameTime);
            buttonCancel.Update(scene, gameTime);
            base.Update(scene, gameTime);
        }
    }
}
