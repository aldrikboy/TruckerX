using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TruckerX.Extensions;
using TruckerX.State;
using TruckerX.Widgets;

namespace TruckerX.Scenes
{
    public class EmployeeDetailScene : DetailScene
    {
        EmployeeState employee;

        PlaceState place;
        int selectedDockIndex = 0;
        int selectedTruckIndex = -1;
        List<ScheduleWidget> schedules = new List<ScheduleWidget>();
        TabControlWidget tabcontrol;

        BannerWidget truckBanner;
        ArrowButtonWidget arrowButtonLeft;
        ArrowButtonWidget arrowButtonRight;

        DetailButtonWidget buttonAccept;

        public EmployeeDetailScene(EmployeeState employee, PlaceState state) : base(employee.Name + ", " + employee.Age + ", " + employee.Id)
        {
            this.employee = employee;
            this.place = state;

            if (this.employee.AssignedTruck != null)
            {
                for (int i = 0; i < place.Trucks.Count; i++)
                {
                    var truck = place.Trucks[i];
                    if (truck == employee.AssignedTruck)
                    {
                        selectedTruckIndex = i;
                        break;
                    }
                }
            }

            List<TabControlItemWidget> tabs = new List<TabControlItemWidget>();
            for (int i = 0; i < place.Docks.Count; i++)
            {
                var item = place.Docks[i];
                var tab = new TabControlItemWidget(this, "Dock " + (i + 1), item);
                tab.OnClick += Tab_OnClick;
                tabs.Add(tab);
                var newSchedule = new ScheduleWidget(this, item.Schedule, null, item.Unlocked, false, employee);
                schedules.Add(newSchedule);
            }
            tabcontrol = new TabControlWidget(this, tabs);

            buttonAccept = new DetailButtonWidget(true);
            buttonAccept.Text = "Accept";
            buttonAccept.OnClick += ButtonAccept_OnClick;

            arrowButtonLeft = new ArrowButtonWidget(PointingTo.Left);
            arrowButtonLeft.OnClick += ArrowButtonLeft_OnClick;
            arrowButtonRight = new ArrowButtonWidget(PointingTo.Right);
            arrowButtonRight.OnClick += ArrowButtonRight_OnClick;
            if (employee.AssignedTruck == null)
                truckBanner = new BannerWidget();
            else
                truckBanner = new TruckBannerWidget(employee.AssignedTruck);
            truckBanner.Disabled = true;
        }

        private void ArrowButtonRight_OnClick(object sender, EventArgs e)
        {
            selectedTruckIndex++;
            if (selectedTruckIndex > place.Trucks.Count-1) selectedTruckIndex = 0;
            truckBanner = new TruckBannerWidget(place.Trucks[selectedTruckIndex]);

            // TODO: maybe just store old position and reset here...
            CustomUpdate(new GameTime());
            CustomUpdate(new GameTime());
        }

        private void ArrowButtonLeft_OnClick(object sender, EventArgs e)
        {
            selectedTruckIndex--;
            if (selectedTruckIndex < 0) selectedTruckIndex = place.Trucks.Count - 1;
            truckBanner = new TruckBannerWidget(place.Trucks[selectedTruckIndex]);

            // TODO: maybe just store old position and reset here...
            CustomUpdate(new GameTime());
            CustomUpdate(new GameTime());
        }

        private void ButtonAccept_OnClick(object sender, EventArgs e)
        {
            this.SwitchSceneTo(new PlaceDetailScene(place.Place));
        }

        private void Tab_OnClick(object sender, EventArgs e)
        {
            var tab = sender as TabControlItemWidget;

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

            //textY += AddLine(batch, "From: " + offer.From.Name, font, startLeft, startTop + textY);
            //textY += AddLine(batch, "To: " + offer.To.Name, font, startLeft, startTop + textY);

            schedules[selectedDockIndex].Draw(batch, gameTime);
            tabcontrol.Draw(batch, gameTime);

            arrowButtonLeft.Draw(batch, gameTime);
            truckBanner.Draw(batch, gameTime);
            arrowButtonRight.Draw(batch, gameTime);

            buttonAccept.Draw(batch, gameTime);
        }

        public override void CustomUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.SwitchSceneTo(new PlaceDetailScene(place.Place));
            }

            var rec = TruckerX.TargetRetangle;
            var startLeft = (Padding * 2  * rec.Width);
            var height = 300 * GetRDMultiplier();
            var startTop = rec.Height - height - (Padding * rec.Height) - (Padding * rec.Width);
            var scheduleWidth = rec.Width - (rec.Width * Padding * 4);

            tabcontrol.Size = new Vector2(scheduleWidth, 26 * GetRDMultiplier());
            tabcontrol.Position = schedules[selectedDockIndex].Position - new Vector2(0, tabcontrol.Size.Y - 1);
            tabcontrol.Update(this, gameTime);

            schedules[selectedDockIndex].Position = new Vector2(rec.X + startLeft, rec.Y + startTop);
            schedules[selectedDockIndex].Size = new Vector2(scheduleWidth, height);
            schedules[selectedDockIndex].Update(this, gameTime);

            int buttonStartY = (int)(80.0f * GetRDMultiplier());
            buttonAccept.Position = buttonAccept.Position.FromPercentageWithOffset(0.95f, 0.05f) + new Vector2(-buttonAccept.Size.X, buttonStartY);
            buttonAccept.Update(this, gameTime);

            float buttonsOffsetY = (30.0f * ContentLoader.GetRDMultiplier());
            arrowButtonLeft.Position = new Vector2(startLeft, startTop - tabcontrol.Size.Y - truckBanner.Size.Y - buttonsOffsetY);
            arrowButtonLeft.Update(this, gameTime);

            truckBanner.Position = new Vector2(startLeft + arrowButtonLeft.Size.X + 1, arrowButtonLeft.Position.Y);
            truckBanner.Update(this, gameTime);

            arrowButtonRight.Position = new Vector2(startLeft + arrowButtonLeft.Size.X + truckBanner.Size.X + 2, arrowButtonLeft.Position.Y);
            arrowButtonRight.Update(this, gameTime);

        }
    }
}
