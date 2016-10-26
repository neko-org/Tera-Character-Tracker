﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using TCTSniffer;

namespace TCTMain
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {

        public class Threads
        {
            public static void NetThread()
            {
                TCTSniffer.SnifferProgram.startNewSniffingSession();

            }
            public static void UIThread()
            {
                try
                {

                    Tera.TeraMainWindow w = new Tera.TeraMainWindow();
                    w.InitializeComponent();

                    Tera.TeraLogic.TryReset();
                    
                    w.ShowDialog();

                    Tera.TeraLogic.SaveSettings();
                    
                    Environment.Exit(0);


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                }
            }
            public static void LoadDatabases()
            {
                Tera.TeraLogic.LoadTeraDB();
                Tera.TeraLogic.LoadAccounts();
                Tera.TeraLogic.LoadCharacters();
                Tera.TeraLogic.LoadDungeons();

                if (Tera.TeraLogic.CharList != null && Tera.TeraLogic.DungList != null)
                {
                    Tera.TeraLogic.CheckDungeonsList(); 
                }

                Tera.TeraLogic.LoadGuildsDB();

                if (!Tera.TeraLogic.GuildDictionary.ContainsKey(0))
                {
                    Tera.TeraLogic.GuildDictionary.Add(0, "No guild");
                }

            }
        }
            


        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [STAThread]
        public static void Main()
        {
            /*load settings*/
            Tera.TeraLogic.LoadSettings();
            Tera.TeraLogic.ResetCheck();


            Thread uiThread = new Thread(new ThreadStart(Threads.UIThread));
            Thread netThread = new Thread(new ThreadStart(Threads.NetThread));

            uiThread.SetApartmentState(ApartmentState.STA);
            netThread.SetApartmentState(ApartmentState.STA);
            Threads.LoadDatabases();
            uiThread.Start();
            netThread.Start();

        }


    }

}
