using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// JUNO XML Proxy Entities  
/// </summary>
namespace REWJUNO
{
    public abstract class JUNOProxy 
    {
        public abstract XElement GetProxy();
    }

    public class Craft : JUNOProxy
    {
        private XDocument xDoc;
        private XElement xCraft;
        public List<Part> parts;

        public Craft(XDocument xDoc)
        {
            // INIT
            this.xDoc = xDoc;
            this.xCraft = xDoc.Element("Craft");

            // SET PARTS
            var xParts = this.xCraft.Element("Assembly").Element("Parts");
            this.parts = new List<Part>();
            foreach(var xPart in xParts.Elements())
            {
                Part part = new Part(xPart);
                this.parts.Add(part);
            }
        }

        public override XElement GetProxy()
        {
            return xCraft;
        }

        public string Name { get { return this.xCraft.Attribute("name").Value; } }

        public void SaveXML(string path)
        {
            xDoc.Save(path);
        }
    }

    public class Part : JUNOProxy
    {
        private XElement xPart;
        public List<Modifier> modifiers;


        public Part(XElement xPart)
        {
            // INIT
            this.xPart = xPart;

            // SET MODIFIERS
            this.modifiers = new List<Modifier>();
            foreach(var xModif in xPart.Elements())
            {
                Modifier modif = new Modifier(xModif);
                this.modifiers.Add(modif);
            }
        }

        public override XElement GetProxy()
        {
            return xPart;
        }

        public int id
        {
            get { return (xPart.Attribute("id") != null ? int.Parse( xPart.Attribute("id").Value ): -1); }
        }

        public string partType 
        {
            get { return (xPart.Attribute("partType") != null ? xPart.Attribute("partType").Value : "None"); }
        }

        public string partName
        {
            get { return (xPart.Attribute("name") != null ? xPart.Attribute("name").Value : "None"); }
        }

        public string this[string attrName]
        {
            get
            {
                return xPart.Attribute(attrName) != null ? xPart.Attribute(attrName).Value : null;
            }
            set
            {
                if (xPart.Attribute(attrName) != null)
                {
                    xPart.Attribute(attrName).Value = value;
                }
                else
                {
                    xPart.Add(new XAttribute(attrName, value));
                }
            }
        }




        public Modifier GetModifier(string modifierName)
        {
            var xmodif = this.xPart.Element(modifierName);
            if (xmodif != null)
                return new Modifier(xmodif);
            else
                return null;
        }
        public bool HasModifier(string modifierName)
        {
            return (xPart.Element(modifierName) != null);
        }
        public Modifier AddModifier(string modifierName)
        {
            var newModif = new Modifier(modifierName);
            this.xPart.Add(newModif.xModifier);
            this.modifiers.Add(newModif);
            return newModif;
        }

        public void SetProgram(XElement xProgram)
        {
            if (!HasModifier("FlightProgram"))
            {
                Console.WriteLine("Add FlightProgram Modifier...");
                AddModifier("FlightProgram");
            }

            var xFlightProgram = GetModifier("FlightProgram").GetProxy();

            //Replace
            var xProgramOld = xFlightProgram.Element("Program");
            if (xProgramOld != null) xProgramOld.Remove();
            xFlightProgram.Add(xProgram);
        }
    }

    public class Modifier : JUNOProxy
    {
        public XElement xModifier;

        /// <summary>
        /// Create From Xelement
        /// </summary>
        /// <param name="xModif"></param>
        public Modifier(XElement xModif)
        {
            this.xModifier = xModif;
        }

        /// <summary>
        /// Create Preset  
        /// </summary>
        /// <param name="modifierName"></param>
        public Modifier(string modifierName)
        {
            XElement xModif;
            switch(modifierName)
            {
                case "FlightProgram":
                    {
                        xModif = new XElement("FlightProgram", new XAttribute("powerConsumptionPerInstruction", "0.01"), new XAttribute("broadcastPowerConsumptionPerByte", "0.1") );
                    }
                    break;
                default:
                    {
                        xModif = new XElement("Unknown");
                    }
                    break;
            }

            this.xModifier = xModif;
        }

        /// <summary>
        /// Get Proxy
        /// </summary>
        /// <returns></returns>
        public override XElement GetProxy()
        {
            return xModifier;
        }
        /// <summary>
        /// Attribute
        /// </summary>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public string this[string attrName]
        {
            get
            {
                return xModifier.Attribute(attrName) != null ? xModifier.Attribute(attrName).Value : null;
            }
            set
            {
                if (xModifier.Attribute(attrName) != null)
                {
                    xModifier.Attribute(attrName).Value = value;
                }
                else
                {
                    xModifier.Add(new XAttribute(attrName, value));
                }
            }
        }
    }
}
