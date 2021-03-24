﻿using Microsoft.Xna.Framework;
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

        public OfferDetailScene(JobOffer offer, PlaceState state) : base("Company Name")
        {
            this.offer = offer;
            this.place = state;
            this.ContentLoader.OnLoaded += ContentLoader_OnLoaded;
            distance = this.offer.GetDistanceInKm();
            travelHours = this.offer.GetTravelTime();
        }

        private void ContentLoader_OnLoaded(object sender, EventArgs e)
        {
            List<TabControlItemWidget> tabs = new List<TabControlItemWidget>();
            for (int i = 0; i < place.Docks.Count; i++)
            {
                var item = place.Docks[i];
                var tab = new TabControlItemWidget(this, "Dock " + (i + 1), item);
                tab.OnClick += Tab_OnClick;
                tabs.Add(tab);
                schedules.Add(new ScheduleWidget(this, item.Schedule, offer, item.Unlocked));
            }
            tabcontrol = new TabControlWidget(this, tabs);
            employeeFinderWidget = new EmployeeFinderWidget(this);

            buttonAccept = new DetailButtonWidget(this, Vector2.Zero, Vector2.Zero, true);
            buttonAccept.OnClick += ButtonAccept_OnClick;
        }

        private void ButtonAccept_OnClick(object sender, EventArgs e)
        {
            place.PlanJob(place.Docks[selectedDockIndex], schedules[selectedDockIndex].newScheduledJob);
            this.SwitchSceneTo(new PlaceDetailScene(place.Place));
        }

        private void Tab_OnClick(object sender, EventArgs e)
        {
            var tab = sender as TabControlItemWidget;

            for (int i = 0; i < place.Docks.Count; i++)
            {
                schedules[selectedDockIndex].ClearSchedulingJob();

                var item = place.Docks[i];
                if (item == ((DockState)tab.Data)) selectedDockIndex = i;
            }
        }

        public override void DeclareAssets()
        {
            Textures.AddRange(new Dictionary<string, AssetDefinition<Texture2D>>()
            {
                // Images
                { "tab-background", new AssetDefinition<Texture2D>("Textures/tab-background") },
                { "padlock", new AssetDefinition<Texture2D>("Textures/padlock") },
                { "detail-button", new AssetDefinition<Texture2D>("Textures/detailbutton") },
            });
            base.DeclareAssets();
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
        }

        public override void CustomUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.SwitchSceneTo(new PlaceDetailScene(offer.From));
            }

            var rec = TruckerX.TargetRetangle;
            var startLeft = (Padding * 2  * rec.Width);
            var height = 200 * GetRDMultiplier();
            var startTop = rec.Height - height - (Padding * rec.Height) - (Padding * rec.Width);

            tabcontrol.Size = new Vector2(132 * place.Docks.Count, 26) * GetRDMultiplier();
            tabcontrol.Position = schedules[selectedDockIndex].Position - new Vector2(0, tabcontrol.Size.Y - 1);
            tabcontrol.Update(gameTime);

            schedules[selectedDockIndex].Position = new Vector2(rec.X + startLeft, rec.Y + startTop);
            schedules[selectedDockIndex].Size = new Vector2(rec.Width - (rec.Width * Padding * 4), height);
            schedules[selectedDockIndex].Update(gameTime);

            int buttonStartY = (int)(80.0f * GetRDMultiplier());
            buttonAccept.Text = "Accept";
            buttonAccept.Size = new Vector2(300, 70) * GetRDMultiplier();
            buttonAccept.Position = buttonAccept.Position.FromPercentageWithOffset(0.95f, 0.05f) + new Vector2(-buttonAccept.Size.X, buttonStartY);
            buttonAccept.Update(gameTime);
            buttonAccept.Disabled = !schedules[selectedDockIndex].DoneSchedulingJob();

            employeeFinderWidget.Size = buttonAccept.Size;
            employeeFinderWidget.Position = employeeFinderWidget.Position.FromPercentageWithOffset(0.4f, 0.2f);
        }
    }
}
