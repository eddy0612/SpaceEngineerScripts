using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Audio;

namespace IngameScript
{
    partial class Program
    {
        class Core_AudioIn
        {
        }

        public partial class Core
        {
            private bool audioInState = false; //JJJ Was it true or false. True == mic input 150 chars avail?

            private Core(Core_AudioIn _) : this(new Core_AudioOut())
            {
                /* n/a
                audioInState = false;
                audioInSampler = new WaveIn()
                {
                    WaveFormat = new WaveFormat(baseFreq, 8, 1)
                };
                audioInSampler.DataAvailable += AudioIn_DataAvailable;
                */
            }

            /* JJJ Removed
            private void AudioIn_DataAvailable(object sender, WaveInEventArgs e)
            {
                /*
                 * for (int i = 0; i < e.BytesRecorded; i++) {
                    audioInState = e.Buffer[i] > 150;
                    CPU_Execute_Block();
                }
            }  */
        }
    }
}