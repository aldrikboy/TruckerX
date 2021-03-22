using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.State;

namespace TruckerX.Widgets
{
    public class WorldLocationWidget : BaseWidget
    {
        private Texture2D bg;

        public BasePlace Place { get; set; }
        public bool owned = false;

        public WorldLocationWidget(BaseScene scene, BasePlace place) : base(new Vector2(0,0), new Vector2(10, 10))
        {
            bg = scene.GetTexture("menu-button");
            Place = place;
            owned = WorldState.PlaceOwned(place);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (this.State == WidgetState.MouseHover)
            {
                batch.Draw(bg, new Rectangle((this.Position).ToPoint(), (this.Size).ToPoint()), Color.White);
            }
            else if (this.State == WidgetState.MouseDown)
            {
                batch.Draw(bg, new Rectangle((this.Position).ToPoint(), (this.Size).ToPoint()), Color.White);
            }
            else
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), owned ? Color.Red : Color.Black);
            }
        }
    }
}
