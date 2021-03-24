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

        public ScheduleDetailScene(PlaceState state) : base("Schedule for " + state.Place.Name)
        {
            this.place = state;
            this.ContentLoader.OnLoaded += ContentLoader_OnLoaded;
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
                schedules.Add(new ScheduleWidget(this, item.Schedule, null, item.Unlocked));
            }
            tabcontrol = new TabControlWidget(this, tabs);
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

        public override void CustomDraw(SpriteBatch batch, GameTime gameTime)
        {
            schedules[selectedDockIndex].Draw(batch, gameTime);
            tabcontrol.Draw(batch, gameTime);
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

            tabcontrol.Size = new Vector2(132 * place.Docks.Count, 26) * GetRDMultiplier();
            tabcontrol.Position = schedules[selectedDockIndex].Position - new Vector2(0, tabcontrol.Size.Y - 1);
            tabcontrol.Update(gameTime);

            schedules[selectedDockIndex].Position = new Vector2(rec.X + startLeft, rec.Y + startTop);
            schedules[selectedDockIndex].Size = new Vector2(rec.Width - (rec.Width * Padding * 4), height);
            schedules[selectedDockIndex].Update(gameTime);
        }
    }
}
