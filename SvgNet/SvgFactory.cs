/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System.Reflection;
using System.Xml;

using SvgNet.Elements;

namespace SvgNet;
/// <summary>
/// Static methods to produce/write/copy Svg documents reside in this class.
/// </summary>
public static class SvgFactory {
    public const string svgNamespaceURI = "http://www.w3.org/2000/svg";
    public const string xlinkNamespaceURI = "http://www.w3.org/1999/xlink";

    /// <summary>
    /// Used by LoadFromXML
    /// </summary>
    public static Hashtable BuildElementNameDictionary() {
        var dict = new Hashtable();
        var asm = Assembly.GetExecutingAssembly();
        Type[] ta = asm.GetExportedTypes();
        foreach (Type t in ta) {
            if (t.IsSubclassOf(typeof(SvgElement)) && !t.IsAbstract) {
                ConstructorInfo ci = t.GetConstructor([]) ?? throw new InvalidOperationException($"Type {t.Name} doesn't have the mandatory public parameterless constructor");
                var e = (SvgElement)ci.Invoke([]);
                if (e.Name != "?" /* default name of abstract SvgElements */) {
                    dict[e.Name] = e.GetType();
                }
            }
        }
        return dict;
    }

    /// <summary>
    /// Create a complete deep copy of the given tree of <c>SvgElement</c> objects.
    /// A new set of elements is created, and if the attributes are cloneable they are deep-copied too.
    /// Since strings and all SvgType classes are cloneable, the new tree is independant of the old.
    /// </summary>
    /// <param name="el"></param>
    /// <returns></returns>
    public static SvgElement CloneElement(SvgElement el) {
        var clone = (SvgElement)el.GetType().GetConstructor([]).Invoke([]);

        foreach (string key in el.Attributes.Keys) {
            clone[key] = el[key].CloneIfPossible();
        }

        foreach (SvgElement ch in el.Children) {
            clone.AddChild(CloneElement(ch));
        }

        return clone;
    }

    internal static Dictionary<string, string> _namespaces = new() { ["xmlns"] = svgNamespaceURI, ["xmlns:xlink"] = xlinkNamespaceURI };

    /// <summary>
    /// Given an xml document and (optionally) a particular element to start from, read the xml nodes and construct
    /// a tree of <see cref="SvgElement"/> objects.  Xml tags that do not correspond to a particular class will be
    /// represented by an <see cref="SvgGenericElement"/>.  This means that literally any XML input can be read in
    /// and written out, even if it has nothing to do with Svg.  More usefully, it means that new and unsupported tags
    /// and attributes will be represented in the <c>SvgElement</c> tree and written out correctly even if SvgNet does
    /// not understand them.
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="el"></param>
    /// <returns></returns>
    public static SvgElement LoadFromXML(XmlDocument doc, XmlElement el) {
        ResetNamespaces();
        if (el == null) {
            foreach (XmlNode noddo in doc.ChildNodes) {
                if (noddo.GetType() == typeof(XmlElement)) {
                    el = (XmlElement)noddo;
                    break;
                }
            }
        }

        if (el == null)
            return null;

        _elementNameDictionary ??= BuildElementNameDictionary();

        var t = (Type)_elementNameDictionary[el.Name];

        var e = (SvgElement)t.GetConstructor([]).Invoke([]);

        RecLoadFromXML(e, doc, el);

        return e;
    }

    /// <summary>
    /// Helper function to compress long xml attributes into entities.
    /// <para>
    /// This would work on any XML, it is not SVG specific, so it should eventually be in some 'xml tools' class.
    /// </para>
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="el"></param>
    /// <returns>A string of entities which can be inserted into the DOCTYPE when the document is written.</returns>
    internal static string CompressXML(XmlDocument doc, XmlElement el) {
        var entities = new Hashtable();
        var singletons = new Hashtable();

        int idx = 0;
        RecCompXML(entities, singletons, doc, el, ref idx);

        // "Uncompress" attribute values with only one reference as it would actually
        // make the resulting XML bigger
        foreach (DictionaryEntry pair in singletons) {
            string attributeValue = (string)pair.Key;
            var singleton = (EntitySingleton)pair.Value;

            // This is an inverse behavior of what RecCompXML did
            singleton.Element.RemoveAttribute(singleton.AttributeName);
            XmlAttribute attr = doc.CreateAttribute(singleton.AttributeName);
            attr.Value = attributeValue;
            _ = singleton.Element.SetAttributeNode(attr);

            entities.Remove(attributeValue);
        }

        var ents = new StringBuilder();

        if (entities.Count > 0) {
            _ = ents.Append('\n');

            foreach (string key in entities.Keys) {
                _ = ents.Append("<!ENTITY ");
                _ = ents.Append(entities[key]);
                _ = ents.Append(" '");
                _ = ents.Append(key.Replace("%", "&#37;"));//I guess we need to escape fully, but surely no other special char can appear?
                _ = ents.Append("'>");
            }

            _ = ents.Append('\n');
        }

        return ents.ToString();
    }

    private static Hashtable _elementNameDictionary;

    /// <summary>
    /// Used by CompressXML
    /// </summary>
    /// <param name="entities">Map of attribute to entity name</param>
    /// <param name="singletons">Output: List of single use attributes to 'uncompress'</param>
    /// <param name="doc"></param>
    /// <param name="el"></param>
    /// <param name="idx">Number that is incremented to provide new entity names</param>
    private static void RecCompXML(Hashtable entities, Hashtable singletons, XmlDocument doc, XmlElement el, ref int idx) {
        var keys = new ArrayList();

        foreach (XmlAttribute att in el.Attributes) {
            _ = keys.Add(att.Name);
        }

        foreach (string s in keys) {
            string val = el.Attributes[s].Value;

            if (val.Length > 30) {
                string entname;

                if (entities[val] == null) {
                    idx++;
                    entname = "E" + idx.ToString();
                    entities[val] = entname;

                    singletons[val] = new EntitySingleton() { Element = el, AttributeName = s };
                } else {
                    entname = (string)entities[val];

                    singletons.Remove(val);
                }

                XmlAttribute attr = doc.CreateAttribute(s);
                _ = attr.AppendChild(doc.CreateEntityReference(entname));
                _ = el.SetAttributeNode(attr);
            }
        }

        foreach (XmlNode ch in el.ChildNodes) {
            if (ch.GetType() == typeof(XmlElement))
                RecCompXML(entities, singletons, doc, (XmlElement)ch, ref idx);
        }
    }

    /// <summary>
    /// Used by LoadFromXML
    /// </summary>
    /// <param name="e"></param>
    /// <param name="doc"></param>
    /// <param name="el"></param>
    private static void RecLoadFromXML(SvgElement e, XmlDocument doc, XmlElement el) {
        e.ReadXmlElement(doc, el);

        foreach (XmlNode noddo in el.ChildNodes) {
            if (noddo.GetType() == typeof(XmlElement)) {
                var childXml = (XmlElement)noddo;

                var t = (Type)_elementNameDictionary[childXml.Name];

                SvgElement childSvg = t switch {
                    null => new SvgGenericElement(childXml.Name),
                    _ => (SvgElement)t.GetConstructor([]).Invoke([]),
                };
                e.AddChild(childSvg);

                RecLoadFromXML(childSvg, doc, childXml);
            } else if (noddo.GetType() == typeof(XmlText)) {
                var xt = (XmlText)noddo;

                var tn = new TextNode(xt.InnerText);

                e.AddChild(tn);
            }
        }
    }

    public static void ResetNamespaces() => _namespaces = new() { ["xmlns"] = svgNamespaceURI, ["xmlns:xlink"] = xlinkNamespaceURI };

    private struct EntitySingleton {
        public string AttributeName;
        public XmlElement Element;
    }
}
