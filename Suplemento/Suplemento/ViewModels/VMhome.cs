using Newtonsoft.Json;
using Suplemento.Models;
using Suplemento.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Suplemento.ViewModels
{
    public class VMhome : BaseViewModel
    {
        public Grid menuControl { get; set; }
        string font = Preferences.Get("fontFamily", "GandhiR");

        private ObservableCollection<Mhymns> _listHymns;
        private ObservableCollection<Mhymns> _listHymnsLikes;

        String _text;
        private bool _isLoadBusy = true;
        bool _isVisibility = true;
        bool _isRefreshing;
        bool _isMenu = false;
        int _transitionMenu = -200;
        bool _isFontGandhi ;
        bool _isFontCourier;

        int fontSizeValue = Preferences.Get("fontSize", 20);

        public VMhome(INavigation navigation)
        {
            this.Navigation = navigation;

            var IsFont = (font == "GandhiR") ? true : false;

            if (IsFont) { IsFontGandhi = true; IsFontCourier = false; } else { IsFontCourier = true; IsFontGandhi = false; }

            this.IsVisibility = false;

            Device.StartTimer(TimeSpan.FromSeconds(2), () => {

                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowHymns();
                    ShowHymnsLikes();
                });

                return false;

            });

        }

        public string text { get { return _text; } set { SetValue(ref _text, value); } }

        public ObservableCollection<Mhymns> ListHymns { get { return _listHymns; } set { SetProperty(ref _listHymns, value); } }

        public ObservableCollection<Mhymns> ListHymnsLikes { get { return _listHymnsLikes; } set { SetProperty(ref _listHymnsLikes, value); } }
        public bool IsLoadBusy
        {
            get { return _isLoadBusy; }
            set
            {
                SetProperty(ref _isLoadBusy, value);


            }
        }

        public bool IsVisibility {

            get { return _isVisibility; }
            set { SetProperty(ref _isVisibility, value); OnPropertyChanged(); }
        }

        public bool IsRefreshing { get { return _isRefreshing; } set { SetValue(ref _isRefreshing, value); } }

        public bool IsMenu { get { return _isMenu; } set { SetValue(ref _isMenu, value); } }

        public int TransitionMenu { get { return _transitionMenu; } set { SetValue(ref _transitionMenu, value); } }

        public string FontFamily { get { return font; } set { SetValue(ref font, value); } }

        public bool IsFontGandhi { get { return _isFontGandhi; } set { SetValue(ref _isFontGandhi, value); } }

        public bool IsFontCourier { get { return _isFontCourier; } set { SetValue(ref _isFontCourier,value); } }
        
        public int FontSize { get { return fontSizeValue; } set { SetValue(ref fontSizeValue,value); Preferences.Set("fontSize", FontSize);  } }

        public async Task ShowMenu()
        {

            if (!IsMenu)
            {
                IsMenu = true;
                await menuControl.TranslateTo(0, 0, 500, Easing.Linear);

            }
            else
            {
                await menuControl.TranslateTo(-300, 0, 500, Easing.Linear);
                IsMenu = false;
            }
        }
        public async Task ShowPageAnthem(Mhymns args)
        {

            await Navigation.PushAsync(new Anthem(args));
        }
        public async Task ShowHymns()
        {

            var deserializeDatabase = JsonConvert.DeserializeObject<Mdatabase>(FilesData.ReadFileInPackage("database", "json"));

   
            this.IsVisibility = true;


            var listComponents = LoadComponents(deserializeDatabase, new Mhymns { IsVisibility = true , IsLoadBusy = true });


            ListHymns = new ObservableCollection<Mhymns>(listComponents);

            this.IsLoadBusy = true;
            await Task.Delay(11000);
            this.IsLoadBusy = false;


            listComponents = LoadComponents(deserializeDatabase, new Mhymns { IsVisibility = true,IsLoadBusy = false });

            ListHymns = new ObservableCollection<Mhymns>(listComponents);



            this.IsRefreshing = false;
       

        }

  

        public async Task SearchHymns(String Text)
        {
            
                var regex = new Regex("\\d+");

                var match = regex.Match(Text);

                var deserializeData = JsonConvert.DeserializeObject<Mdatabase>(FilesData.ReadFileInPackage("database", "json"));

       
                ObservableCollection<Schema> Items = new ObservableCollection<Schema>();

                if (match.Success)
                {
                    foreach (var item in deserializeData.schema)
                    {
                        if (item.Number == int.Parse(match.Value))
                        {
                            Items.Add(item);
                        }
                    }
                }
                else
                {

                    Text = Text.ToLower();
                    foreach(var item in deserializeData.schema)
                {
                    if(item.Number.ToString().Contains(Text) || item.Title.Contains(Text))
                    {
                        Items.Add(item);
                    }
                }
                }



                this.IsLoadBusy = true;

                deserializeData.schema = Items;

                var listComponents = LoadComponents(deserializeData, new Mhymns { IsVisibility = true , IsLoadBusy = true});

                ListHymns = new ObservableCollection<Mhymns>(listComponents);

                await Task.Delay(1200);

                this.IsLoadBusy = false;

                listComponents = LoadComponents(deserializeData, new Mhymns { IsVisibility = true , IsLoadBusy = false});

                ListHymns = new ObservableCollection<Mhymns>(listComponents);

             
        }


        public async Task ShowHymnsLikes()
        {

            this.IsLoadBusy = true;

            var deserializeDatabase = JsonConvert.DeserializeObject<Mdatabase>(FilesData.ReadFileInPackage("database", "json"));

            var listComponents = LoadComponents(deserializeDatabase, new Mhymns { IsVisibility = true , IsLoadBusy = true });

            var listComponentsLikes = new List<Mhymns>();

            if (GetCollectionsLikes().Count > 0)
            {
                for (var index = 0; index < listComponents.Count; index++)
                {
                    if (getLiked(listComponents[index].Number))
                    {
                        listComponentsLikes.Add(listComponents[index]);
                    }
                }
            }
            else
            {
                listComponents.Clear();
                listComponentsLikes.Clear();
            }

                ListHymnsLikes = new ObservableCollection<Mhymns>(listComponentsLikes);

                await Task.Delay(1200);


                foreach (var component in listComponentsLikes)
                {
                    component.IsLoadBusy = false;
                }

                ListHymnsLikes = new ObservableCollection<Mhymns>(listComponentsLikes);
            
         
            this.IsLoadBusy = false;
            this.IsRefreshing = false;
            
        }

        public void SelectedFontGandhi(bool isToggled)
        {

            IsFontGandhi = isToggled;

 
        
            if (!IsFontGandhi)
            {
         
                IsFontCourier = true;
                ChangeFont("Courier");
                ShowHymns();
                ShowHymnsLikes();

            }
            else
            {
                IsFontCourier = false;
                ChangeFont("Gandhi Sans");
                ShowHymns();
                ShowHymnsLikes();
            }


           
           

        }


        public void SelectedFontCourier(bool isToggled)
        {
            IsFontCourier = isToggled;

            if (!IsFontCourier)
            {
                IsFontGandhi = true;

                ChangeFont("Gandhi Sans");
                ShowHymns();
                ShowHymnsLikes();

            }
            else
            {
                IsFontGandhi = false;

                ChangeFont("Courier");
                ShowHymns();
                ShowHymnsLikes();
            }

            


        }
        private void ChangeFont(string _fontFamily)
        {
            _fontFamily = (_fontFamily == "Gandhi Sans") ? "GandhiR" : "Courier";

            Preferences.Set("fontFamily", _fontFamily);

            FontFamily = _fontFamily;

       
        }

        private List<Mhymns> LoadComponents(Mdatabase database,Mhymns item)
        {
            var collection = new List<Mhymns>();

            foreach(var data in database.schema)
            {

                collection.Add(CreateComponent(data, AddParagraph(data), item.IsVisibility ,item.IsLoadBusy));
            }

            return collection;

        }

        private ObservableCollection<Mletter> AddParagraph(Schema data)
        {
            var collection = new ObservableCollection<Mletter>();

            foreach (var p in data.Letters)
            {
                collection.Add(new Mletter()
                {
                    AthemFontSize = FontSize,
                    AthemFontFamily = FontFamily,
                    Paragraph = ParagraphText(p),
                });
            }

            return collection;
        }
        private Mhymns CreateComponent(Schema data, ObservableCollection<Mletter> _listParagraph,bool _isVisibility,bool isLoadBusy = true)
        {
            return new Mhymns
            {
                Title = data.Title,
                ImagePath = "hymn",
                Number = data.Number,
                Letters = _listParagraph,
                IsVisibility = _isVisibility,
               IsLoadBusy = IsLoadBusy 
            };
        }
       

        private String ParagraphText(String[] lines)
        {
            var result = new StringBuilder();
            foreach(var line in lines)
            {

                result.AppendLine(Environment.NewLine + line  + "<br/>");
            }

            return result.ToString();
        }


   
        private bool getLiked(int number)
        {
            var result = false;
            var iterator = 0;

            while (iterator < GetCollectionsLikes().Count && !result)
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

            Console.WriteLine(json);

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



        public ICommand NotificationCommand => new Command(async () => { await DisplayAlert("Proximamente", "Esta funcion no esta disponible de momento", "ok"); });

        public ICommand ShowAthemCommand => new Command<Mhymns>(async (args) => { if (!this.IsLoadBusy) { await ShowPageAnthem(args); } });
        public ICommand LoadData => new Command(async () => await ShowHymns());

        public ICommand SearchCommand => new Command<String>( async(String Text) => { await SearchHymns(Text); });

        public ICommand LoadDataLikes => new Command(async () => { await ShowHymnsLikes(); });

        public ICommand ShowMenuCommand => new Command(async () => { await ShowMenu(); });

        public ICommand FontChangeCommand => new Command<string>((string font) => { ChangeFont(font); Console.WriteLine("FONT " + font); });

        public ICommand SelectedFontCourierCommand => new Command<bool>((toggled) => { SelectedFontCourier(toggled); });
        public ICommand SelectedFontGandhiCommand => new Command<bool>((toggled) => { SelectedFontGandhi(toggled); });

        public ICommand DeleteFileLikesCommand => new Command(async () => { FilesData.DeleteFile("", "likes.json"); await DisplayAlert("Success", "La lista de favoritos se ha eliminado","ok"); });


    }

    
}
