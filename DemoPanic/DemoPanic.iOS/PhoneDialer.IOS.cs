using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;
using DemoPanic.iOS;
using DemoPanic.Interface;

[assembly: Dependency(typeof(PhoneDialer))]

namespace DemoPanic.iOS
{
    public class PhoneDialer : IDialer
    {
        public Task<bool> DialAsync(string number)
        {
            //Verificar si permite la salida de *#
            return Task.FromResult(
                UIApplication.SharedApplication.OpenUrl(
                new NSUrl("tel:" + number))
            );
        }
    }
}