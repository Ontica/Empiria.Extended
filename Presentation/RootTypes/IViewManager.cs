﻿/* Empiria Presentation Framework 2015 ***********************************************************************
*                                                                                                            *
*  Solution  : Empiria Presentation Framework                   System   : Presentation Framework Library    *
*  Namespace : Empiria.Presentation                             Assembly : Empiria.Presentation.dll          *
*  Type      : IViewManager                                     Pattern  : Model View Controller             *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Interface that represents a view manager. Each type of user interface technology has an       *
*              associated view manager, making it easier to add different application types.                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Presentation {

  /// <summary>Interface that represents a view manager. Each type of user interface technology has an
  ///associated view manager, making it easier to add different application types.</summary>
  public interface IViewManager {

    #region Methods definition

    void ActivateView(ViewModel viewModel);
    void ActivateView(ViewModel viewModel, bool preserve);
    void ActivateView(ViewModel viewModel, string viewParameters);
    void ActivateView(ViewModel viewModel, string viewParameters, bool preserve);

    #endregion Methods definition

  } // class IViewManager

} // namespace Empiria.Presentation
