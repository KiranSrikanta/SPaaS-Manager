﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace EMC.SPaaS.Utility
{
    public class XMLUtility
    {
     public  String CreateDocument(Configuration xmlConfigTemplate)
        {
            XmlDocument tempDoc=null;
            Configuration xmlConfig = new Configuration();
            XmlSerializer xSer = new XmlSerializer(typeof(Configuration));
            xmlConfig = xmlConfigTemplate;
            using (MemoryStream xmlMstream = new MemoryStream())
            {
                xSer.Serialize(xmlMstream, xmlConfig);
                tempDoc.Load(xmlMstream);

            }

                return tempDoc.InnerXml;
        }
        
       
    }
}
