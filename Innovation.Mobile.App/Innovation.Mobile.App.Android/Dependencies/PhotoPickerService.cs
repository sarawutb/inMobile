﻿using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Xamarin.Forms;
using Innovation.Mobile.App.Droid.Dependencies;

[assembly: Dependency(typeof(PhotoPickerService))]

namespace Innovation.Mobile.App.Droid.Dependencies
{
    public class PhotoPickerService : IPhotoPickerService
    {
        public Task<Stream> GetImageStreamAsync()
        {
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            // Start the picture-picker activity (resumes in MainActivity.cs)
            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

            // Return Task object
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        }
    }
}