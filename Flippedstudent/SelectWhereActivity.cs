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
using Flippedstudent.Class;
using Java.IO;

namespace Flippedstudent
{
    [Activity(Label = "", Theme = "@style/Theme.Custom1")]
    public class SelectWhereActivity : AppCompatActivity
    {
        TextView watchVId, ReadNote, TakeEval, AskQuestion;

        string course, lectures, couse, lecs, vidurl, vidname, notename, noteurl, student;
        enum stroke
        {
            REGULAR = 3,

            THIN = 2,
            EXTRA_THIN = 1


        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Selectwhere);
            watchVId = FindViewById<TextView>(Resource.Id.selectwatchvid);
            ReadNote = FindViewById<TextView>(Resource.Id.selectreadnote);
            TakeEval = FindViewById<TextView>(Resource.Id.selecttakeeval);
            AskQuestion = FindViewById<TextView>(Resource.Id.selectaskaquestion);
            couse = Intent.GetStringExtra("course") ?? "";
            lecs = Intent.GetStringExtra("title") ?? "";
            vidurl = Intent.GetStringExtra("vidurl") ?? "";
            vidname = Intent.GetStringExtra("vidname") ?? "";
            student = Intent.GetStringExtra("student") ?? "";
            notename = Intent.GetStringExtra("noteName") ?? "";
            noteurl = Intent.GetStringExtra("noteurl") ?? "";

            course =  "\"" + couse.ToString() + "\"";
            lectures = "\"" + lecs.ToString() + "\"";
            File folder = new File(Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedNote");
            File notefile = new File(Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedNote/" + notename);

            var toolbardellecture = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarselect);
            SetSupportActionBar(toolbardellecture);
            MaterialMenuDrawable materialMenu = new MaterialMenuDrawable(this, Color.Purple, (int)stroke.EXTRA_THIN, MaterialMenuDrawable.DefaultScale, MaterialMenuDrawable.DefaultTransformDuration);
            materialMenu.SetIconState(MaterialMenuDrawable.IconState.Arrow);
            toolbardellecture.NavigationIcon = materialMenu;
            toolbardellecture.NavigationClick += delegate {
                OnBackPressed();
                Finish();
            };
            // Create your application here
            watchVId.Click += delegate {
                Intent intent = new Intent(this, typeof(DownloadVidActivity));

                intent.PutExtra("course", couse);
                intent.PutExtra("title", lecs);
                intent.PutExtra("vidurl", vidurl);
                intent.PutExtra("vidname", vidname);             
                StartActivity(intent);
            };
            ReadNote.Click += delegate {
                bool success = true;
                if (!notefile.Exists())
                {
                    if (!folder.Exists())
                    {
                        success = folder.Mkdir();

                        if (success)
                        {
                            DownloadNoteUrl downloadvid = new DownloadNoteUrl(this, this, notename);
                            downloadvid.Execute(noteurl);
                        }
                        else
                        {
                            Toast.MakeText(this, ";)", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        DownloadNoteUrl downloadvid = new DownloadNoteUrl(this, this, notename);
                        downloadvid.Execute(noteurl);
                    }
                }
                else
                {




                  

                   // Intent intent = new Intent(android.content.Intent.ACTION_VIEW);
                  //  intent.setType(mimeType);
                    //intent.setDataAndType(notefile, mimeType);
                    //StartActivity(intent);


                    string storagePath = Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedNote";

                    string filepath = System.IO.Path.Combine(storagePath, notename);
                    Android.Net.Uri note = Android.Net.Uri.Parse(filepath);

                    Intent intent = new Intent(Intent.ActionDefault);
                  //  intent.SetDataAndType(note, "*/*");
                 //   StartActivity(intent);
                   // Intent intent = new Intent(Intent.ACTION_VIEW);
                    intent.SetDataAndType(note, "application/*");
                    StartActivityForResult(intent,1101);
                }
            };
            TakeEval.Click += delegate {
                Intent intent = new Intent(this, typeof(EvaluationActivity));

                intent.PutExtra("course", couse);
                intent.PutExtra("title", lecs);
               intent.PutExtra("student", student);
              //  intent.PutExtra("vidname", vidname);
                StartActivity(intent);
            };
            AskQuestion.Click += delegate {
                Intent intent = new Intent(this, typeof(AskQuestionActivity));

                intent.PutExtra("course", couse);
                intent.PutExtra("title", lecs);
               // intent.PutExtra("student", student);
                //  intent.PutExtra("vidname", vidname);
                StartActivity(intent);
            };
        }
    }
}