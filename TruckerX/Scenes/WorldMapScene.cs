using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TruckerX.Extensions;
using TruckerX.Particles;
using TruckerX.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using MonoGame;
using TruckerX.Input;

namespace TruckerX.Scenes
{
    public class WorldMapScene : BaseScene
    {
        public List<WorldLocationWidget> Locations { get; } = new List<WorldLocationWidget>();
        int placeDotSize = 5;

        float zoom = 1.0f;
        int currentOffsetX = 0;
        int currentOffsetY = 0;
        int dragDiffX = 0;
        int dragDiffY = 0;

        public int OffsetX { get { return -(currentOffsetX + dragDiffX); } }
        public int OffsetY { get { return -(currentOffsetY + dragDiffY); } }

        public WorldMapScene()
        {
            foreach (var country in WorldData.Countries)
            {
                foreach (var place in country.Places)
                {
                    var btn = new WorldLocationWidget(this, place);
                    btn.OnClick += Btn_OnClick;
                    Locations.Add(btn);
                }
            }
        }

        private void Btn_OnClick(object sender, EventArgs e)
        {
            var btn = sender as WorldLocationWidget;
            this.SwitchSceneTo(new PlaceDetailScene(btn.Place));
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            {
                var bg = ContentLoader.GetTexture("world");
                var targetRec = TruckerX.TargetRetangle;
                targetRec.X += OffsetX;
                targetRec.Y += OffsetY;
                targetRec.Width = (int)(targetRec.Width / zoom);
                targetRec.Height = (int)(targetRec.Height / zoom);
                batch.Draw(bg, targetRec, Color.White);
            }

            var rec = TruckerX.TargetRetangle;
            foreach (var item in Locations)
            {
                foreach (var connection in item.Place.Connections)
                {
                    int dotCenterOrig = placeDotSize / (int)item.Place.Size / 2;
                    int dotCenterDest = placeDotSize / (int)connection.Size / 2;

                    Vector2 start = new Vector2(OffsetX + rec.X + (int)(item.Place.MapX * rec.Width / zoom),
                        OffsetY + rec.Y + (int)(item.Place.MapY * rec.Height / zoom));
                    Vector2 end = new Vector2(OffsetX + rec.X + (int)(connection.MapX * rec.Width / zoom),
                        OffsetY + rec.Y + (int)(connection.MapY * rec.Height / zoom));
                    Primitives2D.DrawLine(batch, start, end, Color.FromNonPremultiplied(100,100,100,255));
                }
            }

            foreach(var activeJob in Simulation.simulation.ActiveJobs)
            {
                Vector2 size = new Vector2(placeDotSize / (int)PlaceSize.Small, placeDotSize / (int)PlaceSize.Small);
                var pos = activeJob.GetCurrentWorldLocation();
                Vector2 location = new Vector2(OffsetX + rec.X + (int)(pos.X * rec.Width / zoom),
                        OffsetY + rec.Y + (int)(pos.Y * rec.Height / zoom));
                Primitives2D.DrawCircle(batch, location.X, location.Y, size.X/2.0f, 32, Color.Orange, size.X/2);
            }

            foreach (var item in Locations)
            {
                item.Draw(batch, gameTime);
            }

#if true
            var font = GetRDFont("main_font_15");
            float mousePercentageX = ((Mouse.GetState().X - rec.X - OffsetX) / (float)rec.Width * zoom);
            float mousePercentageY = ((Mouse.GetState().Y - rec.Y - OffsetY) / (float)rec.Height * zoom);
            batch.DrawString(font, string.Format("{0:0.000} {1:0.000}", mousePercentageX, mousePercentageY), new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Red);
#endif
            base.Draw(batch, gameTime);
        }

        void handleMapZoom()
        {
            float prevMapOverflowX = (TruckerX.TargetRetangle.Width / zoom) - TruckerX.TargetRetangle.Width;
            float prevMapOverflowY = (TruckerX.TargetRetangle.Height / zoom) - TruckerX.TargetRetangle.Height;

            var scroll = InputHelper.ScrollValue();
            //float currentMapWidth = TruckerX.TargetRetangle.Width / zoom;
            //float currentMapHeight = TruckerX.TargetRetangle.Height / zoom;
            float prevZoom = zoom;
            zoom -= scroll / 10.0f;
            if (zoom > 1.0f) zoom = 1.0f;
            if (zoom < 0.3f) zoom = 0.3f;
            float mapOverflowX = (TruckerX.TargetRetangle.Width / zoom) - TruckerX.TargetRetangle.Width;
            float mapOverflowY = (TruckerX.TargetRetangle.Height / zoom) - TruckerX.TargetRetangle.Height;
            if (scroll != 0 && prevZoom != zoom)
            {      
                currentOffsetX -= (int)((prevMapOverflowX - mapOverflowX) / 2);
                currentOffsetY -= (int)((prevMapOverflowY - mapOverflowY) / 2);

                var rec = TruckerX.TargetRetangle;
                var state = Mouse.GetState();
                //float mouseOffsetX = (state.X - rec.X) / (float)rec.Width;
                //float mouseOffsetY = (state.Y - rec.Y) / (float)rec.Height;

                //if (mouseOffsetX <= 0.5) currentOffsetX -= (int)((0.5 - mouseOffsetX) *currentMapWidth / 2);
                //if (mouseOffsetX > 0.5) currentOffsetX += (int)((mouseOffsetX - 0.5) *currentMapWidth / 2);
                //if (mouseOffsetY <= 0.5) currentOffsetY -= (int)((0.5 - mouseOffsetY) *currentMapHeight / 2);
                //if (mouseOffsetY > 0.5) currentOffsetY += (int)((mouseOffsetY - 0.5) *currentMapHeight / 2);
            }
        }

        int startDragMouseX = -1;
        int startDragMouseY = -1;
        void handleMapDragging()
        {
            var mouse = Mouse.GetState();
            if (mouse.RightButton == ButtonState.Pressed)
            {
                if (startDragMouseX == -1)
                {
                    startDragMouseX = mouse.X;
                    startDragMouseY = mouse.Y;
                }

                dragDiffX = mouse.X - startDragMouseX;
                dragDiffY = mouse.Y - startDragMouseY;
                dragDiffX = -dragDiffX;
                dragDiffY = -dragDiffY;
            }
            else if (startDragMouseX != -1)
            {
                currentOffsetX += dragDiffX;
                currentOffsetY += dragDiffY;
                dragDiffX = 0;
                dragDiffY = 0;
                startDragMouseX = -1;
                startDragMouseY = -1;
            }
            if (OffsetX > 0)
            {
                dragDiffX = 0 - currentOffsetX;
            }
            if (OffsetY > 0)
            {
                dragDiffY = 0 - currentOffsetY;
            }
            int maxX = (int)(-(TruckerX.TargetRetangle.Width / zoom)) - (int)(-TruckerX.TargetRetangle.Width);
            if (OffsetX < maxX)
            {
                int overflow = OffsetX - maxX;
                dragDiffX = dragDiffX + overflow;
            }
            int maxY = (int)(-(TruckerX.TargetRetangle.Height / zoom)) - (int)(-TruckerX.TargetRetangle.Height);
            if (OffsetY < maxY)
            {
                int overflow = OffsetY - maxY;
                dragDiffY = dragDiffY + overflow;
            }
        }

        private Vector2 LatLonToOffset(double lat, double lon)
        {
            var rec = TruckerX.TargetRetangle;
            const double FE = 180;
            double radius = ((rec.Width / zoom) / (2.0*Math.PI));
            double latRad = (Math.PI / 180) * lat;
            double lonRad = (Math.PI / 180) * (lon + FE);
            double x = lonRad * radius;
            double YfromEquator = radius * Math.Log(Math.Tan(Math.PI / 4 + latRad / 2));
            double y = (rec.Height / zoom) / 2 - YfromEquator;
            return new Vector2((float)x, (float)y);
        }

        public override void Update(GameTime gameTime)
        {
            const int defaultDotSize = 5;
            var rec = TruckerX.TargetRetangle;
            placeDotSize = (int)(defaultDotSize / zoom / (1280.0f / rec.Width));

            if (TruckerX.Game.IsActive)
            {
                handleMapZoom();
                handleMapDragging();
            }

            foreach (WorldLocationWidget item in Locations)
            {
                int dotSize = placeDotSize / (int)item.Place.Size;
                item.Position = new Vector2(
                    OffsetX + rec.X + (int)(item.Place.MapX * rec.Width / zoom) - (dotSize / 2),
                    OffsetY + rec.Y + (int)(item.Place.MapY * rec.Height / zoom) - (dotSize / 2));
                item.Size = new Vector2(dotSize, dotSize);
                item.Update(this, gameTime);
            }

            base.Update(gameTime);
        }
    }
}
