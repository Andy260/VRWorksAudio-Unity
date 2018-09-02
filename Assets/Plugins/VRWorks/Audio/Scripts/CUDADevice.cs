namespace NVIDIA.VRWorksAudio
{
    /// <summary>
    /// Handle to a CUDA device for use within VRWorks Audio
    /// </summary>
    public sealed class CUDADevice
    {
        #region Properties

        /// <summary>
        /// Device number
        /// </summary>
        public int number { get; }

        /// <summary>
        /// Device name
        /// </summary>
        public string name { get; }

        /// <summary>
        /// Is this device preferred by VRWorks Audio for processing?
        /// </summary>
        public bool isPreferred { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new CUDA device handle for VRWorks Audio contexts
        /// (Not to be used by client code)
        /// </summary>
        /// <param name="a_number">Device number</param>
        /// <param name="a_name">Device name</param>
        /// <param name="a_isPreferred">Is this device preferred by VRWorks Audio for processing?</param>
        internal CUDADevice(int a_number, string a_name, bool a_isPreferred)
        {
            number      = a_number;
            name        = a_name;
            isPreferred = a_isPreferred;
        }

        #endregion
    }
}
