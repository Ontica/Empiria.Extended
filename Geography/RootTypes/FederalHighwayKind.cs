﻿/* Empiria Extended Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extended Framework                     System   : Geographic Information Services     *
*  Namespace : Empiria.Geography                              Assembly : Empiria.Geography.dll               *
*  Type      : FederalHighwayKind                             Pattern  : Value object                        *
*  Version   : 6.0        Date: 23/Oct/2014                   License  : GNU AGPLv3  (See license.txt)       *
*                                                                                                            *
*  Summary   : String value type that describes a kind of federal highway.                                   *
*                                                                                                            *
********************************** Copyright (c) 2009-2014 La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Linq;

using Empiria.Ontology;

namespace Empiria.Geography {

  /// <summary>String value type that describes a kind of federal highway.</summary>
  public class FederalHighwayKind : ValueObject<string>, IHighwayKind {

    #region Fields

    private const string thisTypeName = "ValueType.ListItem.FederalHighwayKind";

    static private FixedList<FederalHighwayKind> valuesList =
    FederalHighwayKind.ValueTypeInfo.GetValuesList<FederalHighwayKind, string>((x) => new FederalHighwayKind(x));

    #endregion Fields

    #region Constructors and parsers

    private FederalHighwayKind(string value) : base(value) {

    }

    static public FederalHighwayKind Parse(string value) {
      Assertion.AssertObject(value, "value");

      if (value == FederalHighwayKind.Empty.Value) {
        return FederalHighwayKind.Empty;
      }
      if (value == FederalHighwayKind.Unknown.Value) {
        return FederalHighwayKind.Unknown;
      }
      return valuesList.First((x) => x.Value == value);
    }

    static public FederalHighwayKind Empty {
      get {
        FederalHighwayKind empty = new FederalHighwayKind("No determinado");
        empty.MarkAsEmpty();

        return empty;
      }
    }

    static public FederalHighwayKind Unknown {
      get {
        FederalHighwayKind unknown = new FederalHighwayKind("No proporcionado");
        unknown.MarkAsUnknown();

        return unknown;
      }
    }

    static public ValueTypeInfo ValueTypeInfo {
      get {
        return ValueTypeInfo.Parse(thisTypeName);
      }
    }

    static public FixedList<FederalHighwayKind> GetList() {
      return valuesList;
    }

    #endregion Constructors and parsers

  } // class FederalHighwayKind

} // namespace Empiria.Geography