using NUnit.Framework;

namespace NVIDIA.VRWorksAudio.Tests
{
    [TestFixture]
    internal sealed class CUDADeviceTests
    {
        #region Constructor Tests

        [Test]
        [Category("Property Test")]
        [Description("Tests normal/expected usage of the full constructor")]
        public void FullConstructor()
        {
            int deviceNumber    = 3;
            string deviceName   = "Test Device (Full Constructor Test)";

            // Construct new CUDADevice object
            CUDADevice cudaDevice = new CUDADevice(deviceNumber, deviceName);

            // Ensure CUDADevice object has expected data
            Assert.AreEqual(deviceNumber, cudaDevice.number);
            Assert.AreEqual(deviceName, cudaDevice.name);
        }

        #endregion

        #region Property Tests

        [Test]
        [Category("Property Test")]
        [Description("Tests normal/expected usage of the 'number' property")]
        public void Number()
        {
            int deviceNumber    = 1;
            string deviceName   = "Test Device (Number Test)";

            // Construct new CUDADevice object
            CUDADevice cudaDevice = new CUDADevice(deviceNumber, deviceName);

            // Ensure number property returns expected value
            Assert.AreEqual(deviceNumber, cudaDevice.number);
        }

        [Test]
        [Category("Property Test")]
        [Description("Tests normal/expected usage of the 'number' property")]
        public void Name()
        {
            int deviceNumber    = 0;
            string deviceName   = "Test Device (Name Test)";

            // Construct new CUDADevice object
            CUDADevice cudaDevice = new CUDADevice(deviceNumber, deviceName);

            // Ensure name property returns expected value
            Assert.AreEqual(deviceName, cudaDevice.name);
        }

        #endregion
    }
}
