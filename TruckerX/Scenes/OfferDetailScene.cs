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
    public class OfferDetailScene : DetailScene
    {
        JobOffer offer;
        double distance = 0;
        double travelHours = 0;

        PlaceState place;
        int selectedDockIndex = 0;
        List<ScheduleWidget> schedules = new List<ScheduleWidget>();
        TabControlWidget tabcontrol;

        DetailButtonWidget buttonAccept;
        EmployeeFinderWidget employeeFinderWidget;
        CheckmarkWidget checkmarkWidget;
        bool isRescheduling;

        public OfferDetailScene(JobOffer offer, PlaceState state, bool isRescheduling = false) : base(offer.Company)
        {
            this.isRescheduling = isRescheduling;
            this.offer = offer;
            this.place = state;
            distance = this.offer.GetDistanceInKm();
            travelHours = this.offer.GetTravelTime();

            List<TabControlItemWidget> tabs = new List<TabControlItemWidget>();
            for (int i = 0; i < place.Docks.Count; i++)
            {
                var item = place.Docks[i];
                var tab = new TabControlItemWidget(this, "Dock " + (i + 1), item);
                tab.OnClick += Tab_OnClick;
                tabs.Add(tab);
                var newSchedule = new ScheduleWidget(this, item.Schedule, offer, item.Unlocked, isRescheduling);
                newSchedule.OnNewShipTimeSelected += NewSchedule_OnNewShipTimeSelected;
                schedules.Add(newSchedule);
            }
            tabcontrol = new TabControlWidget(this, tabs);
            employeeFinderWidget = new EmployeeFinderWidget(this);
            employeeFinderWidget.OnEmployeeSelected += EmployeeFinderWidget_OnEmployeeSelected;

            checkmarkWidget = new CheckmarkWidget("Stay At Destination");
            checkmarkWidget.OnCheckChanged += CheckmarkWidget_OnCheckChanged;

            buttonAccept = new DetailButtonWidget(true);
            buttonAccept.OnClick += ButtonAccept_OnClick;
        }

        private void CheckmarkWidget_OnCheckChanged(object sender, EventArgs e)
        {
            // Assign selected option to timeslot.
            if (!schedules[selectedDockIndex].Disabled)
            {
                var stayAtLocation = sender as bool?;
                schedules[selectedDockIndex].AssignStayAtLocationSettingsToNewTimeslot(stayAtLocation.Value);
            }
        }

        private void NewSchedule_OnNewShipTimeSelected(object sender, NewShipTimeSelectedEvent e)
        {
            // timeslot on schedule selected for new job.
            var assignment = sender as ShipTimeAssignment;


            for (int i = 0; i < schedules.Count; i++)
            {
                if (i == selectedDockIndex) continue;
                var values = schedules[i].ResetTimeslotForNewScheduledJob(e.Day);
                if (values.Item1 != null) assignment.AssignedEmployee = values.Item1;
                if (values.Item2 != false) assignment.StayAtLocation = values.Item2;
            }

            employeeFinderWidget.SetSelectedEmployee(assignment.AssignedEmployee);
            checkmarkWidget.SetValue(assignment.StayAtLocation);
        }

        private void EmployeeFinderWidget_OnEmployeeSelected(object sender, EventArgs e)
        {
            // Assign selected employee to timeslot.
            if (!schedules[selectedDockIndex].Disabled)
            {
                var employee = sender as EmployeeState;
                schedules[selectedDockIndex].AssignEmployeeToNewTimeslot(employee);
            }
        }

        private void ButtonAccept_OnClick(object sender, EventArgs e)
        {
            if (isRescheduling)
            {
                // remove old timeslots.
                for (int i = 0; i < schedules.Count; i++)
                {
                    place.Docks[i].Schedule.RemoveJobById(schedules[i].newScheduledJob.Job.Id);
                }
            }

            // Job timeslots can be distributed over different docks.
            for (int i = 0; i < schedules.Count; i++)
            {
                if (schedules[i].newScheduledJob.ShipTimes.Count != 0)
                    place.PlanJob(place.Docks[i], schedules[i].newScheduledJob);
            }

            this.SwitchSceneTo(new PlaceDetailScene(place.Place));
        }

        private void Tab_OnClick(object sender, EventArgs e)
        {
            var tab = sender as TabControlItemWidget;
            employeeFinderWidget.SetSelectedEmployee(null);

            for (int i = 0; i < place.Docks.Count; i++)
            {
                schedules[i].UnselectTimeslot();
                var item = place.Docks[i];
                if (item == ((DockState)tab.Data)) selectedDockIndex = i;
            }
        }

        private int AddLine(SpriteBatch batch, string text, SpriteFont font, float x, float y)
        {
            var strSize = font.MeasureString(text);
            batch.DrawString(font, text, new Vector2(x + 1, y + 1), Color.Gray);
            batch.DrawString(font, text, new Vector2(x, y), Color.White);
            return (int)strSize.Y;
        }

        public override void CustomDraw(SpriteBatch batch, GameTime gameTime)
        {
            var rec = TruckerX.TargetRetangle;
            var font = this.GetRDFont("main_font_18");
            var startLeft = rec.X + (Padding * 2 * rec.Width);
            var startTop = rec.Y + (Padding * 2 * rec.Height);
            int textY = (int)(70.0f * GetRDMultiplier());

            textY += AddLine(batch, "From: " + offer.From.Name, font, startLeft, startTop + textY);
            textY += AddLine(batch, "To: " + offer.To.Name, font, startLeft, startTop + textY);

            int hrs = (int)Math.Floor(travelHours);
            double min = travelHours - hrs;
            min = 60 * min;
            min = Math.Round(min);
            textY += AddLine(batch, "Distance: " + distance + "KM / " + hrs + "H" + min + "M", font, startLeft, startTop + textY);

            schedules[selectedDockIndex].Draw(batch, gameTime);
            tabcontrol.Draw(batch, gameTime);
            employeeFinderWidget.Draw(batch, gameTime);

            buttonAccept.Draw(batch, gameTime);
            checkmarkWidget.Draw(batch, gameTime);
        }

        private bool doneSchedulingJob()
        {
            List<Weekday> lookingFor = new List<Weekday>();
            foreach (var day in offer.ShipDays)
            {
                lookingFor.Add(day);
            }

            foreach(var day in offer.ShipDays)
            {
                for (int i = 0; i < schedules.Count; i++)
                {
                    if (schedules[i].newScheduledJob.ShipTimes.ContainsKey(day) && 
                        schedules[i].newScheduledJob.ShipTimes[day].AssignedEmployee != null) 
                        lookingFor.Remove(day);
                }
            }
            return lookingFor.Count == 0;
        }

        public override void CustomUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.SwitchSceneTo(new PlaceDetailScene(offer.From));
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

            int buttonStartY = (int)(80.0f * GetRDMultiplier());
            buttonAccept.Text = "Accept";
            buttonAccept.Position = buttonAccept.Position.FromPercentageWithOffset(0.95f, 0.05f) + new Vector2(-buttonAccept.Size.X, buttonStartY);
            buttonAccept.Update(this, gameTime);
            buttonAccept.Disabled = !doneSchedulingJob();

            employeeFinderWidget.Position = new Vector2(rec.X + (rec.Width * 0.9f) - employeeFinderWidget.Size.X, schedules[selectedDockIndex].Position.Y);
            employeeFinderWidget.Update(this, gameTime);

            checkmarkWidget.Size = new Vector2(employeeFinderWidget.Size.X, 40 * GetRDMultiplier());
            checkmarkWidget.Position = employeeFinderWidget.Position - new Vector2(0, checkmarkWidget.Size.Y + 20 * GetRDMultiplier());
            checkmarkWidget.Update(this, gameTime);

            checkmarkWidget.Disabled = !schedules[selectedDockIndex].IsEditingTimeslot();
            employeeFinderWidget.Disabled = checkmarkWidget.Disabled;
        }
    }
}
