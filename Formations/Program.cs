#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
#endregion

namespace Formations
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Formations())
            //using (var db = new PlayerContext()) 
            //{
                game.Run();
                Console.Write("New Player Database has been created.");
                //var player1 = new Player { playerName = "missmagdalene" };

                //db.Players.Add(player1);
                //db.SaveChanges();
                Console.Write("Player 1 has been added to the database.");

                
            //}          
        }
    }
#endif
}
