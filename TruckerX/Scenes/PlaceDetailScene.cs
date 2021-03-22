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

        BannerListWidget employeeBanners;
        BannerListWidget jobBanners;

        SelectedDetailView currentView = SelectedDetailView.Employees;

        public PlaceDetailScene(BasePlace place) : base(place.Name)
        {
            this.place = place;
            this.ContentLoader.OnLoaded += ContentLoader_OnLoaded;
            this.state = WorldState.GetStateForPlace(place);
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
            this.SwitchSceneTo(new OfferDetailScene(banner.Job));
        }

        private void createEmployeeBanners()
        {
            var employeeBannerList = new List<BannerWidget>();
            foreach (var item in state.Employees)
            {
                employeeBannerList.Add(new EmployeeBannerWidget(this, item));
            }
            employeeBanners = new BannerListWidget(this, employeeBannerList);
        }

        private void ContentLoader_OnLoaded(object sender, EventArgs e)
        {
            employeeButton = new DetailButtonWidget(this, new Vector2(0, 0), new Vector2(300, 70));
            offersButton = new DetailButtonWidget(this, new Vector2(0, 0), new Vector2(300, 70));

            employeeButton.OnClick += EmployeeButton_OnClick;
            offersButton.OnClick += JobsButton_OnClick;

            createEmployeeBanners();
            createJobBanners();
        }

        private void JobsButton_OnClick(object sender, EventArgs e)
        {
            currentView = SelectedDetailView.Jobs;
        }

        private void EmployeeButton_OnClick(object sender, EventArgs e)
        {
            currentView = SelectedDetailView.Employees;
        }

        public override void DeclareAssets()
        {
            Textures.AddRange(new Dictionary<string, AssetDefinition<Texture2D>>()
            {
                // Images
                { "detail-button", new AssetDefinition<Texture2D>("Textures/detailbutton") },
                { "detail-view", new AssetDefinition<Texture2D>("Textures/detailview") },

                { "portrait", new AssetDefinition<Texture2D>("Textures/portrait") },
            });

            Songs.AddRange(new Dictionary<string, AssetDefinition<Song>>()
            {
                // Songs
            });

            Samples.AddRange(new Dictionary<string, AssetDefinition<SoundEffect>>()
            {
                // Samples
                { "pop2", new AssetDefinition<SoundEffect>("Sounds/pop2") },
            });

            base.DeclareAssets();
        }

        public override void CustomDraw(SpriteBatch batch, GameTime gameTime)
        {
            DrawContainer(batch, gameTime);
        }

        private void DrawContainer(SpriteBatch batch, GameTime gameTime)
        {
            employeeButton.Draw(batch, gameTime);
            offersButton.Draw(batch, gameTime);

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

            employeeButton.Text = String.Format("Employees: ({0}/{1})", state.Employees.Count, state.ActiveEmployeeCount());
            employeeButton.Size = new Vector2(300, 70) * GetRDMultiplier();
            employeeButton.Position = employeeButton.Position.FromPercentageWithOffset(0.05f, 0.05f) + new Vector2(0,buttonStartY);
            employeeButton.Update(gameTime);

            offersButton.Text = "Offers";
            offersButton.Size = employeeButton.Size;
            offersButton.Position = employeeButton.Position + new Vector2(0, buttonPadY*1);
            offersButton.Update(gameTime);

            employeeBanners.Size = new Vector2().FromPercentage(0.32f, 0.8f);
            employeeBanners.Position = new Vector2().FromPercentageWithOffset(0.565f, 0.1f);

            jobBanners.Size = employeeBanners.Size;
            jobBanners.Position = employeeBanners.Position;

            switch(currentView)
            {
                case SelectedDetailView.Employees: employeeBanners.Update(gameTime); break;
                case SelectedDetailView.Jobs: jobBanners.Update(gameTime); break;
            }
        }
    }
}
