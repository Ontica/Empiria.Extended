﻿/* Empiria Extended Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extended Framework                     System   : Geographic Information Services     *
*  Namespace : Empiria.Geography                              Assembly : Empiria.Geography.dll               *
*  Type      : IHighwayKind                                   Pattern  : Loose coupling interface            *
*  Version   : 6.0        Date: 23/Oct/2014                   License  : GNU AGPLv3  (See license.txt)       *
*                                                                                                            *
*  Summary   : Interface that represents a highway type kind: federal, state, municipal and rural.           *
*                                                                                                            *
********************************** Copyright (c) 2009-2014 La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Geography {

  /// <summary>Interface that represents a highway type kind: federal, state, municipal and rural.</summary>
  public interface IHighwayKind : IValueObject<string> {

  } // interface IHighwayKind

}  // namespace Empiria.Geography
