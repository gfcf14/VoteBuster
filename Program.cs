using System;
using System.Windows.Forms;

namespace VoteBuster
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                using (VoteBuster game = new VoteBuster())
                {
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
#endif
}

