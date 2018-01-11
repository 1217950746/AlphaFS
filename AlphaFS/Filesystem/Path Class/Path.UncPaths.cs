/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using Alphaleonis.Win32.Network;
using System;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      /// <summary>[AlphaFS] Gets the connection name of the locally mapped drive.</summary>
      /// <returns>The server and share as: \\servername\sharename.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="path">The local path with drive name.</param>
      [SecurityCritical]
      public static string GetMappedConnectionName(string path)
      {
         return Host.GetRemoteNameInfoCore(path, true).lpConnectionName;
      }




      /// <summary>[AlphaFS] Gets the network share name from the locally mapped path.</summary>
      /// <returns>The network share connection name of <paramref name="path"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="path">The local path with drive name.</param>
      [SecurityCritical]
      public static string GetMappedUncName(string path)
      {
         return Host.GetRemoteNameInfoCore(path, true).lpUniversalName;
      }




      /// <summary>[AlphaFS] Determines if a path string is a valid Universal Naming Convention (UNC) path.</summary>
      /// <returns><see langword="true"/> if the specified path is a Universal Naming Convention (UNC) path, <see langword="false"/> otherwise.</returns>
      /// <param name="path">The path to check.</param>
      [SecurityCritical]
      public static bool IsUncPath(string path)
      {
         return IsUncPathCore(path, false, true);
      }




      /// <summary>[AlphaFS] Converts a local path to a network share path.  
      ///   <para>A Local path, e.g.: "C:\Windows" or "C:\Windows\" will be returned as: "\\localhost\C$\Windows".</para>
      ///   <para>If a logical drive points to a network share path (mapped drive), the share path will be returned without a trailing <see cref="DirectorySeparator"/> character.</para>
      /// </summary>
      /// <returns>On successful conversion a UNC path is returned.
      ///   <para>If the conversion fails, <paramref name="localPath"/> is returned.</para>
      ///   <para>If <paramref name="localPath"/> is an empty string or <see langword="null"/>, <see langword="null"/> is returned.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      [SecurityCritical]
      public static string LocalToUnc(string localPath)
      {
         return LocalToUncCore(localPath, PathFormat.RelativePath, GetFullPathOptions.None);
      }


      /// <summary>[AlphaFS] Converts a local path to a network share path.  
      ///   <para>A Local path, e.g.: "C:\Windows" or "C:\Windows\" will be returned as: "\\localhost\C$\Windows".</para>
      ///   <para>If a logical drive points to a network share path (mapped drive), the share path will be returned without a trailing <see cref="DirectorySeparator"/> character.</para>
      /// </summary>
      /// <returns>On successful conversion a UNC path is returned.
      ///   <para>If the conversion fails, <paramref name="localPath"/> is returned.</para>
      ///   <para>If <paramref name="localPath"/> is an empty string or <see langword="null"/>, <see langword="null"/> is returned.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      [SecurityCritical]
      public static string LocalToUnc(string localPath, PathFormat pathFormat)
      {
         return LocalToUncCore(localPath, pathFormat, GetFullPathOptions.None);
      }
      

      /// <summary>[AlphaFS] Converts a local path to a network share path, optionally returning it in a long path format and the ability to add or remove a trailing backslash.
      ///   <para>A Local path, e.g.: "C:\Windows" or "C:\Windows\" will be returned as: "\\localhost\C$\Windows".</para>
      ///   <para>If a logical drive points to a network share path (mapped drive), the share path will be returned without a trailing <see cref="DirectorySeparator"/> character.</para>
      /// </summary>
      /// <returns>On successful conversion a UNC path is returned.
      ///   <para>If the conversion fails, <paramref name="localPath"/> is returned.</para>
      ///   <para>If <paramref name="localPath"/> is an empty string or <see langword="null"/>, <see langword="null"/> is returned.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <param name="fullPathOptions">Options for controlling the full path retrieval.</param>
      [SecurityCritical]
      public static string LocalToUnc(string localPath, GetFullPathOptions fullPathOptions)
      {
         return LocalToUncCore(localPath, PathFormat.RelativePath, fullPathOptions);
      }


      #region Internal Methods

      /// <summary>[AlphaFS] Determines if a path string is a valid Universal Naming Convention (UNC) path, optionally skip invalid path character check.</summary>
      /// <returns><see langword="true"/> if the specified path is a Universal Naming Convention (UNC) path, <see langword="false"/> otherwise.</returns>
      /// <param name="path">The path to check.</param>
      /// <param name="isRegularPath">When <see langword="true"/> indicates that <paramref name="path"/> is already in regular path format.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      [SecurityCritical]
      internal static bool IsUncPathCore(string path, bool isRegularPath, bool checkInvalidPathChars)
      {
         if (!isRegularPath)
            path = GetRegularPathCore(path, checkInvalidPathChars ? GetFullPathOptions.CheckInvalidPathChars : GetFullPathOptions.None, false);

         else if (checkInvalidPathChars)
            CheckInvalidPathChars(path, false, false);


         Uri uri;
         return Uri.TryCreate(path, UriKind.Absolute, out uri) && uri.IsUnc;
      }


      /// <summary>[AlphaFS] Converts a local path to a network share path, optionally returning it in a long path format and the ability to add or remove a trailing backslash.
      ///   <para>A Local path, e.g.: "C:\Windows" or "C:\Windows\" will be returned as: "\\localhost\C$\Windows".</para>
      ///   <para>If a logical drive points to a network share path (mapped drive), the share path will be returned without a trailing <see cref="DirectorySeparator"/> character.</para>
      /// </summary>
      /// <returns>On successful conversion a UNC path is returned.
      ///   <para>If the conversion fails, <paramref name="localPath"/> is returned.</para>
      ///   <para>If <paramref name="localPath"/> is an empty string or <see langword="null"/>, <see langword="null"/> is returned.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <param name="fullPathOptions">Options for controlling the full path retrieval.</param>
      [SecurityCritical]
      internal static string LocalToUncCore(string localPath, PathFormat pathFormat, GetFullPathOptions fullPathOptions)
      {
         if (Utils.IsNullOrWhiteSpace(localPath))
            return null;

         if (pathFormat == PathFormat.RelativePath)
            CheckSupportedPathFormat(localPath, true, true);
         

         var addTrailingDirectorySeparator = (fullPathOptions & GetFullPathOptions.AddTrailingDirectorySeparator) != 0;
         var removeTrailingDirectorySeparator = (fullPathOptions & GetFullPathOptions.RemoveTrailingDirectorySeparator) != 0;

         if (addTrailingDirectorySeparator && removeTrailingDirectorySeparator)
            throw new ArgumentException(Resources.GetFullPathOptions_Add_And_Remove_DirectorySeparator_Invalid, "fullPathOptions");


         if (!removeTrailingDirectorySeparator && !addTrailingDirectorySeparator)
         {
            // Add a trailing backslash when "localPath" ends with a backslash.
            if (localPath.EndsWith(DirectorySeparator, StringComparison.Ordinal))
            {
               fullPathOptions &= ~GetFullPathOptions.RemoveTrailingDirectorySeparator; // Remove removal of trailing backslash.
               fullPathOptions |= GetFullPathOptions.AddTrailingDirectorySeparator;     // Add adding trailing backslash.
            }
         }


         var getAsLongPath = (fullPathOptions & GetFullPathOptions.AsLongPath) != 0;

         var returnUncPath = GetRegularPathCore(localPath, fullPathOptions | GetFullPathOptions.CheckInvalidPathChars, false);
         

         if (!IsUncPathCore(returnUncPath, true, false))
         {
            if (returnUncPath[0] == CurrentDirectoryPrefixChar || !IsPathRooted(returnUncPath, false))
               returnUncPath = GetFullPathCore(null, returnUncPath, GetFullPathOptions.None);


            var drive = GetPathRoot(returnUncPath, false);

            if (Utils.IsNullOrWhiteSpace(drive))
               return returnUncPath;
            

            var remoteInfo = Host.GetRemoteNameInfoCore(returnUncPath, true);


            // Network share.
            if (!Utils.IsNullOrWhiteSpace(remoteInfo.lpUniversalName))
               return getAsLongPath ? GetLongPathCore(remoteInfo.lpUniversalName, fullPathOptions) : GetRegularPathCore(remoteInfo.lpUniversalName, fullPathOptions, false);


            // Network root.
            if (!Utils.IsNullOrWhiteSpace(remoteInfo.lpConnectionName))
               return getAsLongPath ? GetLongPathCore(remoteInfo.lpConnectionName, fullPathOptions) : GetRegularPathCore(remoteInfo.lpConnectionName, fullPathOptions, false);


            // Split: localDrive[0] = "C", localDrive[1] = "\Windows"
            var localDrive = returnUncPath.Split(VolumeSeparatorChar);

            // Return: "\\localhost\C$\Windows"
            returnUncPath = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}${3}", Host.GetUncName(), DirectorySeparator, localDrive[0], localDrive[1]);
         }


         return getAsLongPath ? GetLongPathCore(returnUncPath, fullPathOptions) : GetRegularPathCore(returnUncPath, fullPathOptions, false);
      }

      #endregion // Internal Methods
   }
}
