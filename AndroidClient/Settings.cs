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
    public static class Settings
    {
        private static ISharedPreferences _sharedPreferences = Application.Context.GetSharedPreferences("client", FileCreationMode.Private);

        public static string Address
        {
            get
            {
                return _sharedPreferences.GetString("Address", "");
            }
            set
            {
                ISharedPreferencesEditor sharedPreferencesEditor = _sharedPreferences.Edit();
                sharedPreferencesEditor.PutString("Address", value);
                sharedPreferencesEditor.Commit();
            }
        }

        public static int Port
        {
            get
            {
                return _sharedPreferences.GetInt("Port", 0);
            }
            set
            {
                ISharedPreferencesEditor sharedPreferencesEditor = _sharedPreferences.Edit();
                sharedPreferencesEditor.PutInt("Port", value);
                sharedPreferencesEditor.Commit();
            }
        }
    }
}