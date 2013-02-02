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
    public class RoundManager : GameComponent, IRoundManager
    {
        public Round Current { get; protected set; }

        public int NextWaveTimer { get; protected set; }
        public int RemainingParticipants { get; set; }

        List<Vector2> spawnLocations;

        List<Escapee> currentEscapees;
        public int ParticipantsInPlay { get { return currentEscapees.Count; } }

        IEscapeeManager escapeeManager;

        Random rand;

        public int TimeOnRound { get; protected set; }

        public RoundManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            currentEscapees = new List<Escapee>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            escapeeManager = (IEscapeeManager)Game.Services.GetService(typeof(IEscapeeManager));
            spawnLocations = ((ITilemapRenderer)Game.Services.GetService(typeof(ITilemapRenderer))).GetEscapeeSpawns();
            Game.Services.AddService(typeof(IRoundManager), this);

            rand = new Random();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            TimeOnRound += gameTime.ElapsedGameTime.Milliseconds;

            if (RemainingParticipants > 0)
            {
                NextWaveTimer += gameTime.ElapsedGameTime.Milliseconds;

                if ((NextWaveTimer / 1000f) >= Current.WaveInterval)
                {
                    Escapee e;

                    foreach (Vector2 spawn in spawnLocations)
                    {
                        e = new Escapee { Position = spawn, Alive = true, Free = false, Tint = Escapee.Tints[rand.Next(0, 3)] };
                        escapeeManager.AddEscapee(e);
                        currentEscapees.Add(e);

                        RemainingParticipants--;

                        if (RemainingParticipants == 0)
                            break;
                    }

                    NextWaveTimer = 0;
                }
            }

            currentEscapees.RemoveAll(new Predicate<Escapee>((x) => !x.InPlay));

            if (ParticipantsInPlay == 0 && RemainingParticipants == 0 && Next != null)
            {
                Current = Next;
                next = null;
                NextWaveTimer = 0;
                RemainingParticipants = Current.ParticipantCount;

                if (RoundChanged != null)
                    RoundChanged(this, null);
            }

            base.Update(gameTime);
        }

        Round next;
        public Round Next
        {
            get { return next; }
            set { next = value; }
        }

        public event EventHandler RoundChanged;
    }
}
