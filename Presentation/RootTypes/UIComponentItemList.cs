﻿/* Empiria Presentation Framework 2015 ***********************************************************************
*                                                                                                            *
*  Solution  : Empiria Presentation Framework                   System   : Presentation Framework Library    *
*  Namespace : Empiria.Presentation                             Assembly : Empiria.Presentation.dll          *
*  Type      : UIComponentItemList                              Pattern  : Empiria List Class                *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents an ordered named list of storage Item instances. Implements the IList interface    *
*              again because needs convert DataRow items to Item items in the IList indexer.                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

using Empiria.Collections;

namespace Empiria.Presentation {

  public class UIComponentItemList : BaseList<UIComponentItem> {

    #region Constructors and parsers

    public UIComponentItemList() {
      //no-op
    }

    public UIComponentItemList(int capacity) : base(capacity) {
      // no-op
    }

    #endregion Constructors and parsers

    #region Public methods

    public new void Add(UIComponentItem item) {
      base.Add(item);
    }

    public override UIComponentItem this[int index] {
      get {
        return (UIComponentItem) base[index];
      }
    }

    public override void CopyTo(UIComponentItem[] array, int index) {
      for (int i = index, j = Count; i < j; i++) {
        array.SetValue(base[i], i);
      }
    }

    public int Remove(UIComponentItemList list) {
      IEnumerator<UIComponentItem> enumerator = this.GetEnumerator();

      int counter = 0;
      for (int i = 0; i < list.Count; i++) {
        if (base.Remove(list[i])) {
          counter++;
        }
      }
      return counter;
    }

    #endregion Public methods

  } // class FixedList

} // namespace Empiria.Ontology
