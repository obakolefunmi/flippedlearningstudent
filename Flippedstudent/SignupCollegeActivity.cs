using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Flippedstudent.Adapter;
using Flippedstudent.Class;
using static Android.Widget.AdapterView;

namespace Flippedstudent
{
    [Activity(Label = "SignupCollegeActivity", Theme = "@style/Theme.Custom")]
    public class SignupCollegeActivity : AppCompatActivity, IOnItemClickListener
    {
        ListView collegeListView;
        DbHelper db;
        TextView collWelcmMsg;
        SQLiteDatabase sqliteDB = null;
        List<DataClass> collegelist = new List<DataClass>();
        DataClass collegeselected = null;
        string name, level, password, email, mattnum;

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            collegeselected = collegelist[position];
            var j = collegeselected.Info.ToString();
            Intent gotodep = new Intent(this, typeof(SignupDepartmentActivity));
            gotodep.PutExtra("college", j);
            gotodep.PutExtra("name", name);
            gotodep.PutExtra("level", level);
            gotodep.PutExtra("mattnum", mattnum);
            gotodep.PutExtra("email", email);
            gotodep.PutExtra("password", password);
            StartActivity(gotodep);
            Android.Widget.Toast.MakeText(this, j, Android.Widget.ToastLength.Short).Show();
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signupcoll);
            name = Intent.GetStringExtra("name") ?? "";
            level = Intent.GetStringExtra("level") ?? "";
            password = Intent.GetStringExtra("password") ?? "";
            email = Intent.GetStringExtra("email") ?? "";
            mattnum = Intent.GetStringExtra("mattnum") ?? "";
            collWelcmMsg = FindViewById<TextView>(Resource.Id.collWelcome);

            collegeListView = FindViewById<ListView>(Resource.Id.collegeList);
            db = new DbHelper(this);
            sqliteDB = db.WritableDatabase;
            AddData();
            BasicListAdapter bookadpt = new BasicListAdapter(this, collegelist);
            collegeListView.Adapter = bookadpt;
            collegeListView.OnItemClickListener = this;
            collWelcmMsg.Text = "HI " + name + "! Welcome to Flipped CU. Please let us know your College";

        }
        private void AddData()
        {
            ICursor selectData = sqliteDB.RawQuery("SELECT DISTINCT College FROM courses ORDER BY College", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    DataClass val = new DataClass();
                    string value = selectData.GetString(selectData.GetColumnIndex("College"));
                    val.Info = value;
                    collegelist.Add(val);
                }
                while (selectData.MoveToNext());
                selectData.Close();
            }


        }

    }
}