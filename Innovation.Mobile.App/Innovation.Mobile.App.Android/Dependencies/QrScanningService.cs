using System.Threading.Tasks;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Droid.Dependencies;
using ZXing.Mobile;
using Xamarin.Forms;
using System;

[assembly: Dependency(typeof(QrScanningService))]

namespace Innovation.Mobile.App.Droid.Dependencies
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Scan the QR Code",
                BottomText = "Please Wait"                
            };

            //ZXing.Result result = null;
            //TimeSpan ts = new TimeSpan(0, 0, 0, 1, 0);
            //Device.StartTimer(ts, () =>
            //{
            //    if (result == null)
            //    {
            //        scanner.AutoFocus();
            //        if (true)
            //        {
            //            scanner.Torch(true);
            //        }
            //        return true;
            //    }
            //    return false;
            //});

            var scanResult = await scanner.Scan(optionsCustom);
            return scanResult.Text;
        }
    }
}