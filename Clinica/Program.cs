using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Task.Run(() => IniciarBot());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmIngreso());
        }
        static void IniciarBot()
        {
            var bot = new ServicioBot("7846844086:AAHQ986q87WCkA2N_ZzifZT7wGGJZlyMcz0");
            bot.Start();

            Console.WriteLine("Bot iniciado. Presiona Ctrl + C en la consola para detener.");
            Task.Delay(-1).Wait();
        }
    }
}
