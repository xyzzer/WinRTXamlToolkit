using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Device
{
    /// <summary>
    /// Helper class for Device related stuff
    /// </summary>
    public static class DeviceHelper
    {
        #region GetDeviceId
        /// <summary>
        /// Gets a unique Device indetifier
        /// </summary>
        /// <returns>Unique Device indetifier</returns>
        public static string GetDeviceId()
        {
            var packageSpecificToken = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);

            var hardwareId = packageSpecificToken.Id;
            var signature = packageSpecificToken.Signature;
            var certificate = packageSpecificToken.Certificate;
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

            var array = new byte[hardwareId.Length];
            dataReader.ReadBytes(array);            

            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < array.Length; i++)
            {
                sb.Append(array[i].ToString());
            }

            return sb.ToString();
        }
        #endregion
    }
}
