﻿/* Empiria Extended Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extended Framework                     System   : Geographic Information Services     *
*  Namespace : Empiria.Geography                              Assembly : Empiria.Geography.dll               *
*  Type      : Location                                       Pattern  : Empiria Object Type                 *
*  Version   : 6.0        Date: 23/Oct/2014                   License  : GNU AGPLv3  (See license.txt)       *
*                                                                                                            *
*  Summary   : Represents a location within a municipality.                                                  *
*                                                                                                            *
********************************** Copyright (c) 2009-2014 La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

namespace Empiria.Geography {

  /// <summary>Represents a location within a municipality.</summary>
  public class Location : GeographicRegion {

    #region Fields

    private Lazy<List<Settlement>> settlementsList = null;
    private Lazy<List<Roadway>> roadwaysList = null;

    #endregion Fields

    #region Constructors and parsers

    private Location() {
      // Required by Empiria Framework.
    }

    internal Location(Municipality municipality, string locationName) : base(locationName) {
      this.Municipality = municipality;
    }

    protected override void OnInitialize() {
      base.OnInitialize();
      settlementsList = new Lazy<List<Settlement>>(() => GeographicData.GetChildGeoItems<Settlement>(this));
      roadwaysList = new Lazy<List<Roadway>>(() => GeographicData.GetChildGeoItems<Roadway>(this));
    }

    static public new Location Parse(int id) {
      return BaseObject.ParseId<Location>(id);
    }

    static private readonly Location _empty = BaseObject.ParseEmpty<Location>();
    static public new Location Empty {
      get {
        return _empty.Clone<Location>();
      }
    }

    static private readonly Location _unknown = BaseObject.ParseUnknown<Location>();
    static public new Location Unknown {
      get {
        return _unknown.Clone<Location>();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("GeoItemParentId")]
    public Municipality Municipality {
      get;
      private set;
    }

    protected internal override GeographicRegion Parent {
      get {
        return this.Municipality;
      }
    }

    public FixedList<Settlement> Settlements {
      get {
        return settlementsList.Value.ToFixedList();
      }
    }

    public State State {
      get {
        return this.Municipality.State;
      }
    }

    #endregion Public properties

    #region Public methods

    public Roadway AddRoadway(RoadwayKind roadwayKind, string name) {
      Assertion.AssertObject(roadwayKind, "roadwayKind");
      Assertion.AssertObject(name, "name");

      var roadway = new Roadway(this, roadwayKind, name);
      roadwaysList.Value.Add(roadway);

      return roadway;
    }

    public Settlement AddSettlement(SettlementKind settlementKind, string settlementName) {
      Assertion.AssertObject(settlementKind, "settlementKind");
      Assertion.AssertObject(settlementName, "settlementName");

      var settlement = new Settlement(this, settlementKind, settlementName);

      settlementsList.Value.Add(settlement);

      return settlement;
    }

    public void RemoveRoadway(Roadway roadway) {
      Assertion.AssertObject(roadway, "roadway");

      roadway.Remove();
      roadwaysList.Value.Remove(roadway);
    }

    #endregion Public methods

  } // class Location

} // namespace Empiria.Geography
