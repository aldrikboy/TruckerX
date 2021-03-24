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
using MonoGame;

namespace TruckerX.Widgets
{
    class EmployeeFinderWidget : BaseWidget
    {
        public EmployeeFinderWidget(BaseScene scene) : base(Vector2.Zero, Vector2.Zero)
        {
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            Primitives2D.FillRectangle(batch, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.White);
        }
    }
}
