using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using static Android.Views.View;

namespace Flippedstudent
{
    [Activity(Label = "SignupActivity", Theme = "@style/Theme.Custom")]
    public class SignupActivity : AppCompatActivity, IOnClickListener
    {
        EditText signupName, signupEmail, signupPassword, signupMatnum;
        TextView signupDone, signuplogin;
        ProgressBar signuppgb;
        LinearLayout SignupHolder;
        Spinner signupLevel;
        ConnectivityManager connectivityManager;
        FirebaseAuth auth;
        string level;

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.signupLogin:
                    StartActivity(typeof(LoginActivity));
                    break;
                case Resource.Id.signupNext:
                    Signup();
                    break;

            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Signup);
            auth = FirebaseAuth.Instance;
            //``````````````````````````````````````````````````````````````````````
            signupName = FindViewById<EditText>(Resource.Id.signupName);
            signupEmail = FindViewById<EditText>(Resource.Id.signupEmail);
            signupPassword = FindViewById<EditText>(Resource.Id.signupPassword);
            signupMatnum = FindViewById<EditText>(Resource.Id.signupmatric);
            signupLevel = FindViewById<Spinner>(Resource.Id.signuplevel);
            signupDone = FindViewById<TextView>(Resource.Id.signupNext);
            signuplogin = FindViewById<TextView>(Resource.Id.signupLogin);
            signuplogin.SetOnClickListener(this);
            signupDone.SetOnClickListener(this);
            signupLevel.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Level, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            signupLevel.Adapter = adapter;
            //``````````````````````````````````````````````````````````````````````  


            // Create your application here
        }
        private void Signup()
        {
            if (signupEmail.Text.ToString().Trim() != "" && signupPassword.Text.ToString().Trim() != "" && signupMatnum.Text.ToString().Trim() != "" && signupName.Text.ToString().Trim() != "" && signupEmail.Text.Contains("@") && signupEmail.Text.Contains("@stu.cu.edu.ng"))
            {
                //signupHolder.Visibility = ViewStates.Gone;
                //login.Visibility = ViewStates.Gone;
                //signupProgressBar.Visibility = ViewStates.Visible;
                Intent gotocoll = new Intent(this, typeof(SignupCollegeActivity));
                gotocoll.PutExtra("level", level);
                gotocoll.PutExtra("mattnum", signupMatnum.Text.ToString());
                gotocoll.PutExtra("email", signupEmail.Text.ToString());
                gotocoll.PutExtra("name", signupName.Text.ToString());
                gotocoll.PutExtra("password", signupPassword.Text.ToString());
                StartActivity(gotocoll);
            }
            else
            {
                if (signupMatnum.Text.ToString().Trim() == "")
                {
                    signupMatnum.SetError("Required", null);
                }
                if (signupEmail.Text.ToString().Trim() == "")
                {
                    signupEmail.SetError("Required", null);

                }
                if (!signupEmail.Text.ToString().Contains("@"))
                {
                    signupEmail.SetError("Email Not Valid", null);
                }
                if (!signupEmail.Text.ToString().Contains("@stu.cu.edu.ng"))
                {
                    signupEmail.SetError("Please use your Covenant University E-mail", null);
                }
                if (signupPassword.Text.ToString().Trim() == "")
                {
                    signupPassword.SetError("Required", null);
                }
                if (signupName.Text.ToString().Trim() == "")
                {
                    signupName.SetError("Required", null);
                }
            }
        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            level = spinner.GetItemAtPosition(e.Position).ToString();
            //string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            //Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

    }
}