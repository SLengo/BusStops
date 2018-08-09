using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;

namespace RouteMarksViewer.Models
{
    public class Map : INotifyPropertyChanged, IEquatable<Map>
    {

        private int mark_id;
        private int coord_x;
        private int coord_y;
        private int default_width;
        private int default_height;
        private string map_image;
        private string mark_color;

        [SQLite.PrimaryKey]
        [SQLite.AutoIncrement]
        [SQLite.Unique]
        [SQLite.NotNull]
        public int Id { get; set; }

        [ForeignKey(typeof(Models.Mark))]
        public int MarkId
        {
            get { return mark_id; }
            set
            {
                mark_id = value;
                OnPropertyChanged("MarkId");
            }
        }
        public int CoordX
        {
            get { return coord_x; }
            set
            {
                coord_x = value;
                OnPropertyChanged("CoordX");
            }
        }
        public int CoordY
        {
            get { return coord_y; }
            set
            {
                coord_y = value;
                OnPropertyChanged("CoordY");
            }
        }
        public int DefaultWidth
        {
            get { return default_width; }
            set
            {
                default_width = value;
                OnPropertyChanged("DefaultWidth");
            }
        }
        public int DefaultHeight
        {
            get { return default_height; }
            set
            {
                default_height = value;
                OnPropertyChanged("DefaultHeight");
            }
        }
        public string MapImage
        {
            get { return map_image; }
            set
            {
                map_image = value;
                OnPropertyChanged("MapImage");
            }
        }
        public string MarkColor
        {
            get { return mark_color; }
            set
            {
                mark_color = value;
                OnPropertyChanged("MarkColor");
            }
        }

        [JsonIgnore]
        [ManyToOne]
        public Models.Mark Mark { get; set; }

        public bool Equals(Map other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id) &&
           (
               object.ReferenceEquals(this.MapImage, other.MapImage) ||
               this.MapImage != null &&
               this.MapImage.Equals(other.MapImage)
           ) &&
           (
           this.MarkId.Equals(other.MarkId)
           ) &&
           (
           this.CoordX.Equals(other.CoordX)
           ) &&
           (
           this.CoordY.Equals(other.CoordY)
           ) &&
           (
           this.DefaultWidth.Equals(other.DefaultWidth)
           ) &&
           (
           this.DefaultHeight.Equals(other.DefaultHeight)
           ) &&
           (
               object.ReferenceEquals(this.MarkColor, other.MarkColor) ||
               this.MarkColor != null &&
               this.MarkColor.Equals(other.MarkColor)
           );

        }

        public static Models.Map GetCopyOfMark(Models.Map entry_to_copy)
        {
            return new Models.Map()
            {
                Id = entry_to_copy.Id,
                CoordX = entry_to_copy.CoordX,
                CoordY = entry_to_copy.CoordY,
                DefaultHeight = entry_to_copy.DefaultHeight,
                DefaultWidth = entry_to_copy.DefaultWidth,
                MapImage = entry_to_copy.MapImage,
                MarkId = entry_to_copy.MarkId,
                Mark = entry_to_copy.Mark,
                MarkColor = entry_to_copy.MarkColor
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
