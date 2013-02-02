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
    public class UIManager : DrawableGameComponent
    {
        ScoreData score;
        SpriteBatch spriteBatch;

        SpriteFont font;

        IRoundManager roundMgr;

        public UIManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            score = (ScoreData)Game.Services.GetService(typeof(ScoreData));

            font = Game.Content.Load<SpriteFont>("font");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if(roundMgr == null)
                roundMgr = (IRoundManager)Game.Services.GetService(typeof(IRoundManager));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font,string.Format("score: {0}", score.Score), Vector2.Zero, Color.White);

            spriteBatch.DrawString(font, string.Format("time: {0}s", Math.Round(roundMgr.TimeOnRound / 1000f, 1)), new Vector2(0, Game.Window.ClientBounds.Height - 14), Color.White);

            string wave = string.Format("wave: {0}s", Math.Round((roundMgr.Current.WaveInterval) - (roundMgr.NextWaveTimer / 1000f), 1));
            spriteBatch.DrawString(font, wave, new Vector2(Game.Window.ClientBounds.Width - font.MeasureString(wave).X, 0), Color.White);

            string rem = string.Format("remain: {0}", roundMgr.RemainingParticipants);
            spriteBatch.DrawString(font, rem, new Vector2(Game.Window.ClientBounds.Width - font.MeasureString(rem).X, Game.Window.ClientBounds.Height - 14), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
