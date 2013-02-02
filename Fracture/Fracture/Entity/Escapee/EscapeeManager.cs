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
    public class EscapeeManager : DrawableGameComponent, IEscapeeManager
    {
        public List<Escapee> Escapees { get; set; }

        ITilemapRenderer tmapRenderer;
        ICollisionManager collisions;
        ScoreData score;
        Dictionary<string, SoundEffect> sfx;

        public EscapeeManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            Escapees = new List<Escapee>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            tmapRenderer = (ITilemapRenderer)Game.Services.GetService(typeof(ITilemapRenderer));
            score = (ScoreData)Game.Services.GetService(typeof(ScoreData));
            collisions = (ICollisionManager)Game.Services.GetService(typeof(ICollisionManager));
            sfx = (Dictionary<string, SoundEffect>)Game.Services.GetService(typeof(Dictionary<string, SoundEffect>));

            Game.Services.AddService(typeof(IEscapeeManager), this);


            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Vector2 dist;
            Vector2 direction;

            foreach (Escapee e in Escapees)
            {
                if (e.Alive)
                {
                    dist = e.Position - tmapRenderer.EscapeDoor;

                    if (e.Hitbox.Intersects(tmapRenderer.EscapeDoorHitbox))
                    {
                        score.AddFreeEscapee();
                        e.Free = true;
                        sfx["door"].Play();
                    }

                    direction = dist.X > 0 ? -Vector2.One : Vector2.One;

                    if (dist.Length() > 4f)
                        e.PendingMovement += new Vector2(4, 0) * direction;
                    else
                        e.PendingMovement += new Vector2(dist.Length(), 0) * direction;
                }
                else
                {
                    if(e.DeathTimer == 0)
                        collisions.RemoveEntity(e);

                    e.DeathTimer += gameTime.ElapsedGameTime.Milliseconds;
                }

                e.EnabledGraphic.Update(gameTime);

            }

            Escapees.RemoveAll(new Predicate<Escapee>((x) => x.Free || (!x.Alive && x.DeathTimer >= 3*75)));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Escapee escapee in Escapees)
            {
                escapee.EnabledGraphic.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        private void Escapee_Collide(object sender, CollisionEventArgs e)
        {
            if (e.KillTile)
            {
                Escapee s = ((Escapee)sender);
                if (s.Alive)
                {
                    s.Alive = false;
                    s.SwitchGraphic("die");
                    sfx["explode"].Play();
                }
            }
        }

        public void AddEscapee(Escapee e)
        {
            Escapees.Add(e);
            e.SetupGraphics(Game);
            e.SwitchGraphic("run");
            e.Collided += new EventHandler<CollisionEventArgs>(Escapee_Collide);
            collisions.AddEntity(e);
        }
    }
}
