using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AccreteSharp.Ssx
{
    [Serializable]
    public class StarSystem
    {
        [XmlAttribute]
        public string Name { get; set; }
        public List<Body> Bodies;

        public StarSystem()
        {
            Bodies = new List<Body>();
        }
    }

    [Serializable]
    public class Body
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public double Mass { get; set; }
        [XmlAttribute]
        public double Radius { get; set; }
        [XmlAttribute]
        public double Period { get; set; }
        [XmlAttribute]
        public double XPosition { get; set; }
        [XmlAttribute]
        public double YPosition { get; set; }
        [XmlAttribute]
        public double ZPosition { get; set; }
        [XmlAttribute]
        public double XVelocity { get; set; }
        [XmlAttribute]
        public double YVelocity { get; set; }
        [XmlAttribute]
        public double ZVelocity { get; set; }
    }

    public class SystemDisplaySsx
    {
        public void SerializeXml(IList<AccreteSharp.StarSystem> starSystems)
        {
            if (starSystems != null)
            {
                MemoryStream memXmlStream = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(typeof(StarSystem), null, new Type[] { typeof(Body), }, null, null, null);

                StarSystem ssx = new AccreteSharp.Ssx.StarSystem();
                ssx.Name = "Generated";
                ssx.Bodies.Add(
                    new Body
                    {
                        Name = "Primary",
                        Mass = starSystems[0].Primary.SM * PhysicalConstants.SOLAR_MASS_IN_GRAMS / 1000,
                        Period = 10832,
                        Radius = starSystems[0].Primary.radius * PhysicalConstants.SUN_RADIUS,
                        XPosition = 0,
                        YPosition = 0,
                        ZPosition = 0,
                        XVelocity = 0,
                        YVelocity = 0,
                        ZVelocity = 0
                    });
                int c = 0;
                foreach (Planet p in starSystems[0].planetsList)
                {
                    c++;
                    ssx.Bodies.Add(
                        new Body
                        {
                            Name = "Planet" + c.ToString(),
                            Mass = p.mass * PhysicalConstants.EARTH_MASS_IN_GRAMS / 1000,
                            Period = p.orb_period,
                            Radius = p.radius,
                            XPosition = p.X,
                            YPosition = p.Y,
                            ZPosition = p.Z,
                            XVelocity = 0,
                            YVelocity = p.Velocity,
                            ZVelocity = 0
                        });
                }

                serializer.Serialize(memXmlStream, ssx);

                XmlDocument xmlDoc = new XmlDocument();

                memXmlStream.Seek(0, SeekOrigin.Begin);
                xmlDoc.Load(memXmlStream);

                XmlTextWriter wr = new XmlTextWriter("system.ssx", System.Text.Encoding.ASCII);
                wr.Formatting = Formatting.Indented;
                wr.IndentChar = '\t';
                wr.Indentation = 1;

                XmlWriterSettings settings = new XmlWriterSettings();
                XmlWriter writer = XmlWriter.Create(wr, settings);

                xmlDoc.WriteTo(writer);
                writer.Flush();
            }
        }
    }
}
