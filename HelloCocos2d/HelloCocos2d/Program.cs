using System;
#if ANDROID
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
#endif
using Microsoft.Xna.Framework;

namespace tests
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
#if ANDROID
    [Activity(Label = "Hello Cocos2d",
               MainLauncher = true,
               Icon = "@drawable/ic_launcher",
               Theme = "@style/Theme.NoTitleBar",
               ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
               LaunchMode = Android.Content.PM.LaunchMode.SingleInstance,
               ConfigurationChanges = Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class TestsActivity : AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create our OpenGL view, and display it
            Game1.Activity = this;
            var g = new Game1();
            SetContentView(g.Window);
            g.Run();
        }

    }

#endif
}

