using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IMark.Core.Converters
{
  public  class UrlConverter: IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var test = value as string;
            if (!string.IsNullOrEmpty(test))
            {
                var data = DownloadImageAsync(test);
                return ImageSource.FromStream(() => { return new MemoryStream(data.Result); });
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
           
            HttpClient _client;
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            _client = new HttpClient(httpClientHandler);
            try
            {
                using (var httpResponse = await _client.GetAsync(imageUrl))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                      //  HideLoading();
                        return await httpResponse.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                       // HideLoading();
                        //Url is Invalid
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                //HideLoading();
                //Handle Exception
                return null;
            }
        }
    }
}
