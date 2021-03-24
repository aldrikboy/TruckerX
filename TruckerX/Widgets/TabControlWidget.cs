using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.State;
using TruckerX.Extensions;

namespace TruckerX.Widgets
{
    public class TabControlItemWidget : BaseWidget
    {
        private Texture2D bg;
        public string Title { get; set; }
        public object Data { get; set; }
        private BaseScene scene;

        public TabControlItemWidget(BaseScene scene, string title, object data) : base()
        {
            bg = scene.GetTexture("tab-background");
            Title = title;
            Data = data;
            this.scene = scene;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.White);

            if (this.State == WidgetState.MouseHover)
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(0, 0, 0, 50));
            }
            else if (this.State == WidgetState.MouseDown)
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(0, 0, 0, 150));
            }

            {
                var font = scene.GetRDFont("main_font_12");
                var strSize = font.MeasureString(Title);
                float x = this.Position.X + (this.Size.X / 2) - (strSize.X / 2);
                float y = this.Position.Y + (this.Size.Y / 2) - (strSize.Y / 2);
                batch.DrawString(font, Title, new Vector2(x + 1, y + 1), Color.Gray);
                batch.DrawString(font, Title, new Vector2(x, y), Color.White);
            }
        }
    }

    public class TabControlWidget : BaseWidget
    {
        
        private List<TabControlItemWidget> items;
        private BaseScene scene;

        public TabControlWidget(BaseScene scene, List<TabControlItemWidget> items) : base(false)
        {
            this.scene = scene;
            this.items = items;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            foreach(var item in items)
            {
                item.Draw(batch, gameTime);
            }
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            base.Update(scene, gameTime);

            float widthPerItem = this.Size.X / items.Count;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.Size = new Vector2(widthPerItem, 26 * scene.GetRDMultiplier());
                item.Position = new Vector2(this.Position.X + ((item.Size.X) * i), this.Position.Y);
                item.Update(scene, gameTime);
            }
        }
    }
}
