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
using static Android.Views.View;
using static Android.Widget.AdapterView;

namespace Flippedstudent
{
    [Activity(Label = "SignupProgramActivity", Theme = "@style/Theme.Custom")]
    public class SignupProgramActivity : AppCompatActivity, IOnItemClickListener, IOnClickListener
    {
        public static string college, collss, name, department, depss, email, level, password, mattnum;
        TextView depWelcmMsg, prevButt;
        ListView departmentListView;
        DbHelper db;
        SQLiteDatabase sqliteDB = null;
        List<DataClass> departmentlist = new List<DataClass>();
        DataClass departmentselected = null;

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.programePrevious:
                    Intent gotocoll = new Intent(this, typeof(SignupDepartmentActivity));
                    gotocoll.PutExtra("name", name);
                    gotocoll.PutExtra("level", level);
                    gotocoll.PutExtra("mattnum", mattnum);
                    gotocoll.PutExtra("email", email);
                    gotocoll.PutExtra("password", password);
                    StartActivity(gotocoll);
                    //Android.Widget.Toast.MakeText(this,j , Android.Widget.ToastLength.Short).Show();
                    Finish();
                    break;
            }
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            departmentselected = departmentlist[position];
            var j = departmentselected.Info.ToString();
            Intent gotochap = new Intent(this, typeof(SignupCoursesActivity));
            gotochap.PutExtra("programe", j);

            gotochap.PutExtra("department", department);
            gotochap.PutExtra("college", college);
            gotochap.PutExtra("name", name);
            gotochap.PutExtra("level", level);
            gotochap.PutExtra("mattnum", mattnum);
            gotochap.PutExtra("email", email);
            gotochap.PutExtra("password", password);

            StartActivity(gotochap);
            Android.Widget.Toast.MakeText(this, j, Android.Widget.ToastLength.Short).Show();
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signupProg);
            college = Intent.GetStringExtra("college") ?? "";
            department = Intent.GetStringExtra("department") ?? "";
            name = Intent.GetStringExtra("name") ?? "";
            level = Intent.GetStringExtra("level") ?? "";
            password = Intent.GetStringExtra("password") ?? "";
            email = Intent.GetStringExtra("email") ?? "";
            mattnum = Intent.GetStringExtra("mattnum") ?? "";
            name = Intent.GetStringExtra("name") ?? "";
            collss = "\"" + college + "\"";
            depss = "\"" + department + "\"";
            departmentListView = FindViewById<ListView>(Resource.Id.programeListView);
            depWelcmMsg = FindViewById<TextView>(Resource.Id.programeWelcome);
            prevButt = FindViewById<TextView>(Resource.Id.programePrevious);
            db = new DbHelper(this);
            sqliteDB = db.WritableDatabase;
            AddData();
            BasicListAdapter bookadpt = new BasicListAdapter(this, departmentlist);
            departmentListView.Adapter = bookadpt;
            departmentListView.OnItemClickListener = this;
            prevButt.SetOnClickListener(this);
            depWelcmMsg.Text = name + "! Please let us know your Course of study";

            // Create your application here
        }
        private void AddData()
        {
            ICursor selectData = sqliteDB.RawQuery("SELECT DISTINCT Program FROM courses WHERE College LIKE " + collss + " AND Department LIKE " + depss + " ORDER BY Department", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    DataClass val = new DataClass();
                    string value = selectData.GetString(selectData.GetColumnIndex("Program"));
                    val.Info = value;
                    departmentlist.Add(val);
                }
                while (selectData.MoveToNext());
                selectData.Close();
            }


        }

    }
}