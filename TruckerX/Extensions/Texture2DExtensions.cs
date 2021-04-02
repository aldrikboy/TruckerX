﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Extensions
{
    public static class Texture2DExtensions
    {
        public static Vector2 ScaleToWindow(this Texture2D texture, float percentage)
        {
            if (texture != null)
            {
                if (texture.Width <= texture.Height)
                {
                    return ScaleToWindowHeight(texture, percentage);
                }
                else
                {
                    return ScaleToWindowWidth(texture, percentage);
                }
            }
            throw new Exception("Texture is null");
        }

        public static Vector2 ScaleToWindowHeight(this Texture2D texture, float percentage)
        {
            if (texture != null)
            {
                float newHeight = TruckerX.WindowHeight * percentage;
                float ratio = texture.Height / newHeight;
                float newWidth = texture.Width / ratio;

                return new Vector2(newWidth, newHeight);
            }
            throw new Exception("Texture is null");
        }

        public static Vector2 ScaleToWindowWidth(this Texture2D texture, float percentage)
        {
            if (texture != null)
            {
                float newWidth = TruckerX.WindowWidth * percentage;
                float ratio = texture.Height / newWidth;
                float newHeight = texture.Height / ratio;

                return new Vector2(newWidth, newHeight);
            }
            throw new Exception("Texture is null");
        }

        public static Vector2 ScaleToWidth(this Texture2D texture, float width, float percentage)
        {
            if (texture != null)
            {
                float newWidth = width * percentage;
                float ratio = texture.Height / newWidth;
                float newHeight = texture.Height / ratio;

                return new Vector2(newWidth, newHeight);
            }
            throw new Exception("Texture is null");
        }

        public static Vector2 ScaleToHeight(this Texture2D texture, float height, float percentage)
        {
            if (texture != null)
            {
                float newHeight = height * percentage;
                float ratio = texture.Height / newHeight;
                float newWidth = texture.Width / ratio;

                return new Vector2(newWidth, newHeight);
            }
            throw new Exception("Texture is null");
        }
    }
}
