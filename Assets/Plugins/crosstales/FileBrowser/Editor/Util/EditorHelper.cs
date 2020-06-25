﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Crosstales.FB.EditorUtil
{
   /// <summary>Editor helper class.</summary>
   public abstract class EditorHelper : Common.EditorUtil.BaseEditorHelper
   {
      #region Static variables

      private static Texture2D logo_asset;
      private static Texture2D logo_asset_small;

      private static Texture2D icon_file;

      #endregion


      #region Static properties

      public static Texture2D Logo_Asset
      {
         get { return loadImage(ref logo_asset, "logo_asset_pro.png"); }
      }

      public static Texture2D Logo_Asset_Small
      {
         get { return loadImage(ref logo_asset_small, "logo_asset_small_pro.png"); }
      }

      public static Texture2D Icon_File
      {
         get { return loadImage(ref icon_file, "icon_file.png"); }
      }

      #endregion


      #region Static methods

      /// <summary>Loads an image as Texture2D from 'Editor Default Resources'.</summary>
      /// <param name="logo">Logo to load.</param>
      /// <param name="fileName">Name of the image.</param>
      /// <returns>Image as Texture2D from 'Editor Default Resources'.</returns>
      private static Texture2D loadImage(ref Texture2D logo, string fileName)
      {
         if (logo == null)
         {
#if CT_DEVELOP
            logo = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets" + EditorConfig.ASSET_PATH + "Icons/" + fileName, typeof(Texture2D));
#else
                logo = (Texture2D)EditorGUIUtility.Load("crosstales/FileBrowser/" + fileName);
#endif

            if (logo == null)
            {
               Debug.LogWarning("Image not found: " + fileName);
            }
         }

         return logo;
      }

      #endregion
   }
}
#endif
// © 2019-2020 crosstales LLC (https://www.crosstales.com)