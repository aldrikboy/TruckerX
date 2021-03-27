using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Scenes;

namespace TruckerX.Widgets
{
    public class CheckmarkWidget : BaseWidget
    {
        private string text;
        private bool checkedValue = false;

        private Texture2D padlock;
        private Texture2D checkmark;

        public event EventHandler OnCheckChanged;

        public CheckmarkWidget(string text, bool checkedValue = false)
        {
            this.text = text;
            this.checkedValue = checkedValue;
            padlock = ContentLoader.GetTexture("padlock");
            checkmark = ContentLoader.GetTexture("checkmark");

            this.OnClick += CheckmarkWidget_OnClick;
        }

        public void SetValue(bool value)
        {
            this.checkedValue = value;
        }

        private void CheckmarkWidget_OnClick(object sender, EventArgs e)
        {
            checkedValue = !checkedValue;
            OnCheckChanged?.Invoke(checkedValue, null);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            Primitives2D.FillRectangle(batch, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(230, 230, 230, 255));
            Primitives2D.DrawRectangle(batch, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(60, 60, 60, 255), 2.0f);

            float pad = (7 * ContentLoader.GetRDMultiplier());
            float checkboxSize = this.Size.Y - pad*2;
            var checkboxRec = new Rectangle((int)(this.Position.X + pad), (int)(this.Position.Y + pad), (int)checkboxSize, (int)checkboxSize);
            Primitives2D.FillRectangle(batch, checkboxRec, Color.FromNonPremultiplied(230, 230, 230, 255));
            Primitives2D.DrawRectangle(batch, checkboxRec, Color.FromNonPremultiplied(60, 60, 60, 255), 2.0f);

            var font = ContentLoader.GetFont("main_font_15");
            var str = text;
            var strSize = font.MeasureString(str);
            float angle = (float)Math.PI * 0.0f / 180.0f;
            int offsetx = (int)(this.Position.X + checkboxSize + pad*2);
            int offsety = (int)this.Position.Y - (int)strSize.Y / 2 + (int)this.Size.Y / 2;
            batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(20, 20, 20, 255), angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);

            Texture2D iconToDraw = null;
            if (this.State == WidgetState.Disabled) iconToDraw = padlock;
            else if (this.checkedValue) iconToDraw = checkmark;
            if (iconToDraw != null) batch.Draw(iconToDraw, new Rectangle(checkboxRec.X+3, checkboxRec.Y+3, checkboxRec.Width-5, checkboxRec.Height-5), Color.FromNonPremultiplied(20, 20, 20, 255));
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            if (this.State == WidgetState.MouseHover) Helper.CursorToSet = MouseCursor.Hand;
            base.Update(scene, gameTime);
        }
    }
}
