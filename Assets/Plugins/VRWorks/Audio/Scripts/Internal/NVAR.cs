using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace NVIDIA.VRWorksAudio.Internal
{
    /// <summary>
    /// Managed interface to NVAR
    /// </summary>
    internal static class NVAR
    {
        #region Enumerations

        /// <summary>
        /// Compute pre-sets
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
        /// Effect strength pre-sets
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
            /// The NVAR library has not been initialised
            /// with <see cref="Initialize(int)"/> or an attempt to initialise 
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
            /// incomplete set-up, an active asynchronous operation, or other
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
        /// The default compute pre-set
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
        /// The suggested re-verb length in seconds
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
        /// The default effect pre-set
        /// </summary>
        public const EffectPreset EffectPresetDefault = EffectPreset.Medium;

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

        #region Nested Types

        /// <summary>
        /// An opaque handle to the NVAR processing context handle
        /// </summary>
        internal struct Context
        {
            #region Properties

            /// <summary>
            /// Internal pointer to NVAR processing context
            /// </summary>
            internal IntPtr pointer { get; set; }

            #endregion

            #region Constructors

            internal Context(IntPtr a_nvarPointer)
            {
                pointer = a_nvarPointer;
            }

            #endregion
        }

        /// <summary>
        /// An opaque handle to a user defined acoustic material.
        /// </summary>
        internal struct Material
        {
            #region Properties

            /// <summary>
            /// Internal pointer to NVAR acoustic material
            /// </summary>
            internal IntPtr pointer { get; set; }

            #endregion

            #region Constructors

            internal Material(IntPtr a_nvarPointer)
            {
                pointer = a_nvarPointer;
            }

            #endregion
        }

        /// <summary>
        /// An opaque handle to an acoustic mesh.
        /// </summary>
        internal struct Mesh
        {
            #region Properties

            /// <summary>
            /// Internal pointer to NVAR acoustic mesh
            /// </summary>
            internal IntPtr pointer { get; set; }

            #endregion

            #region Constructors

            internal Mesh(IntPtr a_nvarPointer)
            {
                pointer = a_nvarPointer;
            }

            #endregion
        }

        /// <summary>
        /// 3D positions and vectors
        /// </summary>
        /// <remarks>
        /// This type is used to pass 3D positions and vectors to the
        /// NVAR API. The NVAR API coordinate system does not have a
        /// handed preference, but expects the caller to be consistent
        /// with its coordinate system for the listener, geometry,
        /// and sources.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private struct Float3
        {
            /// <summary>
            /// x
            /// </summary>
            internal float x;
            /// <summary>
            /// y
            /// </summary>
            internal float y;
            /// <summary>
            /// z
            /// </summary>
            internal float z;

            #region Operator Overloads

            public static explicit operator Float3(Vector3 a_vector3)
            {
                Float3 float3 = new Float3();
                float3.x = a_vector3.x;
                float3.y = a_vector3.y;
                float3.z = a_vector3.z;

                return float3;
            }

            public static explicit operator Vector3(Float3 a_float3)
            {
                return new Vector3(a_float3.x, a_float3.y, a_float3.z);
            }

            #endregion
        }

        /// <summary>
        /// Type used to store a transformation matrix
        /// </summary>
        /// <remarks>
        /// This type describes the affine transformation matrix of the 
        /// geometry objects in the scene.Transformation matrices are specified
        /// to NVAR in row major ordering where a[3], a[7], and a[11]
        /// are the translation components. If vIn is a 4x1
        /// input vector and mat4x4 is the 4x4 transformation matrix, the output 
        /// vector vOut = mat4x4 * Vin.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential, Size = 64)]
        private struct Matrix4x4
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            private float[] _a;

            #region Properties

            /// <summary>
            /// 4 rows by 4 columns
            /// </summary>
            internal float[] a
            {
                get
                {
                    if (_a == null)
                    {
                        _a = new float[16];
                    }

                    return _a;
                }
            }

            #endregion

            #region Constructors

            internal Matrix4x4(UnityEngine.Matrix4x4 a_matrix)
            {
                _a = new float[]
                {
                    a_matrix.m00, a_matrix.m01, a_matrix.m02, a_matrix.m03,
                    a_matrix.m10, a_matrix.m11, a_matrix.m12, a_matrix.m13,
                    a_matrix.m20, a_matrix.m21, a_matrix.m22, a_matrix.m23,
                    a_matrix.m30, a_matrix.m31, a_matrix.m32, a_matrix.m33
                };
            }

            #endregion

            #region Operator Overloads

            public static explicit operator Matrix4x4(UnityEngine.Matrix4x4 a_matrix)
            {
                return new Matrix4x4(a_matrix);
            }

            public static explicit operator UnityEngine.Matrix4x4(Matrix4x4 a_matrix)
            {
                return new UnityEngine.Matrix4x4(
                    new Vector4(a_matrix.a[0], a_matrix.a[4], a_matrix.a[8], a_matrix.a[12]),
                    new Vector4(a_matrix.a[1], a_matrix.a[5], a_matrix.a[9], a_matrix.a[13]),
                    new Vector4(a_matrix.a[2], a_matrix.a[6], a_matrix.a[10], a_matrix.a[14]),
                    new Vector4(a_matrix.a[3], a_matrix.a[7], a_matrix.a[11], a_matrix.a[15]));
            }

            #endregion
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// Updates the processing engine with changes to the geometry
        /// </summary>
        /// <remarks>
        /// Updates the scene's acoustic geometry. Because this
        /// update can be an expensive operation,
        /// this function provides a mechanism to update the geometry 
        /// outside of calling <see cref="TraceAudio(Context, IntPtr)"/>. If the geometry has changed
        /// and this function has not been not called before a call to <see cref="TraceAudio(Context, IntPtr)"/>,
        /// the geometry changes will be automatically updated in the call
        /// to <see cref="TraceAudio(Context, IntPtr)"/>.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: nvar is not a valid context.</para>
        ///     <para><see cref="Status.NotReady"/>: Audio trace is in progress.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarCommitGeometry")]
        private static extern Status Internal_CommitGeometry(IntPtr a_nvar);

        /// <summary>
        /// Returns an identifier string for a device
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
        /// Gets the preferred NVAR device
        /// </summary>
        /// <remarks>
        /// Returns CUDA ordinal of the preferred NVAR device. If a valid <see cref="a_DXGIAdapter"/>
        /// is passed, NVAR will prefer to use a supported not in use for
        /// graphical rendering. If more than one supported device is available,
        /// the first device not being used for graphics is returned.
        /// If there is only one supported device, its CUDA ordinal is returned.
        /// </remarks>
        /// <param name="a_DXGIAdapter">(Optional) Pointer to the IDXGIAdapter corresponding to the rendering device.</param>
        /// <param name="a_device">On return, the CUDA ordinal of the preferred device for NVAR.</param>
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

        /// <summary>
        /// Creates an NVAR processing context
        /// </summary>
        /// <remarks>
        /// Creates and initialises an NVAR processing context. If no
        /// name string is passed, a default context will be created. If the
        /// context to be created already exists, the existing handle
        /// will be returned to the caller the context's internal reference count
        /// will be incremented when the function call succeeds. Only one unnamed
        /// and one named context are simultaneously supported.
        /// </remarks>
        /// <param name="a_nvar">Returned NVAR processing context</param>
        /// <param name="a_name">The name of the context, which may be up to <see cref="CreateNameLength"/> characters</param>
        /// <param name="a_nameLength">The number of characters in name</param>
        /// <param name="a_preset">NVAR compute pre-set that controls performance of the acoustic trace.</param>
        /// <param name="a_deviceNum">Pointer to the CUDA device number; if NULL, the device 0 will be used</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called</para>
        ///     <para><see cref="Status.NotSupported"/>: The context has already been created
        ///           with a different parameter set (for example the CUDA device num).
        ///           Mismatched parameters will be returned through parameters
        ///           marked as [optional, in, out] with the values of the existing 
        ///           NVAR processing context. The contents of <see cref="a_nvar"/> will be will be invalid.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarCreate")]
        private static extern Status Internal_Create(out IntPtr a_nvar, string a_name, IntPtr a_nameLength, EffectPreset a_preset, ref int a_deviceNum);

        /// <summary>
        /// Destroys an NVAR processing context
        /// </summary>
        /// <remarks>
        /// Decrements the reference count on an NVAR
        /// context and, if the reference count becomes zero, destroys
        /// the processing context and frees any associated resources.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context to be destroyed</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarDestroy")]
        private static extern Status Internal_Destroy(IntPtr a_nvar);

        /// <summary>
        /// Gets the decay factor
        /// </summary>
        /// <remarks>
        /// Returns the re-verb decay factor from the NVAR processing context
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_decayFactor">Returned decay factor</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_decayFactor"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetDecayFactor")]
        private static extern Status Internal_GetDecayFactor(IntPtr a_nvar, out float a_decayFactor);

        /// <summary>
        /// Sets the decay factor
        /// </summary>
        /// <remarks>
        /// <para>
        /// Sets the decay factor for sources in this processing context. The
        /// decay factor controls the longevity of energy from reflections
        /// according to the equation(1-decayFactor)^N
        /// where N is the number of traces.A decay factor of 0.8, for example,
        /// results in a path's contributed energy being reducing to &lt; 1% within 21
        /// traces from the trace when the path is originally discovered.
        /// </para>
        /// 
        /// <para>
        /// The default decay factor is <see cref="DefaultDecayFactor"/> if
        /// this function is not called.
        /// </para>
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_decayFactor">Decay factor. Must be in the range (0.0, 1.0].</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="nvarInitialize"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_decayFactor"/> is not in the range
        ///           (0.0f, 1.0f).</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetDecayFactor")]
        private static extern Status Internal_SetDecayFactor(IntPtr a_nvar, float a_decayFactor);

        /// <summary>
        /// Gets the CUDA device number from the NVAR processing context
        /// </summary>
        /// <remarks>
        /// Returns the CUDA device number specified to create the NVAR processing context.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_deviceNum">Returned device number</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_deviceNum"/> is NULL</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetDeviceNum")]
        private static extern Status Internal_GetDeviceNum(IntPtr a_nvar, out int a_deviceNum);

        /// <summary>
        /// Gets the location of the listener
        /// </summary>
        /// <remarks>
        /// Returns the location of the listener in the scene.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_location">Returned location of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_location"/> is NULL</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetListenerLocation")]
        private static extern Status Internal_GetListenerLocation(IntPtr a_nvar, out Float3 a_location);

        /// <summary>
        /// Sets the location of the listener
        /// </summary>
        /// <remarks>
        /// Sets the location of the listener in the scene. 
        /// The default orientation of the listener is (0.0f,
        /// 0.0f, -1.0f) for the forward vector and(0.0f, 1.0f, 0.0f)
        /// for the up vector. These defaults do not imply a preferred 
        /// coordinate system.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_location">The location of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetListenerLocation")]
        private static extern Status Internal_SetListenerLocation(IntPtr a_nvar, Float3 a_location);

        /// <summary>
        /// Gets the output format
        /// </summary>
        /// <remarks>
        /// Returns the output format of filters or filtered audio from the NVAR processing context.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_outputFormat">Returned output format</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_outputFormat"/> is NULL.</para>
        ///     <para></para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetOutputFormat")]
        private static extern Status Internal_GetOutputFormat(IntPtr a_nvar, out OutputFormat a_outputFormat);

        /// <summary>
        /// Sets the output format
        /// </summary>
        /// <remarks>
        /// Sets the output format of filters or filtered audio
        /// from the NVAR processing context.If this function is not called,
        /// the default output format
        /// <see cref="OutputFormat.OutputFormatStereoHeadphones"/> is used.
        /// This function can be expensive because of reallocation
        /// of internal buffers; it should ideally called only once,
        /// before any sources have been created.
        /// Audio continuity is not guaranteed across calls to this function.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_outputFormat">The output format</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_outputFormat"/> 
        ///           is not a valid output format.</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetOutputFormat")]
        private static extern Status Internal_SetOutputFormat(IntPtr a_nvar, OutputFormat a_outputFormat);

        /// <summary>
        /// Gets the re-verb length
        /// </summary>
        /// <remarks>
        /// Returns the re-verb length, in seconds, from the NVAR processing context.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_reverbLength">Returned re-verb length</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_reverbLength"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetReverbLength")]
        private static extern Status Internal_GetReverbLength(IntPtr a_nvar, out float a_reverbLength);

        /// <summary>
        /// Sets the re-verb length
        /// </summary>
        /// <remarks>
        /// Sets the re-verb length, in seconds, in the NVAR
        /// processing context.If this function is not called, the
        /// default re-verb length given by <see cref="DefaultReverbLength"/>
        /// is used. This function can be expensive because of reallocation
        /// of internal buffers. It should ideally called once before any
        /// sources exist. Audio continuity is not guaranteed across
        /// calls to this function. The API does not restrict the re-verb length
        /// to enable non-real-time uses. Real-time applications should take care
        /// in setting this value.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_reverbLength">Re-verb length, in seconds. Must be in the range (0.0, Inf).</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_reverbLength"/> is not in the range(0.0f, inf).</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetReverbLength")]
        private static extern Status Internal_SetReverbLength(IntPtr a_nvar, float a_reverbLength);

        /// <summary>
        /// Gets the sample rate
        /// </summary>
        /// <remarks>
        /// Returns the sample rate in samples per second of sound sources in the NVAR processing context.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_sampleRate">Returned sample rate in samples per second</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_sampleRate"/> is NULL</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetSampleRate")]
        private static extern Status Internal_GetSampleRate(IntPtr a_nvar, out int a_sampleRate);

        /// <summary>
        /// Sets the sample rate
        /// </summary>
        /// <remarks>
        /// Sets the sample rate in samples per second of sound
        /// sources in the NVAR processing context.The default
        /// sample rate if this function is not called is
        /// <see cref="DefaultSampleRate"/> hertz. This function can be 
        /// expensive because of reallocation
        /// of internal buffers. It should ideally called once before any
        /// sources exist.Audio continuity is not guaranteed across
        /// calls to this function.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_sampleRate">Sample rate. Must be in the range (<see cref="MinSampleRate"/>, inf).</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.NVAR_STATUS_INVALID_VALUE"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_sampleRate"/> 
        ///     is not in the range [<see cref="MinSampleRate"/>, inf].</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetSampleRate")]
        private static extern Status Internal_SetSampleRate(IntPtr a_nvar, int a_sampleRate);

        /// <summary>
        /// Gets the units per meter from the NVAR processing context
        /// </summary>
        /// <remarks>
        /// Returns the units per meter from the NVAR
        /// processing context. The default unit length per meter ratio
        /// is <see cref="DefaultUnitLengthPerMeterRatio"/>.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_ratio">Returned unit length per meter ratio</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_ratio"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetUnitLength")]
        private static extern Status Internal_GetUnitLength(IntPtr a_nvar, out float a_ratio);

        /// <summary>
        /// Sets the unit length per meter ratio of the NVAR processing context
        /// </summary>
        /// <remarks>
        /// Sets the unit length per meter ratio of the
        /// NVAR processing context. If each unit in the geometry passed
        /// to the NVAR processing context is specified in centimetres, for example,
        /// a unit length per meter of 0.01 units per meter gives the processing
        /// context the appropriate scale. If this function is not called,
        /// the default unit length per meter ratio
        /// <see cref="DefaultUnitLengthPerMeterRatio"/> is used.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_ratio">The new unit length per meter ratio. Must be in the range(0.0, Inf).</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_ratio"/> is not in the range (0.0f, inf).</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetUnitLength")]
        private static extern Status Internal_SetUnitLength(IntPtr a_nvar, float a_ratio);

        /// <summary>
        /// Wait for nvar command stream to idle
        /// </summary>
        /// <remarks>
        /// Blocks the calling thread until all activity in the 
        /// asynchronous command queue has been completed. Can be used
        /// to ensure synchronisation between the NVAR processing context
        /// and the calling thread. 
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSynchronize")]
        private static extern Status Internal_Synchronize(IntPtr a_nvar);

        /// <summary>
        /// Traces the audio paths between the listener and the sound sources
        /// </summary>
        /// <remarks>
        /// <para>
        /// Schedule an acoustic trace. Acoustic traces are the main 
        /// computation of NVAR that trace paths between all sources and 
        /// the listener in the specified geometry. The result of an acoustic
        /// trace is a set of filters.
        /// </para>
        /// 
        /// <para>
        /// <see cref="TraceAudio(Context, Action)"/> returns once the trace has been added to the
        /// asynchronous command queue. The trace will be run asynchronously to the
        /// calling thread. If <see cref="a_traceDoneEvent"/> is not NULL, the Windows event passed
        /// in that argument will be signalled by a call to SetEvent()
        /// once the trace scheduled by this call is completed.
        /// </para>
        /// 
        /// <para>
        /// Because traceAudio commands are enqueued, applications should use
        /// the <see cref="a_traceDoneEvent"/> or <see cref="Synchronize(Context)"/> to
        /// ensure that previously started traces are completed before issuing
        /// new traces. If <see cref="nvarTraceAudio"/> is called faster  
        /// than traces complete, a backlog of traces will accumulate
        /// in the command queue.
        /// </para>
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_traceDoneEvent">Windows event object that will be signalled when tracing is complete.</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context.</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarTraceAudio")]
        private static extern Status Internal_TraceAudio(IntPtr a_nvar, IntPtr a_traceDoneEvent);

        /// <summary>
        /// Records an event in nvar command queue.
        /// </summary>
        /// <remarks>
        /// Adds an event to the asynchronous command queue and triggers
        /// the specified windows event once all commands in the queue prior
        /// to the event have been executed.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_event">The event which is signalled</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_event"/> is not a valid Windows event.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarEventRecord")]
        private static extern Status Internal_EventRecord(IntPtr a_nvar, IntPtr a_event);

        /// <summary>
        /// Exports NVAR geometry to Wavefront .obj file.
        /// </summary>
        /// <remarks>
        /// Dumps the current state of the scene geometry in the NVAR context
        /// to a Wavefront.obj file with a generic.mtl material file.
        /// This function involves disk I/O and is heavyweight as a result.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_objFileBaseName">The base file name of the generated Wavefront .obj and .mtl 
        /// files to which the NVAR geometry will be exported.</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_objFileBaseName"/> is NULL.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarExportOBJs", CharSet = CharSet.Ansi)]
        private static extern Status Internal_ExportOBJs(IntPtr a_nvar, string a_objFileBaseName);

        /// <summary>
        /// Gets the orientation of the listener
        /// </summary>
        /// <remarks>
        /// Returns the orientation of the listener in the scene.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_forward">Returned forward orientation of the listener</param>
        /// <param name="a_up">Returned up orientation of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context, or <see cref="a_forward"/> or <see cref="a_up"/> is NULL.</para>
        /// </returns>
        /// <summary>
        /// Gets the orientation of the listener
        /// </summary>
        /// <remarks>
        /// Returns the orientation of the listener in the scene.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_forward">Returned forward orientation of the listener</param>
        /// <param name="a_up">Returned up orientation of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context, or <see cref="a_forward"/> or <see cref="a_up"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetListenerOrientation")]
        private static extern Status Internal_GetListenerOrientation(IntPtr a_nvar, out Float3 a_forward, out Float3 a_up);

        /// <summary>
        /// Sets the orientation of the listener
        /// </summary>
        /// <remarks>
        /// Sets the forward and up orthogonal orientation
        /// vectors of the listener in the scene. The forward vector is
        /// directed away from the listener in the direction the listener
        /// is facing. The up vector is directed away from the top of
        /// listener. The specified vectors must be orthogonal.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_forward">The forward orientation of the listener</param>
        /// <param name="a_up">The up orientation of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context, or <see cref="a_forward"/> and 
        ///           <see cref="a_up"/> are not orthogonal.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetListenerOrientation")]
        private static extern Status Internal_SetListenerOrientation(IntPtr a_nvar, Float3 a_forward, Float3 a_up);

        /// <summary>
        /// Creates an acoustic material
        /// </summary>
        /// <remarks>
        /// Creates an acoustic material with default properties.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_material">Returned material handle</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_material"/> is NULL.</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarCreateMaterial")]
        private static extern Status Internal_CreateMaterial(IntPtr a_nvar, out IntPtr a_material);

        /// <summary>
        /// Creates a predefined acoustic material
        /// </summary>
        /// <remarks>
        /// This function creates an acoustic material with predefined acoustic properties.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_material">Returned material object</param>
        /// <param name="a_predefinedMaterial">Enumerated predefined material value</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context, <see cref="a_material"/> is NULL, 
        ///           or <see cref="a_predefinedMaterial"/> is not in the range (<see cref="PredefinedMaterial.Absorber"/>.</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarCreatePredefinedMaterial")]
        private static extern Status Internal_CreatePredefinedMaterial(IntPtr a_nvar, out IntPtr a_material, PredefinedMaterial a_predefinedMaterial);

        /// <summary>
        /// Destroys the specified acoustic material
        /// </summary>
        /// <remarks>
        /// Destroys the specified acoustic material. The material should not be currently attached to a mesh object.
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material object.</para>
        ///     <para><see cref="Status.NotReady"/>: The material is still attached to a mesh object.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarDestroyMaterial")]
        private static extern Status Internal_DestroyMaterial(IntPtr a_material);

        /// <summary>
        /// Gets the reflection coefficient of the acoustic material
        /// </summary>
        /// <remarks>
        /// This function gets the reflection coefficient of the acoustic material.
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <param name="a_reflection">Returned reflection coefficient</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material or <see cref="a_reflection"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetMaterialReflection")]
        private static extern Status Internal_GetMaterialReflection(IntPtr a_material, out float a_reflection);

        /// <summary>
        /// Sets the reflection coefficient of the acoustic material
        /// </summary>
        /// <remarks>
        /// This function sets the reflection coefficient of the acoustic
        /// material.Physically, this value should be in the range 
        /// [0, 1], and the reflection coefficient and transmission
        /// coefficients should have a sum &lt;= 1.0. The API does not 
        /// enforce this restriction
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <param name="a_reflection">Reflection coefficient</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material or <see cref="a_reflection"/> is not in the range[0.0, 1.0].</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetMaterialReflection")]
        private static extern Status Internal_SetMaterialReflection(IntPtr a_material, float a_reflection);

        /// <summary>
        /// Gets the transmission coefficient of the acoustic material
        /// </summary>
        /// <remarks>
        /// Returns the transmission coefficient of the acoustic material.
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <param name="a_transmission">Returned transmission coefficient</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material or <see cref="a_transmission"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetMaterialTransmission")]
        private static extern Status Internal_GetMaterialTransmission(IntPtr a_material, out float a_transmission);

        /// <summary>
        /// Sets the transmission coefficient of the acoustic material
        /// </summary>
        /// <remarks>
        /// This function sets the transmission coefficient of the acoustic
        /// material.Physically, this value should be in the range 
        /// [0, 1], and the reflection coefficient and transmission
        /// coefficients should have a sum &lt;= 1.0. The API does not 
        /// enforce this restriction.
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <param name="transmission">Transmission coefficient</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material or <see cref="a_transmission"/> is not in the range[0.0, 1.0].</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetMaterialTransmission")]
        private static extern Status Internal_SetMaterialTransmission(IntPtr a_material, float a_transmission);

        /// <summary>
        /// Creates an acoustic mesh
        /// </summary>
        /// <remarks>
        /// Creates an acoustic mesh from the vertices, faces, and acoustic
        /// material. The function scales and places the mesh in the scene
        /// using the specified transformation matrix. Changes will be incorporated
        /// into the scene when the next call to::nvarCommitGeometry
        /// or <see cref="nvarTraceAudio"/> is executed.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_mesh">Returned mesh object</param>
        /// <param name="a_transform">The transform of the mesh</param>
        /// <param name="a_vertices">The array of vertices</param>
        /// <param name="a_numVertices">The number of <see cref="a_vertices"/></param>
        /// <param name="a_faces">The array of faces</param>
        /// <param name="a_numFaces">The number of <see cref="a_faces"/></param>
        /// <param name="a_material">The material applied to the mesh</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: Possible causes: <see cref="a_nvar"/> is not a valid context;
        ///           <see cref="a_mesh"/>, <see cref="a_transform"/>, <see cref="a_vertices"/>, or <see cref="a_faces"/>
        ///           is NULL; the number of <see cref="a_numVertices"/> is not valid; the number of <see cref="a_numFaces"/>
        ///           is not valid; or the <see cref="a_material"/> value is not valid.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarCreateMesh")]
        private static extern Status Internal_CreateMesh(IntPtr a_nvar, out IntPtr a_mesh, Matrix4x4 a_transform, Float3[] a_vertices, int a_numVertices,
                                                               int[] a_faces, int a_numFaces, IntPtr a_material);

        /// <summary>
        /// Destroys the specified acoustic mesh
        /// </summary>
        /// <remarks>
        /// Destroys the specified acoustic mesh and
        /// releases any associated resources. The mesh will be removed from
        /// the scene when the next call to <see cref="CommitGeometry(Context)"/> or
        /// <see cref="TraceAudio(Context, IntPtr)"/> is executed.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh object.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarDestroyMesh")]
        private static extern Status Internal_DestroyMesh(IntPtr a_mesh);

        /// <summary>
        /// Gets the acoustic material of the mesh
        /// </summary>
        /// <remarks>
        /// Returns the acoustic material applied to the specified mesh.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <param name="a_material">Returned material object</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh or <see cref="a_material"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetMeshMaterial")]
        private static extern Status Internal_GetMeshMaterial(IntPtr a_mesh, out IntPtr a_material);

        /// <summary>
        /// Sets the acoustic material of the mesh
        /// </summary>
        /// <remarks>
        /// Sets acoustic material of the specified mesh.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <param name="a_material">Valid material handle</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh or <see cref="a_material"/> is not a valid material.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetMeshMaterial")]
        private static extern Status Internal_SetMeshMaterial(IntPtr a_mesh, IntPtr a_material);

        /// <summary>
        /// Gets the transform of the mesh
        /// </summary>
        /// <remarks>
        /// Returns the transformation matrix of the specified mesh.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <param name="a_transform">Returned transformation matrix</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh or <see cref="a_transform"/> is NULL.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarGetMeshTransform")]
        private static extern Status Internal_GetMeshTransform(IntPtr a_mesh, out Matrix4x4 a_transform);

        /// <summary>
        /// Sets the transform of the mesh
        /// </summary>
        /// <remarks>
        /// Sets transformation matrix for the specified mesh object.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <param name="a_transform">The transformation matrix</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarSetMeshTransform")]
        private static extern Status Internal_SetMeshTransform(IntPtr a_mesh, Matrix4x4 a_transform);

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
        /// Finalises the NVAR API
        /// </summary>
        /// <remarks>
        /// Finalise resets the API to the default state. After this
        /// call, any calls requiring the API to be initialised will
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
        /// Returns an identifier string for a device
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
        /// Gets the flags used to initialise the API
        /// </summary>
        /// <remarks>
        /// Returns the flags used to initialise the API.
        /// </remarks>
        /// <param name="a_flags">Returned initialise flags</param>
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
        /// Gets the preferred NVAR device
        /// </summary>
        /// <remarks>
        /// Returns CUDA ordinal of the preferred NVAR device. (NVAR's API allows
        /// can return a device which isn't being used for graphics processing, but we
        /// don't have access to the DirectX handle from Unity)
        /// If there is only one supported device, its CUDA ordinal is returned.
        /// </remarks>
        /// <param name="a_device">On return, the CUDA ordinal of the preferred device for NVAR.</param>
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
        /// Initialises the NVAR API
        /// </summary>
        /// <remarks>
        /// Initialises the API and must be called before any other function that can return 
        /// <see cref="Status.NotInitialized"/>. However, functions that do not return 
        /// <see cref="Status.NotInitialized"/> may be called before this function.
        /// </remarks>
        /// <param name="a_flags">Initialisation flags</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotSupported"/>: The underlying NVAR support libraries are incompatible.</para>
        ///     <para><see cref="Status.InvalidValue"/>: flags is not zero as there are no current initialisation flags.</para>
        /// </returns>
        [DllImport("nvar", EntryPoint = "nvarInitialize")]
        internal static extern Status Initialize(int a_flags);

        #endregion

        #region Processing Context Functions

        /// <summary>
        /// Updates the processing engine with changes to the geometry
        /// </summary>
        /// <remarks>
        /// Updates the scene's acoustic geometry. Because this
        /// update can be an expensive operation,
        /// this function provides a mechanism to update the geometry 
        /// outside of calling <see cref="TraceAudio(Context, IntPtr)"/>. If the geometry has changed
        /// and this function has not been not called before a call to <see cref="TraceAudio(Context, IntPtr)"/>,
        /// the geometry changes will be automatically updated in the call
        /// to <see cref="TraceAudio(Context, IntPtr)"/>.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: nvar is not a valid context.</para>
        ///     <para><see cref="Status.NotReady"/>: Audio trace is in progress.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status CommitGeometry(Context a_context)
        {
            return Internal_CommitGeometry(a_context.pointer);
        }

        /// <summary>
        /// Creates an NVAR processing context
        /// </summary>
        /// <remarks>
        /// Creates and initialises an NVAR processing context. If no
        /// name string is passed, a default context will be created. If the
        /// context to be created already exists, the existing handle
        /// will be returned to the caller the context's internal reference count
        /// will be incremented when the function call succeeds. Only one unnamed
        /// and one named context are simultaneously supported.
        /// </remarks>
        /// <param name="a_nvar">Returned NVAR processing context</param>
        /// <param name="a_name">The name of the context, which may be up to <see cref="CreateNameLength"/> characters</param>
        /// <param name="a_preset">NVAR compute pre-set that controls performance of the acoustic trace.</param>
        /// <param name="a_deviceNum">CUDA device number</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="nvarInitialize"/> has not been called</para>
        ///     <para><see cref="Status.NotSupported"/>: The context has already been created
        ///           with a different parameter set (for example the CUDA device num).
        ///           Mismatched parameters will be returned through parameters
        ///           marked as [optional, in, out] with the values of the existing 
        ///           NVAR processing context. The contents of <see cref="a_nvar"/> will be will be invalid.</para>
        /// </returns>
        internal static Status Create(out Context a_nvar, string a_name, EffectPreset a_preset, int a_deviceNum = 0)
        {
            // Create NVAR context
            IntPtr contextPointer;
            Status status = Internal_Create(out contextPointer, a_name, new IntPtr(a_name.Length), a_preset, ref a_deviceNum);

            // Create NVAR context reference object
            a_nvar = new Context(contextPointer);

            return status;
        }

        /// <summary>
        /// Destroys an NVAR processing context
        /// </summary>
        /// <remarks>
        /// Decrements the reference count on an NVAR
        /// context and, if the reference count becomes zero, destroys
        /// the processing context and frees any associated resources.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context to be destroyed</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context</para>
        /// </returns>
        internal static Status Destroy(Context a_nvar)
        {
            return Internal_Destroy(a_nvar.pointer);
        }

        /// <summary>
        /// Records an event in nvar command queue.
        /// </summary>
        /// <remarks>
        /// Adds an event to the asynchronous command queue and triggers
        /// the specified windows event once all commands in the queue prior
        /// to the event have been executed. 
        /// 
        /// <para>
        /// In C# we use the native
        /// handle from <see cref="SafeHandle.DangerousGetHandle(void)"/> from classes which derive from
        /// <see cref="WaitHandle"/> to give NVAR the reference to our event objects.
        /// </para>
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_event">Native handle to the event which is signalled</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_event"/> is not a valid Windows event.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status EventRecord(Context a_nvar, IntPtr a_event)
        {
            return Internal_EventRecord(a_nvar.pointer, a_event);
        }

        /// <summary>
        /// Exports NVAR geometry to Wavefront .obj file.
        /// </summary>
        /// <remarks>
        /// Dumps the current state of the scene geometry in the NVAR context
        /// to a Wavefront.obj file with a generic.mtl material file.
        /// This function involves disk I/O and is heavyweight as a result.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_objFileBaseName">The base file name of the generated Wavefront .obj and .mtl 
        /// files to which the NVAR geometry will be exported.</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_objFileBaseName"/> is NULL.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status ExportOBJs(Context a_nvar, string a_objFileBaseName)
        {
            return Internal_ExportOBJs(a_nvar.pointer, a_objFileBaseName);
        }

        /// <summary>
        /// Gets the decay factor
        /// </summary>
        /// <remarks>
        /// Returns the re-verb decay factor from the NVAR processing context
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_decayFactor">Returned decay factor</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_decayFactor"/> is NULL.</para>
        /// </returns>
        internal static Status GetDecayFactor(Context a_nvar, out float a_decayFactor)
        {
            return Internal_GetDecayFactor(a_nvar.pointer, out a_decayFactor);
        }

        /// <summary>
        /// Sets the decay factor
        /// </summary>
        /// <remarks>
        /// <para>
        /// Sets the decay factor for sources in this processing context. The
        /// decay factor controls the longevity of energy from reflections
        /// according to the equation(1-decayFactor)^N
        /// where N is the number of traces.A decay factor of 0.8, for example,
        /// results in a path's contributed energy being reducing to &lt; 1% within 21
        /// traces from the trace when the path is originally discovered.
        /// </para>
        /// 
        /// <para>
        /// The default decay factor is <see cref="DefaultDecayFactor"/> if
        /// this function is not called.
        /// </para>
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_decayFactor">Decay factor. Must be in the range (0.0, 1.0].</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_decayFactor"/> is not in the range
        ///           (0.0f, 1.0f).</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status SetDecayFactor(Context a_nvar, float a_decayFactor)
        {
            return Internal_SetDecayFactor(a_nvar.pointer, a_decayFactor);
        }

        /// <summary>
        /// Gets the CUDA device number from the NVAR processing context
        /// </summary>
        /// <remarks>
        /// Returns the CUDA device number specified to create the NVAR processing context.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_deviceNum">Returned device number</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_deviceNum"/> is NULL</para>
        /// </returns>
        internal static Status GetDeviceNum(Context a_nvar, out int a_deviceNum)
        {
            return Internal_GetDeviceNum(a_nvar.pointer, out a_deviceNum);
        }

        /// <summary>
        /// Gets the location of the listener
        /// </summary>
        /// <remarks>
        /// Returns the location of the listener in the scene.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_location">Returned location of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_location"/> is NULL</para>
        /// </returns>
        internal static Status GetListenerLocation(Context a_nvar, out Vector3 a_location)
        {
            // Get NVAR listener location
            Float3 location;
            Status status = Internal_GetListenerLocation(a_nvar.pointer, out location);

            // Convert NVAR location to Unity Vector3
            a_location = (Vector3)location;

            return status;
        }

        /// <summary>
        /// Sets the location of the listener
        /// </summary>
        /// <remarks>
        /// Sets the location of the listener in the scene. 
        /// The default orientation of the listener is (0.0f,
        /// 0.0f, -1.0f) for the forward vector and(0.0f, 1.0f, 0.0f)
        /// for the up vector. These defaults do not imply a preferred 
        /// coordinate system.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_location">The location of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status SetListenerLocation(Context a_nvar, Vector3 a_location)
        {
            return Internal_SetListenerLocation(a_nvar.pointer, (Float3)a_location);
        }

        /// <summary>
        /// Gets the orientation of the listener
        /// </summary>
        /// <remarks>
        /// Returns the orientation of the listener in the scene.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_forward">Returned forward orientation of the listener</param>
        /// <param name="a_up">Returned up orientation of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context, or <see cref="a_forward"/> or <see cref="a_up"/> is NULL.</para>
        /// </returns>
        /// <summary>
        /// Gets the orientation of the listener
        /// </summary>
        /// <remarks>
        /// Returns the orientation of the listener in the scene.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_forward">Returned forward orientation of the listener</param>
        /// <param name="a_up">Returned up orientation of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context, or <see cref="a_forward"/> or <see cref="a_up"/> is NULL.</para>
        /// </returns>
        internal static Status GetListenerOrientation(Context a_nvar, out Vector3 a_forward, out Vector3 a_up)
        {
            // Get NVAR listener orientation
            Float3 forward, up;
            Status status = Internal_GetListenerOrientation(a_nvar.pointer, out forward, out up);

            // Assign out parameters
            a_forward   = (Vector3)forward;
            a_up        = (Vector3)up;

            return status;
        }

        /// <summary>
        /// Sets the orientation of the listener
        /// </summary>
        /// <remarks>
        /// Sets the forward and up orthogonal orientation
        /// vectors of the listener in the scene. The forward vector is
        /// directed away from the listener in the direction the listener
        /// is facing. The up vector is directed away from the top of
        /// listener. The specified vectors must be orthogonal.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_forward">The forward orientation of the listener</param>
        /// <param name="a_up">The up orientation of the listener</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred.</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context, or <see cref="a_forward"/> and 
        ///           <see cref="a_up"/> are not orthogonal.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status SetListenerOrientation(Context a_nvar, Vector3 a_forward, Vector3 a_up)
        {
            return Internal_SetListenerOrientation(a_nvar.pointer, (Float3)a_forward, (Float3)a_up);
        }

        /// <summary>
        /// Gets the output format
        /// </summary>
        /// <remarks>
        /// Returns the output format of filters or filtered audio from the NVAR processing context.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_outputFormat">Returned output format</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_outputFormat"/> is NULL.</para>
        ///     <para></para>
        /// </returns>
        internal static Status GetOutputFormat(Context a_nvar, out OutputFormat a_outputFormat)
        {
            return Internal_GetOutputFormat(a_nvar.pointer, out a_outputFormat);
        }

        /// <summary>
        /// Sets the output format
        /// </summary>
        /// <remarks>
        /// Sets the output format of filters or filtered audio
        /// from the NVAR processing context.If this function is not called,
        /// the default output format
        /// <see cref="OutputFormat.OutputFormatStereoHeadphones"/> is used.
        /// This function can be expensive because of reallocation
        /// of internal buffers; it should ideally called only once,
        /// before any sources have been created.
        /// Audio continuity is not guaranteed across calls to this function.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_outputFormat">The output format</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_outputFormat"/> 
        ///           is not a valid output format.</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        /// </returns>
        internal static Status SetOutputFormat(Context a_nvar, OutputFormat a_outputFormat)
        {
            return Internal_SetOutputFormat(a_nvar.pointer, a_outputFormat);
        }

        /// <summary>
        /// Gets the re-verb length
        /// </summary>
        /// <remarks>
        /// Returns the re-verb length, in seconds, from the NVAR processing context.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_reverbLength">Returned re-verb length</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_reverbLength"/> is NULL.</para>
        /// </returns>
        internal static Status GetReverbLength(Context a_nvar, out float a_reverbLength)
        {
            return Internal_GetReverbLength(a_nvar.pointer, out a_reverbLength);
        }

        /// <summary>
        /// Sets the re-verb length
        /// </summary>
        /// <remarks>
        /// Sets the re-verb length, in seconds, in the NVAR
        /// processing context. If this function is not called, the
        /// default re-verb length given by <see cref="DefaultReverbLength"/>
        /// is used. This function can be expensive because of reallocation
        /// of internal buffers. It should ideally called once before any
        /// sources exist. Audio continuity is not guaranteed across
        /// calls to this function. The API does not restrict the re-verb length
        /// to enable non-real-time uses. Real-time applications should take care
        /// in setting this value.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_reverbLength">Re-verb length, in seconds. Must be in the range (0.0, Inf).</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_reverbLength"/> is not in the range(0.0f, inf).</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        /// </returns>
        internal static Status SetReverbLength(Context a_nvar, float a_reverbLength)
        {
            return Internal_SetReverbLength(a_nvar.pointer, a_reverbLength);
        }

        /// <summary>
        /// Gets the sample rate
        /// </summary>
        /// <remarks>
        /// Returns the sample rate in samples per second of sound sources in the NVAR processing context.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_sampleRate">Returned sample rate in samples per second</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_sampleRate"/> is NULL</para>
        /// </returns>
        internal static Status GetSampleRate(Context a_nvar, out int a_sampleRate)
        {
            return Internal_GetSampleRate(a_nvar.pointer, out a_sampleRate);
        }

        /// <summary>
        /// Sets the sample rate
        /// </summary>
        /// <remarks>
        /// Sets the sample rate in samples per second of sound
        /// sources in the NVAR processing context.The default
        /// sample rate if this function is not called is
        /// <see cref="DefaultSampleRate"/> hertz. This function can be 
        /// expensive because of reallocation
        /// of internal buffers. It should ideally called once before any
        /// sources exist.Audio continuity is not guaranteed across
        /// calls to this function.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_sampleRate">Sample rate. Must be in the range (<see cref="MinSampleRate"/>, inf).</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.NVAR_STATUS_INVALID_VALUE"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_sampleRate"/> 
        ///     is not in the range [<see cref="MinSampleRate"/>, inf].</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        /// </returns>
        internal static Status SetSampleRate(Context a_nvar, int a_sampleRate)
        {
            return Internal_SetSampleRate(a_nvar.pointer, a_sampleRate);
        }

        /// <summary>
        /// Gets the units per meter from the NVAR processing context
        /// </summary>
        /// <remarks>
        /// Returns the units per meter from the NVAR
        /// processing context. The default unit length per meter ratio
        /// is <see cref="DefaultUnitLengthPerMeterRatio"/>.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_ratio">Returned unit length per meter ratio</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_ratio"/> is NULL.</para>
        /// </returns>
        internal static Status GetUnitLength(Context a_nvar, out float a_ratio)
        {
            return Internal_GetUnitLength(a_nvar.pointer, out a_ratio);
        }

        /// <summary>
        /// Sets the unit length per meter ratio of the NVAR processing context
        /// </summary>
        /// <remarks>
        /// Sets the unit length per meter ratio of the
        /// NVAR processing context. If each unit in the geometry passed
        /// to the NVAR processing context is specified in centimetres, for example,
        /// a unit length per meter of 0.01 units per meter gives the processing
        /// context the appropriate scale. If this function is not called,
        /// the default unit length per meter ratio
        /// <see cref="DefaultUnitLengthPerMeterRatio"/> is used.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_ratio">The new unit length per meter ratio. Must be in the range(0.0, Inf).</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_ratio"/> is not in the range (0.0f, inf).</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status SetUnitLength(Context a_nvar, float a_ratio)
        {
            return Internal_SetUnitLength(a_nvar.pointer, a_ratio);
        }

        /// <summary>
        /// Wait for nvar command stream to idle
        /// </summary>
        /// <remarks>
        /// Blocks the calling thread until all activity in the 
        /// asynchronous command queue has been completed. Can be used
        /// to ensure synchronisation between the NVAR processing context
        /// and the calling thread. 
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status Synchronize(Context a_nvar)
        {
            return Internal_Synchronize(a_nvar.pointer);
        }

        /// <summary>
        /// Traces the audio paths between the listener and the sound sources
        /// </summary>
        /// <remarks>
        /// <para>
        /// Schedule an acoustic trace. Acoustic traces are the main 
        /// computation of NVAR that trace paths between all sources and 
        /// the listener in the specified geometry. The result of an acoustic
        /// trace is a set of filters.
        /// </para>
        ///
        /// <para>
        /// <see cref="TraceAudio(Context)"/> returns once the trace has been added to the
        /// asynchronous command queue. The trace will be run asynchronously to the
        /// calling thread. This overload doesn't allow for the calling thread to wait
        /// for the audio trace to complete. See <see cref="TraceAudio(Context, IntPtr)"/>
        /// over the overload.
        /// </para>
        /// 
        /// <para>
        /// Because traceAudio commands are enqueued, applications should use
        /// the <see cref="TraceAudio(Context, IntPtr)"/> overload or <see cref="Synchronize(Context)"/> to
        /// ensure that previously started traces are completed before issuing
        /// new traces. If <see cref="TraceAudio(Context)"/> is called faster  
        /// than traces complete, a backlog of traces will accumulate
        /// in the command queue.
        /// </para>
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context.</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status TraceAudio(Context a_nvar)
        {
            return TraceAudio(a_nvar, IntPtr.Zero);
        }

        /// <summary>
        /// Traces the audio paths between the listener and the sound sources
        /// </summary>
        /// <remarks>
        /// <para>
        /// Schedule an acoustic trace. Acoustic traces are the main 
        /// computation of NVAR that trace paths between all sources and 
        /// the listener in the specified geometry. The result of an acoustic
        /// trace is a set of filters.
        /// </para>
        ///
        /// <para>
        /// <see cref="TraceAudio(Context, IntPtr)"/> returns once the trace has been added to the
        /// asynchronous command queue. The trace will be run asynchronously to the
        /// calling thread. If <see cref="a_traceDoneEvent"/> is not NULL, the Windows event passed
        /// in that argument will be signalled by a call to SetEvent()
        /// once the trace scheduled by this call is completed.
        /// In C# we use the native
        /// handle from <see cref="SafeHandle.DangerousGetHandle(void)"/> from classes which derive from
        /// <see cref="WaitHandle"/> to give NVAR the reference to our event objects.
        /// </para>
        /// 
        /// <para>
        /// Because traceAudio commands are enqueued, applications should use
        /// the <see cref="a_traceDoneEvent"/> or <see cref="Synchronize(Context)"/> to
        /// ensure that previously started traces are completed before issuing
        /// new traces. If <see cref="TraceAudio(Context, IntPtr)"/> is called faster  
        /// than traces complete, a backlog of traces will accumulate
        /// in the command queue.
        /// </para>
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_traceDoneEvent">Native Windows event object that will be signalled when tracing is complete.</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context.</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        ///     <para><see cref="Status.Error"/>: A generic error has occurred.</para>
        /// </returns>
        internal static Status TraceAudio(Context a_nvar, IntPtr a_traceDoneEvent)
        {
            return Internal_TraceAudio(a_nvar.pointer, a_traceDoneEvent);
        }

        #endregion

        #region Acoustic Material Functions

        /// <summary>
        /// Creates an acoustic material
        /// </summary>
        /// <remarks>
        /// Creates an acoustic material with default properties.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_material">Returned material handle</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_nvar"/> is not a valid context or <see cref="a_material"/> is NULL.</para>
        ///     <para><see cref="Status.OutOfResources"/>: An internal allocation has failed.</para>
        /// </returns>
        internal static Status CreateMaterial(Context a_nvar, out Material a_material)
        {
            // Create NVAR acoustic material
            IntPtr materialPointer;
            Status status = Internal_CreateMaterial(a_nvar.pointer, out materialPointer);

            // Create wrapper object for acoustic material
            a_material = new Material(materialPointer);

            return status;
        }

        /// <summary>
        /// Destroys the specified acoustic material
        /// </summary>
        /// <remarks>
        /// Destroys the specified acoustic material. The material should not be currently attached to a mesh object.
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material object.</para>
        ///     <para><see cref="Status.NotReady"/>: The material is still attached to a mesh object.</para>
        /// </returns>
        internal static Status CreatePredefinedMaterial(Context a_nvar, out Material a_material, PredefinedMaterial a_predefinedMaterial)
        {
            // Create NVAR acoustic material
            IntPtr materialPointer;
            Status status = Internal_CreatePredefinedMaterial(a_nvar.pointer, out materialPointer, a_predefinedMaterial);

            // Create wrapper object for acoustic material
            a_material = new Material(materialPointer);

            return status;
        }

        /// <summary>
        /// Destroys the specified acoustic material
        /// </summary>
        /// <remarks>
        /// Destroys the specified acoustic material. The material should not be currently attached to a mesh object.
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material object.</para>
        ///     <para><see cref="Status.NotReady"/>: The material is still attached to a mesh object.</para>
        /// </returns>
        internal static Status DestroyMaterial(Material a_material)
        {
            return Internal_DestroyMaterial(a_material.pointer);
        }

        /// <summary>
        /// Gets the reflection coefficient of the acoustic material
        /// </summary>
        /// <remarks>
        /// This function gets the reflection coefficient of the acoustic material.
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <param name="a_reflection">Returned reflection coefficient</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material or <see cref="a_reflection"/> is NULL.</para>
        /// </returns>
        internal static Status GetMaterialReflection(Material a_material, out float a_reflection)
        {
            return Internal_GetMaterialReflection(a_material.pointer, out a_reflection);
        }

        /// <summary>
        /// Sets the reflection coefficient of the acoustic material
        /// </summary>
        /// <remarks>
        /// This function sets the reflection coefficient of the acoustic
        /// material.Physically, this value should be in the range 
        /// [0, 1], and the reflection coefficient and transmission
        /// coefficients should have a sum &lt;= 1.0. The API does not 
        /// enforce this restriction
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <param name="a_reflection">Reflection coefficient</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material or <see cref="a_reflection"/> is not in the range[0.0, 1.0].</para>
        /// </returns>
        internal static Status SetMaterialReflection(Material a_material, float a_reflection)
        {
            return Internal_SetMaterialReflection(a_material.pointer, a_reflection);
        }

        /// <summary>
        /// Gets the transmission coefficient of the acoustic material
        /// </summary>
        /// <remarks>
        /// Returns the transmission coefficient of the acoustic material.
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <param name="a_transmission">Returned transmission coefficient</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material or <see cref="a_transmission"/> is NULL.</para>
        /// </returns>
        internal static Status GetMaterialTransmission(Material a_material, out float a_transmission)
        {
            return Internal_GetMaterialTransmission(a_material.pointer, out a_transmission);
        }

        /// <summary>
        /// Sets the transmission coefficient of the acoustic material
        /// </summary>
        /// <remarks>
        /// This function sets the transmission coefficient of the acoustic
        /// material.Physically, this value should be in the range 
        /// [0, 1], and the reflection coefficient and transmission
        /// coefficients should have a sum &lt;= 1.0. The API does not 
        /// enforce this restriction.
        /// </remarks>
        /// <param name="a_material">The material object</param>
        /// <param name="transmission">Transmission coefficient</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_material"/> is not a valid material or <see cref="a_transmission"/> is not in the range[0.0, 1.0].</para>
        /// </returns>
        internal static Status SetMaterialTransmission(Material a_material, float a_transmission)
        {
            return Internal_SetMaterialTransmission(a_material.pointer, a_transmission);
        }

        #endregion

        #region Acoustic Mesh Functions

        /// <summary>
        /// Creates an acoustic mesh
        /// </summary>
        /// <remarks>
        /// Creates an acoustic mesh from the vertices, faces, and acoustic
        /// material. The function scales and places the mesh in the scene
        /// using the specified transformation matrix. Changes will be incorporated
        /// into the scene when the next call to::nvarCommitGeometry
        /// or <see cref="nvarTraceAudio"/> is executed.
        /// </remarks>
        /// <param name="a_nvar">The NVAR processing context</param>
        /// <param name="a_mesh">Returned mesh object</param>
        /// <param name="a_transform">The transform of the mesh</param>
        /// <param name="a_vertices">The array of vertices</param>
        /// <param name="a_faces">The array of faces</param>
        /// <param name="a_material">The material applied to the mesh</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: Possible causes: <see cref="a_nvar"/> is not a valid context;
        ///           <see cref="a_mesh"/>, <see cref="a_transform"/>, <see cref="a_vertices"/>, or <see cref="a_faces"/>
        ///           is NULL; or the <see cref="a_material"/> value is not valid.</para>
        /// </returns>
        internal static Status CreateMesh(Context a_nvar, out Mesh a_mesh, UnityEngine.Matrix4x4 a_transform, Vector3[] a_vertices, int[] a_faces, Material a_material)
        {
            // Convert vertices array to NVAR type
            Float3[] nvarVertices = Array.ConvertAll(a_vertices, vertice => (Float3)vertice);

            // Create NVAR mesh
            IntPtr nvarMesh;
            Status status = Internal_CreateMesh(a_nvar.pointer, out nvarMesh, (Matrix4x4)a_transform, nvarVertices, nvarVertices.Length,
                a_faces, a_faces.Length, a_material.pointer);

            // Create wrapped NVAR Mesh
            a_mesh = new Mesh(nvarMesh);

            return status;
        }

        /// <summary>
        /// Destroys the specified acoustic mesh
        /// </summary>
        /// <remarks>
        /// Destroys the specified acoustic mesh and
        /// releases any associated resources. The mesh will be removed from
        /// the scene when the next call to <see cref="CommitGeometry(Context)"/> or
        /// <see cref="TraceAudio(Context, IntPtr)"/> is executed.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh object.</para>
        /// </returns>
        internal static Status DestroyMesh(Mesh a_mesh)
        {
            return Internal_DestroyMesh(a_mesh.pointer);
        }

        /// <summary>
        /// Gets the acoustic material of the mesh
        /// </summary>
        /// <remarks>
        /// Returns the acoustic material applied to the specified mesh.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <param name="a_material">Returned material object</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh or <see cref="a_material"/> is NULL.</para>
        /// </returns>
        internal static Status GetMeshMaterial(Mesh a_mesh, out Material a_material)
        {
            // Get NVAR acoustic material
            IntPtr nvarMaterial;
            Status status = Internal_GetMeshMaterial(a_mesh.pointer, out nvarMaterial);

            // Create wrapped acoustic material object
            a_material = new Material(nvarMaterial);

            return status;
        }

        /// <summary>
        /// Sets the acoustic material of the mesh
        /// </summary>
        /// <remarks>
        /// Sets acoustic material of the specified mesh.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <param name="a_material">Valid material handle</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh or <see cref="a_material"/> is not a valid material.</para>
        /// </returns>
        internal static Status SetMeshMaterial(Mesh a_mesh, Material a_material)
        {
            return Internal_SetMeshMaterial(a_mesh.pointer, a_material.pointer);
        }

        /// <summary>
        /// Gets the transform of the mesh
        /// </summary>
        /// <remarks>
        /// Returns the transformation matrix of the specified mesh.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <param name="a_transform">Returned transformation matrix</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh or <see cref="a_transform"/> is NULL.</para>
        /// </returns>
        internal static Status GetMeshTransform(Mesh a_mesh, out UnityEngine.Matrix4x4 a_transform)
        {
            // Get NVAR acoustic mesh transform
            Matrix4x4 matrix;
            Status status = Internal_GetMeshTransform(a_mesh.pointer, out matrix);

            // Convert NVAR matrix to Unity matrix
            a_transform = (UnityEngine.Matrix4x4)matrix;

            return status;
        }

        /// <summary>
        /// Sets the transform of the mesh
        /// </summary>
        /// <remarks>
        /// Sets transformation matrix for the specified mesh object.
        /// </remarks>
        /// <param name="a_mesh">Valid mesh handle</param>
        /// <param name="a_transform">The transformation matrix</param>
        /// <returns>
        ///     <para><see cref="Status.Success"/>: No error has occurred</para>
        ///     <para><see cref="Status.NotInitialized"/>: <see cref="Initialize(int)"/> has not been called.</para>
        ///     <para><see cref="Status.InvalidValue"/>: <see cref="a_mesh"/> is not a valid mesh.</para>
        /// </returns>
        internal static Status SetMeshTransform(Mesh a_mesh, UnityEngine.Matrix4x4 a_transform)
        {
            return Internal_SetMeshTransform(a_mesh.pointer, (Matrix4x4)a_transform);
        }

        #endregion
    }
}
