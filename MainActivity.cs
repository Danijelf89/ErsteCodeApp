using Android;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using CsvHelper;
using Google.Android.Material.Snackbar;
using Java.Lang;
using NHibernate.Loader;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class PostDelayed : AppCompatActivity
    {
        private int column = 0;
        private int row = 0;
        EditText textResult;
        private string filePath = string.Empty;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            CheckAppPermissions();
            GetFilePathAsync();


            Button button1 = FindViewById<Button>(Resource.Id.button1);
            textResult = FindViewById<EditText>(Resource.Id.textView1);
            

            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.columns_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            //spinner2
            Spinner spinner2 = FindViewById<Spinner>(Resource.Id.spinnerRow);

            spinner2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelectedRow);
            var adapterRow = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.row_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner2.Adapter = adapterRow;

            button1.Click += FabOnClick;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            column = e.Position;
            spinner.SetSelection(e.Position);
        }

        private void spinner_ItemSelectedRow(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner2 = (Spinner)sender;
            row = e.Position;
            spinner2.SetSelection(e.Position);
           
        }

        private void CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                    RequestPermissions(permissions, 1);
                }
            }
        }

        private async Task GetFilePathAsync()
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file != null || File.Exists(file.FilePath))
            {
                if(file.FileName == "ErsteCode.csv")
                {
                    filePath = file.FilePath;
                }
                else
                {
                    Toast.MakeText(this, "Niste izabrali dobar file.}", ToastLength.Long).Show();
                    GetFilePathAsync();
                }
                
            }
        }

       
        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var file = File.ReadLines(filePath).ToArray();
                string line = file[row];
                
                var columns = line.Split(';').ToArray();
                textResult.Text = columns[column];
            }
            catch (System.Exception e)
            {
                Toast.MakeText(this, string.Format("Doslo je do greske. {0}", e.Message), ToastLength.Long).Show();
            }

        }

        

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        
        
    }
}