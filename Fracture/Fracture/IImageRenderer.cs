using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fracture
{
    public delegate Vector2 PositionProviderDelegate();
    public delegate Color ColourProviderDelegate();
    public delegate SpriteEffects FlipDataProviderDelegate();

    public interface IImageRenderer
    {
        PositionProviderDelegate PositionProvider { get; set; }
        ColourProviderDelegate TintProvider { get; set; }
        FlipDataProviderDelegate FlipProvider { get; set; }

        void Disable();
        void Enable();

        void Draw(GameTime gt);
        void Update(GameTime gt);
    }
}
