using System;

namespace AccreteSharp
{
    /// <summary> Given a Planet object, a PlanetClassifier will supply descriptive
    /// text and a graphical image representing the planet.  You need one
    /// PlanetClassifier for each classification scheme you want to implement.
    /// Override the methods of this class to change the scheme, or reimplement
    /// the class to produce general behavior instead of the hard-wired stuff here.
    /// <p><strong><center>Copyright information</center></strong></p>
    /// <p>This Java class is copyright 1998 by Carl Burke. All rights reserved.
    /// Substantial sections of this code were previously distributed in
    /// different form as part of 'starform' (copyright 1989 Matthew Burdick)<p>
    /// <p>This software is provided absolutely free and without warranty,
    /// including but not limited to the implied warranties of merchantability
    /// and fitness for a purpose.  You may use this code for any legal purpose
    /// provided that you do not charge for it; this implies that you <em>may</em>
    /// use this code as a component of a commercial system as long as the additional
    /// functionality of the commercial system is greater than what this code
    /// provides and that the commercial system is not primarily intended as
    /// a simulation of solar system formation.  In other words, if you want to
    /// write a science-fiction computer game that uses the code in this package
    /// to build objects which are used in the game, that's great and permitted;
    /// if you use this code to make a kickass solar-system-builder, you are not
    /// allowed to distribute that software except for free.
    /// <p>You are allowed and encouraged to modify this software, provided that
    /// this copyright notice remains intact.  This notice may be reformatted,
    /// but not removed.
    /// <p>If you do use this software, I and the contributing authors listed
    /// under "Acknowledgements" would appreciate some recognition.  If you make
    /// changes, I would appreciate it if you would pass those changes back to me
    /// for possible inclusion in the master.  At the time this notice was prepared,
    /// my email address is <a href="mailto:cburke@mitre.org">cburke@mitre.org</a> and the home page for this software is
    /// <a href="http://www.geocities.com/Area51/6902/w_accr.html">http://www.geocities.com/Area51/6902/w_accr.html</a>.
    /// <a name="ack"><strong><center>Acknowledgements</center></strong></a>
    /// <p>Matt Burdick, the author of 'starform' (freeware copyright 1989);
    /// much of the code (particularly planetary environments) was adapted from this.</p>
    /// <p>Andrew Folkins, the author of 'accretion' (public domain) for the Amiga; I used chunks
    /// of his code when creating my displays.</p>
    /// <p>Ed Taychert of <a href="http://www.irony.com/">Irony Games</a>, for the algorithm he uses 
    /// to classify terrestrial planets in his tabular CGI implementation of 'starform'.</p>
    /// <p>Paul Schlyter, who provided information about 
    /// <a href="http://spitfire.ausys.se/psr/comp/ppcomp.html">computing planetary positions</a>.</p>
    /// </summary>
    //UPGRADE_ISSUE: Interface 'java.awt.image.ImageObserver' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageObserver'"
    public class PlanetClassifier //: ImageObserver
    {


        /// <summary> Returns a string describing the planet in general terms.</summary>
        /// <param name="p">Planet object to be described
        /// </param>
        /// <returns>s Constant string description
        /// </returns>
        public virtual System.String planetType(Planet p)
        {
            if (p.mass < 0.01)
                return "Asteroidal";
            if (p.gas_giant)
            {
                //I use effective temp based on distance from sun, becuse gas giants don't
                //have a surface, and thus don't have surface temperature
                string hot = (p.eff_temp(p.r_ecosphere) > 500) ? "Hot, " : "";
                if (p.mass < 50.0)
                    return hot + "Small gas giant";
                else if (p.mass > 1000.0)
                    return hot + "Brown dwarf";
                else
                    return hot + "Large gas giant";
            }
            else
            {
                if ((p.age > 1e+9) // changed from 2.7, it's in megayears
                    && (p.a > 0.8 * p.r_ecosphere)
                    && (p.a < 1.2 * p.r_ecosphere)
                    && (p.day < 96.0)
                    && (p.surf_temp > (273.15 - 30.0)) //changed from -1     Even if average temp not suitable, on equator/poles can be habitable
                    && (p.surf_temp < (273.15 + 40.0)) //changed from +30
                    && (p.surf_pressure > 360)         //changed from 0.36 ? 
                    && (p.surf_pressure < 2600.0)
                    && ((p.ice_cover + p.hydrosphere < 1.0)
                    && (p.hydrosphere > 0.0)))
                    return "Habitable";
                if ((p.mass > 0.4) && (p.a > 0.65 * p.r_ecosphere) && (p.a < 1.35 * p.r_ecosphere) && (p.surf_temp > (273.15 - 45.0)) && (p.surf_pressure > 0.05) && (p.surf_pressure < 8000.0) && ((p.ice_cover > 0.0) || (p.hydrosphere > 0.0)))
                    return "Marginally habitable";
                if (p.ice_cover > 0.95)
                    return "Iceworld";
                if (p.hydro_fraction() > 0.95
                    && p.avg_temp > PhysicalConstants.FREEZING_POINT_OF_WATER
                    && p.avg_temp < p.boil_point)
                    return "Waterworld";
                if (p.surf_temp < 100.0)
                    return "Frigid airless rock";
                if (p.surf_pressure < 0.01)
                {
                    if (p.surf_temp < 273.0)
                        return "Cold airless rock";
                    return "Hot airless rock";
                }
                else if (p.surf_pressure > 10000.0)
                {
                    if (p.surf_temp > 273.0)
                        return "Hot, dense atmosphere";
                    else
                        return "Cold, dense atmosphere";
                }
                else
                {
                    if (p.surf_temp > (((p.boil_point - 273.0) / 2.0) + 273.0))
                        return "Hot terrestrial";
                    if (p.surf_temp < 273.0)
                        return "Frozen terrestrial";
                    return "Terrestrial";
                }
            }
        }
    }
}