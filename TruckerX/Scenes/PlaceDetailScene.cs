﻿using Microsoft.Xna.Framework;
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
        Garage,
    }

    public class PlaceDetailScene : DetailScene
    {
        private PlaceState state;

        DetailButtonWidget employeeButton;
        DetailButtonWidget offersButton;
        DetailButtonWidget schedulesButton;
        DetailButtonWidget garageButton;

        BannerListWidget employeeBanners;
        BannerListWidget jobBanners;
        BannerListWidget truckBanners;

        SelectedDetailView currentView = SelectedDetailView.Employees;

        public PlaceDetailScene(BasePlace place) : base(place.Name)
        {
            this.state = WorldState.GetStateForPlace(place);
            state.OnEmployeeArrived += State_OnEmployeeArrived; 

            employeeButton = new DetailButtonWidget();
            offersButton = new DetailButtonWidget();
            schedulesButton = new DetailButtonWidget();
            garageButton = new DetailButtonWidget();

            employeeButton.Text = "Employees";
            schedulesButton.Text = "Schedules";
            offersButton.Text = "Job Offers";
            garageButton.Text = "Garage";

            employeeButton.OnClick += EmployeeButton_OnClick;
            offersButton.OnClick += JobsButton_OnClick;
            schedulesButton.OnClick += SchedulesButton_OnClick;
            garageButton.OnClick += GarageButton_OnClick;

            createEmployeeBanners();
            createJobBanners();
            createTruckBanners();
        }

        private void GarageButton_OnClick(object sender, EventArgs e)
        {
            currentView = SelectedDetailView.Garage;
        }

        private void State_OnEmployeeArrived(object sender, EventArgs e)
        {
            createEmployeeBanners();
        }

        private void createTruckBanners()
        {
            var truckBannerList = new List<BannerWidget>();
            var newTruckButton = new BannerWidget("Purchase Truck");
            newTruckButton.OnClick += NewTruckButton_OnClick;
            truckBannerList.Add(newTruckButton);
            foreach (var item in state.Trucks)
            {
                var banner = new TruckBannerWidget(item);
                //banner.OnClick += Banner_OnClick;
                truckBannerList.Add(banner);
            }
            truckBanners = new BannerListWidget(this, truckBannerList);
        }

        private void NewTruckButton_OnClick(object sender, EventArgs e)
        {
            this.SwitchSceneTo(new TruckManufacturerSelectionScene(this.state.Place));
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

            bool headerAdded = false;
            foreach (var employee in WorldState.InternalEmployeesFromPlace(state.Place))
            {
                if (!headerAdded)
                {
                    employeeBannerList.Add(new SplitterBannerWidget("Internal"));
                    headerAdded = true;
                }

                if (employee.OriginalLocation == state.Place)
                {
                    var employeeBanner = new EmployeeBannerWidget(this, employee);
                    employeeBannerList.Add(employeeBanner);
                    employeeBanner.OnClick += EmployeeBanner_OnClick; 
                }
            }

            headerAdded = false;
            foreach (var employee in WorldState.ExternalEmployeesCurrentlyAt(state.Place))
            {
                if (!headerAdded)
                {
                    employeeBannerList.Add(new SplitterBannerWidget("External"));
                    headerAdded = true;
                }

                var employeeBanner = new EmployeeBannerWidget(this, employee);
                employeeBannerList.Add(employeeBanner);
                employeeBanner.OnClick += EmployeeBanner_OnClick;
            }

            employeeBanners = new BannerListWidget(this, employeeBannerList);
            employeeBanners.Update(this, null);
        }

        private void EmployeeBanner_OnClick(object sender, EventArgs e)
        {
            var banner = sender as EmployeeBannerWidget;
            this.SwitchSceneTo(new EmployeeDetailScene(banner.Employee, state));
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
            garageButton.Draw(batch, gameTime);

            switch (currentView)
            {
                case SelectedDetailView.Employees: employeeBanners.Draw(batch, gameTime); break;
                case SelectedDetailView.Jobs: jobBanners.Draw(batch, gameTime); break;
                case SelectedDetailView.Garage: truckBanners.Draw(batch, gameTime); break;
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

            employeeButton.Position = employeeButton.Position.FromPercentageWithOffset(0.05f, 0.05f) + new Vector2(0,buttonStartY);
            employeeButton.Update(this, gameTime);
            
            schedulesButton.Size = employeeButton.Size;
            schedulesButton.Position = employeeButton.Position + new Vector2(0, buttonPadY * 1);
            schedulesButton.Update(this, gameTime);

            offersButton.Size = employeeButton.Size;
            offersButton.Position = employeeButton.Position + new Vector2(0, buttonPadY * 2);
            offersButton.Update(this, gameTime);

            garageButton.Size = employeeButton.Size;
            garageButton.Position = employeeButton.Position + new Vector2(0, buttonPadY * 3);
            garageButton.Update(this, gameTime);

            employeeBanners.Size = new Vector2().FromPercentage(0.32f, 0.8f);
            employeeBanners.Position = new Vector2().FromPercentageWithOffset(0.565f, 0.1f);

            jobBanners.Size = employeeBanners.Size;
            jobBanners.Position = employeeBanners.Position;

            truckBanners.Size = employeeBanners.Size;
            truckBanners.Position = employeeBanners.Position;

            switch (currentView)
            {
                case SelectedDetailView.Employees: employeeBanners.Update(this, gameTime); break;
                case SelectedDetailView.Jobs: jobBanners.Update(this, gameTime); break;
                case SelectedDetailView.Garage: truckBanners.Update(this, gameTime); break;
            }
        }

        public override void Dispose()
        {
            this.state.OnEmployeeArrived -= State_OnEmployeeArrived;
            base.Dispose();
        }
    }
}
