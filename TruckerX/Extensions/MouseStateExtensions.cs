using Microsoft.Xna.Framework.Input;
using TruckerX.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TruckerX.Extensions
{
    public static class MouseStateExtensions
    {
        private static bool wasDown = false;
        public static bool MouseUsedThisFrame { get; set; } = false;

        public static bool HoveringRectangle(this MouseState state, Rectangle rec)
        {
            if (MouseUsedThisFrame) return false;
            var scissor = TruckerX.Game.GraphicsDevice.ScissorRectangle;
            var result = (state.X >= rec.X && state.X <= rec.X + rec.Width) &&
                (state.Y >= rec.Y && state.Y <= rec.Y + rec.Height)
                &&
                (state.X >= scissor.X && state.X <= scissor.X + scissor.Size.X) &&
                (state.Y >= scissor.Y && state.Y <= scissor.Y + scissor.Size.Y);
            if (result) { MouseUsedThisFrame = true; }
            return result;
        }

        public static bool NotHovering(this MouseState state, IWidget widget)
        {
            if (MouseUsedThisFrame) return false;
            var scissor = TruckerX.Game.GraphicsDevice.ScissorRectangle;
            var result = (state.X >= widget.Position.X && state.X <= widget.Position.X + widget.Size.X) &&
                (state.Y >= widget.Position.Y && state.Y <= widget.Position.Y + widget.Size.Y)
                &&
                (state.X >= scissor.X && state.X <= scissor.X + scissor.Size.X) &&
                (state.Y >= scissor.Y && state.Y <= scissor.Y + scissor.Size.Y);
            if (result) { /* Dont invalidate mouse here*/ widget.State = WidgetState.MouseHover; }
            else widget.State = WidgetState.Idle;
            return !result;
        }

        public static bool Hovering(this MouseState state, IWidget widget)
        {
            if (MouseUsedThisFrame) return false;
            var scissor = TruckerX.Game.GraphicsDevice.ScissorRectangle;
            var result = (state.X >= widget.Position.X && state.X <= widget.Position.X + widget.Size.X) &&
                (state.Y >= widget.Position.Y && state.Y <= widget.Position.Y + widget.Size.Y)
                &&
                (state.X >= scissor.X && state.X <= scissor.X + scissor.Size.X) &&
                (state.Y >= scissor.Y && state.Y <= scissor.Y + scissor.Size.Y);
            if (result) { MouseUsedThisFrame = true; widget.State = WidgetState.MouseHover; }
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
            if (MouseUsedThisFrame) return false;
            var result = Hovering(state, widget) && state.LeftButton == ButtonState.Pressed && widget.State == WidgetState.MouseHover;
            if (result)
            {
                MouseUsedThisFrame = true;
                widget.State = WidgetState.MouseDown;
                wasDown = true;
            }
            return result;
        }

        public static bool Clicked(this MouseState state, float x, float y, float w, float h)
        {
            if (state.LeftButton == ButtonState.Released)
            {
                wasDown = false;
            }
            if (wasDown) return false;
            if (MouseUsedThisFrame) return false;
            var result = state.X >=x  && state.Y >= y && state.X <= x + w && state.Y <= y + h && state.LeftButton == ButtonState.Pressed;
            if (result)
            {
                wasDown = true;
                MouseUsedThisFrame = true;
            }
            return result;
        }
    }
}
