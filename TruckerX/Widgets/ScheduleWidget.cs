using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.State;
using MonoGame;
using Microsoft.Xna.Framework.Input;

namespace TruckerX.Widgets
{
    public class ScheduleWidget : BaseWidget
    {
        BaseScene scene;
        PlaceSchedule schedule;

        Vector2 hoveringTile = new Vector2(-1,-1);
        float cutoffDaysColumn;
        float rowHeight;
        int columns;
        float columnWidth;

        public ScheduleWidget(BaseScene scene, PlaceSchedule schedule) : base(Vector2.Zero, Vector2.Zero)
        {
            this.scene = scene;
            this.schedule = schedule;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            var bg = scene.GetTexture("white");
            batch.Draw(bg, new Rectangle((this.Position).ToPoint(), (this.Size).ToPoint()), Color.White);

            var color = Color.FromNonPremultiplied(60,60,60,155);

            Primitives2D.DrawLine(batch, this.Position.X + cutoffDaysColumn, 
                this.Position.Y, this.Position.X + cutoffDaysColumn, this.Position.Y + this.Size.Y, color);

            var font = scene.GetRDFont("main_font_15");

            // Top row
            {
                float x = this.Position.X;
                float y = this.Position.Y;
                Primitives2D.FillRectangle(batch,
                    x-1, y-1, this.Size.X+1, rowHeight+2, color, 0.0f);
            }

            // Hours
            for (int i = 0; i < columns; i++)
            {
                float x = this.Position.X + cutoffDaysColumn + (i * columnWidth);
                string text = String.Format("{0}:00", schedule.StartHour + i);
                var size = font.MeasureString(text);
                batch.DrawString(font, text,
                   new Vector2(x + (columnWidth / 2) - (size.X / 2),
                   this.Position.Y  + (rowHeight / 2) - (size.Y / 2)), Color.Black);

                Primitives2D.DrawLine(batch, x, this.Position.Y, x, this.Position.Y + this.Size.Y, color);
            }

            // Days
            foreach(var day in Enum.GetValues(typeof(Weekday)))
            {
                string val = day.ToString().Substring(0, 2);
                var size = font.MeasureString(val);
                float y = this.Position.Y + ((int)day+1) * rowHeight;
                batch.DrawString(font, val, 
                    new Vector2(this.Position.X + (cutoffDaysColumn/2)-(size.X/2),
                    y + (rowHeight/2) - (size.Y/2)), Color.Black);

                Primitives2D.DrawLine(batch, this.Position.X, y, this.Position.X + this.Size.X, y, color);
            }

            if (hoveringTile.X >= 0 && hoveringTile.X < columns && hoveringTile.Y >= 0 && hoveringTile.Y < 7)
            {
                float x = this.Position.X + cutoffDaysColumn + (hoveringTile.X * columnWidth);
                float y = this.Position.Y + (hoveringTile.Y + 1) * rowHeight;
                Primitives2D.FillRectangle(batch,
                    x, y, columnWidth, rowHeight, color, 0.0f);
            }

            /*
            var strSize = font.MeasureString(text);
            float angle = (float)Math.PI * 0.0f / 180.0f;
            int offsetx = (int)this.Position.X - (int)strSize.X / 2 + (int)this.Size.X / 2;
            int offsety = (int)this.Position.Y - (int)strSize.Y / 2 + (int)this.Size.Y / 2;
            if (this.State == WidgetState.MouseHover) offsety -= 2;
            if (this.State == WidgetState.MouseDown) offsety += 2;

            batch.DrawString(font, text, new Vector2(offsetx, offsety + 2), Color.Black, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
            batch.DrawString(font, text, new Vector2(offsetx, offsety), Color.White, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
            */
        }

        public override void Update(GameTime gameTime)
        {
            cutoffDaysColumn = 60 * scene.GetRDMultiplier();
            rowHeight = this.Size.Y / 8;
            columns = schedule.EndHour - schedule.StartHour + 1;
            columnWidth = (this.Size.X - cutoffDaysColumn) / columns;

            var pos = Mouse.GetState();
            hoveringTile = new Vector2((int)(pos.X - this.Position.X - cutoffDaysColumn) / (int)columnWidth, (int)(pos.Y - this.Position.Y - rowHeight) / (int)rowHeight);

            base.Update(gameTime);
        }
    }
}
