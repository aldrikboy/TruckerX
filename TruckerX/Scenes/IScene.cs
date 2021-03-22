using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Scenes
{
    public interface IScene
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch batch, GameTime gameTime);
    }
}
