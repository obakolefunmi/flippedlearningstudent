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
    [Activity(Label = "SignupDepartmentActivity", Theme = "@style/Theme.Custom")]
    public class SignupDepartmentActivity : AppCompatActivity, IOnItemClickListener, IOnClickListener
    {
        string college, collss, name, level, password, email, mattnum;
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
                case Resource.Id.departmentPrevious:
                    Intent gotocoll = new Intent(this, typeof(SignupCollegeActivity));
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
            Intent gotochap = new Intent(this, typeof(SignupProgramActivity));
            gotochap.PutExtra("department", j);
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
            SetContentView(Resource.Layout.signupDep);
            college = Intent.GetStringExtra("college") ?? "College of Science and Technology";
            name = Intent.GetStringExtra("name") ?? "";
            level = Intent.GetStringExtra("level") ?? "";
            password = Intent.GetStringExtra("password") ?? "";
            email = Intent.GetStringExtra("email") ?? "";
            mattnum = Intent.GetStringExtra("mattnum") ?? "";

            collss = "\"" + college + "\"";
            departmentListView = FindViewById<ListView>(Resource.Id.departmentListView);
            depWelcmMsg = FindViewById<TextView>(Resource.Id.departmentWelcome);
            prevButt = FindViewById<TextView>(Resource.Id.departmentPrevious);
            db = new DbHelper(this);
            sqliteDB = db.WritableDatabase;
            AddData();
            BasicListAdapter bookadpt = new BasicListAdapter(this, departmentlist);
            departmentListView.Adapter = bookadpt;
            departmentListView.OnItemClickListener = this;
            prevButt.SetOnClickListener(this);
            depWelcmMsg.Text = name + "! Please let us know your Department";
        }
        private void AddData()
        {
            ICursor selectData = sqliteDB.RawQuery("SELECT DISTINCT Department FROM courses WHERE College LIKE " + collss + " ORDER BY Department", new string[] { });
            if (selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    DataClass val = new DataClass();
                    string value = selectData.GetString(selectData.GetColumnIndex("Department"));
                    val.Info = value;
                    departmentlist.Add(val);
                }
                while (selectData.MoveToNext());
                selectData.Close();
            }


        }

    }
}