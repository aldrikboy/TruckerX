using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Scenes;

namespace TruckerX.Widgets
{
    public enum WidgetState
    {
        MouseDown,
        MouseUp,
        MouseHover,
        Idle,
        Disabled,
    }

    public interface IWidget
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public WidgetState State { get; set; }

        void Update(BaseScene scene, GameTime gameTime);
        void Draw(SpriteBatch batch, GameTime gameTime);
    }
}
