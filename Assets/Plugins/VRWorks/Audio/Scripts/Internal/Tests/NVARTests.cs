using NUnit.Framework;

namespace NVIDIA.VRWorksAudio.Internal.Tests
{
    [TestFixture]
    [SingleThreaded]
    public sealed class ErrorHandlingTests
    {
        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarGetStatusDescription() in the managed API")]
        public void GetStatusDescription()
        {
            // Status enumeration to get the description string from NVAR
            NVAR.Status testingStatus = NVAR.Status.Error;

            // Get status enumeration string description from NVAR
            string description;
            NVAR.Status status = NVAR.GetStatusDescription(out description, testingStatus);

            // Ensure call to NVAR succeeded
            Assert.AreEqual(NVAR.Status.Success, status);

            // Ensure returned value is as expected
            Assert.AreEqual("The API returned an unspecified error.", description);
        }

        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarGetStatusString() in the managed API")]
        public void GetStatusString()
        {
            // Status enumeration to get the string name representation from NVAR
            NVAR.Status testingStatus = NVAR.Status.Error;

            // Get status enumeration string name from NVAR
            string name;
            NVAR.Status status = NVAR.GetStatusString(out name, testingStatus);

            // Ensure call to NVAR succeeded
            Assert.AreEqual(NVAR.Status.Success, status);

            // Ensure returned value is as expected
            Assert.AreEqual("NVAR_STATUS_ERROR", name);
        }
    }

    [TestFixture]
    [SingleThreaded]
    public sealed class GeneralTests
    {
        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarGetVersion() in the managed API")]
        public void GetVersion()
        {
            // Get NVAR version
            int version;
            NVAR.Status status = NVAR.GetVersion(out version);

            // Ensure NVAR call succeded
            Assert.AreEqual(NVAR.Status.Success, status);
        }

        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarGetOutputFormatChannels() in the managed API")]
        public void GetOutputFormatChannels()
        {
            NVAR.OutputFormat outputFormat = NVAR.OutputFormat.OutputFormatStereoHeadphones;

            // Get Output format channels count
            int channels;
            NVAR.Status status = NVAR.GetOutputFormatChannels(outputFormat, out channels);

            // Ensure call to NVAR suceeded
            Assert.AreEqual(NVAR.Status.Success, status);

            // Ensure returned values are as expected
            Assert.AreEqual(2, channels);
        }

        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarIntialize() and nvarFinalize() in the managed API")]
        public void InitializeAndFinalize()
        {
            // Test initialising NVAR
            NVAR.Status status = NVAR.Initialize(0);
            Assert.AreEqual(NVAR.Status.Success, status);

            // Test finalising NVAR
            status = NVAR.Finalize();
            Assert.AreEqual(NVAR.Status.Success, status);
        }

        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarGetInitializeFlags() in the managed API")]
        public void GetInitializeFlags()
        {
            int initialiseFlags = 0;
            InitialiseNVAR(initialiseFlags);
            {
                // Get Intiailise flags from NVAR
                int returnValue;
                NVAR.Status status = NVAR.GetInitializeFlags(out returnValue);

                // Ensure call to NVAR succeded
                Assert.AreEqual(NVAR.Status.Success, status);

                // Ensure returned value is expected value
                Assert.AreEqual(initialiseFlags, returnValue);
            }
            FinaliseNVAR();
        }

        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarGetDeviceCount() in the managed API")]
        public void GetDeviceCount()
        {
            InitialiseNVAR(0);
            {
                int deviceCount = -1;
                NVAR.Status status = NVAR.GetDeviceCount(out deviceCount);

                // Ensure NVAR call succeded
                Assert.AreEqual(NVAR.Status.Success, status);
                // Ensure expected value within expected range
                // (Will generate different values on different machines, so no specific value is guranteed)
                Assert.GreaterOrEqual(deviceCount, 0);
            }
            FinaliseNVAR();
        }

        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarGetDevices() in the managed API")]
        public void GetDevices()
        {
            InitialiseNVAR(0);
            {
                // Get CUDA ordinals from NVAR
                int[] devices;
                NVAR.Status status = NVAR.GetDevices(out devices);

                // Ensure NVAR call succeded
                Assert.AreEqual(NVAR.Status.Success, status);

                // Attempt to get CUDA ordinal count to ensure
                // returned data is consistent
                int deviceCount;
                status = NVAR.GetDeviceCount(out deviceCount);
                Assert.AreEqual(NVAR.Status.Success, status);

                // Ensure returned value is expected value
                Assert.IsNotNull(devices);
                Assert.AreEqual(deviceCount, devices.Length);
            }
            FinaliseNVAR();
        }

        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarGetDeviceName() in the managed API")]
        public void GetDeviceName()
        {
            InitialiseNVAR(0);
            {
                // Get list of device IDs
                int[] devices;
                NVAR.Status status = NVAR.GetDevices(out devices);
                Assert.AreEqual(NVAR.Status.Success, status);

                // Ensure we have devices to excecute this test
                if (devices.Length < 1)
                {
                    Assert.Fail("No CUDA devices returned from NVAR");
                }

                // Get device 0's name
                string deviceName;
                status = NVAR.GetDeviceName(devices[0], out deviceName);
                Assert.AreEqual(NVAR.Status.Success, status);

                // Ensure returned string is valid
                Assert.IsFalse(string.IsNullOrEmpty(deviceName));
            }
            FinaliseNVAR();
        }

        [Test]
        [Category("Managed Binding API Test")]
        [Description("Tests nvarGetPreferedDevice() in the managed API")]
        public void GetPreferedDevice()
        {
            InitialiseNVAR(0);
            {
                int preferedDevice = -1;
                NVAR.Status status = NVAR.GetPreferedDevice(out preferedDevice);

                // Ensure call to NVAR suceeded
                Assert.AreEqual(NVAR.Status.Success, status);

                // Ensure returned value is as expected
                Assert.AreNotEqual(-1, preferedDevice);
            }
            FinaliseNVAR();
        }

        #region Helper Functions

        private void InitialiseNVAR(int a_flags)
        {
            NVAR.Status status = NVAR.Initialize(a_flags);

            // Ensure NVAR was intialised successfully
            Assert.AreEqual(NVAR.Status.Success, status);
        }

        private void FinaliseNVAR()
        {
            NVAR.Status status = NVAR.Finalize();
            Assert.AreEqual(NVAR.Status.Success, status);
        }

        #endregion
    }
}
