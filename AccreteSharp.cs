using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using AccreteSharp.Ssx;

namespace AccreteSharp
{

    public enum SortDirection
    {
        // Summary:
        //     Sort from smallest to largest. For example, from A to Z.
        Ascending = 0,
        //
        // Summary:
        //     Sort from largest to smallest. For example, from Z to A.
        Descending = 1,
    }

    public sealed class PlanetSort : IComparer
    {
        private SortDirection m_direction = SortDirection.Ascending;

        public PlanetSort() : base() { }

        /// <summary>
        /// Can be used for both planet and protoplanet sorting by average distance from star.
        /// This replaced unclear code in Protoplanet.coalesce_planetesimals
        /// </summary>
        public PlanetSort(SortDirection direction)
        {
            this.m_direction = direction;
        }

        int IComparer.Compare(object x, object y)
        {
            if ((x is Protoplanet) && (y is Protoplanet))
            {
                Protoplanet p1 = (Protoplanet)x;
                Protoplanet p2 = (Protoplanet)y;

                if (p1 == null && p2 == null)
                {
                    return 0;
                }
                else if (p1 == null && p2 != null)
                {
                    return (this.m_direction == SortDirection.Ascending) ? -1 : 1;
                }
                else if (p1 != null && p2 == null)
                {
                    return (this.m_direction == SortDirection.Ascending) ? 1 : -1;
                }
                else
                {
                    if (p1.a > 0 && p2.a > 0)
                    {
                        return (this.m_direction == SortDirection.Ascending) ?
                         p1.a.CompareTo(p2.a) :
                         p2.a.CompareTo(p1.a);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if ((x is Planet) && (y is Planet))
            {
                Planet p1 = (Planet)x;
                Planet p2 = (Planet)y;

                if (p1 == null && p2 == null)
                {
                    return 0;
                }
                else if (p1 == null && p2 != null)
                {
                    return (this.m_direction == SortDirection.Ascending) ? -1 : 1;
                }
                else if (p1 != null && p2 == null)
                {
                    return (this.m_direction == SortDirection.Ascending) ? 1 : -1;
                }
                else
                {
                    if (p1.a > 0 && p2.a > 0)
                    {
                        return (this.m_direction == SortDirection.Ascending) ?
                         p1.a.CompareTo(p2.a) :
                         p2.a.CompareTo(p1.a);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                return 0;
            }
        }
    }

    class AccreteSharpApp
    {
        public static ArgumentParser argParser;
        [XmlElement("StarSystems")]
        public static IList<StarSystem> starSystems = new List<StarSystem>();

        [STAThread]
        static void Main(string[] args)
        {
            int stars = 1;
            string displaymode= string.Empty;
            argParser = new ArgumentParser(args);

            if (int.TryParse(argParser.GetArgValue("stars"), out stars))
            {
                if (stars == 0) { stars = 1; }
            }
            displaymode = argParser.GetArgValue("disp");

            switch (displaymode)
            {
                case "xml":

                    SystemDisplayXml sdx = new SystemDisplayXml();

                    for (int i = 1; i <= stars; i++)
                    {
                        starSystems.Add(new StarSystem());
                    }
                    sdx.SerializeXml(starSystems);
                    break;

                case "ssx":

                    SystemDisplaySsx ssx = new SystemDisplaySsx();
                    starSystems.Add(new StarSystem());
                    ssx.SerializeXml(starSystems);
                    break;

                case "2d":
                    SystemDisplay2D sd = new SystemDisplay2D(new StarSystem());
                    sd.ShowDialog();
                    break;

                case "viewer":
                    Console.WriteLine("Not yet implemented");
                    break;

                default:
                    Console.WriteLine("Star system generator v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    Console.WriteLine("accretesharp -disp [xml|2d] (-stars n)");
                    Console.WriteLine(string.Empty);
                    Console.WriteLine("-disp\t\tDisplay mode (currently supported only xml and 2d.");
                    Console.WriteLine("\t\tXml dump output to console, pipe it using \">>\" to file.");
                    Console.WriteLine("-stars\t\tNumber of stars to generate. Keep it below 1000, please.");
                    Console.WriteLine("\t\tIgnored if -disp is set to 2d.");
                    break;
            }
        }
    }
}
