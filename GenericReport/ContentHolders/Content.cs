using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericReport
{
    public class Content
    {
        public List<Table> listTables;
        public List<Image> listImages;
        public List<Textbox> listTextboxes;

        public Content()
        {
            listTables = new List<Table>();
            listImages = new List<Image>();
            listTextboxes = new List<Textbox>();
        }


    }
}
