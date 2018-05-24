/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Shell32 and is intended to contain all Shell32 and Shell32Info Unit Tests.</summary>
   [TestClass]
   public partial class AlphaFS_Shell32Test
   {
      private void DumpPathFileExists(string path, bool doesExist)
      {
         Console.WriteLine("\n\tPath: [{0}]\n", path);

         var fileExists = Shell32.PathFileExists(path);
         Console.WriteLine("\t\tShell32.PathFileExists() == [{0}]: {1}\t\t[{2}]", doesExist ? UnitTestConstants.TextTrue : UnitTestConstants.TextFalse, doesExist == fileExists, path);
         Console.WriteLine("\t\tFile.Exists()            == [{0}]: {1}\t\t[{2}]", doesExist ? UnitTestConstants.TextTrue : UnitTestConstants.TextFalse, doesExist == File.Exists(path), path);
         Console.WriteLine("\t\tDirectory.Exists()       == [{0}]: {1}\t\t[{2}]", doesExist ? UnitTestConstants.TextTrue : UnitTestConstants.TextFalse, doesExist == Directory.Exists(path), path);

         if (doesExist)
            Assert.IsTrue(fileExists);

         if (!doesExist)
            Assert.IsTrue(!fileExists);
      }


      [TestMethod]
      public void AlphaFS_Shell32_PathCreateFromUrl()
      {
         Console.WriteLine("Filesystem.Shell32.PathCreateFromUrl()");

         var urlPath = Shell32.UrlCreateFromPath(UnitTestConstants.AppData);
         var filePath = Shell32.PathCreateFromUrl(urlPath);

         Console.WriteLine("\n\tDirectory                  : [{0}]", UnitTestConstants.AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath(): [{0}]", urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrl() == [{0}]\n", filePath);

         var startsWith = urlPath.StartsWith("file:///");
         var equalsAppData = filePath.Equals(UnitTestConstants.AppData);

         Console.WriteLine("\n\turlPath.StartsWith(\"file:///\") == [{0}]: {1}", UnitTestConstants.TextTrue, startsWith);
         Console.WriteLine("\n\tfilePath.Equals(AppData)       == [{0}]: {1}\n", UnitTestConstants.TextTrue, equalsAppData);
         Assert.IsTrue(startsWith);
         Assert.IsTrue(equalsAppData);
      }


      [TestMethod]
      public void AlphaFS_Shell32_PathCreateFromUrlAlloc()
      {
         Console.WriteLine("Filesystem.Shell32.PathCreateFromUrlAlloc()");

         var urlPath = Shell32.UrlCreateFromPath(UnitTestConstants.AppData);
         var filePath = Shell32.PathCreateFromUrlAlloc(urlPath);

         Console.WriteLine("\n\tDirectory                  : [{0}]", UnitTestConstants.AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath(): [{0}]", urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrlAlloc() == [{0}]\n", filePath);

         var startsWith = urlPath.StartsWith("file:///");
         var equalsAppData = filePath.Equals(UnitTestConstants.AppData);

         Console.WriteLine("\n\turlPath.StartsWith(\"file:///\") == [{0}]: {1}", UnitTestConstants.TextTrue, startsWith);
         Console.WriteLine("\n\tfilePath.Equals(AppData)       == [{0}]: {1}\n", UnitTestConstants.TextTrue, equalsAppData);
         Assert.IsTrue(startsWith);
         Assert.IsTrue(equalsAppData);
      }


      [TestMethod]
      public void AlphaFS_Shell32_PathFileExists()
      {
         Console.WriteLine("Filesystem.Shell32.PathFileExists()");

         var path = UnitTestConstants.SysRoot;
         DumpPathFileExists(path, true);
         DumpPathFileExists(Path.LocalToUnc(path), true);
         DumpPathFileExists("BlaBlaBla", false);
         DumpPathFileExists(Path.Combine(UnitTestConstants.SysRoot, "BlaBlaBla"), false);

         var cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (var file in Directory.EnumerateFiles(UnitTestConstants.SysRoot))
         {
            var fileExists = Shell32.PathFileExists(file);

            Console.WriteLine("\t#{0:000}\tShell32.PathFileExists() == [{1}]: {2}\t\t[{3}]", ++cnt, UnitTestConstants.TextTrue, fileExists, file);
            Assert.IsTrue(fileExists);
         }
         Console.WriteLine("\n\t{0}\n", UnitTestConstants.Reporter(true));
      }


      [TestMethod]
      public void AlphaFS_Shell32_UrlCreateFromPath()
      {
         Console.WriteLine("Filesystem.Shell32.UrlCreateFromPath()");

         AlphaFS_Shell32_PathCreateFromUrl();
         AlphaFS_Shell32_PathCreateFromUrlAlloc();
      }


      [TestMethod]
      public void AlphaFS_Shell32_UrlIs()
      {
         Console.WriteLine("Filesystem.Shell32.UrlIs()");

         var urlPath = Shell32.UrlCreateFromPath(UnitTestConstants.AppData);
         var filePath = Shell32.PathCreateFromUrlAlloc(urlPath);

         var isFileUrl1 = Shell32.UrlIsFileUrl(urlPath);
         var isFileUrl2 = Shell32.UrlIsFileUrl(filePath);
         var isNoHistory = Shell32.UrlIs(filePath, Shell32.UrlType.IsNoHistory);
         var isOpaque = Shell32.UrlIs(filePath, Shell32.UrlType.IsOpaque);

         Console.WriteLine("\n\tDirectory: [{0}]", UnitTestConstants.AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath()      == IsFileUrl == [{0}] : {1}\t\t[{2}]", UnitTestConstants.TextTrue, isFileUrl1, urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrlAlloc() == IsFileUrl == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextFalse, isFileUrl2, filePath);

         Console.WriteLine("\n\tShell32.UrlIsFileUrl()   == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextTrue, isFileUrl1, urlPath);
         Console.WriteLine("\n\tShell32.UrlIsNoHistory() == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextTrue, isNoHistory, urlPath);
         Console.WriteLine("\n\tShell32.UrlIsOpaque()    == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextTrue, isOpaque, urlPath);
         
         Assert.IsTrue(isFileUrl1);
         Assert.IsTrue(isFileUrl2 == false);
      }
   }
}
