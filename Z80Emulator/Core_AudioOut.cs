namespace IngameScript
{
    partial class Program
    {
        /* No speaker noises supported */
        class Core_AudioOut
        {
        }

        public partial class Core
        {
            private Core(Core_AudioOut _) : this(new Core_IO())
            {
            }
        }
    }
}