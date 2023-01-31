using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Suplemento.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Suplemento.Views;
using Newtonsoft.Json;
using Xamarin.Essentials;
namespace Suplemento.ViewModels
{
    public class VMathem : BaseViewModel
    {

 
        String _title;
        int _number;
        ObservableCollection<Mletter> _letters;
        string _likedImagePath;
        bool _liked = false;
        int _fontSize = Preferences.Get("fontSize",20);
        Thickness paddingText = new Thickness(45,30,20,0);
        
        public VMathem(INavigation navigation,Mhymns mhymns)
        {
            Navigation = navigation;

            _title= mhymns.Title;
            _number = mhymns.Number;
            _letters = mhymns.Letters;


            /* FilesData.DeleteFile("", "likes.json");*/

            var number = int.Parse(AthemNumber.Replace("#", ""));

            LikedImagePath = getLiked(number) ? "favorite" : "Menufavorite";

            IsLiked = getLiked(number);

        }

        public string AthemTitle { get { return _title; } set { SetValue(ref _title, value); } }
        public String AthemNumber { get { return "#" + _number; } set { SetValue(ref _number, int.Parse(value)); } }

        public int FontSize { get { return _fontSize; } set { SetProperty(ref _fontSize, value); OnPropertyChanged(); } }

        public ObservableCollection<Mletter> AthemLetters { get { return _letters; } set { SetProperty(ref _letters, value);  } }

        public string LikedImagePath { get { return _likedImagePath; } set { SetValue(ref _likedImagePath, value); } }

        public bool IsLiked { get { return _liked; } set { SetValue(ref _liked, value); OnpropertyChanged(); } }

        public Thickness PaddingText { get { return paddingText; } set { SetValue(ref paddingText, value); } }
        public  async Task toGetBack()
        {
           await Navigation.PopAsync();
        }

        

        public void Liked()
        {
            var number = int.Parse(AthemNumber.Replace("#", ""));

            if (!IsLiked)
            {
                AddFileLikes(AthemTitle, number);
                IsLiked = true;
            }
            else {
                RemoveFileLikes(number);
                IsLiked = false;
            } 

            LikedImagePath = IsLiked ? "favorite" : "Menufavorite";

        }

        public void LoadFontSize()
        {
          
     
            var dataBackup = AthemLetters;

            if (FontSize <= 50)
            {

               FontSize += 5;

                if (PaddingText.Left >= 4)
                {
                    PaddingText = new Thickness(PaddingText.Left - 5,30, PaddingText.Left,0) ;
                }

            }
            else
            {
               FontSize = 20;
               PaddingText = new Thickness(45,30,20,0) ;
               
            }

            foreach (var p in dataBackup)
            {
                p.AthemFontSize = FontSize;
              
            }

            AthemLetters = new ObservableCollection<Mletter>(dataBackup);
      
        }


        private void AddFileLikes(string title, int number)
        {
            var schema_collection = GetCollectionsLikes();

             schema_collection.Add(new Schema()
            {
                Number = number,
                Title = title
            });

            CreateFileLikes(schema_collection);
        }

        private void RemoveFileLikes(int number)
        {
            var schema_collection = GetCollectionsLikes();

            for(var index = 0; index < schema_collection.Count; index++)
            {
                if (schema_collection[index].Number == number)
                {
                    schema_collection.RemoveAt(index);
                }
            }

            CreateFileLikes(schema_collection);
        }

        private bool getLiked(int number)
        {
            var result = false;
            var iterator = 0;

            while(iterator < GetCollectionsLikes().Count && !result)
            {
                if (GetCollectionsLikes()[iterator].Number == number)
                {
                    result = true;
                }
                iterator++;

            }
            return result;
        }


        private ObservableCollection<Schema> GetCollectionsLikes()
        {

            var collections = new ObservableCollection<Schema>();

            var json = FilesData.ReadFile("", "likes.json");

            if (json != null)
            {
                var database = JsonConvert.DeserializeObject<Mdatabase>(json);

                foreach (var item in database.schema)
                {
                    collections.Add(item);
                }

            }

            return collections;
        }

        private void CreateFileLikes(ObservableCollection<Schema> collections)
        {
            var data = new Mdatabase()
            {
                name = "Cantos Favoritos",
                total = 0,
                DataTime = DateTime.Now.Date.ToString(),
                schema = collections


            };

            var output = JsonConvert.SerializeObject(data);

            FilesData.CreateFile("", "likes.json", output);

       

        }


        public ICommand toGetBackCommand => new Command(async () => { await toGetBack(); });

        public ICommand AddFontSizeCommand => new Command(() => { LoadFontSize(); });

        public ICommand LikedCommand => new Command(() => { Liked(); });

      
      


    }
}
