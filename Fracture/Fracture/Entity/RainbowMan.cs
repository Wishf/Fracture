using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fracture
{
    public abstract class RainbowMan : Entity
    {
        public bool Alive { get; set; }
        public bool Free { get; set; }
        public bool LeftwardFacing { get; set; }
        public bool InPlay { get { return Alive && !Free; } }

        public Color Tint { get; set; }

        public Dictionary<string, IImageRenderer> Graphics { get; protected set; }
        public IImageRenderer EnabledGraphic { get; protected set; }

        public void SwitchGraphic(string name)
        {
            EnabledGraphic.Disable();

            EnabledGraphic = Graphics[name];
            EnabledGraphic.Enable();
        }

        // horrible delegate abuse
        private Vector2 PositionProvider() { return new Vector2((float)Math.Round(Position.X, 0), (float)Math.Round(Position.Y, 0)); }
        private Color TintProvider() { return Tint; }
        private SpriteEffects FlipProvider() { return LeftwardFacing ? SpriteEffects.FlipHorizontally : SpriteEffects.None; }

        private Color DeathTintProvider() { return Color.White; }

        public void SetupGraphics(Game game)
        {
            Graphics = new Dictionary<string, IImageRenderer>();

            // idle
            Graphics.Add("idle", new StaticImageRenderer(game, "rainbowmen") { PositionProvider = PositionProvider, TintProvider = TintProvider, FlipProvider = FlipProvider });

            // running
            Graphics.Add("run", new AnimationRenderer(game, "rainbow-runners", 15, 150) { PositionProvider = PositionProvider, TintProvider = TintProvider, FlipProvider = FlipProvider }); // 150ms frametime

            // jump
            Graphics.Add("jump", new StaticImageRenderer(game, "rainbow-jump") { PositionProvider = PositionProvider, TintProvider = TintProvider, FlipProvider = FlipProvider });

            // death
            Graphics.Add("die", new AnimationRenderer(game, "explode-anim", 24, 75) { PositionProvider = PositionProvider, TintProvider = DeathTintProvider, FlipProvider = FlipProvider });

            foreach (GameComponent gc in Graphics.Values)
                gc.Initialize();

            EnabledGraphic = Graphics["idle"];
            EnabledGraphic.Enable();
        }
    }
}
