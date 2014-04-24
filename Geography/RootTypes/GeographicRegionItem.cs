﻿/* Empiria Extended Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extended Framework                     System   : Geographic Information Services     *
*  Namespace : Empiria.Geography                              Assembly : Empiria.Geography.dll               *
*  Type      : GeographicRegionItem                           Pattern  : Empiria Object Type                 *
*  Version   : 5.5        Date: 25/Jun/2014                   License  : GNU AGPLv3  (See license.txt)       *
*                                                                                                            *
*  Summary   : Represents a geographic area or region: city, country, world zone, zip code region, ...       *
*                                                                                                            *
********************************* Copyright (c) 2009-2014 La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Ontology;

namespace Empiria.Geography {

  /// <summary>Represents a geographic area or region: city, country, world zone, zip code region, ...</summary>
  public class GeographicRegionItem : GeographicItem {

    #region Fields

    private const string thisTypeName = "ObjectType.GeographicItem.GeographicRegionItem";

    private string webPage = String.Empty;
    private string phonePrefix = String.Empty;
    private decimal population = 0m;
    private decimal areaSqKm = 0m;
    private Money gdpPerCapita = new Money();

    #endregion Fields

    #region Constructors and parsers

    protected GeographicRegionItem()
      : base(thisTypeName) {
      // For create instances use GeographicItemType.CreateInstance method instead
    }

    protected GeographicRegionItem(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public new GeographicRegionItem Parse(int id) {
      return BaseObject.Parse<GeographicRegionItem>(thisTypeName, id);
    }

    static internal GeographicRegionItem Parse(DataRow row) {
      return BaseObject.Parse<GeographicRegionItem>(thisTypeName, row);
    }

    static public GeographicRegionItem Empty {
      get { return BaseObject.ParseEmpty<GeographicRegionItem>(thisTypeName); }
    }

    static public GeographicRegionItem Unknown {
      get { return BaseObject.ParseUnknown<GeographicRegionItem>(thisTypeName); }
    }

    static public FixedList<GeographicRegionItem> GetList(string filter) {
      return GeographicData.GetRegions(filter);
    }

    #endregion Constructors and parsers

    #region Public properties

    public decimal AreaSqKm {
      get { return areaSqKm; }
      set { areaSqKm = value; }
    }

    public string CompoundName {
      get { return base.Name + " (" + base.GeographicItemType.DisplayName + ")"; }
    }

    public Money GDPPerCapita {
      get { return gdpPerCapita; }
      set { gdpPerCapita = value; }
    }

    public string PhonePrefix {
      get { return phonePrefix; }
      set { phonePrefix = value; }
    }

    public decimal Population {
      get { return population; }
      set { population = value; }
    }

    public string WebPage {
      get { return webPage; }
      set { webPage = value; }
    }

    #endregion Public properties

    #region Public methods

    public void AddMember(string roleName, GeographicItem member) {
      TypeAssociationInfo role = base.ObjectTypeInfo.Associations[roleName];
      base.Link(role, member);
    }

    public FixedList<T> GetContacts<T>(string roleName) where T : Contact {
      return base.GetLinks<T>(roleName);
    }

    public FixedList<GeographicPathItem> GetPaths(string pathRoleName) {
      var list = base.GetLinks<GeographicPathItem>(pathRoleName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    public FixedList<GeographicPathItem> GetPaths(string pathRoleName,
                                                   GeographicItemType pathItemType) {
      var list = base.GetLinks<GeographicPathItem>(pathRoleName, (x) => (pathItemType.IsTypeOf(x)));

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    public FixedList<Person> GetPeople(string roleName) {
      var list = base.GetLinks<Person>(roleName);

      list.Sort((x, y) => x.FamilyFullName.CompareTo(y.FamilyFullName));

      return list;
    }

    public FixedList<GeographicRegionItem> GetRegions(string regionRoleName) {
      var list = base.GetLinks<GeographicRegionItem>(regionRoleName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    public FixedList<GeographicRegionItem> GetRegions(string regionRoleName,
                                                       GeographicItemType geoItemType) {
      var list = base.GetLinks<GeographicRegionItem>(regionRoleName, (x) => (geoItemType.IsTypeOf(x)));

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    public FixedList<GeographicRegionItem> GetRegions(string regionRoleName,
                                                       Predicate<GeographicRegionItem> predicate) {
      var list = base.GetLinks<GeographicRegionItem>(regionRoleName, predicate);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    protected override void ImplementsLoadObjectData(DataRow row) {
      base.ImplementsLoadObjectData(row);
      this.webPage = (string) row["GeoItemWebPage"];
      this.phonePrefix = (string) row["PhonePrefix"];
      this.population = (decimal) row["Population"];
      this.areaSqKm = (decimal) row["AreaSqKm"];
      this.gdpPerCapita = Money.Parse(Currency.Parse((int) row["GDPCurrencyId"]), (decimal) row["GDPPerCapita"]);
    }

    protected override void ImplementsSave() {
      base.Keywords = EmpiriaString.BuildKeywords(this.Name, this.ObjectTypeInfo.DisplayName);
      GeographicData.WriteGeographicRegionItem(this);
    }

    #endregion Public methods

  } // class GeographicRegionItem

} // namespace Empiria.Geography