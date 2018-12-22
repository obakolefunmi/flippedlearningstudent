using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Flippedstudent.Class;
using Flippedstudent.Helper;
using Java.Interop;
using Newtonsoft.Json;

namespace Flippedstudent
{
    [Activity(Label = "EvaluationActivity", Theme = "@style/Theme.Custom1")]
    public class EvaluationActivity : AppCompatActivity
    {

        public TextView question, evalNext, evalFinish;
        public RadioGroup evalradiogroup;
        public string title, titl, myanswer, course, student, answer;
        public RadioButton optionA, optionB, optionC, optionD;
        public ScrollView Holder;
        public ProgressBar evalpgb;
        public List<Evaluation> evaluationlist = new List<Evaluation>();
        public Evaluation evalselected;
        int score = 0;
        int Count = 0;
        FirebaseAuth auth;
        public int evalcount = 0;
        [Export("option_selected")]
        public void option_selected(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.evaloptionA:
                    myanswer = "A";
                    break;
                case Resource.Id.evaloptionB:
                    myanswer = "B";
                    break;
                case Resource.Id.evaloptionC:
                    myanswer = "C";
                    break;
                case Resource.Id.evaloptionD:
                    myanswer = "D";
                    break;


            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Evaluation);
            question = FindViewById<TextView>(Resource.Id.evalquestion);
            evalNext = FindViewById<TextView>(Resource.Id.evalNext);
            evalFinish = FindViewById<TextView>(Resource.Id.evalFinish);
            evalradiogroup = FindViewById<RadioGroup>(Resource.Id.evalradiogroup);
            optionA = FindViewById<RadioButton>(Resource.Id.evaloptionA);
            optionB = FindViewById<RadioButton>(Resource.Id.evaloptionB);
            optionC = FindViewById<RadioButton>(Resource.Id.evaloptionC);
            optionD = FindViewById<RadioButton>(Resource.Id.evaloptionD);
            Holder = FindViewById<ScrollView>(Resource.Id.evalholder);
            evalpgb = FindViewById<ProgressBar>(Resource.Id.evalpgb);
            titl = Intent.GetStringExtra("title") ?? "";
            title = "\"" + titl.ToString() + "\"";            
            course  = Intent.GetStringExtra("course") ?? "";
            student = Intent.GetStringExtra("student") ?? "";
            auth = FirebaseAuth.Instance;

            //new GetEvaluaationSpecificdata(this,Count).Execute(Common.getAddresApiEvaluationspecific(title));
            Toast.MakeText(this, evalcount.ToString(), ToastLength.Short).Show();
            LoadQuestion(Count);

            evalNext.Click += delegate {
                if(myanswer == answer)
                {
                    score = score + 1;
                    Count++;
                    LoadQuestion(Count);
                }
                else
                {
                    Count++;
                    LoadQuestion(Count);
                }
            };
            evalFinish.Click += delegate {
                if (Count > 0)
                {
                    new AddEval(course, titl, auth.CurrentUser.Email.ToString(), (score / evalcount) * 100, this).Execute(Common.getAddresApiStudentEvaluation());
                }
            };
            // Create your application here
        }
        private  void LoadQuestion(int counts)
        {
            new GetEvaluaationSpecificdata(this, counts).Execute(Common.getAddresApiEvaluationspecific(title));

        }
        private class AddEval : AsyncTask<string, Java.Lang.Void, string>
        {
            string course = "";
            string title = "";
            string student = "";
            int score = 0;

           // string lecturer = SignupCoursesActivity.email;
            EvaluationActivity activity = new EvaluationActivity();
            public AddEval(string course, string title, string student, int score,EvaluationActivity activity)
            {
                this.course = course;
                this.activity = activity;
                this.score = score;
                this.student = student;
                this.title = title;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                activity.Holder.Visibility = ViewStates.Gone;
                activity.evalpgb.Visibility = ViewStates.Visible;
            }
            protected override string RunInBackground(params string[] @params)
            {
                string url = @params[0];
                HttpHandler http = new HttpHandler();
                StudentEval studeval = new StudentEval();
                studeval.course = course;
                studeval.lecture = title;
                studeval.score = score;
                studeval.studentname = student;
                string json = JsonConvert.SerializeObject(studeval);
                http.PostHttpData(url, json);
                return String.Empty;
            }
            protected override void OnPostExecute(string result)
            {
                base.OnPostExecute(result);
                activity.Holder.Visibility = ViewStates.Visible;
                activity.evalpgb.Visibility = ViewStates.Gone;
                activity.StartActivity(typeof(MainActivity));
            }

        }

    }
}