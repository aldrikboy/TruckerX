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
using TruckerX.Extensions;

namespace TruckerX.Widgets
{
    public class ScheduleWidget : BaseWidget
    {
        BaseScene scene;
        PlaceSchedule schedule;
        JobOffer offerToPlan;

        ScheduledJob selectedScheduledJob = null;
        Vector2 hoveringTile = new Vector2(-1,-1);
        float cutoffDaysColumn;
        float rowHeight;
        int columns;
        float columnWidth;
        bool enabled;
        Texture2D bg;
        Texture2D padlock;
        float quarterHour;

        public ScheduledJob newScheduledJob;

        public event EventHandler OnScheduledOfferSelected;

        public ScheduleWidget(BaseScene scene, PlaceSchedule schedule, JobOffer offerToPlan, bool enabled = true) : base(Vector2.Zero, Vector2.Zero)
        {
            this.scene = scene;
            this.schedule = schedule;
            this.offerToPlan = offerToPlan;
            this.enabled = enabled;
            bg = scene.GetTexture("white");
            padlock = scene.GetTexture("padlock");
            newScheduledJob = new ScheduledJob(offerToPlan);
        }

        public void ClearSchedulingJob()
        {
            newScheduledJob.ShipTime.Clear();
            selectedScheduledJob = null;
        }

        public bool DoneSchedulingJob()
        {
            return newScheduledJob.ShipTime.Keys.Count == newScheduledJob.Job.ShipDays.Count;
        }

        private Vector2 getQuarterPositionForHoveredTile()
        {
            float x = this.Position.X + cutoffDaysColumn + (hoveringTile.X * quarterHour);
            float y = this.Position.Y + (hoveringTile.Y + 1) * rowHeight;
            return new Vector2(x, y);
        }

        private Vector2 getQuarterPositionForPlannedShipDate(Weekday day, TimeSpan time)
        {
            int quarterCount = (int)time.Subtract(TimeSpan.FromHours(6)).TotalMinutes / 15;
            float x = this.Position.X + cutoffDaysColumn + (quarterCount * quarterHour);
            float y = this.Position.Y + ((int)day + 1) * rowHeight;
            return new Vector2(x, y);
        }

        private void DrawQuarterBlock(SpriteBatch batch, float x, float y, Color color)
        {
            Primitives2D.FillRectangle(batch, x, y, quarterHour, rowHeight, color, 0.0f);
            Primitives2D.FillRectangle(batch, x+2, y+2, quarterHour-4, rowHeight-3, color, 0.0f);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(bg, new Rectangle((this.Position).ToPoint(), (this.Size).ToPoint()), Color.White);

            var color = Color.FromNonPremultiplied(60,60,60,155);

            Primitives2D.DrawLine(batch, this.Position.X + cutoffDaysColumn, 
                this.Position.Y, this.Position.X + cutoffDaysColumn, this.Position.Y + this.Size.Y, color);

            var font = scene.GetRDFont("main_font_12");

            // Top row
            {
                float x = this.Position.X;
                float y = this.Position.Y;
                Primitives2D.FillRectangle(batch,
                    x-1, y, this.Size.X+1, rowHeight+1, Color.FromNonPremultiplied(255, 173, 123, 255), 0.0f);
            }

            // Hours
            for (int i = 0; i < columns; i++)
            {
                float x = this.Position.X + cutoffDaysColumn + (i * columnWidth);
                string text = String.Format("{0}:00", schedule.StartHour + i);
                var size = font.MeasureString(text);
                batch.DrawString(font, text,
                   new Vector2(x + (columnWidth / 2) - (size.X / 2),
                   this.Position.Y  + (rowHeight / 2) - (size.Y / 2)+(2*scene.GetRDMultiplier())), Color.Black);

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

            if (offerToPlan != null)
            {
                // Days highlight
                foreach (var item in offerToPlan.ShipDays)
                {
                    float x = this.Position.X + cutoffDaysColumn;
                    float y = this.Position.Y + ((int)item + 1) * rowHeight;
                    Primitives2D.FillRectangle(batch,
                        x, y, this.Size.X - cutoffDaysColumn, rowHeight, Color.FromNonPremultiplied(255, 0, 0, 50), 0.0f);
                }
            }

            if (hoveringTile.X >= 0 && hoveringTile.X < columns*4 && hoveringTile.Y >= 0 && hoveringTile.Y < 7)
            {
                Vector2 pos = getQuarterPositionForHoveredTile();
                DrawQuarterBlock(batch, pos.X, pos.Y, color);
            }

            // Already planned times
            foreach (var job in schedule.Jobs)
            {
                var c = Color.FromNonPremultiplied(134, 232, 85, 100);
                if (job == selectedScheduledJob) c = Color.FromNonPremultiplied(255, 165, 0, 100);
                foreach (var item in job.ShipTime)
                {
                    Vector2 pos = getQuarterPositionForPlannedShipDate(item.Key, item.Value);
                    DrawQuarterBlock(batch, pos.X, pos.Y, c);
                }
            }

            // Current selection for ship times
            foreach (var item in newScheduledJob.ShipTime)
            {
                Vector2 pos = getQuarterPositionForPlannedShipDate(item.Key, item.Value);
                DrawQuarterBlock(batch, pos.X, pos.Y, Color.FromNonPremultiplied(255,0,0,100));
            }

            Primitives2D.DrawRectangle(batch, new Rectangle((this.Position).ToPoint(), (this.Size).ToPoint()), Color.FromNonPremultiplied(60, 60, 60, 255), 2);

            if (!enabled)
            {
                Primitives2D.FillRectangle(batch, new Rectangle((this.Position).ToPoint(), (this.Size).ToPoint()), Color.FromNonPremultiplied(60, 60, 60, 225));
                float lockW = padlock.Width / 2 * scene.GetRDMultiplier();
                float lockH = padlock.Height / 2 * scene.GetRDMultiplier();
                batch.Draw(padlock, new Rectangle((int)(this.Position.X + (this.Size.X / 2) - (lockW / 2)), (int)(this.Position.Y + (this.Size.Y / 2) - (lockH / 2)), (int)lockW, (int)lockH), Color.White);
            }
        }

        private void placeOfferShipTimeAtHoveredTile()
        {
            Weekday day = (Weekday)hoveringTile.Y;
            TimeSpan time = TimeSpan.FromHours(6) + TimeSpan.FromMinutes(15 * hoveringTile.X);
            if (schedule.CanScheduleJob(day, time))
            {
                if (newScheduledJob.ShipTime.ContainsKey(day))
                    newScheduledJob.ShipTime[day] = time;
                else
                    newScheduledJob.ShipTime.Add(day, time);
            }
        }

        private void selectHoveredTile()
        {
            selectedScheduledJob = null;
            Weekday day = (Weekday)hoveringTile.Y;
            TimeSpan time = TimeSpan.FromHours(6) + TimeSpan.FromMinutes(15 * hoveringTile.X);
            foreach (var job in schedule.Jobs)
            {
                if (job.ShipTime.ContainsKey(day) && job.ShipTime[day] == time)
                {
                    selectedScheduledJob = job;
                    OnScheduledOfferSelected?.Invoke(job, null);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            cutoffDaysColumn = 60 * scene.GetRDMultiplier();
            rowHeight = this.Size.Y / 8;
            columns = schedule.EndHour - schedule.StartHour + 1;
            columnWidth = (this.Size.X - cutoffDaysColumn) / columns;

            if (enabled)
            {
                var pos = Mouse.GetState();
                quarterHour = columnWidth / 4;
                if (pos.X > this.Position.X + cutoffDaysColumn && pos.Y > this.Position.Y + rowHeight && pos.X < this.Position.X + this.Size.X && pos.Y < this.Position.Y + this.Size.Y)
                    hoveringTile = new Vector2((int)((pos.X - this.Position.X - cutoffDaysColumn) / quarterHour), (int)(pos.Y - this.Position.Y - rowHeight) / (int)rowHeight);
                else
                    hoveringTile = new Vector2(-1, -1);

                bool hoveringSelectableDay = false;
                if (offerToPlan != null) // When planning offer only allow available days to be selected.
                {
                    foreach (var item in offerToPlan.ShipDays)
                    {
                        if (hoveringTile.Y == (int)item)
                        {
                            hoveringSelectableDay = true;
                            break;
                        }
                    }
                }
                else
                {
                    hoveringSelectableDay = true;
                }
                if (!hoveringSelectableDay) hoveringTile = new Vector2(-1, -1);

                var pp = getQuarterPositionForHoveredTile();
                if (pos.Clicked(pp.X, pp.Y, quarterHour, rowHeight))
                {
                    if (offerToPlan != null) placeOfferShipTimeAtHoveredTile();
                    else selectHoveredTile();
                }
            }

            base.Update(gameTime);
        }
    }
}
