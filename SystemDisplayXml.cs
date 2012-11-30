/////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2007                                                      //
//                                                                         //
// This code is free software under the Creative Commons licence           //
// Attribution-Noncommercial-Share Alike 2.0 Generic                       //
// http://creativecommons.org/licenses/by-nc-sa/2.0/                       //
//                                                                         //          
// Janko Svetlik                                                           //
//                                                                         //
// http://sveco.aspweb.cz                                                  //
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

namespace AccreteSharp
{
    class SystemDisplayXml
    {
        //ArrayList openElements = new ArrayList();

        public void SerializeXml(IList<StarSystem> starSystems)
        {
            MemoryStream memXmlStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(starSystems.GetType(), null, new Type[] { typeof(Planet), typeof(StarSystem) }, new XmlRootAttribute("Stars"), null, null);

            serializer.Serialize(memXmlStream, starSystems);

            XmlDocument xmlDoc = new XmlDocument();

            memXmlStream.Seek(0, SeekOrigin.Begin);
            xmlDoc.Load(memXmlStream);

            XmlProcessingInstruction newPI;
            String PItext = string.Format("type='text/xsl' href='{0}'", "system.xslt");
            newPI = xmlDoc.CreateProcessingInstruction("xml-stylesheet", PItext);

            xmlDoc.InsertAfter(newPI, xmlDoc.FirstChild);

            // Now write the document

            // out to the final output stream

            XmlTextWriter wr = new XmlTextWriter("system.xml", System.Text.Encoding.ASCII);
            wr.Formatting = Formatting.Indented;
            wr.IndentChar = '\t';
            wr.Indentation = 1;

            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(wr, settings);

            xmlDoc.WriteTo(writer);
            writer.Flush();
            //Console.Write(xmlDoc.InnerXml);
        }
    }
}
