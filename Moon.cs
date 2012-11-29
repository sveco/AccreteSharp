using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AccreteSharp
{
    class Moon : Planet
    {
        public bool planetary_ring = false; //indicates wether the moon is inside roche lobe and has been destroyed by tidal forces

        //private Moon(Protoplanet p) : base (p)
        //{

        //}
    }
}
