using PEExtractor.Common;
using PEExtractor.Common.Extension;
using PEExtractor.Extractors.DataBlock;
using PEExtractor.Structs;
using PEExtractor.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Info
{
    public partial class TildeStreamInfo
    {

        /// <summary>
        /// Tables schemes. https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf  II.22
        /// </summary>
        internal static Dictionary<int, TableInfo> TablesInfo = new Dictionary<int, TableInfo>()
        {
            {0x00, new TableInfo("Module", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Generation", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Mvid", new HeapIndex(HeapType.GUID)),
                                            new Tuple<string, IColumnElement>("EncId",new HeapIndex(HeapType.GUID)),
                                            new Tuple<string, IColumnElement>("EncBaseId", new HeapIndex(HeapType.GUID)),
                                        })

            },
            {0x01, new TableInfo("TypeRef", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("ResolutionScope", new CodedIndex("ResolutionScope")),
                                            new Tuple<string, IColumnElement>("TypeName", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("TypeNamespace", new HeapIndex(HeapType.Strings)),
                                        })
            },
            {0x02, new TableInfo("TypeDef", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("TypeName", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("TypeNamespace", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Extends", new CodedIndex("TypeDefOrRef")),
                                            new Tuple<string, IColumnElement>("FieldList", new TableIndex("Field")),
                                            new Tuple<string, IColumnElement>("MethodList", new TableIndex("MethodDef")),
                                        })
            },
            {0x04, new TableInfo("Field", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Signature", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x06, new TableInfo("MethodDef", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("RVA", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("ImplFlags", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Signature", new HeapIndex(HeapType.Blob)),
                                            new Tuple<string, IColumnElement>("ParamList", new TableIndex("Param")),
                                        })
            },
            {0x08, new TableInfo("Param", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Sequence", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                        })
            },
            {0x09, new TableInfo("InterfaceImpl", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Class", new TableIndex("TypeDef")),
                                            new Tuple<string, IColumnElement>("Interface", new CodedIndex("TypeDefOrRef")),
                                        })
            },
            {0x0A, new TableInfo("MemberRef", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Class", new CodedIndex("MemberRefParent")),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Signature", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x0B, new TableInfo("Constant", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Type", new ConstantByte()),
                                            new Tuple<string, IColumnElement>("Type[Padding]", new ConstantByte()),
                                            new Tuple<string, IColumnElement>("Parent", new CodedIndex("HasConstant")),
                                            new Tuple<string, IColumnElement>("Value", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x0C, new TableInfo("CustomAttribute", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Parent", new CodedIndex("HasCustomAttribute")),
                                            new Tuple<string, IColumnElement>("Type", new CodedIndex("CustomAttributeType")),
                                            new Tuple<string, IColumnElement>("Value", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x0D, new TableInfo("FieldMarshal", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Parent", new CodedIndex("HasFieldMarshal")),
                                            new Tuple<string, IColumnElement>("NativeType", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x0E, new TableInfo("DeclSecurity", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Action", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Parent", new CodedIndex("HasDeclSecurity")),
                                            new Tuple<string, IColumnElement>("PermissionSet", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x0F, new TableInfo("ClassLayout", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("PackingSize", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("ClassSize", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("Parent", new TableIndex("TypeDef")),
                                        })
            },
            {0x10, new TableInfo("FieldLayout", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Offset", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("Field", new TableIndex("Field")),
                                        })
            },
            {0x11, new TableInfo("StandAloneSig", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Signature", new HeapIndex(HeapType.Blob))
                                        })
            },
            {0x12, new TableInfo("EventMap", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Parent", new TableIndex("TypeDef")),
                                            new Tuple<string, IColumnElement>("EventList", new TableIndex("Event")),
                                        })
            },
            {0x14, new TableInfo("Event", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("EventFlags", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("EventType", new CodedIndex("TypeDefOrRef")),
                                        })
            },
            {0x15, new TableInfo("PropertyMap", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Parent", new TableIndex("TypeDef")),
                                            new Tuple<string, IColumnElement>("PropertyList", new TableIndex("Property")),
                                        })
            },
            {0x17, new TableInfo("Property", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Type", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x18, new TableInfo("MethodSematincs", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Semantics", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Method", new TableIndex("MethodDef")),
                                            new Tuple<string, IColumnElement>("Association", new CodedIndex("HasSemantics")),
                                        })
            },
            {0x19, new TableInfo("MethodImpl", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Class", new TableIndex("TypeDef")),
                                            new Tuple<string, IColumnElement>("MethodBody", new CodedIndex("MethodDefOrRef")),
                                            new Tuple<string, IColumnElement>("MethodDeclaration", new CodedIndex("MethodDefOrRef")),
                                        })
            },
            {0x1A, new TableInfo("ModuleRef", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings))
                                        })
            },
            {0x1B, new TableInfo("TypeSpec", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Signature", new HeapIndex(HeapType.Blob))
                                        })
            },
            {0x1C, new TableInfo("ImplMap", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("MappingFlags", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("MemberForwarded", new CodedIndex("MemberForwarded")),
                                            new Tuple<string, IColumnElement>("ImportName", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("ImportScope", new TableIndex("ModuleRef")),
                                        })
            },
            {0x1D, new TableInfo("FieldRVA", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("RVA", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("Field", new TableIndex("Field")),
                                        })
            },
            {0x20, new TableInfo("Assembly", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("HashAlgId", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("MajorVersion", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("MinorVersion", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("BuildNumber", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("RevisionNumber", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("PublicKey", new HeapIndex(HeapType.Blob)),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Culture", new HeapIndex(HeapType.Strings))
                                        })
            },
            {0x21, new TableInfo("AssemblyProcessor", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Processor", new ConstantUInt32()),
                                        })
            },
            {0x22, new TableInfo("AssemblyOS", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("OSPlatformID", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("OSMajorVersion", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("OSMinorVersion", new ConstantUInt32()),
                                        })
            },
            {0x23, new TableInfo("AssemblyRef", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("MajorVersion", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("MinorVersion", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("BuildNumber", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("RevisionNumber", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("PublicKeyOrToken", new HeapIndex(HeapType.Blob)),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Culture", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("HashValue", new HeapIndex(HeapType.Blob)),

                                        })
            },
            {0x24, new TableInfo("AssemblyRefProcessor", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Processor", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("AssemblyRef", new TableIndex("AssemblyRef"))
                                        })
            },
            {0x25, new TableInfo("AssemblyRefOS", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("OSPlatformId", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("OSMajorVersion", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("OSMinorVersion", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("AssemblyRef", new TableIndex("AssemblyRef"))
                                        })
            },
            {0x26, new TableInfo("File", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("HashValue", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x27, new TableInfo("ExportedType", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("TypeDefId", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("TypeName", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("TypeNamespace", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Implementation", new CodedIndex("Implementation")),
                                        })
            },
            {0x28, new TableInfo("ManifestResource", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Offset", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Implementation", new CodedIndex("Implementation")),
                                        })
            },
            {0x29, new TableInfo("NestedClass", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("NestedClass", new TableIndex("TypeDef")),
                                            new Tuple<string, IColumnElement>("EnclosingClass", new TableIndex("TypeDef")),
                                        })
            },
            {0x2A, new TableInfo("GenericParam", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Number", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Flags", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Owner", new CodedIndex("TypeOrMethodDef")),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings))
                                        })
            },
            {0x2B, new TableInfo("MethodSpec", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Method", new CodedIndex("MethodDefOrRef")),
                                            new Tuple<string, IColumnElement>("Instantiation", new HeapIndex(HeapType.Blob))
                                        })
            },
            {0x2C, new TableInfo("GenericParamConstraint", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Owner", new TableIndex("GenericParam")),
                                            new Tuple<string, IColumnElement>("Constraint", new CodedIndex("TypeDefOrRef")),
                                        })
            },

            /// New tables
            /// https://github.com/dotnet/corefx/blob/master/src/System.Reflection.Metadata/specs/PortablePdb-Metadata.md
            /// 

            {0x30, new TableInfo("Document", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Blob)),
                                            new Tuple<string, IColumnElement>("HashAlgorithm", new HeapIndex(HeapType.GUID)),
                                            new Tuple<string, IColumnElement>("Hash", new HeapIndex(HeapType.Blob)),
                                            new Tuple<string, IColumnElement>("Language", new HeapIndex(HeapType.GUID)),
                                        })
            },
            {0x31, new TableInfo("MethodDebugInformation", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Document", new TableIndex("Document")),
                                            new Tuple<string, IColumnElement>("SequencePoints", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x32, new TableInfo("LocalScope", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Method", new TableIndex("MethodDef")),
                                            new Tuple<string, IColumnElement>("ImportScope", new TableIndex("ImportScope")),
                                            new Tuple<string, IColumnElement>("VariableList", new TableIndex("LocalVariable")),
                                            new Tuple<string, IColumnElement>("ConstantList", new TableIndex("LocalConstant")),
                                            new Tuple<string, IColumnElement>("StartOffset", new ConstantUInt32()),
                                            new Tuple<string, IColumnElement>("Length", new ConstantUInt32()),
                                        })
            },
            {0x33, new TableInfo("LocalVariable", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Attributes", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Index", new ConstantUInt16()),
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                        })
            },
            {0x34, new TableInfo("LocalConstant", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Name", new HeapIndex(HeapType.Strings)),
                                            new Tuple<string, IColumnElement>("Signature", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x35, new TableInfo("ImportScope", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Parent", new TableIndex("ImportScope")),
                                            new Tuple<string, IColumnElement>("Imports", new HeapIndex(HeapType.Blob)),
                                        })
            },
            {0x36, new TableInfo("StateMachineMethod", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("MoveNextMethod", new TableIndex("MethodDef")),
                                            new Tuple<string, IColumnElement>("KickoffMethod", new TableIndex("MethodDef")),
                                        })
            },
            {0x37, new TableInfo("CustomDebugInformation", new List<Tuple<string, IColumnElement>>()
                                        {
                                            new Tuple<string, IColumnElement>("Parent", new CodedIndex("HasCustomDebugInformation")),
                                            new Tuple<string, IColumnElement>("Kind", new HeapIndex(HeapType.GUID)),
                                            new Tuple<string, IColumnElement>("Value", new HeapIndex(HeapType.Blob)),
                                        })
            },
        };
    }
}
