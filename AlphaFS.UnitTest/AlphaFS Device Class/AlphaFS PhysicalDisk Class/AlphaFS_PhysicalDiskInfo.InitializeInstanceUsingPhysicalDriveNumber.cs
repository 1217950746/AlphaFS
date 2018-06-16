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

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_PhysicalDiskInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_PhysicalDiskInfo_InitializeInstanceUsingPhysicalDriveNumber_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var physicalDisks = Alphaleonis.Win32.Filesystem.Volume.QueryAllDosDevices().Where(device => device.StartsWith("PhysicalDrive", StringComparison.OrdinalIgnoreCase)).ToArray();

         Console.WriteLine("Found: [{0}] physical drives.\n", physicalDisks.Length);


         for (var physicalDiskNumber = 0; physicalDiskNumber < physicalDisks.Length; physicalDiskNumber++)
         {
            Console.WriteLine("#{0:000}\tInput Physical Disk Number: [{1}]", physicalDiskNumber + 1, physicalDiskNumber);

            var pDisk = new Alphaleonis.Win32.Device.PhysicalDiskInfo(physicalDiskNumber);

            UnitTestConstants.Dump(pDisk);

            UnitTestConstants.Dump(pDisk.StorageAdapterInfo, true);

            UnitTestConstants.Dump(pDisk.StorageDeviceInfo, true);

            UnitTestConstants.Dump(pDisk.StoragePartitionInfo, true);


            Assert.AreEqual(physicalDiskNumber, pDisk.StorageAdapterInfo.DeviceNumber);

            Assert.AreEqual(physicalDiskNumber, pDisk.StorageDeviceInfo.DeviceNumber);

            Assert.AreEqual(physicalDiskNumber, pDisk.StoragePartitionInfo.DeviceNumber);

            Console.WriteLine();
         }
      }
   }
}