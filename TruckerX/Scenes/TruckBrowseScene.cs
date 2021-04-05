using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;
using TruckerX.Extensions;
using TruckerX.Locations;
using TruckerX.State;
using TruckerX.Trucks;
using TruckerX.Widgets;

namespace TruckerX.Scenes
{
    class TruckBrowseScene : DetailScene
    {
        private BasePlace place;
        private BaseTruckManufacturer manufacturer;
        private BaseTruck selectedTruck;
        private int selectedIndex = 0;
        private Texture2D truckIcon;

        // Widgets
        DetailButtonWidget purchasePurchase;
        ArrowButtonWidget arrowButtonLeft;
        ArrowButtonWidget arrowButtonRight;

        public TruckBrowseScene(BasePlace place, BaseTruckManufacturer manufacturer) : base(manufacturer.Name + " Dealership")
        {
            this.place = place;
            this.manufacturer = manufacturer;
            selectedTruck = manufacturer.Trucks[selectedIndex];
            truckIcon = ContentLoader.GetTexture("truck-icon");

            purchasePurchase = new DetailButtonWidget(true);
            purchasePurchase.Text = "Purchase";
            purchasePurchase.OnClick += PurchasePurchase_OnClick;

            arrowButtonLeft = new ArrowButtonWidget(PointingTo.Left);
            arrowButtonRight = new ArrowButtonWidget(PointingTo.Right);
            arrowButtonLeft.OnClick += ArrowButtonLeft_OnClick;
            arrowButtonRight.OnClick += ArrowButtonRight_OnClick;
        }

        private void PurchasePurchase_OnClick(object sender, EventArgs e)
        {
            var state = WorldState.GetStateForPlace(place);
            Simulation.simulation.Money -= selectedTruck.Price;
            state.Trucks.Add((BaseTruck)selectedTruck.Clone());
            this.SwitchSceneTo(new PlaceDetailScene(place));
        }

        private void ArrowButtonRight_OnClick(object sender, EventArgs e)
        {
            selectedIndex++;
            if (selectedIndex >= manufacturer.Trucks.Count) selectedIndex = 0;
            selectedTruck = manufacturer.Trucks[selectedIndex];
        }

        private void ArrowButtonLeft_OnClick(object sender, EventArgs e)
        {
            selectedIndex--;
            if (selectedIndex < 0) selectedIndex = manufacturer.Trucks.Count;
            selectedTruck = manufacturer.Trucks[selectedIndex];
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

            Vector2 size = truckIcon.ScaleToWindowHeight(0.4f);
            batch.Draw(truckIcon, new Rectangle((int)((rec.Width/2)-(size.X/2)), (int)((rec.Height / 2) - (size.Y / 2)), (int)size.X, (int)size.Y), Color.White);

            int textY = (int)(500.0f * GetRDMultiplier());
            textY += AddLine(batch, "Name: " + selectedTruck.Name, font, arrowButtonLeft.Position.X, textY);
            textY += AddLine(batch, "Consumption: " + selectedTruck.LiterPer100Km + "L/KM", font, arrowButtonLeft.Position.X, textY);
            textY += AddLine(batch, "HP: " + selectedTruck.HorsePower, font, arrowButtonLeft.Position.X, textY);
            textY += AddLine(batch, "Price: " + Currency.USD.Sign + selectedTruck.Price, font, arrowButtonLeft.Position.X, textY);

            purchasePurchase.Draw(batch, gameTime);
            arrowButtonLeft.Draw(batch, gameTime);
            arrowButtonRight.Draw(batch, gameTime);
        }

        public override void CustomUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.SwitchSceneTo(new TruckManufacturerSelectionScene(place));
            }

            int buttonStartY = (int)(80.0f * GetRDMultiplier());
            purchasePurchase.Update(this, gameTime);
            purchasePurchase.Position = purchasePurchase.Position.FromPercentageWithOffset(0.95f, 0.05f) + new Vector2(-purchasePurchase.Size.X, buttonStartY);

            arrowButtonLeft.Update(this, gameTime);
            arrowButtonLeft.Position = new Vector2((int)(200 * GetRDMultiplier()), (SceneHeightHalfRD) - (arrowButtonLeft.Size.Y / 2));

            arrowButtonRight.Update(this, gameTime);
            arrowButtonRight.Position = new Vector2((int)((SceneWidth - 200)* GetRDMultiplier()), (SceneHeightHalfRD) - (arrowButtonLeft.Size.Y / 2));
        }
    }
}
