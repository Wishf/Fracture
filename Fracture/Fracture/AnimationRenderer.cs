using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Fracture
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AnimationRenderer : DrawableGameComponent, IImageRenderer
    {
        public int FrameCount { get; protected set; }
        public int FrameTime { get; protected set; }
        public Texture2D Sheet { get; protected set; }
        public int FrameWidth { get; protected set; }

        public int CurrentFrame { get; protected set; }

        int timer;

        SpriteBatch spriteBatch;
        GameCamera camera;

        Rectangle frameRect;

        public PositionProviderDelegate PositionProvider { get; set; }
        public ColourProviderDelegate TintProvider { get; set; }
        public FlipDataProviderDelegate FlipProvider { get; set; }

        public AnimationRenderer(Game game, string sheet, int fWidth, int fTime)
            : base(game)
        {
            Sheet = game.Content.Load<Texture2D>(sheet);
            FrameWidth = fWidth;
            FrameTime = fTime;

            FrameCount = Sheet.Width / fWidth;

            frameRect = new Rectangle(0, 0, fWidth, Sheet.Height);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            camera = (GameCamera)Game.Services.GetService(typeof(GameCamera));

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (timer >= FrameTime)
            {
                timer = 0;
                CurrentFrame = (CurrentFrame + 1) % FrameCount;
                frameRect.X = (CurrentFrame * FrameWidth);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Sheet, camera.DrawOffset + PositionProvider(), frameRect, TintProvider(), 0f, Vector2.Zero, 1f, FlipProvider(), 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Reset()
        {
            CurrentFrame = 0;
            timer = 0;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void Enable()
        {
            Enabled = true;
        }
    }
}
