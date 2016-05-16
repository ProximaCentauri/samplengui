using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PsxNet;
using RPSWNET;
using System.Threading;

namespace SSCOControls
{
    public class ControlsAudio : Audio
    {
        public ControlsAudio(): base()
        {}

        public static new void PlaySound(string sound)
        {
            try
            {
                if (null != sound)
                {
                    Audio.PlaySound(sound);
                }
            }catch(PsxException ex)
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "ControlsAudio.PlaySound({0}) - psx exception: {1}", sound, ex.Message);
            }
        }       
    }
}
