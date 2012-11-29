using System;
using System.Collections;
using System.Xml.Serialization;

namespace AccreteSharp
{

    /// <summary> Implements a body which can accrete dust and gas.
    /// <h1>Copyright information</h1>
    /// This Java class is copyright 1998 by Carl Burke. All rights reserved.
    /// Substantial sections of this code were previously distributed in
    /// different form as part of 'starform' (copyright 1989 Matthew Burdick)
    /// This software is provided absolutely free and without warranty,
    /// including but not limited to the implied warranties of merchantability
    /// and fitness for a purpose.  You may use this code for any legal purpose
    /// provided that you do not charge for it; this implies that you may
    /// use this code as a component of a commercial system as long as the additional
    /// functionality of the commercial system is greater than what this code
    /// provides and that the commercial system is not primarily intended as
    /// a simulation of solar system formation.  In other words, if you want to
    /// write a science-fiction computer game that uses the code in this package
    /// to build objects which are used in the game, that's great and permitted;
    /// if you use this code to make a kickass solar-system-builder, you are not
    /// allowed to distribute that software except for free.
    /// You are allowed and encouraged to modify this software, provided that
    /// this copyright notice remains intact.  This notice may be reformatted,
    /// but not removed.
    /// If you do use this software, I and the contributing authors listed
    /// under "Acknowledgements" would appreciate some recognition.  If you make
    /// changes, I would appreciate it if you would pass those changes back to me
    /// for possible inclusion in the master.  At the time this notice was prepared,
    /// my email address is <a href="mailto:cburke@mitre.org">cburke@mitre.org</a> and the home page for this software is
    /// <a href="http://www.geocities.com/Area51/6902/w_accr.html">http://www.geocities.com/Area51/6902/w_accr.html</a>.
    /// <a name="ack"><strong><center>Acknowledgements</center></strong></a>
    /// Matt Burdick, the author of 'starform' (freeware copyright 1989);
    /// much of the code (particularly planetary environments) was adapted from this.
    /// Andrew Folkins, the author of 'accretion' (public domain) for the Amiga; I used chunks
    /// of his code when creating my displays.
    /// Ed Taychert of <a href="http://www.irony.com/">Irony Games</a>, for the algorithm he uses 
    /// to classify terrestrial planets in his tabular CGI implementation of 'starform'.
    /// Paul Schlyter, who provided information about 
    /// <a href="http://spitfire.ausys.se/psr/comp/ppcomp.html">computing planetary positions</a>.
    /// </summary>
    public class Protoplanet : AccreteObject
    {
        public static double PROTOPLANET_MASS = (1.0e-15) * random_number(0.5, 1.5); // Units of solar masses
        public double a;
        public double e;
        public double mass;
        public double reduced_mass; // used in some calculations; does not replace mass!
        public double crit_mass;
        public double dust_density;
        public bool gas_giant;

        [XmlIgnore]
        public ArrayList moons;
        //public Protoplanet next_planet;

        /// <summary> Calculates innermost limit of gravitational influence.
        /// The limit depends on orbital eccentricity of the protoplanet
        /// and the shape of the initial cloud as well as the mass.
        /// </summary>
        /// <param name="cloud_eccentricity">Eccentricity of the dust disc (0.0 to 1.0)
        /// </param>
        /// <returns>s Inner effect limit in AU
        /// </returns>
        public virtual double inner_effect_limit(double cloud_eccentricity)
        {
            return (a * (1.0 - e) * (1.0 - mass) / (1.0 + cloud_eccentricity));
        }

        /// <summary> Calculates outermost limit of gravitational influence.
        /// The limit depends on orbital eccentricity of the protoplanet
        /// and the shape of the initial cloud as well as the mass.
        /// </summary>
        /// <param name="cloud_eccentricity">Eccentricity of the dust disc (0.0 to 1.0)
        /// </param>
        /// <returns>s Outer effect limit in AU
        /// </returns>
        public virtual double outer_effect_limit(double cloud_eccentricity)
        {
            return (a * (1.0 + e) * (1.0 + mass) / (1.0 - cloud_eccentricity));
        }

        /// <summary> Calculates innermost limit of gravitational influence.
        /// This version uses 'reduced mass'... I don't have a copy of Dole's
        /// paper here, so I'm not sure what that really means.
        /// The limit depends on orbital eccentricity of the protoplanet
        /// and the shape of the initial cloud as well as the mass.
        /// </summary>
        /// <param name="cloud_eccentricity">Eccentricity of the dust disc (0.0 to 1.0)
        /// </param>
        /// <returns>s Inner effect limit in AU
        /// </returns>
        public virtual double inner_reduced_limit(double cloud_eccentricity)
        {
            return (a * (1.0 - e) * (1.0 - reduced_mass) / (1.0 + cloud_eccentricity));
        }

        /// <summary> Calculates outermost limit of gravitational influence.
        /// This version uses 'reduced mass'... I don't have a copy of Dole's
        /// paper here, so I'm not sure what that really means.
        /// The limit depends on orbital eccentricity of the protoplanet
        /// and the shape of the initial cloud as well as the mass.
        /// </summary>
        /// <param name="cloud_eccentricity">Eccentricity of the dust disc (0.0 to 1.0)
        /// </param>
        /// <returns>s Outer effect limit in AU
        /// </returns>
        public virtual double outer_reduced_limit(double cloud_eccentricity)
        {
            return (a * (1.0 + e) * (1.0 + reduced_mass) / (1.0 - cloud_eccentricity));
        }

        /// <summary> Not currently implemented; really should be toString() anyway.</summary>
        public virtual void print()
        {
        }

        /// <summary> Verifies that the protoplanet mass is non-zero and also
        /// different from the injected seed size.
        /// </summary>
        /// <returns>s True if the protoplanet is more than a seed
        /// </returns>
        public virtual bool massOK()
        {
            return (mass != 0.0) && (mass != PROTOPLANET_MASS);
        }

        /// <summary> Determines if the protoplanet is massy enough to accrete gas.</summary>
        /// <returns>s True if the protoplanet is a gas giant.
        /// </returns>
        public virtual bool accretes_gas()
        {
            return (mass > crit_mass);
        }

        /// <summary> Performs the mass 'reduction' calculation for the inner accretion loop.</summary>
        public virtual void reduce_mass()
        {
            if (mass < 0.0)
                reduced_mass = 0.0;
            else
            {
                double temp = mass / (1.0 + mass);
                try
                {
                    reduced_mass = System.Math.Pow(temp, (1.0 / 4.0));
                }
                catch (System.ArithmeticException ae)
                {
                    reduced_mass = 0.0;
                }
            }
        }

        public Protoplanet()
        {

        }

        /// <summary> Constructs a new seed protoplanet at a random location within
        /// the specified range.
        /// </summary>
        /// <param name="in">Minimum semi-major axis of orbit
        /// </param>
        /// <param name="out">Maximum semi-major axis of orbit
        /// </param>
        public Protoplanet(double in_Renamed, double out_Renamed)
        {
            a = random_number(in_Renamed, out_Renamed);
            e = random_eccentricity();
            mass = PROTOPLANET_MASS;
            gas_giant = false;
            crit_mass = 0.0;
            dust_density = 0.0;
            moons = new ArrayList();
            //next_planet = null;
        }

        /// <summary> Copy constructor</summary>
        public Protoplanet(Protoplanet p)
        {
            a = p.a; e = p.e; mass = p.mass; //next_planet = p.next_planet;
            gas_giant = p.gas_giant; crit_mass = p.crit_mass;
            dust_density = p.dust_density;
            moons = new ArrayList();
            if (p.moons != null)
            {
                foreach (Protoplanet moon in p.moons)
                {
                    moons.Add(moon);
                }
            }
        }

        /// <summary> Calculates unit density of material to be accreted from the 
        /// specified dust band.
        /// </summary>
        /// <param name="inner_limit_of_dust">Inner edge of the dust band (in AU)
        /// </param>
        /// <param name="outer_limit_of_dust">Outer edge of the dust band (in AU)
        /// </param>
        public virtual double mass_density(bool dust, bool gas)
        {
            if (!dust)
                return 0.0;
            if (((mass < crit_mass) || (!gas)))
                return dust_density;
            else
                return PhysicalConstants.K * dust_density / (1.0 + System.Math.Sqrt(crit_mass / mass) * (PhysicalConstants.K - 1.0));
        }
    }
}