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
    /// This is the main type for your game
    /// </summary>
    public class FractureGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TilemapRenderer tmapRenderer;
        Tilemap map;
        Tileset set;

        PlayerManager playerManager;
        Player player;

        EscapeeManager escapeeManager;

        ScoreData score;

        RoundManager roundManager;
        Round round;

        CollisionManager collisionManager;

        GameCamera camera;

        UIManager uiManager;

        Dictionary<string, SoundEffect> sfx;

        public FractureGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Services.AddService(typeof(SpriteBatch), spriteBatch);

            score = new ScoreData();
            Services.AddService(typeof(ScoreData), score);

            camera = new GameCamera { X = 0, Y = 0, Width = Window.ClientBounds.Width, Height = Window.ClientBounds.Height };
            Services.AddService(typeof(GameCamera), camera);

            collisionManager = new CollisionManager(this,24,24);
            Components.Add(collisionManager);

            tmapRenderer = new TilemapRenderer(this);
            Components.Add(tmapRenderer);

            map = Tilemap.Load("testmap.tmx");
            tmapRenderer.Map = map;

            playerManager = new PlayerManager(this);
            Components.Add(playerManager);

            player = new Player();
            playerManager.Player = player;

            escapeeManager = new EscapeeManager(this);
            Components.Add(escapeeManager);

            sfx = new Dictionary<string, SoundEffect>();
            Services.AddService(typeof(Dictionary<string, SoundEffect>), sfx);

            round = new Round { ParticipantCount = 9, WaveInterval = 10 };

            uiManager = new UIManager(this);
            Components.Add(uiManager);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            set = new Tileset(Content.Load<Texture2D>("tileset"), 24, 24);
            tmapRenderer.Tileset = set;

            roundManager = new RoundManager(this);
            roundManager.Initialize();
            roundManager.Next = round;
            Components.Add(roundManager);

            player.Position = tmapRenderer.GetPlayerSpawn();

            sfx.Add("door", Content.Load<SoundEffect>("door"));
            sfx.Add("explode", Content.Load<SoundEffect>("explode"));
            sfx.Add("jump", Content.Load<SoundEffect>("jump"));
            sfx.Add("fall", Content.Load<SoundEffect>("fall"));
            sfx.Add("clear", Content.Load<SoundEffect>("stage-clear"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
