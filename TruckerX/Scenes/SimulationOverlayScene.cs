using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;
using TruckerX.Animations;
using TruckerX.Extensions;
using TruckerX.Messaging;
using TruckerX.State;
using TruckerX.Widgets;

namespace TruckerX.Scenes
{
    public enum SimulationTab
    {
        Log,
    }

    public class SimulationOverlayScene : BaseScene
    {
        Texture2D background = null;
        OverlayButtonWidget buttonLog;
        int topRowRefHeight = 50;
        SimulationTab openTab = SimulationTab.Log;
        LinearAnimation openTabAnimation;
        Rectangle tabRec;
        float tabBorderWidth;

        public SimulationOverlayScene()
        {
            openTabAnimation = new LinearAnimation(TimeSpan.FromMilliseconds(250));
            openTabAnimation.Reverse();
        }

        private void drawTabBackground(SpriteBatch batch)
        {
            Primitives2D.FillRectangle(batch, tabRec, Color.FromNonPremultiplied(60, 60, 60, 255));
            Primitives2D.FillRectangle(batch, new Rectangle(tabRec.X, tabRec.Y, 
                (int)tabBorderWidth, tabRec.Height), Color.FromNonPremultiplied(97, 97, 97, 255));
        }

        private void drawLog(SpriteBatch batch)
        {
            var font = ContentLoader.GetRDFont("main_font_12");
            float y = tabRec.Y + tabRec.Height;
            int pad = 20;
            int padHalf = (pad/2);

            var mouse = Mouse.GetState();
            Vector2 spaceSize = font.MeasureString(" ");

            for (int i = 0; i < 20; i++)
            {
                int index = MessageLog.Messages.Count - 1 - i;
                if (index >= MessageLog.Messages.Count || index < 0) break;
                var item = MessageLog.Messages[index];

                int lineCount = 1;
                float lineWidth = 0;
                foreach(var word in item.Words)
                {
                    var size = font.MeasureString(word);
                    lineWidth += size.X + spaceSize.X;
                    if (lineWidth > tabRec.Width)
                    {
                        lineCount++;
                        lineWidth = 0;
                    }
                }
                lineWidth = 0;
                y -= (spaceSize.Y*lineCount) + (padHalf);

                float totalHeight = (spaceSize.Y * lineCount);
                if (mouse.X >= tabRec.X && mouse.X <= tabRec.X + tabRec.Width && mouse.Y >= y - padHalf / 2 && mouse.Y < y + totalHeight + (padHalf / 2))
                {
                    Primitives2D.FillRectangle(batch, 
                        new Rectangle((int)(tabRec.X + tabBorderWidth), (int)y- padHalf/2, (int)(tabRec.Width - tabBorderWidth), (int)totalHeight+(pad/2)), 
                        Color.FromNonPremultiplied(255,255,255,50));
                }

                Color c = Color.White;
                if (item.Type == MessageType.Warning) c = Color.Orange;
                if (item.Type == MessageType.Error) c = Color.Red;

                lineCount = 0;
                foreach (var word in item.Words)
                {
                    var size = font.MeasureString(word);
                    float newLineWidth = lineWidth + size.X + spaceSize.X;
                    if (newLineWidth > tabRec.Width - tabBorderWidth - pad)
                    {
                        lineCount++;
                        newLineWidth = size.X + spaceSize.X;
                        lineWidth = 0;
                    }
                    batch.DrawString(font, word, new Vector2(tabRec.X + tabBorderWidth + (pad / 2) + lineWidth, y + (lineCount*spaceSize.Y)), c);
                    lineWidth = newLineWidth;
                }
            }
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (!ContentLoader.Done) return;
            if (background == null) background = ContentLoader.GetTexture("overlay-background");

            drawTabBackground(batch);
            switch (openTab)
            {
                case SimulationTab.Log: drawLog(batch); break;
            }

            var rec = TruckerX.TargetRetangle;
            Vector2 size = new Vector2(293, 250) * GetRDMultiplier();
            Vector2 pos = new Vector2(rec.X + rec.Width - size.X, rec.Y);
            batch.Draw(background, new Rectangle((int)pos.X+2, (int)pos.Y, (int)(size.X), (int)(size.Y)), Color.White);

            float textY = (topRowRefHeight * GetRDMultiplier()) / 2;
            SpriteFont font = this.GetRDFont("main_font_12");
            string dateFormatted = Currency.USD.Sign + string.Format("{0:0.00}", Simulation.simulation.Money) + " - " + Simulation.simulation.Time.ToString("ddd dd/MM HH:mm");
            var stringSize = font.MeasureString(dateFormatted);
            batch.DrawString(font, dateFormatted, new Vector2(rec.X + rec.Width - stringSize.X - (10*GetRDMultiplier()), rec.Y + textY - (stringSize.Y/2)), Color.White);

            buttonLog.Draw(batch, gameTime);

            base.Draw(batch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!ContentLoader.Done) return;
            base.Update(gameTime);
            var rec = TruckerX.TargetRetangle;

            if (buttonLog == null)
            {
                buttonLog = new OverlayButtonWidget(this, "list");
                buttonLog.OnClick += ButtonLog_OnClick;
            }
            buttonLog.Update(this, gameTime);
            buttonLog.Position = new Vector2(rec.X + rec.Width - buttonLog.Size.X - (6 * GetRDMultiplier()), rec.Y + (topRowRefHeight * GetRDMultiplier()));

            if (openTabAnimation.Percentage != 0.0f)
                openTabAnimation.Update(gameTime);

            {
                float w = rec.Width / 3;
                float offset = (w * openTabAnimation.Percentage);
                float x = rec.X + rec.Width - offset;
                float y = rec.Y;
                float h = rec.Height;
                tabRec = new Rectangle((int)x, (int)y, (int)w, (int)h);
                Mouse.GetState().HoveringRectangle(tabRec);

                tabBorderWidth = (5 * ContentLoader.GetRDMultiplier());
            }
        }

        private void ButtonLog_OnClick(object sender, EventArgs e)
        {
            openTabAnimation.Reverse();
            openTabAnimation.Update(new GameTime(TimeSpan.Zero,TimeSpan.FromMilliseconds(1)));
            openTab = SimulationTab.Log;
        }
    }
}
