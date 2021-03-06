﻿#if UWP
using Bit.View;
using Bit.ViewModel;
using Bit.ViewModel.Implementations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Xamarin.Forms;

namespace Bit.UWP
{
    public class BitApplication : Windows.UI.Xaml.Application
    {
        public BitApplication()
        {
            UnhandledException += BitApplication_UnhandledException;
        }

        private void BitApplication_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            BitExceptionHandler.Current.OnExceptionReceived(e.Exception);
        }

        private bool _useDefaultConfiguration;

        /// <summary>
        /// Configures Xamarin Essentials | RgPluginsPopup | Bit.Client.Xamarin.Controls (DatePicker, Frame) | Set Min Width and Height
        /// </summary>
        protected virtual void UseDefaultConfiguration()
        {
            Xamarin.Essentials.VersionTracking.Track();
            Rg.Plugins.Popup.Popup.Init();
            BitCSharpClientControls.Init();
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Windows.Foundation.Size
            {
                Height = 1,
                Width = 1
            });
            SetBitPlatformServices();
            _useDefaultConfiguration = true;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            if (_useDefaultConfiguration)
                Xamarin.Essentials.Platform.OnLaunched(args);
        }

        protected virtual async void SetBitPlatformServices()
        {
            while (true)
            {
                await Task.Delay(25);
                if (Forms.IsInitialized)
                {
                    Device.PlatformServices = new BitPlatformServices
                    {
                        OriginalPlatformService = Device.PlatformServices
                    };
                    break;
                }
            }
        }

        /// <summary>
        /// Bit.Client.Xamarin.Controls | Rg.Plugins.Popup
        /// </summary>
        protected virtual Assembly[] GetBitRendererAssemblies()
        {
            return new[] { typeof(BitCSharpClientControls).GetTypeInfo().Assembly }
                .Union(Rg.Plugins.Popup.Popup.GetExtraAssemblies())
                .ToArray();
        }
    }
}
#endif
