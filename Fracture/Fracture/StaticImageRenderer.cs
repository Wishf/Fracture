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
    public class StaticImageRenderer : DrawableGameComponent, IImageRenderer
    {
        public PositionProviderDelegate PositionProvider { get; set; }
        public ColourProviderDelegate TintProvider { get; set; }
        public FlipDataProviderDelegate FlipProvider { get; set; }

        public Texture2D Image { get; protected set; }

        SpriteBatch spriteBatch;
        GameCamera camera;

        public StaticImageRenderer(Game game, string image)
            : base(game)
        {
            Image = game.Content.Load<Texture2D>(image);
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
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Image, camera.DrawOffset + PositionProvider(), null, TintProvider(), 0f, Vector2.Zero, 1f, FlipProvider(), 0f);

            spriteBatch.End();

            base.Draw(gameTime);
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
