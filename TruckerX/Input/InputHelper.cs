using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Input
{
    public static class InputHelper
    {
        static int lastScrollValue = 0;
        public static int ScrollValue()
        {
            var state = Mouse.GetState();
            var result = 0;
            if (state.ScrollWheelValue > lastScrollValue) result = 1;
            if (state.ScrollWheelValue < lastScrollValue) result = -1;
            lastScrollValue = state.ScrollWheelValue;
            return result;
        }
    }
}
