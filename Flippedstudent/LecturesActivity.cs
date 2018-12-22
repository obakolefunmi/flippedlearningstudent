using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Balysv.Material.Drawable.Menu;
using Firebase.Auth;
using Flippedstudent.Class;
using static Android.Views.View;
using static Android.Widget.AdapterView;


namespace Flippedstudent
{
    [Activity(Label = "Lectures", Theme = "@style/Theme.Custom1")]
    public class LecturesActivity : AppCompatActivity, IOnItemClickListener
       {
        public ListView lectureListview;
        public List<Lectures> lectureslistss = new List<Lectures>();
        List<int> positions = new List<int>();

        public LinearLayout delitemHolder;
        public ProgressBar delpgb;
        string curruser, cours, course, student;
        FirebaseAuth auth;

        Lectures lectureselected = null;
        enum stroke
        {
            REGULAR = 3,

            THIN = 2,
            EXTRA_THIN = 1


        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DeleteLecturelayout);
            auth = FirebaseAuth.Instance;
            curruser = "\"" + auth.CurrentUser.Email.ToString() + "\"";
            cours = Intent.GetStringExtra("course") ?? "";
            student = Intent.GetStringExtra("student") ?? "";

            course = "\"" + cours.ToString() + "\"";

            var toolbardellecture = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbardelLecture);
            SetSupportActionBar(toolbardellecture);
            MaterialMenuDrawable materialMenu = new MaterialMenuDrawable(this, Color.Purple, (int)stroke.EXTRA_THIN, MaterialMenuDrawable.DefaultScale, MaterialMenuDrawable.DefaultTransformDuration);
            materialMenu.SetIconState(MaterialMenuDrawable.IconState.Arrow);
            toolbardellecture.NavigationIcon = materialMenu;
            toolbardellecture.NavigationClick += delegate {
                OnBackPressed();
                Finish();
            };
            // Create your application here
            delitemHolder = FindViewById<LinearLayout>(Resource.Id.DeleteLectureInfoHolder);
            delpgb = FindViewById<ProgressBar>(Resource.Id.deleteLecpgb);
            lectureListview = FindViewById<ListView>(Resource.Id.DeleteLectureListview);
            //mlab code
            new GetLectureSpecifictDataLectures(this).Execute(Common.getAddresApiLecturesspecificdelete(course));

            lectureListview.OnItemClickListener = this;

        }
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            lectureselected = lectureslistss[position];
            positions.Add(position);
            var t = lectureselected.title.ToString();
            var vurl = lectureselected.vidurl.ToString();
            var nurl = lectureselected.noteurl.ToString();
            var vname = lectureselected.vidname.ToString();
            var nname = lectureselected.notename.ToString();
            Intent intent = new Intent(this, typeof(SelectWhereActivity));

            intent.PutExtra("course", cours);
            intent.PutExtra("title", t);
            intent.PutExtra("vidurl", vurl);
            intent.PutExtra("student", student);            
            intent.PutExtra("vidname", vname);
            intent.PutExtra("noteName", nname);
            intent.PutExtra("noteurl", nurl);


            StartActivity(intent);
            Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
        }

          }
}