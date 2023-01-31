using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Suplemento.Models
{
    public class Mletter
    {

        String _fontFamily = "Courier";
        int _fontSize = 20;
        public String AthemFontFamily { get { return _fontFamily; } set { _fontFamily = value; } }
        public int AthemFontSize { get { return _fontSize; } set { _fontSize = value; } }

        public String Paragraph { get; set; }


    }
}
