﻿/* Empiria Extended Framework 2015 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extended Framework                     System   : Geographic Information Services     *
*  Namespace : Empiria.Geography                              Assembly : Empiria.Geography.dll               *
*  Type      : State                                          Pattern  : Empiria Object Type                 *
*  Version   : 6.0        Date: 04/Jan/2015                   License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Represents a state within a country.                                                          *
*                                                                                                            *
********************************* Copyright (c) 2009-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Empiria.Geography {

  /// <summary>Represents a state within a country.</summary>
  public class State : GeographicRegion {

    #region Fields

    private Lazy<List<Municipality>> municipalitiesList = null;
    private Lazy<List<Highway>> highwaysList = null;

    #endregion Fields

    #region Constructors and parsers

    private State() {
      // Required by Empiria Framework.
    }

    internal State(Country country, string stateName, string stateCode): base(stateName) {
      this.Country = country;
      this.Code = stateCode;
    }

    protected override void OnInitialize() {
      base.OnInitialize();
      municipalitiesList = 
                new Lazy<List<Municipality>>(() => GeographicData.GetChildGeoItems<Municipality>(this));
      highwaysList = new Lazy<List<Highway>>(() => GeographicData.GetChildGeoItems<Highway>(this));
    }

    static public new State Parse(int id) {
      return BaseObject.ParseId<State>(id);
    }

    static private readonly State _empty = BaseObject.ParseEmpty<State>();
    static public new State Empty {
      get {
        return _empty.Clone<State>();
      }
    }

    static private readonly State _unknown = BaseObject.ParseUnknown<State>();
    static public new State Unknown {
      get {
        return _unknown.Clone<State>();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("GeoItemExtData.Code")]
    public string Code {
      get;
      private set;
    }

    [DataField("GeoItemParentId")]
    public Country Country {
      get;
      private set;
    }

    public FixedList<Highway> Highways {
      get {
        return highwaysList.Value.ToFixedList();
      }
    }

    [DataField("GeoItemExtData.PostalCodesRegEx")]
    public string PostalCodesPattern {
      get;
      private set;
    }

    public FixedList<Municipality> Municipalities {
      get {
        return municipalitiesList.Value.ToFixedList();
      }
    }

    protected internal override GeographicRegion Parent {
      get {
        return this.Country;
      }
    }

    #endregion Public properties

    #region Public methods

    /// <summary>Adds a municipality to the state.</summary>
    public Municipality AddMunicipality(string municipalityName) {
      Assertion.AssertObject(municipalityName, "municipalityName");

      var municipality = new Municipality(this, municipalityName);
      municipalitiesList.Value.Add(municipality);

      return municipality;
    }

    /// <summary>Adds a new state highway without an offical highway number.</summary>
    public Highway AddHighway(StateHighwayKind highwayKind,
                              HighwaySection fromOriginToDestination) {
      Assertion.AssertObject(highwayKind, "highwayKind");
      Assertion.AssertObject(fromOriginToDestination, "fromOriginToDestination");

      var highway = new Highway(this, highwayKind, fromOriginToDestination);
      highwaysList.Value.Add(highway);

      return highway;
    }

    /// <summary>Adds a new state highway with a designated official highway number.</summary>
    public Highway AddHighway(StateHighwayKind highwayKind, string highwayNumber,
                              HighwaySection fromOriginToDestination) {
      Assertion.AssertObject(highwayKind, "highwayKind");
      Assertion.AssertObject(highwayNumber, "highwayNumber");
      Assertion.AssertObject(fromOriginToDestination, "fromOriginToDestination");

      var highway = new Highway(this, highwayKind, highwayNumber, fromOriginToDestination);
      highwaysList.Value.Add(highway);

      return highway;
    }

    /// <summary>Adds a new rural highway to the state.</summary>
    public Highway AddHighway(RuralHighwayKind ruralHighwayKind,
                              HighwaySection fromOriginToDestination) {
      Assertion.AssertObject(ruralHighwayKind, "ruralHighwayKind");
      Assertion.AssertObject(fromOriginToDestination, "fromOriginToDestination");

      var highway = new Highway(this, ruralHighwayKind, fromOriginToDestination);
      highwaysList.Value.Add(highway);

      return highway;
    }

    /// <summary>Throws an exception if the given postal code is not valid for 
    /// the state postal code rules.</summary>
    internal void AssertPostalCodeIsValid(string value) {
      Assertion.Assert(value != null, "value can't be null");
      if (value.Length == 0 || this.PostalCodesPattern.Length == 0) {
        return;
      }
      if (!Regex.IsMatch(value, this.PostalCodesPattern)) {
        throw new GeographyException(GeographyException.Msg.InvalidPostalCode, value, this.Name);
      }
    }

    /// <summary>Remove a highway from the state.</summary>
    public void RemoveHighway(Highway highway) {
      Assertion.AssertObject(highway, "highway");

      highway.Remove();
      highwaysList.Value.Remove(highway);
    }

    #endregion Public methods

  } // class State

} // namespace Empiria.Geography
