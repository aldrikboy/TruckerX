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

        public WorldLocationWidget(BaseScene scene, BasePlace place) : base()
        {
            bg = ContentLoader.GetTexture("menu-button");
            Place = place;
            owned = WorldState.PlaceOwned(place);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            Color c = owned ? Color.Red : Color.Black;
            if (this.State == WidgetState.MouseHover || this.State == WidgetState.MouseDown) c = Color.White;
            MonoGame.Primitives2D.DrawCircle(batch, this.Position + new Vector2(this.Size.X / 2, this.Size.Y / 2), 
                this.Size.X / 2, 32, c, this.Size.X / 2);
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            base.Update(scene, gameTime);
        }
    }
}
