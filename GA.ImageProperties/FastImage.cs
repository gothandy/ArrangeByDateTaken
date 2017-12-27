using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GA.ImageProperties
{
    // http://stackoverflow.com/questions/16891423/date-taken-of-an-image-c-sharp
    // http://stackoverflow.com/questions/180030/how-can-i-find-out-when-a-picture-was-actually-taken-in-c-sharp-running-on-vista

    public class FastImage
    {
        private static Regex r = new Regex(":");
        private DateTime dateTaken;

        public FastImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))

            using (Image img = Image.FromStream(fs, false, false))
            {
                PropertyItem dateTakenProperty = img.GetPropertyItem(36867);
                string dateTakenText = r.Replace(Encoding.UTF8.GetString(dateTakenProperty.Value), "-", 2);
                dateTaken = DateTime.Parse(dateTakenText);
            }
        }

        public DateTime DateTaken
        {
            get
            {
                return dateTaken;
            }
        }
    }
}
