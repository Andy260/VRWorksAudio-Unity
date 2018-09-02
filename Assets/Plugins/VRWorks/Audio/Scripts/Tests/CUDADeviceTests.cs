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
            bool isPreferred    = true;

            // Construct new CUDADevice object
            CUDADevice cudaDevice = new CUDADevice(deviceNumber, deviceName, isPreferred);

            // Ensure CUDADevice object has expected data
            Assert.AreEqual(deviceNumber, cudaDevice.number);
            Assert.AreEqual(deviceName, cudaDevice.name);
            Assert.AreEqual(isPreferred, cudaDevice.isPreferred);
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
            bool isPreferred    = false;

            // Construct new CUDADevice object
            CUDADevice cudaDevice = new CUDADevice(deviceNumber, deviceName, isPreferred);

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
            bool isPreferred    = false;

            // Construct new CUDADevice object
            CUDADevice cudaDevice = new CUDADevice(deviceNumber, deviceName, isPreferred);

            // Ensure name property returns expected value
            Assert.AreEqual(deviceName, cudaDevice.name);
        }

        [Test]
        [Category("Property Test")]
        [Description("Tests normal/expected usage of the 'isPreferred' property")]
        public void IsPreferred()
        {
            int deviceNumber    = 0;
            string deviceName   = "Test Device (IsPreferred Test)";
            bool isPreferred    = true;

            // Construct new CUDADevice object
            CUDADevice cudaDevice = new CUDADevice(deviceNumber, deviceName, isPreferred);

            // Ensure name property returns expected value
            Assert.AreEqual(isPreferred, cudaDevice.isPreferred);
        }

        #endregion
    }
}
