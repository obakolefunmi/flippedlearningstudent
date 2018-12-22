using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Balysv.Material.Drawable.Menu;
using Firebase.Auth;
using Flippedstudent.Class;
using Flippedstudent.Helper;
using Newtonsoft.Json;

namespace Flippedstudent
{
    [Activity(Label = "Ask Question", Theme = "@style/Theme.Custom")]
    public class AskQuestionActivity : AppCompatActivity
    {
        LinearLayout askHolder;
        FloatingActionButton askfab;
        EditText askedit;
        ProgressBar askpgb;
        FirebaseAuth auth;
        string student, course, lecture;
        enum stroke
        {
            REGULAR = 3,

            THIN = 2,
            EXTRA_THIN = 1


        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AskQuestion);
            var toolbardellecture = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarask);
            SetSupportActionBar(toolbardellecture);
            MaterialMenuDrawable materialMenu = new MaterialMenuDrawable(this, Color.Purple, (int)stroke.EXTRA_THIN, MaterialMenuDrawable.DefaultScale, MaterialMenuDrawable.DefaultTransformDuration);
            materialMenu.SetIconState(MaterialMenuDrawable.IconState.Arrow);
            toolbardellecture.NavigationIcon = materialMenu;
            toolbardellecture.NavigationClick += delegate {
                OnBackPressed();
                Finish();
            };
            auth = FirebaseAuth.Instance;
            askedit = FindViewById<EditText>(Resource.Id.askedit);
            askfab = FindViewById<FloatingActionButton>(Resource.Id.askfab);
            askHolder = FindViewById<LinearLayout>(Resource.Id.askHolder);
            askpgb = FindViewById<ProgressBar>(Resource.Id.askpgb);
            course = Intent.GetStringExtra("course") ?? "";
            lecture = Intent.GetStringExtra("title") ?? "";
            student = auth.CurrentUser.Email.ToString();
            askfab.Click += delegate {
                if (askedit.Text.Trim().ToString() == "")
                { askedit.SetError("Required", null); }
                else
                {
                    new AddQuestion(askedit.Text.ToString(), student, course, lecture, this).Execute(Common.getAddresApiQuestions());
                }

            };
            // Create your application here
        }
        private class AddQuestion : AsyncTask<string, Java.Lang.Void, string>
        {
            string course = "";
            string quest = "";
            string lecture = "";
            string student = "";

            AskQuestionActivity activity = new AskQuestionActivity();
            public AddQuestion(string question, string student, string course, string lecture, AskQuestionActivity activity)
            {
                this.quest = question;
                this.student = student;
                this.lecture = lecture;
                this.course = course;

                this.activity = activity;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();

                activity.askpgb.Visibility = ViewStates.Visible;
                activity.askHolder.Visibility = ViewStates.Gone;

            }
            protected override string RunInBackground(params string[] @params)
            {
                string url = @params[0];
                HttpHandler http = new HttpHandler();
                Question question = new Question();
                question.course = course;
                question.question = quest;
                question.lecture = lecture;
                question.student = student;
                string json = JsonConvert.SerializeObject(question);
                http.PostHttpData(url, json);
                return String.Empty;
            }
            protected override void OnPostExecute(string result)
            {
                base.OnPostExecute(result);

                activity.askpgb.Visibility = ViewStates.Gone;
                activity.askHolder.Visibility = ViewStates.Visible;
                activity.askedit.Text = "";
                activity.StartActivity(typeof(MainActivity));
                activity.Finish();

            }

        }

    }
}