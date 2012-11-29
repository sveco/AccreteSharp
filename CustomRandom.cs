using System;
namespace AccreteSharp
{
    /// <summary> This class provides a random number generator with additional
    /// access methods.
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
    [Serializable]
    public class CustomRandom : System.Random
    {
        protected internal bool normdone = false;
        protected internal double normstore;

        /// <summary> Default constructor.  Calls the constructor for Random with no seed.</summary>
        public CustomRandom()
            : base()
        {
        }

        /// <summary> Default constructor.  Calls the constructor for Random with the
        /// specified seed.
        /// </summary>
        public CustomRandom(long seed)
            : base((System.Int32)seed)
        {
        }

        /// <summary> Sets the random seed to the new value, performing additional housekeeping.</summary>
        /// <param name="seed">New pseudo-random generator seed
        /// </param>
        //UPGRADE_NOTE: The equivalent of method 'java.util.Random.setSeed' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
        public void setSeed(long s)
        {
            //UPGRADE_TODO: The differences in the expected value  of parameters for method 'java.util.Random.setSeed'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            Random tmp = new System.Random((System.Int32)s);
            normdone = false;
        }

        /// <summary> Produces a Gaussian random variate with mean=0, standard deviation=1.</summary>
        public virtual double NormalDeviate()
        {
            double v1, v2, r, fac;

            if (normdone)
            {
                normdone = false;
                return normstore;
            }
            else
            {
                v1 = 2.0 * NextDouble() - 1.0;
                v2 = 2.0 * NextDouble() - 1.0;
                r = v1 * v1 + v2 * v2;
                if (r >= 1.0)
                {
                    return NormalDeviate();
                }
                else
                {
                    fac = (double)(System.Math.Sqrt((-2.0) * System.Math.Log(r) / (double)(r)));
                    normstore = v1 * fac;
                    normdone = true;
                    return v2 * fac;
                }
            }
        }

        /// <summary> Produces a random variate whose natural logarithm is from the
        /// Gaussian with mean=0 and the specified standard deviation.
        /// </summary>
        /// <param name="sigma">Standard deviation
        /// </param>
        public virtual double LognormalDeviate(double sigma)
        {
            return (double)(System.Math.Exp((double)(NormalDeviate() * sigma)));
        }

        /// <summary> Returns a uniformly distributed random real number between the specified
        /// inner and outer bounds.
        /// </summary>
        /// <param name="inner">Minimum value desired
        /// </param>
        /// <param name="outer">Maximum value desired
        /// </param>
        public virtual double random_number(double inner, double outer)
        {
            double range = outer - inner;
            return (NextDouble() * range + inner);
        }

        /// <summary>   Returns a value within a certain uniform variation
        /// from the central value.
        /// </summary>
        /// <param name="value">Central value
        /// </param>
        /// <param name="variation">Maximum (uniform) variation above or below center
        /// </param>
        public virtual double about(double value_Renamed, double variation)
        {
            return (value_Renamed + (value_Renamed * random_number(-variation, variation)));
        }

        /// <summary> Returns a value for orbital eccentricity between 0.0 and 1.0</summary>
        public virtual double random_eccentricity()
        {
            return (1.0 - System.Math.Pow(NextDouble(), PhysicalConstants.ECCENTRICITY_COEFF));
        }
    }
}