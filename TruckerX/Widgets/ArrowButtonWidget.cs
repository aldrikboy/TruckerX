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
    public enum PointingTo
    {
        Left,
        Right,
    }

    public class ArrowButtonWidget : BaseWidget
    {
        private PointingTo direction;
        private Texture2D arrowTexture;

        public ArrowButtonWidget(PointingTo direction)
        {
            this.direction = direction;
            arrowTexture = ContentLoader.GetTexture("arrow-left");
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            const int pad = 2;
            const int pad2 = pad * 2;
            Primitives2D.FillRectangle(batch, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(230, 230, 230, 255));
            Color c = Color.FromNonPremultiplied(255, 255, 255, 255);
            if (this.State == WidgetState.MouseHover) c = Color.FromNonPremultiplied(210, 210, 210, 255);
            if (this.State == WidgetState.MouseDown) c = Color.FromNonPremultiplied(190, 190, 190, 255);
            Primitives2D.FillRectangle(batch, new Rectangle((this.Position + new Vector2(pad, pad)).ToPoint(), (this.Size - new Vector2(pad2, pad2)).ToPoint()), c);

            batch.Draw(arrowTexture, new Rectangle((int)this.Position.X, (int)(this.Position.Y+((this.Size.Y / 4))), (int)this.Size.X, (int)(this.Size.Y/2)), 
                null, Color.FromNonPremultiplied(60,60,60,255), 0.0f, Vector2.Zero, direction == PointingTo.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            if (this.State == WidgetState.MouseHover) Helper.CursorToSet = MouseCursor.Hand;
            this.Size = new Vector2(40, 88) * scene.GetRDMultiplier();
            base.Update(scene, gameTime);
        }
    }
}
