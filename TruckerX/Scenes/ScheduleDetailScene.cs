using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Extensions;
using TruckerX.State;
using TruckerX.Widgets;

namespace TruckerX.Scenes
{
    public class ScheduleDetailScene : DetailScene
    {
        PlaceState place;
        int selectedDockIndex = 0;
        List<ScheduleWidget> schedules = new List<ScheduleWidget>();
        TabControlWidget tabcontrol;

        DetailButtonWidget buttonEdit;

        public ScheduleDetailScene(PlaceState state) : base("Schedule for " + state.Place.Name)
        {
            this.place = state;
            List<TabControlItemWidget> tabs = new List<TabControlItemWidget>();
            for (int i = 0; i < place.Docks.Count; i++)
            {
                var item = place.Docks[i];
                var tab = new TabControlItemWidget(this, "Dock " + (i + 1), item);
                tab.OnClick += Tab_OnClick;
                tabs.Add(tab);
                var schedule = new ScheduleWidget(this, item.Schedule, null, item.Unlocked);
                schedule.OnScheduledOfferSelected += Schedule_OnScheduledOfferSelected;
                schedules.Add(schedule);
            }
            tabcontrol = new TabControlWidget(this, tabs);
            buttonEdit = new DetailButtonWidget(this, true);
            buttonEdit.OnClick += ButtonEdit_OnClick;
        }

        private void Schedule_OnScheduledOfferSelected(object sender, EventArgs e)
        {
            var job = sender as ScheduledJob;
            foreach(var schedule in schedules)
            {
                schedule.SetSelectedScheduledJobById(job == null ? 0 : job.Job.Id);
            }
        }

        private void ButtonEdit_OnClick(object sender, EventArgs e)
        {
            this.SwitchSceneTo(new OfferDetailScene(schedules[selectedDockIndex].GetSelectedScheduledJob().Job, place, true));
        }

        private void Tab_OnClick(object sender, EventArgs e)
        {
            var tab = sender as TabControlItemWidget;
            for (int i = 0; i < place.Docks.Count; i++)
            {
                var item = place.Docks[i];
                if (item == ((DockState)tab.Data)) selectedDockIndex = i;
            }
        }

        public override void CustomDraw(SpriteBatch batch, GameTime gameTime)
        {
            schedules[selectedDockIndex].Draw(batch, gameTime);
            tabcontrol.Draw(batch, gameTime);
            buttonEdit.Draw(batch, gameTime);
        }

        public override void CustomUpdate( GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.SwitchSceneTo(new PlaceDetailScene(place.Place));
            }

            var rec = TruckerX.TargetRetangle;
            var startLeft = (Padding * 2  * rec.Width);
            var height = 300 * GetRDMultiplier();
            var startTop = rec.Height - height - (Padding * rec.Height) - (Padding * rec.Width);

            tabcontrol.Size = new Vector2(schedules[selectedDockIndex].Size.X, 26 * GetRDMultiplier());
            tabcontrol.Position = schedules[selectedDockIndex].Position - new Vector2(0, tabcontrol.Size.Y - 1);
            tabcontrol.Update(this, gameTime);

            schedules[selectedDockIndex].Position = new Vector2(rec.X + startLeft, rec.Y + startTop);
            schedules[selectedDockIndex].Size = new Vector2(rec.Width - (rec.Width * Padding * 4), height);
            schedules[selectedDockIndex].Update(this, gameTime);

            int buttonStartY = (int)(80.0f * GetRDMultiplier());
            buttonEdit.Text = "Reschedule";
            buttonEdit.Position = buttonEdit.Position.FromPercentageWithOffset(0.95f, 0.05f) + new Vector2(-buttonEdit.Size.X, buttonStartY);
            buttonEdit.Update(this, gameTime);
            buttonEdit.Disabled = !schedules[selectedDockIndex].IsJobSelected;
        }
    }
}
