using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Net;

namespace Flippedstudent.Class
{
    public class DownloadVidUrl : AsyncTask<string, string, string>
    {
        private ProgressDialog pgd;
        private VideoView vidview;
        private Context context;
        private string vidname = "";
        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            pgd = new ProgressDialog(context);
            pgd.SetMessage("Downloading Video please wait.....");
            pgd.Indeterminate = false;
            pgd.Max = 100;
            pgd.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pgd.SetCancelable(true);
            pgd.Show();
        }
        protected override void OnProgressUpdate(params string[] values)
        {
            base.OnProgressUpdate(values);
            pgd.SetProgressNumberFormat(values[0]);
            pgd.Progress = int.Parse(values[0]);
        }
        public DownloadVidUrl(Context context, VideoView vidview, string vidname)
        {
            this.vidview = vidview;
            this.context = context;
            this.vidname = vidname;
        }
        protected override string RunInBackground(params string[] @params)
        {
            string storagePath = Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedVideo";
            string filepath = System.IO.Path.Combine(storagePath, vidname);
            int count;
            try
            {
                URL url = new URL(@params[0]);
                URLConnection connection = url.OpenConnection();
                connection.Connect();
                int LengthofFile = connection.ContentLength;
                InputStream input = new BufferedInputStream(url.OpenStream(), LengthofFile);
                OutputStream output = new FileOutputStream(filepath);
                byte[] data = new byte[1024];
                long total = 0;
                while ((count = input.Read(data)) != -1)
                {
                    total += count;
                    PublishProgress("" + (int)((total / 100) / LengthofFile));
                    output.Write(data, 0, count);
                }
                output.Flush();
                output.Close();
                input.Close();
            } catch (System.Exception ex)
            {

            }
            return null;
        }
        protected override void OnPostExecute(string result)
        {
            base.OnPostExecute(result);
            string storagePath = Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedVideo";
            string filepath = System.IO.Path.Combine(storagePath, vidname);
            pgd.Dismiss();
            vidview.SetVideoPath(filepath);
            vidview.Start();
        }
    }
    public class DownloadNoteUrl : AsyncTask<string, string, string>
    {
        private ProgressDialog pgd;
        private Context context;
        private string notename = "";
        SelectWhereActivity actit;
        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            pgd = new ProgressDialog(context);
            pgd.SetMessage("Downloading Note please wait.....");
            pgd.Indeterminate = false;
            pgd.Max = 100;
            pgd.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pgd.SetCancelable(true);
            pgd.Show();
        }
        protected override void OnProgressUpdate(params string[] values)
        {
            base.OnProgressUpdate(values);
            pgd.SetProgressNumberFormat(values[0]);
            pgd.Progress = int.Parse(values[0]);
        }
        public DownloadNoteUrl(Context context, SelectWhereActivity actit,string notename)
        {
            this.context = context;
            this.notename = notename;
        }
        protected override string RunInBackground(params string[] @params)
        {
            string storagePath = Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedNote";
            string filepath = System.IO.Path.Combine(storagePath, notename);
            int count;
            try
            {
                URL url = new URL(@params[0]);
                URLConnection connection = url.OpenConnection();
                connection.Connect();
                int LengthofFile = connection.ContentLength;
                InputStream input = new BufferedInputStream(url.OpenStream(), LengthofFile);
                OutputStream output = new FileOutputStream(filepath);
                byte[] data = new byte[1024];
                long total = 0;
                while ((count = input.Read(data)) != -1)
                {
                    total += count;
                    PublishProgress("" + (int)((total / 100) / LengthofFile));
                    output.Write(data, 0, count);
                }
                output.Flush();
                output.Close();
                input.Close();
            }
            catch (System.Exception ex)
            {

            }
            return null;
        }
        protected override void OnPostExecute(string result)
        {
            base.OnPostExecute(result);

            string storagePath = Android.OS.Environment.ExternalStorageDirectory.Path + File.Separator + "FlippedNote";

            string filepath = System.IO.Path.Combine(storagePath, notename);
            Android.Net.Uri note = Android.Net.Uri.Parse(filepath);

            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.SetDataAndType(note, "application/*");
            actit.StartActivity(intent);
        }
    }

}