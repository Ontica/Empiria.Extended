﻿/* Empiria® Extended Framework 2014 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Extended Framework 2014                 System   : Document Management Services      *
*  Namespace : Empiria.Documents.IO                             Assembly : Empiria.Documents.dll             *
*  Type      : RootFilesFolder                                  Pattern  : Storage Item                      *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Describes a root folder or directory that serves as a start point of documents storage.       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/

using Empiria.Ontology;

namespace Empiria.Documents.IO {

  /// <summary>Describes a root folder or directory that serves as a start point of documents storage.</summary>
  public class RootFilesFolder : FilesFolder {

    #region Fields

    private const string thisTypeName = "ObjectType.FilesFolder.RootFilesFolder";

    #endregion Fields

    #region Constructors and parsers

    public RootFilesFolder()
      : base(thisTypeName) {

    }

    private RootFilesFolder(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public new RootFilesFolder Empty {
      get { return BaseObject.ParseEmpty<RootFilesFolder>(thisTypeName); }
    }

    static public new RootFilesFolder Parse(int id) {
      return BaseObject.Parse<RootFilesFolder>(thisTypeName, id);
    }

    static public FilesFolderList GetRootFilesFolders() {
      return DocumentsData.GetFilesFoldersList(ObjectTypeInfo.Parse(thisTypeName));
    }

    #endregion Constructors and parsers

  } // class RootFilesFolder

} // namespace Empiria.Documents.IO