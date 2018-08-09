using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Text;
using System.Linq;
using TelnetCommunication;
using ClientTelentCommucication;
using System.Timers;
using System.Collections.Generic;
using MessageParser;
using Android.Util;
using static Shared.TagWrapper.TagWrapper;
using static Android.Widget.TextView;
using Android.Content.PM;

namespace AndroidClient
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private TelnetHandler _telnetHandler;
        private EditText _inputText;
        private TextView _displayText;
        private RelativeLayout _relativeLayout;
        private Timer _timer;
        private Timer _timer2;
        private List<ParsedMessage> parsedMessagesCache = new List<ParsedMessage>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _relativeLayout = FindViewById<RelativeLayout>(Resource.Id.ScreenLayout);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            _displayText = FindViewById<TextView>(Resource.Id.DisplayText);
            _displayText.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();

            _inputText = FindViewById<EditText>(Resource.Id.InputText);
            _inputText.TextChanged += InputChanged;

            _telnetHandler = new ClientHandler(Settings.Address, Settings.Port, new JsonMudMessage());

            _timer = new Timer();
            _timer.Interval = 100;
            _timer.Elapsed += UpdateDisplayedText;
            _timer.Start();

            _timer2 = new Timer();
            _timer2.Interval = 10;
            _timer2.Elapsed += _timer2_Elapsed;
            _timer2.Start();
        }

        private void _timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateUISize();
            _timer2.Stop();
        }

        private void UpdateUISize()
        {
            Display display = WindowManager.DefaultDisplay;
            DisplayMetrics metrics = new DisplayMetrics();
            display.GetMetrics(metrics);

            int relativeLayoutHeight = _relativeLayout.Height;
            int _inputTextHeight = _inputText.Height;

            RunOnUiThread(() =>
            {
                _displayText.Bottom = relativeLayoutHeight - _inputTextHeight;

                _inputText.Top = relativeLayoutHeight - _inputTextHeight;
                _inputText.Bottom = relativeLayoutHeight;
            });
        }

        private void UpdateDisplayedText(object sender, ElapsedEventArgs e)
        {
            string message;
            while (_telnetHandler.InQueue.TryDequeue(out message))
            {
                if (message.StartsWith("<Sound>"))
                {
                    //_soundHandler?.HandleSounds(message);
                }
                else if (message.StartsWith("<Map>"))
                {
                    //if (_mapWindow != null && !_mapWindow.IsDisposed)
                    //{
                    //    _mapWindow.Update(message);
                    //}
                }
                else if (message.StartsWith("<Data>"))
                {
                    //SaveFile(message);
                }
                else if (message.StartsWith("<FileValidation>"))
                {
                    //ValidateAssets.Validate(message);
                }
                else
                {
                    List<ParsedMessage> parsedMessages = Parser.Parse(message);

                    lock (parsedMessages)
                    {
                        parsedMessagesCache.AddRange(parsedMessages);
                        while (parsedMessagesCache.Count > 50)
                        {
                            parsedMessagesCache.RemoveAt(0);
                        }
                    }

                    SpannableString spannableString = DisplayFormatter.FormatText(parsedMessagesCache);

                    RunOnUiThread(() =>
                    {
                        _displayText.SetText(spannableString, BufferType.Spannable);


                        //this allows the screen to update
                        _displayText.Append("\n");
                    });
                }
            }
        }

        private void InputChanged(object sender, TextChangedEventArgs e)
        {
            string text = e.Text.ToString();
            if (text.Contains((char)10))
            {
                _telnetHandler.OutQueue.Enqueue(text.Trim());
                _inputText.Text = "";
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
    }
}