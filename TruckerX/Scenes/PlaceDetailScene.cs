using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Extensions;
using TruckerX.Locations;
using TruckerX.State;
using TruckerX.Widgets;

namespace TruckerX.Scenes
{
    public enum SelectedDetailView
    {
        Employees,
        Jobs,
    }

    public class PlaceDetailScene : DetailScene
    {
        private BasePlace place;
        private PlaceState state;

        DetailButtonWidget employeeButton;
        DetailButtonWidget offersButton;
        DetailButtonWidget schedulesButton;

        BannerListWidget employeeBanners;
        BannerListWidget jobBanners;

        SelectedDetailView currentView = SelectedDetailView.Employees;

        public PlaceDetailScene(BasePlace place) : base(place.Name)
        {
            this.place = place;
            this.state = WorldState.GetStateForPlace(place);

            employeeButton = new DetailButtonWidget(this);
            offersButton = new DetailButtonWidget(this);
            schedulesButton = new DetailButtonWidget(this);

            employeeButton.OnClick += EmployeeButton_OnClick;
            offersButton.OnClick += JobsButton_OnClick;
            schedulesButton.OnClick += SchedulesButton_OnClick;

            createEmployeeBanners();
            createJobBanners();
        }

        private void createJobBanners()
        {
            var jobBannerList = new List<BannerWidget>();
            foreach (var item in state.AvailableJobs)
            {
                var banner = new JobBannerWidget(this, item);
                banner.OnClick += Banner_OnClick;
                jobBannerList.Add(banner);
            }
            jobBanners = new BannerListWidget(this, jobBannerList);
        }

        private void Banner_OnClick(object sender, EventArgs e)
        {
            var banner = sender as JobBannerWidget;
            this.SwitchSceneTo(new OfferDetailScene(banner.Job, state));
        }

        private void createEmployeeBanners()
        {
            var employeeBannerList = new List<BannerWidget>();
            foreach(var place in WorldState.OwnedPlaces)
            {
                foreach(var employee in place.Employees)
                {
                    if (employee.OriginalLocation == state.Place)
                    {
                        employeeBannerList.Add(new EmployeeBannerWidget(this, employee));
                    }
                }
            }

            foreach (var job in Simulation.simulation.ActiveJobs)
            {
                if (job.Employee.OriginalLocation == state.Place)
                {
                    employeeBannerList.Add(new EmployeeBannerWidget(this, job.Employee));
                }
            }

            employeeBanners = new BannerListWidget(this, employeeBannerList);
        }

        private void SchedulesButton_OnClick(object sender, EventArgs e)
        {
            this.SwitchSceneTo(new ScheduleDetailScene(state));
        }

        private void JobsButton_OnClick(object sender, EventArgs e)
        {
            currentView = SelectedDetailView.Jobs;
        }

        private void EmployeeButton_OnClick(object sender, EventArgs e)
        {
            currentView = SelectedDetailView.Employees;
        }

        public override void CustomDraw(SpriteBatch batch, GameTime gameTime)
        {
            DrawContainer(batch, gameTime);
        }

        private void DrawContainer(SpriteBatch batch, GameTime gameTime)
        {
            employeeButton.Draw(batch, gameTime);
            offersButton.Draw(batch, gameTime);
            schedulesButton.Draw(batch, gameTime);

            switch (currentView)
            {
                case SelectedDetailView.Employees: employeeBanners.Draw(batch, gameTime); break;
                case SelectedDetailView.Jobs: jobBanners.Draw(batch, gameTime); break;
            }
           

            DrawSeparator(batch);
        }

        public override void CustomUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.SwitchSceneTo(new WorldMapScene());
            }

            int buttonStartY = (int)(120.0f * GetRDMultiplier());
            int buttonPadY = (int)(80.0f * GetRDMultiplier());

            employeeButton.Text = "Employees";
            employeeButton.Position = employeeButton.Position.FromPercentageWithOffset(0.05f, 0.05f) + new Vector2(0,buttonStartY);
            employeeButton.Update(this, gameTime);
            
            schedulesButton.Text = "Schedules";
            schedulesButton.Size = employeeButton.Size;
            schedulesButton.Position = employeeButton.Position + new Vector2(0, buttonPadY * 1);
            schedulesButton.Update(this, gameTime);

            offersButton.Text = "Job Offers";
            offersButton.Size = employeeButton.Size;
            offersButton.Position = employeeButton.Position + new Vector2(0, buttonPadY * 2);
            offersButton.Update(this, gameTime);

            employeeBanners.Size = new Vector2().FromPercentage(0.32f, 0.8f);
            employeeBanners.Position = new Vector2().FromPercentageWithOffset(0.565f, 0.1f);

            jobBanners.Size = employeeBanners.Size;
            jobBanners.Position = employeeBanners.Position;

            switch(currentView)
            {
                case SelectedDetailView.Employees: employeeBanners.Update(this, gameTime); break;
                case SelectedDetailView.Jobs: jobBanners.Update(this, gameTime); break;
            }
        }
    }
}
