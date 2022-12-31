using ChEJunkie.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace TempFolderTests
{
    [TestClass]
    public class TempFolderTests
    {
        [TestMethod]
        public void Create_And_Destroy_Success()
        {
            // Arrange
            // Act
            TempFolder dir;
            using (dir = new TempFolder())
            {
                Debug.WriteLine($"Root: {dir.Root}");
                Debug.WriteLine($"FullName: {dir.FullName}");
                Assert.IsTrue(dir.Exists);
            }

            // Assert
            Assert.IsFalse(dir.Exists);
            Debug.WriteLine($"Removed? {!dir.Exists}");
        }

        [TestMethod]
        public void Direct_Create_And_Destroy_Success()
        {
            // Arrange
            string root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Path.GetRandomFileName());
            string name = "blah-blah-blah";
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            // Act
            TempFolder dir;
            using (dir = new TempFolder(root, name))
            {
                Debug.WriteLine($"Root: {dir.Root}");
                Debug.WriteLine($"FullName: {dir.FullName}");
                Assert.IsTrue(dir.Exists);
            }

            // Assert + cleanup
            Assert.IsFalse(dir.Exists);
            try
            {
                Directory.Delete(dir.Root);
            }
            catch (Exception)
            {
                Debug.WriteLine("*** UNABLE TO DELETE ROOT ***");
            }
            Debug.WriteLine($"Removed? {!dir.Exists}");
        }
    }
}