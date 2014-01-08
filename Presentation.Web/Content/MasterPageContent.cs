﻿/* Empiria® Presentation Framework 2014 **********************************************************************
*                                                                                                            *
*  Solution  : Empiria® Presentation Framework                  System   : Web Presentation Framework        *
*  Namespace : Empiria.Presentation.Web.Content                 Assembly : Empiria.Presentation.Web.dll      *
*  Type      : MasterPageContent                                Pattern  : Standard Class                    *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Sealed class that represents the XHTML content of a master view used as a canvas for a        *
*              specific WebPage object.                                                                      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
using System;

using Empiria.Presentation.Components;

namespace Empiria.Presentation.Web.Content {

  /// <summary>Sealed class that represents the XHTML content of a master view used as a canvas for a
  /// specific WebPage object.</summary>
  public sealed class MasterPageContent : WebContent {

    #region Fields

    private string breadcrumbBar = null;
    private MessagingContent messagingContent = null;
    private string navigationButtons = null;
    private string navigationClock = null;

    #endregion Fields

    #region Constructors and parsers

    public MasterPageContent(WebPage webPage)
      : base(webPage) {

    }

    #endregion Constructors and parsers

    #region Public properties

    public string ApplicationIcon {
      get {
        string xhtml = GetContent("ApplicationIcon");

        xhtml = xhtml.Replace("{THEME_FOLDER}", ThemeFolder);
        xhtml = xhtml.Replace("{ICON_FILE}", WebPage.ViewModel.ImageName);
        return xhtml;
      }
    }

    public string NavigationClock {
      get {
        if (navigationClock == null) {
          navigationClock = new NavigationClock(WebPage).GetContent();
        }
        return navigationClock;
      }
    }

    public string NewWindowButton {
      get { return GetContent("NewWindowButton"); }
    }

    public string SmallApplicationIcon {
      get {
        string xhtml = GetContent("SmallApplicationIcon");

        xhtml = xhtml.Replace("{THEME_FOLDER}", ThemeFolder);
        xhtml = xhtml.Replace("{ICON_FILE}", WebPage.ViewModel.ImageName);
        return xhtml;
      }
    }

    public string ServerTitle {
      get { return ExecutionServer.Name; }
    }

    #endregion Public properties

    #region Public methods

    public string GetBreadcrumbBarContent() {
      if (breadcrumbBar == null) {
        breadcrumbBar = new BreadcrumbBarContent(WebPage).GetContent();
      }
      return breadcrumbBar;
    }

    public string GetMasterMenuContent() {
      TabStripMasterMenu masterMenu = TabStripMasterMenu.Parse("MasterMenu");

      return base.GetContent(masterMenu);
    }

    public string GetMasterSubMenusContent() {
      TabStripMasterMenu masterMenu = TabStripMasterMenu.Parse("MasterMenu");
      ObjectList<TabStripMasterChildMenu> subMenuList = masterMenu.GetSubMenus();

      string xhtml = String.Empty;

      for (int i = 0; i < subMenuList.Count; i++) {
        xhtml += base.GetContent(subMenuList[i]);
      }
      return xhtml;
    }

    public MessagingContent GetMessagingContent() {
      if (messagingContent == null) {
        messagingContent = new MessagingContent(WebPage);
      }
      return messagingContent;
    }

    public string GetNavigationButtonsContent() {
      if (navigationButtons == null) {
        navigationButtons = new NavigationButtons(WebPage).GetContent();
      }
      return navigationButtons;
    }

    public string GetWaitScreenContent() {
      string xhtml = GetContent("WaitScreen");

      xhtml = xhtml.Replace("{THEME_FOLDER}", ThemeFolder);
      return xhtml;
    }

    #endregion Public methods

  } // class MasterPageContent

} // namespace Empiria.Presentation.Web.Content