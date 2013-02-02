using System;

namespace Fracture
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FractureGame game = new FractureGame())
            {
                game.Run();
            }
        }
    }
#endif
}

