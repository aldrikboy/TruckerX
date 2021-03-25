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
        //ScheduledJob selectedJob;

        EmployeeFinderWidget employeeFinderWidget;

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

            employeeFinderWidget = new EmployeeFinderWidget(this);
            employeeFinderWidget.OnEmployeeSelected += EmployeeFinderWidget_OnEmployeeSelected;
        }

        private void Schedule_OnScheduledOfferSelected(object sender, EventArgs e)
        {
            var assignment = sender as ShipTimeAssignment;
            employeeFinderWidget.SetSelectedEmployee(assignment == null ? null : assignment.AssignedEmployee);
        }

        private void EmployeeFinderWidget_OnEmployeeSelected(object sender, EventArgs e)
        {
            // Assign selected employee to timeslot.
            if (!schedules[selectedDockIndex].Disabled)
            {
                var employee = sender as EmployeeState;
                schedules[selectedDockIndex].AssignEmployeeToCurrentlySelectedJobAssignment(employee);
            }
        }

        private void Tab_OnClick(object sender, EventArgs e)
        {
            var tab = sender as TabControlItemWidget;

            for (int i = 0; i < place.Docks.Count; i++)
            {
                schedules[selectedDockIndex].ClearSchedulingJob();

                var item = place.Docks[i];
                if (item == ((DockState)tab.Data)) selectedDockIndex = i;
                employeeFinderWidget.Clear();
            }
        }

        public override void CustomDraw(SpriteBatch batch, GameTime gameTime)
        {
            schedules[selectedDockIndex].Draw(batch, gameTime);
            tabcontrol.Draw(batch, gameTime);

            employeeFinderWidget.Draw(batch, gameTime);
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
            var scheduleWidth = rec.Width - (rec.Width * Padding * 4) - employeeFinderWidget.Size.X - 50;

            tabcontrol.Size = new Vector2(scheduleWidth, 26 * GetRDMultiplier());
            tabcontrol.Position = schedules[selectedDockIndex].Position - new Vector2(0, tabcontrol.Size.Y - 1);
            tabcontrol.Update(this, gameTime);

            schedules[selectedDockIndex].Position = new Vector2(rec.X + startLeft, rec.Y + startTop);
            schedules[selectedDockIndex].Size = new Vector2(scheduleWidth, height);
            schedules[selectedDockIndex].Update(this, gameTime);

            employeeFinderWidget.Position = new Vector2(rec.X + (rec.Width * 0.9f) - employeeFinderWidget.Size.X, schedules[selectedDockIndex].Position.Y);
            employeeFinderWidget.Update(this, gameTime);
        }
    }
}
