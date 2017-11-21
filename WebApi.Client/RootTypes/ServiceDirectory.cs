﻿/* Empiria Extensions Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extensions Framework                   System   : Empiria Web API Services            *
*  Namespace : Empiria.WebApi.Client                          Assembly : Empiria.WebApi.Client.dll           *
*  Type      : ServiceDirectory                               Pattern  : Singleton type                      *
*  Version   : 1.2                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Singleton that holds a list of web services that can be searched by path or by its unique ID. *
*                                                                                                            *
********************************* Copyright (c) 2016-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Empiria.WebApi.Client {

  /// <summary>Singleton that holds a list of web services that can be searched
  /// by path or by its unique ID.</summary>
  internal class ServiceDirectory {

    #region Fields

    private Dictionary<string, ServiceHandler> services =
                                          new Dictionary<string, ServiceHandler>();

    private static readonly string DEFAULT_SERVER_UID =
                                          ConfigurationData.GetString("Default.WebApiServer");

    private static readonly string SERVICE_DIRECTORY_PATH =
                                          ConfigurationData.GetString("ServiceDirectory.Path");

    #endregion Fields

    #region Constructors and parsers

    private ServiceDirectory() {
      this.LoadServices();
    }

    private static ServiceDirectory _instance = null;
    private static object syncRoot = new Object();
    static public ServiceDirectory Instance {
      get {
        if (_instance == null) {
          //lock (syncRoot) {
          //  if (_instance == null)
              _instance = new ServiceDirectory();
          //}
        }
        return _instance;
      }
    }

    #endregion Constructors and parsers

    #region Public methods

    public ServiceHandler GetService(HttpMethod method, string serviceUID) {
      Assertion.AssertObject(serviceUID, "serviceUID");

      string searchKey = String.Empty;

      if (UtilityMethods.HasUriPathFormat(serviceUID)) {
        searchKey = method.Method + " " + UtilityMethods.RemoveDataScopeFromPath(serviceUID);
      } else {
        searchKey = UtilityMethods.RemoveDataScopeFromPath(serviceUID);
      }

      if (services.ContainsKey(searchKey)) {
        return services[searchKey];
      } else {
        throw new WebApiClientException(WebApiClientException.Msg.UndefinedServiceUIDOrEndpoint, searchKey);
      }
    }

    #endregion Public methods

    #region Private methods

    private void LoadServices() {
      var httpClient = new HttpApiClient(DEFAULT_SERVER_UID);

      var task = httpClient.GetAsync<ResponseModel<ServiceHandler[]>>(SERVICE_DIRECTORY_PATH);

      task.Wait();

      this.services.Clear();
      foreach (var item in task.Result.Data) {
        this.services.Add(item.UID, item);
        this.services.Add(item.Method + " " + item.Path, item);
      }
    }

    #endregion Private methods

  }  // class ServiceDirectory

}  // namespace Empiria.WebApi.Client
