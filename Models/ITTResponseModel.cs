using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzCollege.Models
{
    public class ITTResponseModel
    {
        public string text { get; set; }
        public BoundingBox bounding_box { get; set; }

        public ITTResponseModel(string text, BoundingBox bounding_box)
        {
            this.text = text;
            this.bounding_box = bounding_box;
        }
    }

    public class BoundingBox
    {
        public int x1 { get; set; }
        public int y1 { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }

        public BoundingBox(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
    }
}
