using Microsoft.Xna.Framework.Input;
using TruckerX.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Extensions
{
    public static class MouseStateExtensions
    {
        private static bool wasDown = false;

        private static bool Hovering(this MouseState state, IWidget widget)
        {
            var scissor = TruckerX.Game.GraphicsDevice.ScissorRectangle;
            var result = (state.X >= widget.Position.X && state.X <= widget.Position.X + widget.Size.X) &&
                (state.Y >= widget.Position.Y && state.Y <= widget.Position.Y + widget.Size.Y)
                &&
                (state.X >= scissor.X && state.X <= scissor.X + scissor.Size.X) &&
                (state.Y >= scissor.Y && state.Y <= scissor.Y + scissor.Size.Y);
            if (result) widget.State = WidgetState.MouseHover;
            else widget.State = WidgetState.Idle;
            return result;
        }

        public static bool Clicked(this MouseState state, IWidget widget)
        {
            if (state.LeftButton == ButtonState.Released)
            {
                wasDown = false;
                widget.State = WidgetState.MouseUp;
            }
            if (wasDown) return false;
            var result = Hovering(state, widget) && state.LeftButton == ButtonState.Pressed && widget.State == WidgetState.MouseHover;
            if (result)
            {
                widget.State = WidgetState.MouseDown;
                wasDown = true;
            }
            return result;
        }
    }
}
