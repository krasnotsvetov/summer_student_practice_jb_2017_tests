using PEExtractor.Common;
using PEExtractor.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Tables
{
    public partial class CodedIndex : IColumnElement
    {

        //ECMA-335, p. 302, for tags encoding
        private static Dictionary<string, Dictionary<int, string>> Tags = new Dictionary<string, Dictionary<int, string>>()
        {
            {"TypeDefOrRef", new Dictionary<int, string>()
                                {
                                     {0, "TypeDef"},
                                     {1, "TypeRef"},
                                     {2, "TypeSpec"},
                                }
            },
            {"HasConstant", new Dictionary<int, string>()
                                {
                                     {0, "Field"},
                                     {1, "Param"},
                                     {2, "Property"},
                                }
            },
            {"HasCustomAttribute", new Dictionary<int, string>()
                                {
                                     {0, "MethodDef"},
                                     {1, "Field"},
                                     {2, "TypeRef"},
                                     {3, "TypeDef"},
                                     {4, "Param"},
                                     {5, "InterfaceImpl"},
                                     {6, "MemberRef"},
                                     {7, "Module"},
                                     /// {8, "Permission"}, What is it???
                                     {9, "Property"},
                                     {10, "Event"},
                                     {11, "StandAloneSig"},
                                     {12, "ModuleRef"},
                                     {13, "TypeSpec"},
                                     {14, "Assembly"},
                                     {15, "AssemblyRef"},
                                     {16, "File"},
                                     {17, "ExportedType"},
                                     {18, "ManifestResource"},
                                     {19, "GenericParam"},
                                     {20, "GenericParamConstraint"},
                                     {21, "MethodSpec"},
                                }
            },
            {"HasFieldMarshal", new Dictionary<int, string>()
                                {
                                     {0, "Field"},
                                     {1, "Param"},
                                }
            },
            {"HasDeclSecurity", new Dictionary<int, string>()
                                {
                                     {0, "TypeDef"},
                                     {1, "MethodDef"},
                                     {2, "Assembly"},
                                }
            },
            {"MemberRefParent", new Dictionary<int, string>()
                                {
                                     {0, "TypeDef"},
                                     {1, "TypeRef"},
                                     {2, "ModuleRef"},
                                     {3, "MethodDef"},
                                     {4, "TypeSpec"},
                                }
            },
            {"HasSemantics", new Dictionary<int, string>()
                                {
                                     {0, "Event"},
                                     {1, "Property"},
                                }
            },
            {"MethodDefOrRef", new Dictionary<int, string>()
                                {
                                     {0, "MethodDef"},
                                     {1, "MemberRef"},
                                }
            },
            {"MemberForwarded", new Dictionary<int, string>()
                                {
                                     {0, "Field"},
                                     {1, "MethodDef"},
                                }
            },
            {"Implementation", new Dictionary<int, string>()
                                {
                                     {0, "File"},
                                     {1, "AssemblyRef"},
                                     {2, "ExportedType"},
                                }
            },
            {"CustomAttributeType", new Dictionary<int, string>()
                                {
                                     {0, "MethodDef"},
                                     {1, "MethodDef"},
                                     {2, "MethodDef"},
                                     {3, "MemberRef"},
                                     {4, "MethodDef"},   /// 0, 1, 4 is not used, so we define them to existing items
                                }
            },
            {"ResolutionScope", new Dictionary<int, string>()
                                {
                                     {0, "Module"},
                                     {1, "ModuleRef"},
                                     {2, "AssemblyRef"},
                                     {3, "TypeRef"}
                                }
            },
            {"TypeOrMethodDef", new Dictionary<int, string>()
                                {
                                     {0, "TypeDef"},
                                     {1, "MethodDef"},
                                }
            },

            ///
            /// https://github.com/dotnet/corefx/blob/master/src/System.Reflection.Metadata/specs/PortablePdb-Metadata.md
            ///

            {"HasCustomDebugInformation", new Dictionary<int, string>()
                                {
                                     {0, "MethodDef"},
                                     {1, "Field"},
                                     {2, "TypeRef"},
                                     {3, "TypeDef"},
                                     {4, "Param"},
                                     {5, "InterfaceImpl"},
                                     {6, "MemberRef"},
                                     {7, "Module"},
                                     {8, "DeclSecurity"}, /// may be Permession == DeclSecurity
                                     {9, "Property"},
                                     {10, "Event"},
                                     {11, "StandAloneSig"},
                                     {12, "ModuleRef"},
                                     {13, "TypeSpec"},
                                     {14, "Assembly"},
                                     {15, "AssemblyRef"},
                                     {16, "File"},
                                     {17, "ExportedType"},
                                     {18, "ManifestResource"},
                                     {19, "GenericParam"},
                                     {20, "GenericParamConstraint"},
                                     {21, "MethodSpec"},
                                     {22, "Document"},
                                     {23, "LocalScope"},
                                     {24, "LocalVariable"},
                                     {25, "LocalConstant"},
                                     {26, "ImportScope"},
                                }
            },
        };
    }
}
