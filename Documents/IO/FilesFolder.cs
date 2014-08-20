﻿/* Empiria Extended Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extended Framework 2014                  System   : Document Management Services      *
*  Namespace : Empiria.Documents.IO                             Assembly : Empiria.Documents.dll             *
*  Type      : FilesFolder                                      Pattern  : Empiria Object Type               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Asbtract class that provides read and write operations on operating system directories.       *
*                                                                                                            *
********************************* Copyright (c) 2004-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
using System.Data;
using System.IO;

using Empiria.Contacts;
using Empiria.Ontology;
using Empiria.Security;

namespace Empiria.Documents.IO {

  /// <summary>Enumerates the different folder status.</summary>
  public enum FilesFolderStatus {
    Pending = 'P',
    Opened = 'O',
    Suspended = 'S',
    Closed = 'C',
    Deleted = 'X',
    Obsolete = 'Z'
  }

  /// <summary>Asbtract class that provides read and write operations on operating system directories.</summary>
  public abstract class FilesFolder : BaseObject {

    #region Fields

    private const string thisTypeName = "ObjectType.FilesFolder";

    private FileInfo[] filesCache = null;

    #endregion Fields

    #region Constructors and parsers

    protected FilesFolder(string typeName) : base(typeName) {
      // Empiria Object Type pattern classes always has this constructor. Don't delete
    }

    static public FilesFolder Parse(int id) {
      return BaseObject.Parse<FilesFolder>(thisTypeName, id);
    }

    static internal FilesFolder Parse(DataRow dataRow) {
      return BaseObject.Parse<FilesFolder>(thisTypeName, dataRow);
    }

    static public FilesFolder Empty {
      get { return BaseObject.ParseEmpty<FilesFolder>(thisTypeName); }
    }

    static public FilesFolderList CreateAllFromPath(ObjectTypeInfo filesFolderTypeInfo,
                                                    FilesFolder parentFilesFolder, string path) {
      FilesFolderList filesFolderList = new FilesFolderList();

      FilesFolder rootFilesFolder = LoadFromPath(filesFolderTypeInfo, parentFilesFolder, path);

      filesFolderList.Add(rootFilesFolder);

      DirectoryInfo[] subdirectories = rootFilesFolder.GetDirectories();

      for (int i = 0; i < subdirectories.Length; i++) {
        FilesFolderList childsFolderList = CreateAllFromPath(filesFolderTypeInfo,
                                                             parentFilesFolder, subdirectories[i].FullName);
        for (int j = 0; j < childsFolderList.Count; j++) {
          filesFolderList.Add(childsFolderList[j]);
        }
      }
      return filesFolderList;
    }

    static public DirectoryInfo GetDirectoryInfo(string directoryKey) {
      string path = Empiria.ConfigurationData.GetString("FilesFolder." + directoryKey);
      if (Directory.Exists(path)) {
        return new DirectoryInfo(path);
      } else {
        return null;
      }
    }

    static public bool IsEmpty(string path) {
      DirectoryInfo dir = new DirectoryInfo(path);

      int length = dir.GetFiles().Length;
      length += dir.GetDirectories().Length;

      return (length == 0);
    }

    static private FilesFolder LoadFromPath(ObjectTypeInfo filesFolderTypeInfo, FilesFolder parentFilesFolder, string path) {
      FilesFolder filesFolder = BaseObject.Create<FilesFolder>(filesFolderTypeInfo);

      filesFolder.Owner = parentFilesFolder.Owner;
      filesFolder.WebServer = WebServer.Current;
      filesFolder.PhysicalRootPath = parentFilesFolder.PhysicalRootPath;
      filesFolder.VirtualRootPath = parentFilesFolder.VirtualRootPath;
      filesFolder.ImpersonationToken = parentFilesFolder.ImpersonationToken;
      filesFolder.FileNameFilters = parentFilesFolder.FileNameFilters;
      filesFolder.Tags = parentFilesFolder.Tags;

      filesFolder.SetDirectoryInfo(path);

      filesFolder.ParentFolder = parentFilesFolder;
      filesFolder.Status = FilesFolderStatus.Pending;

      return filesFolder;
    }

    #endregion Constructors and parsers

    #region Public fields

    [DataField("ApprovedById")]
    LazyObject<Contact> _approvedBy = LazyObject<Contact>.Empty;
    public Contact ApprovedBy {
      get { return _approvedBy.Instance; }
      protected set { _approvedBy.Instance = value; }
    }

    [DataField("CapturedById")]
    LazyObject<Contact> _capturedBy = LazyObject<Contact>.Empty;
    public Contact CapturedBy {
      get { return _capturedBy.Instance; }
      protected set { _capturedBy.Instance = value; }
    }

    [DataField("CreationDate", Default = "DateTime.Now")]
    public DateTime CreationDate {
      get;
      private set;
    }

    [DataField("FilesFolderDisplayName")]
    public string DisplayName {
      get;
      private set;
    }

    [DataField("FilesCount")]
    public int FilesCount {
      get;
      private set;
    }

    [DataField("FileNameFilters")]
    public string FileNameFilters {
      get;
      protected set;
    }

    [DataField("FilesTotalSize")]
    public int FilesTotalSize {
      get;
      private set;
    }

    [DataField("ImpersonationToken")]
    public string ImpersonationToken {
      get;
      protected set;
    }

    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.DisplayName, this.Tags, this.ParentFolder.Keywords);
      }
    }

    [DataField("LastUpdateDate", Default = "DateTime.Now")]
    public DateTime LastUpdateDate {
      get;
      private set;
    }

    [DataField("FilesFolderOwnerId")]
    LazyObject<Contact> _owner = LazyObject<Contact>.Empty;
    public Contact Owner {
      get { return _owner.Instance; }
      set { _owner.Instance = value; }
    }

    [DataField("ParentFilesFolderId")]
    LazyObject<FilesFolder> _parentFilesFolder = LazyObject<FilesFolder>.Empty;
    public FilesFolder ParentFolder {
      get { return _parentFilesFolder.Instance; }
      protected set { _parentFilesFolder.Instance = value; }
    }

    [DataField("PhysicalPath")]
    public string PhysicalPath {
      get;
      private set;
    }

    [DataField("PhysicalRootPath")]
    public string PhysicalRootPath {
      get;
      protected set;
    }

    internal protected abstract IIdentifiable Reference {
      get;
    }

    [DataField("ReviewedById")]
    LazyObject<Contact> _reviewedBy = LazyObject<Contact>.Empty;
    public Contact ReviewedBy {
      get { return _reviewedBy.Instance; }
      protected set { _reviewedBy.Instance = value; }
    }

    [DataField("FilesFolderStatus", Default = FilesFolderStatus.Pending)]
    public FilesFolderStatus Status {
      get;
      protected set;
    }

    [DataField("SubFoldersCount")]
    public int SubFoldersCount {
      get;
      private set;
    }

    [DataField("FilesFolderTags")]
    public string Tags {
      get;
      set;
    }

    [DataField("VirtualRootPath")]
    public string VirtualRootPath {
      get;
      protected set;
    }

    [DataField("WebServerId", Default = "Empiria.Security.WebServer.Current")]
    public WebServer WebServer {
      get;
      private set;
    }

    #endregion Public fields

    #region Internal and protected fields

    protected void ResetStatistics() {
      this.CreationDate = ExecutionServer.DateMinValue;
      this.LastUpdateDate = ExecutionServer.DateMinValue;
      this.filesCache = null;
      this.SubFoldersCount = 0;
      this.FilesCount = 0;
      this.FilesTotalSize = 0;
    }

    #endregion Internal and protected fields

    #region Protected methods

    public void CloneInto(FilesFolder clone) {
      DataRow dataRow = this.GetDataRow();
      clone.OnLoadObjectData(dataRow);
    }

    protected void CopyFrom(DirectoryInfo sourceFolder) {
      this.PhysicalPath = this.PhysicalPath.TrimEnd('\\') + @"\" + sourceFolder.Name;
      this.DisplayName = sourceFolder.Name;

      using (ImpersonationContext context = ImpersonationContext.Open(this.ImpersonationToken)) {
        DirectoryInfo targetDirectory = null;
        if (!Directory.Exists(this.PhysicalPath)) {
          targetDirectory = Directory.CreateDirectory(this.PhysicalPath);
        } else if (!IsEmpty(this.PhysicalPath)) {
          throw new DocumentsException(DocumentsException.Msg.CantCopyToNoneEmptyDirectory, this.PhysicalPath);
        }
        foreach (FileInfo file in sourceFolder.GetFiles()) {
          file.CopyTo(targetDirectory.FullName.TrimEnd('\\') + @"\" + file.Name);
        }
      }
      UpdateStatistics();
    }

    protected void DeleteFileAtIndex(int fileIndex) {
      FileInfo[] files = this.GetFiles();
      using (ImpersonationContext context = ImpersonationContext.Open(this.ImpersonationToken)) {
        files[fileIndex].Delete();
      }
    }

    protected DirectoryInfo[] GetDirectories() {
      DirectoryInfo[] directoriesArray = null;
      using (ImpersonationContext context = ImpersonationContext.Open(this.ImpersonationToken)) {
        DirectoryInfo directoryInfo = new DirectoryInfo(this.PhysicalPath);

        directoriesArray = directoryInfo.GetDirectories();
      }
      return directoriesArray;
    }

    protected FileInfo[] GetFiles() {
      if (filesCache == null) {
        string[] searchPatternArray = this.FileNameFilters.Split('|');

        if (searchPatternArray.Length == 1) {
          filesCache = GetFiles(searchPatternArray[0]);
        } else {
          ArrayList filesArray = new ArrayList();
          for (int i = 0; i < searchPatternArray.Length; i++) {
            filesArray.AddRange(GetFiles(searchPatternArray[i]));
          }
          filesArray.Sort(new FileNameComparer());

          filesCache = (FileInfo[]) filesArray.ToArray(typeof(FileInfo));
        }
      }
      return filesCache;
    }

    protected string MapPath(FileInfo file) {
      string fileName = file.FullName.Replace(this.PhysicalRootPath, this.VirtualRootPath);

      return fileName.Replace(@"\", @"/");
    }

    #endregion Protected methods

    #region Public methods

    protected void Refresh() {
      filesCache = null;
    }

    public FilesFolderList GetChilds() {
      return DocumentsData.GetChildFilesFoldersList(this);
    }

    public void Delete() {
      if (this.Reference.Id != -1) {
        throw new DocumentsException(DocumentsException.Msg.CantDeleteReferencedFilesFolder, this.DisplayName);
      }
      if (this.Status == FilesFolderStatus.Opened || this.Status == FilesFolderStatus.Closed) {
        throw new DocumentsException(DocumentsException.Msg.CantDeleteInUseFolder, this.DisplayName);
      }
      this.FilesCount = 0;
      this.FilesTotalSize = 0;
      this.SubFoldersCount = 0;
      this.Status = FilesFolderStatus.Deleted;
      base.Save();
    }

    public void UpdateStatistics() {
      this.filesCache = null;
      this.SubFoldersCount = this.GetDirectories().Length;
      FileInfo[] files = this.GetFiles();
      this.FilesCount = files.Length;
      this.FilesTotalSize = this.CalculateFilesSize(files);
      if (this.Tags.Length == 0) {
        this.Tags = this.ParentFolder.Tags;
      }
    }

    public void UpdateFilesCount() {
      DirectoryInfo directoryInfo = new DirectoryInfo(this.PhysicalPath);

      if (directoryInfo.Exists) {
        FileInfo[] files = this.GetFiles();
        this.FilesCount = files.Length;
      } else {
        this.FilesCount = 0;
      }
    }

    #endregion Public methods

    #region Private methods

    private int CalculateFilesSize(FileInfo[] files) {
      long size = 0;
      using (ImpersonationContext context = ImpersonationContext.Open(this.ImpersonationToken)) {
        for (int i = 0; i < files.Length; i++) {
          size += files[i].Length;
        }
      }
      return (int) (size / 1024L);
    }

    private FileInfo[] GetFiles(string fileNameFilter) {
      FileInfo[] fileInfoArray = null;
      using (ImpersonationContext context = ImpersonationContext.Open(this.ImpersonationToken)) {
        DirectoryInfo directoryInfo = new DirectoryInfo(this.PhysicalPath);

        fileInfoArray = directoryInfo.GetFiles(fileNameFilter);
      }
      return fileInfoArray;
    }

    protected override void OnSave() {
      Assertion.Assert(this.Reference != null, "FilesFolder.Reference can't be null.");
      DocumentsData.WriteFilesFolder(this);
    }

    private void SetDirectoryInfo(string path) {
      using (ImpersonationContext context = ImpersonationContext.Open(this.ImpersonationToken)) {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        this.PhysicalPath = directoryInfo.FullName;
        this.DisplayName = directoryInfo.Name;
        this.CreationDate = directoryInfo.CreationTime;
        this.LastUpdateDate = directoryInfo.LastAccessTime;
      }
    }

    #endregion Private methods

  } // class FilesFolder

} // namespace Empiria.Documents.IO
