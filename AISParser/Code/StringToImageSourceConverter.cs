using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AISParser.Code
{
    internal class StringToImageSourceConverter:IValueConverter
    {
        #region Converter
     
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = null;
            if ((string)value == ".png") return null;
            imagePath = String.Format("pack://application:,,,/{0};component/Resources/NationImage/{1}", System.Reflection.Assembly.GetEntryAssembly().GetName().Name, (string)value);
            
            if (imagePath==null) return null;
            BitmapImage image = new BitmapImage(new Uri(imagePath,UriKind.RelativeOrAbsolute));
            if (image == null) return null;
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("The method or operation is not implemented.");
        }
        #endregion
    }
}
