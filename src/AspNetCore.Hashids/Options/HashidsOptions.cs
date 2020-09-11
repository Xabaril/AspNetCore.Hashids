namespace AspNetCore.Hashids.Options
{
    public class HashidsOptions
    {
        /// <summary>
        /// Salt (Default "")
        /// </summary>
        public string Salt { get; set; } = "";
        /// <summary>
        /// MinHashLength (Default value 0)
        /// </summary>
        public int MinHashLength { get; set; } = 0;
        /// <summary>
        /// Alphabet (Default value "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")
        /// </summary>
        public string Alphabet { get; set; } = HashidsNet.Hashids.DEFAULT_ALPHABET;
        /// <summary>
        /// Steps (Default "cfhistuCFHISTU")
        /// </summary>
        public string Steps { get; set; } = HashidsNet.Hashids.DEFAULT_SEPS;
    }
}
