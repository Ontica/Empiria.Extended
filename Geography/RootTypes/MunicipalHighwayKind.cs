﻿/* Empiria Extended Framework 2015 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extended Framework                     System   : Geographic Information Services     *
*  Namespace : Empiria.Geography                              Assembly : Empiria.Geography.dll               *
*  Type      : MunicipalHighwayKind                           Pattern  : Value object                        *
*  Version   : 6.5        Date: 25/Jun/2015                   License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : String value type that describes a kind of municipal highway.                                 *
*                                                                                                            *
********************************* Copyright (c) 2009-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Linq;

using Empiria.Ontology;

namespace Empiria.Geography {

  /// <summary>String value type that describes a kind of municipal highway.</summary>
  public class MunicipalHighwayKind : ValueObject<string>, IHighwayKind {

    #region Fields

    private const string thisTypeName = "ValueType.ListItem.MunicipalHighwayKind";

    static private FixedList<MunicipalHighwayKind> valuesList =
      MunicipalHighwayKind.ValueTypeInfo.GetValuesList<MunicipalHighwayKind, string>((x) => new MunicipalHighwayKind(x));

    #endregion Fields

    #region Constructors and parsers

    private MunicipalHighwayKind(string value)
      : base(value) {

    }

    static public MunicipalHighwayKind Parse(string value) {
      Assertion.AssertObject(value, "value");

      if (value == MunicipalHighwayKind.Empty.Value) {
        return MunicipalHighwayKind.Empty;
      }
      if (value == MunicipalHighwayKind.Unknown.Value) {
        return MunicipalHighwayKind.Unknown;
      }
      return valuesList.First((x) => x.Value == value);
    }

    static public MunicipalHighwayKind Empty {
      get {
        var empty = new MunicipalHighwayKind("No determinado");
        empty.MarkAsEmpty();

        return empty;
      }
    }

    static public MunicipalHighwayKind Unknown {
      get {
        var unknown = new MunicipalHighwayKind("No proporcionado");
        unknown.MarkAsUnknown();

        return unknown;
      }
    }

    static public ValueTypeInfo ValueTypeInfo {
      get {
        return ValueTypeInfo.Parse(thisTypeName);
      }
    }

    static public FixedList<MunicipalHighwayKind> GetList() {
      return valuesList;
    }

    #endregion Constructors and parsers

  } // class MunicipalHighwayKind

} // namespace Empiria.Geography
