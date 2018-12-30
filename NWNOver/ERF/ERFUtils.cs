using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver.ERF
{
    public enum ContentTypes
    {
        Invalid = -1,
        Binary,
        Text,
        Text_ini,
        GFF,
        MDL,
    }

    static class ERFUtils
    {
        static ERFUtils()
        {
            ResourceTypes.Add(new ERFResType(0xFFFF, null, ContentTypes.Invalid));
            ResourceTypes.Add(new ERFResType(1, ".bmp", ContentTypes.Binary));
            ResourceTypes.Add(new ERFResType(3, ".tga", ContentTypes.Binary));
            ResourceTypes.Add(new ERFResType(4, ".wav", ContentTypes.Binary));
            ResourceTypes.Add(new ERFResType(6, ".plt", ContentTypes.Binary));
            ResourceTypes.Add(new ERFResType(7, ".ini", ContentTypes.Text_ini));
            ResourceTypes.Add(new ERFResType(10, ".txt", ContentTypes.Text));
            ResourceTypes.Add(new ERFResType(2002, ".mdl", ContentTypes.MDL));
            ResourceTypes.Add(new ERFResType(2009, ".nss", ContentTypes.Text));
            ResourceTypes.Add(new ERFResType(2010, ".ncs", ContentTypes.Binary));
            ResourceTypes.Add(new ERFResType(2012, ".are", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2013, ".set", ContentTypes.Text_ini));
            ResourceTypes.Add(new ERFResType(2014, ".ifo", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2015, ".bic", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2016, ".wok", ContentTypes.MDL));
            ResourceTypes.Add(new ERFResType(2017, ".2da", ContentTypes.Text));
            ResourceTypes.Add(new ERFResType(2022, ".txi", ContentTypes.Text));
            ResourceTypes.Add(new ERFResType(2023, ".git", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2025, ".uti", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2027, ".utc", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2029, ".dlg", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2030, ".itp", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2032, ".utt", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2033, ".dds", ContentTypes.Binary));
            ResourceTypes.Add(new ERFResType(2035, ".uts", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2036, ".ltr", ContentTypes.Binary));
            ResourceTypes.Add(new ERFResType(2037, ".gff", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2038, ".fac", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2040, ".ute", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2042, ".utd", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2044, ".utp", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2045, ".dft", ContentTypes.Text_ini));
            ResourceTypes.Add(new ERFResType(2046, ".gic", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2047, ".gui", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2051, ".utm", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2052, ".dwk", ContentTypes.MDL));
            ResourceTypes.Add(new ERFResType(2053, ".pwk", ContentTypes.MDL));
            ResourceTypes.Add(new ERFResType(2056, ".jrl", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2058, ".utw", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2060, ".ssf", ContentTypes.Binary));
            ResourceTypes.Add(new ERFResType(2064, ".ndb", ContentTypes.Binary));
            ResourceTypes.Add(new ERFResType(2065, ".ptm", ContentTypes.GFF));
            ResourceTypes.Add(new ERFResType(2066, ".ptt", ContentTypes.GFF));
        }

        public static List<ERFResType> ResourceTypes = new List<ERFResType>();

        public static string GetExtension(ushort restype)
        {
            var blah = ResourceTypes.Find(x => x.TypeID == restype);

            if (blah == null)
            {
                blah = ResourceTypes[0];
            }

            return blah.Extension;
        }

        public static ushort GetResType(string extension)
        {
            return ResourceTypes.Find(x => x.Extension == extension).TypeID;
        }


        public class ERFResType
        {
            public ERFResType(ushort typeid, string extension, ContentTypes contenttype)
            {
                TypeID = typeid;
                Extension = extension;
                ContentType = contenttype;
            }

            public ushort TypeID;
            public string Extension;
            public ContentTypes ContentType;
        }
    }
}
