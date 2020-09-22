﻿/* Empiria Extensions ****************************************************************************************
*                                                                                                            *
*  Module   : Empiria User Management                    Component : Integration Layer                       *
*  Assembly : Empiria.UserManagement.dll                 Pattern   : Service provider                        *
*  Type     : EMailServices                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Sends e-mail messages.                                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Security;

using Empiria.Messaging.EMailDelivery;


namespace Empiria.UserManagement.Integration {

  /// <summary>Sends e-mail messages.</summary>
  static internal class EMailServices {

    #region Public methods

    static internal void SendPasswordChangedWarningEMail() {
      Person person = (Person) EmpiriaPrincipal.Current.Identity.User.AsContact();

      var body = GetTemplate("YourPasswordWasChanged");

      body = ParseGeneralFields(body, person);

      var content = new EMailContent($"Your password was changed", body, true);

      SendEmail(content, person);
    }


    #endregion Public methods

    #region Private methods

    static private string GetTemplate(string templateName) {
      string templatesPath = ConfigurationData.GetString("Templates.Path");

      string fileName = System.IO.Path.Combine(templatesPath, $"email.template.{templateName}.html");

      return System.IO.File.ReadAllText(fileName);
    }


    static private string ParseGeneralFields(string body, Person contact) {
      body = body.Replace("{{TO-NAME}}", contact.FirstName);

      return body;
    }


    static private void SendEmail(EMailContent content, Person sendToPerson) {
      var sendTo = new SendTo(sendToPerson.EMail, sendToPerson.Alias);

      EMail.Send(sendTo, content);
    }


    #endregion Private methods

  }  // class EMailServices

}  // namespace Empiria.UserManagement.Integration