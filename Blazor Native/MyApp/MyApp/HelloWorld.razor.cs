using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.MobileBlazorBindings.Elements;
using Plugin.Media;

namespace MyApp
{
    public class VHalloWorld : ComponentBase
    {
        public Image imageData { get; set; } = new Image();
        public bool KameraAktiv { get; set; } = true;
        public string path { get; set; } = "";
        public string filename { get; set; } = "";
        public List<string> fotoliste { get; set; } = new List<string>();
        public List<int> selectDelete { get; set; } = new List<int>();

        public void GetFotos()
        {
            fotoliste.Clear();
            foreach (var file in Directory.GetFiles("/storage/emulated/0/Android/data/com.companyname/files/Pictures/Pictures"))
            {
                if (file.Contains(".jpg"))
                {
                    fotoliste.Add(file);
                }
            }
            StateHasChanged();
            
        }

        public void DelFile(string f)
        {
            
            File.Delete(f);
            GetFotos();
        }

        public void DeleteAll()
        {
            foreach (var foto in fotoliste)
            {
             File.Delete(foto);   
            }
            GetFotos();
        }
        public async Task GetFoto()
        {
            try
            {

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    KameraAktiv = false;
                    StateHasChanged();
                    return;
                }

                var p = "/storeage/emulated/0/DCIM/"; // System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);/storeage/emulated/0/DCIM/
                path = p;
                //if (!Directory.Exists(p))
                //    Directory.CreateDirectory(p);


                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });


                if (file == null)
                    return;

                //  imageData.Source = ImageSource.FromStream(() =>
                //  {
                //      var stream = file.GetStream();
                //      file.Dispose();
                //      return stream;
                //  });


                fotoliste.Add(file.Path);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task CreateFoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    KameraAktiv = false;
                    StateHasChanged();
                    return;
                }



                var file = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                        Directory = "Pictures",
                        Name = "test.jpg",
                        SaveToAlbum = false
                    });

                if (file == null)
                    return;

                filename = file.Path;
                GetFotos();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }

        }
    }
    }
