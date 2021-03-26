using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.State;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace TruckerX.Widgets
{
    public class WorldLocationWidget : BaseWidget
    {
        private Texture2D bg;
        private SoundEffect clickEffect;
        public BasePlace Place { get; set; }
        public bool owned = false;

        public WorldLocationWidget(BaseScene scene, BasePlace place) : base()
        {
            bg = ContentLoader.GetTexture("menu-button");
            Place = place;
            owned = WorldState.PlaceOwned(place);
            clickEffect = ContentLoader.GetSample("pop2");
            this.OnClick += WorldLocationWidget_OnClick;
        }

        private void WorldLocationWidget_OnClick(object sender, EventArgs e)
        {
            clickEffect.Play(0.15f, 0.0f, 0.0f);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            Color c = owned ? Color.Red : Color.Black;
            if (this.State == WidgetState.MouseHover || this.State == WidgetState.MouseDown)
            {
                c = Color.White;
            }
            MonoGame.Primitives2D.DrawCircle(batch, this.Position + new Vector2(this.Size.X / 2, this.Size.Y / 2), 
                this.Size.X / 2, 32, c, this.Size.X / 2);
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            base.Update(scene, gameTime);
            if (this.State == WidgetState.MouseHover) Mouse.SetCursor(MouseCursor.Hand);
        }
    }
}
