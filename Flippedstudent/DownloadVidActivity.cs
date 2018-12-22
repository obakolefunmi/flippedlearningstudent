using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Flippedstudent.Class;
using Java.IO;
using static Android.Media.MediaPlayer;

namespace Flippedstudent

    {
    [Activity(Label = "DownloadVidActivity", Theme = "@style/Theme.Custom1")]
    public class DownloadVidActivity : AppCompatActivity, IOnPreparedListener
    {
        ProgressDialog pgd;
        VideoView lecvidview;
        ProgressBar pgb;
        Button download;
        LinearLayout Holder;
        string vidurl, vidname, course, title;

        public void OnPrepared(MediaPlayer mp)
        {
            pgd.Dismiss();
            lecvidview.Start();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VideoLecture);
            lecvidview = FindViewById<VideoView>(Resource.Id.vidlecvidview);
            pgb = FindViewById<ProgressBar>(Resource.Id.vidlecpgb);
            download = FindViewById<Button>(Resource.Id.vidlecbutt);
            Holder = FindViewById<LinearLayout>(Resource.Id.vidlecholder);
            File folder = new File(Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedVideo");
            File vidfile = new File(Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedVideo/"+vidname);


            course = Intent.GetStringExtra("course") ?? "";
            vidurl = Intent.GetStringExtra("vidurl") ?? "";
            vidname = Intent.GetStringExtra("vidname") ?? "";
            title = Intent.GetStringExtra("title") ?? "";
            pgd = new ProgressDialog(this);
            pgd.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
            pgd.SetMessage("Please Wait.....");
            pgd.SetCanceledOnTouchOutside(false);
            pgd.Show();
            Android.Net.Uri viduri = Android.Net.Uri.Parse(vidurl);
            lecvidview.SetVideoURI(viduri);
            lecvidview.RequestFocus();
            lecvidview.SetOnPreparedListener(this);
            download.Click += delegate {
                bool success = true;
                if (!vidfile.Exists())
                {
                    if (!folder.Exists())
                    {
                        success = folder.Mkdir();

                        if (success)
                        {
                            DownloadVidUrl downloadvid = new DownloadVidUrl(this, lecvidview, vidname);
                            downloadvid.Execute(vidurl);
                        }
                        else
                        {
                            Toast.MakeText(this, ";)", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        DownloadVidUrl downloadvid = new DownloadVidUrl(this, lecvidview, vidname);
                        downloadvid.Execute(vidurl);
                    }
                }
                else
                {
                    string storagePath = Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedVideo";

                    string filepath = System.IO.Path.Combine(storagePath, vidname);
                    lecvidview.SetVideoPath(filepath);
                    lecvidview.Start();
                }
            };
            lecvidview.Click += delegate {
            

                try
                {
                    if (!lecvidview.IsPlaying)
                    {
                       // Android.Net.Uri viduri = Android.Net.Uri.Parse(vidurl);
                        lecvidview.SetVideoURI(viduri);
                        lecvidview.RequestFocus();
                        lecvidview.SetOnPreparedListener(this);
                    }
                    else
                    {
                        lecvidview.Start();
                    }
                }
                catch(Exception ex)
                {

                }
            };

            //   File.Separator + "TollCulator");
            // Create your application here
        }
    }
}