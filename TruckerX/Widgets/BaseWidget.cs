using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TruckerX.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Widgets
{
    public abstract class BaseWidget : IWidget
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public WidgetState State { get; set; } = WidgetState.MouseUp;
        public bool Interactive { get; set; } = true;
        public bool Disabled { get; set; } = false;

        protected BaseWidget(Vector2 position, Vector2 size, bool interactive = true)
        {
            Position = position;
            Size = size;
            Interactive = interactive;
        }

        public event EventHandler OnClick;

        public abstract void Draw(SpriteBatch batch, GameTime gameTime);

        public virtual void Update(GameTime gameTime)
        {
            if (!Interactive) return;
            if (Disabled)
            {
                State = WidgetState.Disabled;
                return;
            }
            var state = Mouse.GetState();

            if (state.Clicked(this))
            {
                OnClick?.Invoke(this, null);
            }
        }
    }
}
