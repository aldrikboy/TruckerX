using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TruckerX.Extensions;
using TruckerX.Particles;
using TruckerX.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using MonoGame;

namespace TruckerX.Scenes
{
    public abstract class DetailScene : BaseScene
    {
        public string Title { get; set; }
        public float Padding { get; set; } = 0.05f;

        public DetailScene(string title)
        {
            Title = title;
        }

        private void ContentLoader_OnLoaded(object sender, EventArgs e)
        {

        }

        public abstract void CustomDraw(SpriteBatch batch, GameTime gameTime);
        public abstract void CustomUpdate(GameTime gameTime);

        public void DrawSeparator(SpriteBatch batch)
        {
            var rec = TruckerX.TargetRetangle;
            int startLeft = rec.X + rec.Width / 2;
            var startTop = rec.Y + (Padding * 2 * rec.Height);
            var height = rec.Height - (Padding * 4 * rec.Height);
            Primitives2D.DrawLine(batch, startLeft, startTop, startLeft, startTop + height, Color.White, 3.0f * this.GetRDMultiplier());
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            var rec = TruckerX.TargetRetangle;
            {
                var bg = ContentLoader.GetTexture("world");
                batch.Draw(bg, rec, Color.White);
            }

            {
                var bg = ContentLoader.GetTexture("white");
                var containerOffset = Padding;
                var containerSize = 1.0f - containerOffset * 2;
                batch.Draw(bg, new Rectangle(
                     rec.X + (int)(containerOffset * rec.Width),
                     rec.Y + (int)(containerOffset * rec.Height),
                     (int)(containerSize * rec.Width),
                     (int)(containerSize * rec.Height)), Color.FromNonPremultiplied(0, 0, 0, 240));
            }

            {
                var font = this.GetRDFont("main_font_27");
                var startLeft = rec.X + (Padding * 2 * rec.Width);
                var startTop = rec.Y + (Padding * 2 * rec.Height);
                batch.DrawString(font, Title, new Vector2(startLeft + 2, startTop + 2), Color.Gray);
                batch.DrawString(font, Title, new Vector2(startLeft, startTop), Color.White);

                startTop += font.LineSpacing + (6 * this.GetRDMultiplier());
                Primitives2D.DrawLine(batch, startLeft, startTop, startLeft + (300 * this.GetRDMultiplier()), startTop, Color.FromNonPremultiplied(255, 255, 255, 255), 3.0f * this.GetRDMultiplier());
                startTop += 9 * this.GetRDMultiplier();
                Primitives2D.DrawLine(batch, startLeft, startTop, startLeft + (200 * this.GetRDMultiplier()), startTop, Color.FromNonPremultiplied(255, 255, 255, 200), 3.0f * this.GetRDMultiplier());
            }

            CustomDraw(batch, gameTime);

            base.Draw(batch, gameTime);
        }

        public override void Update( GameTime gameTime)
        {
            base.Update(gameTime);
            CustomUpdate(gameTime);
        }
    }
}
