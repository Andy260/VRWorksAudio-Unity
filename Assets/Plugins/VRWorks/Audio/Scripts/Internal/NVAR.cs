using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NVIDIA.VRWorksAudio.Internal
{
    internal static class NVAR
    {
        #region Enumerations

        /// <summary>
        /// Compute presets
        /// </summary>
        internal enum ComputePreset
        {
            /// <summary>
            /// High compute
            /// </summary>
            High = 0,

            /// <summary>
            /// Low compute
            /// </summary>
            Low,
        }

        /// <summary>
        /// Effect strength presets
        /// </summary>
        internal enum EffectPreset
        {
            /// <summary>
            /// Low effects strength
            /// </summary>
            Low = 0,

            /// <summary>
            /// Medium effects strength
            /// </summary>
            Medium,

            /// <summary>
            /// High effects strength
            /// </summary>
            High,
        }

        /// <summary>
        /// Supported output formats
        /// </summary>
        /// <remarks>
        /// These formats describe the types of audio device playing 
        /// the audio output, for example, a pair of stereo headphones.
        /// </remarks>
        internal enum OutputFormat
        {
            /// <summary>
            /// 2-channel format intended for headphones
            /// </summary>
            OutputFormatStereoHeadphones = 0
        }

        /// <summary>
        /// Predefined material types
        /// </summary>
        /// <remarks>
        /// A list of materials whose acoustic properties are available for direct
        /// use or to form the basis of materials which a user can modify after 
        /// creating.
        /// </remarks>
        internal enum PredefinedMaterial
        {
            /// <summary>
            /// Concrete
            /// </summary>
            Concrete = 0,

            /// <summary>
            /// Metal
            /// </summary>
            Metal,

            /// <summary>
            /// Plastic
            /// </summary>
            Plastic,

            /// <summary>
            /// Carpet
            /// </summary>
            Carpet,

            /// <summary>
            /// Glass
            /// </summary>
            Glass,

            /// <summary>
            /// Wood
            /// </summary>
            Wood,

            /// <summary>
            /// Cloth
            /// </summary>
            Cloth,

            /// <summary>
            /// No reflections, all sound is absorbed by the material
            /// </summary>
            Absorber
        }

        /// <summary>
        /// All NVAR APIs return one of these error codes.
        /// Refer to each individual error code for a
        /// detailed explanation.
        /// </summary>
        internal enum Status
        {
            /// <summary>
            /// The API call returned with no errors.
            /// </summary>
            Success = 0,

            /// <summary>
            /// The NVAR library has not been initialized
            /// with <see cref="Initialize(int)"/> or an attempt to initialize 
            /// the library failed.
            /// </summary>
            NotInitialized = 1,

            /// <summary>
            /// The operation is not supported due to a mismatch between the
            /// operation requested and the state of one or more objects.
            /// </summary>
            NotSupported = 2,

            /// <summary>
            /// The API is not implemented by the current installation.
            /// </summary>
            NotImplemented = 3,

            /// <summary>
            /// One or more of the parameters passed to the API call
            /// is not an acceptable value or is not within the range of acceptable
            /// values.
            /// </summary>
            InvalidValue = 4,

            /// <summary>
            /// The API call failed because it was unable to allocate enough
            /// memory or other required resource to perform the requested
            /// operation.
            /// </summary>
            OutOfResources = 5,

            /// <summary>
            /// The operation is not available at this time.  The reason could be
            /// incomplete setup, an active asynchronous operation, or other
            /// unspecified reason.
            /// </summary>
            NotReady = 6,

            /// <summary>
            /// The API returned an unspecified error.
            /// </summary>
            Error = 7
        }

        #endregion

        #region Constants

        /// <summary>
        /// NVAR Library Version
        /// </summary>
        /// <remarks>
        /// NVAR Library version is broken into major and minor components
        /// usually denoted major.minor.These components can be determined
        /// from NVAR_API_VERSION according to the following formulas:
        /// <code>
        /// int major = NVAR_API_VERSION / 1000;
        /// int minor = (NVAR_API_VERSION % 1000);
        /// </code>
        /// </remarks>
        public const int APIVersion = 1000;

        /// <summary>
        /// The default compute preset
        /// </summary>
        public const ComputePreset CompPresetDefault = ComputePreset.High;

        /// <summary>
        /// The maximum length of the NVAR processing context name
        /// </summary>
        public const int CreateNameLength = 16;

        /// <summary>
        /// The default decay factor, which affects filter smoothing
        /// </summary>
        public const float DefaultDecayFactor = 0.9f;

        /// <summary>
        /// The default direct path gain
        /// </summary>
        public const float DefaultDirectPathGain = 1f;

        /// <summary>
        /// The default indirect path gain
        /// </summary>
        public const float DefaultIndirectPathGain = 1f;

        /// <summary>
        /// The default output format
        /// </summary>
        public const OutputFormat DefaultOutputFormat = OutputFormat.OutputFormatStereoHeadphones;

        /// <summary>
        /// The default material reflection coefficient
        /// </summary>
        public const float DefaultReflectionCoefficient = 0.9f;

        /// <summary>
        /// The suggested reverb length in seconds
        /// </summary>
        public const float DefaultReverbLength = 1f;

        /// <summary>
        /// The suggested sample rate of all sound sources
        /// </summary>
        public const int DefaultSampleRate = 48000;

        /// <summary>
        /// The default material transmission coefficient
        /// </summary>
        public const float DefaultTransmissionCoefficient = 0f;

        /// <summary>
        /// The default ratio of geometry units per meter
        /// </summary>
        public const float DefaultUnitLengthPerMeterRatio = 1f;

        /// <summary>
        /// The default effect preset
        /// </summary>
        public const EffectPreset NVAR_EFFECT_PRESET_DEFAULT = EffectPreset.Medium;

        /// <summary>
        /// The upper limit on material coefficients
        /// </summary>
        public const float MaxMaterialCoefficient = 1f;

        /// <summary>
        /// The lower limit on material coefficients
        /// </summary>
        public const float MinMaterialCoefficient = 0f;

        /// <summary>
        /// The minimum allowed sample rate
        /// </summary>
        public const int MinSampleRate = 22050;

        #endregion

        #region Internal Functions

        /// <summary>
        /// Returns an identifer string for a device
        /// </summary>
        /// <remarks>
        /// Returns a NULL-terminated ASCII string identifying the device whose CUDA ordinal
        /// is passed as device in name. <see cref="a_length"/> specifies the size of the device
        /// array, that is, maximum <see cref="a_length"/> of the string that may be returned.
        /// </remarks>
        /// <param name="a_deviceID">CUDA device ordinal of the requested device</param>
        /// <param name="a_name">Returned identifier string for the device</param>
        /// <param name="a_length">Maximum length of string to store in name</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="deviceID"/> is not the CUDA ordinal of a device 
        ///           that supports NVAR, name is NULL, or <see cref="a_length"/> is 0.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetDeviceName", CharSet = CharSet.Ansi)]
        private static extern Status Internal_GetDeviceName(int a_deviceID, StringBuilder a_name, int a_length);

        /// <summary>
        /// Gets the list of NVAR supported CUDA device ordinals
        /// </summary>
        /// <remarks>
        /// Returns an array of the CUDA ordinals (CUDA device numbers) of
        /// devices in this system which NVAR can use. On input, a_deviceCount
        /// should contain the length of the array passed in a_devices. On return,
        /// a_deviceCount will contain the number of valid entries in a_devices.
        /// </remarks>
        /// <param name="a_devices">Pointer an array of integers where device array will be written.</param>
        /// <param name="a_deviceCount">Pointer to provide the size of a_devices array and to return copied device count</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_devices"/> or <see cref="a_deviceCount"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetDevices")]
        private static extern Status Internal_GetDevices(int[] a_devices, ref int a_deviceCount);

        /// <summary>
        /// Gets the prefered NVAR device
        /// </summary>
        /// <remarks>
        /// Returns CUDA ordinal of the prefered NVAR device. If a valid <see cref="a_DXGIAdapter"/>
        /// is passed, NVAR will prefer to use a supported not in use for
        /// graphical rendering. If more than one supported device is available,
        /// the first device not being used for graphics is returned.
        /// If there is only one supported device, its CUDA ordinal is returned.
        /// </remarks>
        /// <param name="a_DXGIAdapter">(Optional) Pointer to the IDXGIAdapter corresponding to the rendering device.</param>
        /// <param name="a_device">On return, the CUDA ordinal of the prefered device for NVAR.</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_DXGIAdapter"/> is not a valid DGXIAdaptor or 
        ///           <see cref="a_device"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetPreferedDevice")]
        private static extern Status Internal_GetPreferedDevice(IntPtr a_DXGIAdapter, out int a_device);

        /// <summary>
        /// Gets the string description of a status code
        /// </summary>
        /// /// <remarks>
        /// This function sets <see cref="a_str"/> to the address of a
        /// NULL-terminated string containing a description of the 
        /// status code enumeration status.
        /// </remarks>
        /// <param name="a_str">Returned address of the string pointer</param>
        /// <param name="status">Status code of the description string</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_str"/> is NULL or <see cref="status"/> is an invalid value.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetStatusDescription")]
        private static extern Status Internal_GetStatusDescription(out IntPtr a_str, Status a_status);

        /// <summary>
        /// Gets the string representation of a status code enum
        /// </summary>
        /// <remarks>
        /// This function sets <see cref="a_str"/> to the address of a
        /// NULL-terminated string representation of the name of the
        /// status code enumeration status.
        /// </remarks>
        /// <param name="a_str">Returned address of the string pointer</param>
        /// <param name="a_status">The status code of the enum string</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_str"/> is NULL or <see cref="a_status"/> is an invalid value.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetStatusString")]
        private static extern Status Internal_GetStatusString(out IntPtr a_str, Status a_status);

        #endregion

        #region Error Handling

        /// <summary>
        /// Gets the string description of a status code
        /// </summary>
        /// <remarks>
        /// This function sets <see cref="a_str"/> to the description of the 
        /// status code enumeration status.
        /// </remarks>
        /// <param name="a_str">String description of the status code</param>
        /// <param name="status">Status code of the description string</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_str"/> is NULL or <see cref="status"/> is an invalid value.</para>
        /// </returns>
        internal static Status GetStatusDescription(out string a_str, Status a_status)
        {
            // Get status description from NVAR
            IntPtr stringPointer;
            Status status = Internal_GetStatusDescription(out stringPointer, a_status);

            // Convert unmanaged string to managed string object
            a_str = Marshal.PtrToStringAnsi(stringPointer);

            return status;
        }

        /// <summary>
        /// Gets the string representation of a status code enum
        /// </summary>
        /// <remarks>
        /// This function sets <see cref="a_str"/> to the string representation 
        /// of the name of the status code enumeration status.
        /// </remarks>
        /// <param name="a_str">Returned string name of the status code</param>
        /// <param name="a_status">The status code of the enum string</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_str"/> is NULL or <see cref="a_status"/> is an invalid value.</para>
        /// </returns>
        internal static Status GetStatusString(out string a_str, Status a_status)
        {
            // Get status name from NVAR
            IntPtr stringPointer;
            Status status = Internal_GetStatusString(out stringPointer, a_status);

            // Convert unmanaged string to managed string object
            a_str = Marshal.PtrToStringAnsi(stringPointer);

            return status;
        }

        #endregion

        #region General Functions

        /// <summary>
        /// Finalizes the NVAR API
        /// </summary>
        /// <remarks>
        /// Finalize resets the API to the default state. After this
        /// call, any calls requiring the API to be initialized will
        /// return <see cref="Status.NotInitialized"/>.
        /// </remarks>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarFinalize")]
        internal static extern Status Finalize();

        /// <summary>
        /// Gets the number of nvar supported devices
        /// </summary>
        /// <remarks>
        /// Returns the number of devices in the system that NVAR can use.
        /// </remarks>
        /// <param name="a_deviceCount">Returned device count</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_deviceCount"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetDeviceCount")]
        internal static extern Status GetDeviceCount(out int a_deviceCount);

        /// <summary>
        /// Returns an identifer string for a device
        /// </summary>
        /// <remarks>
        /// Returns a string identifying the device whose CUDA ordinal
        /// is passed as device in <see cref="a_name"/>. 
        /// (Returned name has a limited of 200 characters total)
        /// </remarks>
        /// <param name="a_deviceID">CUDA device ordinal of the requested device</param>
        /// <param name="a_name">Returned identifier string for the device</param>
        /// <returns>
        ///     <para>NVAR_STATUS_SUCCESS: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="deviceID"/> is not the CUDA ordinal of a device 
        ///           that supports NVAR.</para>
        /// </returns>
        internal static Status GetDeviceName(int a_deviceID, out string a_name)
        {
            // Allocate a blank string to populate with the device name
            StringBuilder name = new StringBuilder(200);

            // Get device name from NVAR
            Status status   = Internal_GetDeviceName(a_deviceID, name, name.Capacity);
            a_name          = name.ToString();

            return status;
        }

        /// <summary>
        /// Gets the list of NVAR supported CUDA device ordinals
        /// </summary>
        /// <remarks>
        /// Returns an array of the CUDA ordinals (CUDA device numbers) of
        /// devices in this system which NVAR can use. Returns a maximum of four devices,
        /// since most desktops can only support up to four-way SLI.
        /// </remarks>
        /// <param name="a_devices">Array of integers of available CUDA ordinals (CUDA device numbers)</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        /// </returns>
        internal static Status GetDevices(out int[] a_devices)
        {
            // Native NVAR function expected client application
            // to create array, so we'll make an assumption that the array cannot be
            // larger than 4 at most, since almost all consumer machines will have at most
            // four way SLI configurations or worse
            a_devices = new int[4];

            // Get list of CUDA ordinals from NVAR
            int deviceCount = a_devices.Length;
            Status status = Internal_GetDevices(a_devices, ref deviceCount);

            // Resize allocated array to returned values count
            Array.Resize(ref a_devices, deviceCount);

            return status;
        }

        /// <summary>
        /// Gets the flags used to initialize the API
        /// </summary>
        /// <remarks>
        /// Returns the flags used to initialize the API.
        /// </remarks>
        /// <param name="a_flags">Returned initialize flags</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/> No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_flags"/> is NULL</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetInitializeFlags")]
        internal static extern Status GetInitializeFlags(out int a_flags);

        /// <summary>
        /// Gets the number of channels
        /// </summary>
        /// <remarks>
        /// Returns the number of audio channels in the specified output format.
        /// </remarks>
        /// <param name="a_outputFormat">Output format enumeration</param>
        /// <param name="a_channels">Returned number of audio channels</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_outputFormat"/> is an invalid value or <see cref="a_channels"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetOutputFormatChannels")]
        internal static extern Status GetOutputFormatChannels(OutputFormat a_outputFormat, out int a_channels);

        /// <summary>
        /// Gets the prefered NVAR device
        /// </summary>
        /// <remarks>
        /// Returns CUDA ordinal of the prefered NVAR device. (NVAR's API allows
        /// can return a device which isn't being used for graphics processing, but we
        /// don't have access to the DirectX handle from Unity)
        /// If there is only one supported device, its CUDA ordinal is returned.
        /// </remarks>
        /// <param name="a_device">On return, the CUDA ordinal of the prefered device for NVAR.</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called</para>
        /// </returns>
        internal static Status GetPreferedDevice(out int a_device)
        {
            return Internal_GetPreferedDevice(IntPtr.Zero, out a_device);
        }

        /// <summary>
        /// Gets the NVAR API version
        /// </summary>
        /// <remarks>
        /// Returns the version number of the installed NVAR API.
        /// </remarks>
        /// <param name="a_version">Returns the NVAR API version.</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_version"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetVersion")]
        internal static extern Status GetVersion(out int a_version);

        /// <summary>
        /// Initializes the NVAR API
        /// </summary>
        /// <remarks>
        /// Initializes the API and must be called before any other function that can return 
        /// <see cref="Status.NotInitialized"/>. However, functions that do not return 
        /// <see cref="Status.NotInitialized"/> may be called before this function.
        /// </remarks>
        /// <param name="a_flags">Initialization flags</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotSupported"/>: The underlying NVAR support libraries are incompatible.</para>
        ///     <para><see cref="Status.InvalidValue"/>: flags is not zero as there are no current initialization flags.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarInitialize")]
        internal static extern Status Initialize(int a_flags);

        #endregion
    }
}
