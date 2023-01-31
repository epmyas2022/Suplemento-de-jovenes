using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
namespace Suplemento.Models
{
    public class Mhymns
    {

        public String Title { get; set; }
        public int Number { get; set; }

        public String ImagePath { get; set; }
        public ObservableCollection<Mletter> Letters { get; set; }

       public bool IsLoadBusy { get; set; } 
        public bool IsVisibility { get; set; }
        
    }
}
