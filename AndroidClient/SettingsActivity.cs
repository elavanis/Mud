using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidClient
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class SettingsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings);

            TextView button = FindViewById<TextView>(Resource.Id.bs);

            if (button == null)
            {
                Toast.MakeText(ApplicationContext, "null", ToastLength.Long).Show();
            }
            else
            {
                button.Click += Button_Click;
            }

            if (Settings.Address != string.Empty)
            {
                StartActivity(typeof(MainActivity));
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Settings.Address = FindViewById<EditText>(Resource.Id.address).Text;

            int portNumber = 0;
            int.TryParse(FindViewById<EditText>(Resource.Id.port).Text, out portNumber);
            Settings.Port = portNumber;

            StartActivity(typeof(MainActivity));
        }
    }
}