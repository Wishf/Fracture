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
    public class PlayerManager : DrawableGameComponent
    {
        Player p;
        public Player Player
        {
            get { return p; }
            set
            {
                p = value; 
                p.Collided += new EventHandler<CollisionEventArgs>(Player_Collide);
            }
        }

        GameCamera camera;

        ICollisionManager collisions;
        ITilemapRenderer tmapRenderer;
        Dictionary<string, SoundEffect> sfx;

        int deathTimer;

        public PlayerManager(Game game)
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
            collisions = (ICollisionManager)Game.Services.GetService(typeof(ICollisionManager));
            camera = (GameCamera)Game.Services.GetService(typeof(GameCamera));
            tmapRenderer = (ITilemapRenderer)Game.Services.GetService(typeof(ITilemapRenderer));
            sfx = (Dictionary<string, SoundEffect>)Game.Services.GetService(typeof(Dictionary<string, SoundEffect>));

            p.SetupGraphics(Game);

            collisions.AddEntity(p);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            Vector2 positionDelta = new Vector2();

            if (p != null && p.Alive)
            {
                if (kState.IsKeyDown(Keys.W)) // jump
                {
                }

                if (kState.IsKeyDown(Keys.A)) // move left
                {
                    positionDelta.X -= 4;
                }
                else if (kState.IsKeyDown(Keys.D)) // move right
                {
                    positionDelta.X += 4;
                }


                if (positionDelta.Length() > 0)
                    p.LeftwardFacing = positionDelta.X < 0;

                p.PendingMovement += positionDelta;

                camera.Focus = p.Position;

                if (positionDelta.Length() > 0 && p.EnabledGraphic != p.Graphics["run"])
                {
                    p.SwitchGraphic("run");
                }
                else if (positionDelta.Length() == 0 && p.EnabledGraphic != p.Graphics["idle"])
                {
                    p.SwitchGraphic("idle");
                }

                if (p.Hitbox.Intersects(tmapRenderer.EscapeDoorHitbox) && !p.Free)
                {
                    p.Free = true;
                    sfx["clear"].Play();

                    collisions.RemoveEntity(p);
                    p = null;

                    // display clear
                }
            }
            else
            {
                if(deathTimer == 0)
                    collisions.RemoveEntity(p);

                deathTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (deathTimer >= 3*75)
                {
                    p = null;
                }
            }

            if (p != null)
                p.EnabledGraphic.Update(gameTime);

            base.Update(gameTime);
        }

        private void Player_Collide(object sender, CollisionEventArgs e)
        {
            if (e.KillTile)
            {
                if (p.Alive)
                {
                    p.Alive = false;
                    p.SwitchGraphic("die");
                    sfx["explode"].Play();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if(p != null)
                p.EnabledGraphic.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
