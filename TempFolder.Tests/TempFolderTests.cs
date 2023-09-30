using ChEJunkie.Utilities.IO;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace CheJunkie.Utilities.IO.Tests
{
    public class TempFolderTests
    {
        [Fact]
        public void Create_And_Destroy_Success()
        {
            TempFolder dir;
            using (dir = new TempFolder())
            {
                Debug.WriteLine($"Root: {dir.Root}");
                Debug.WriteLine($"FullName: {dir.FullName}");
                Assert.True(dir.Exists);
            }
            Assert.False(dir.Exists);
            Debug.WriteLine($"Removed? {!dir.Exists}");
        }

        [Fact]
        public void Direct_Create_And_Destroy_Success()
        {
            string root = Path.Combine(Environment
                .GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    "supercalifragilisticexpialidocious"
            );
            const string name = "blah-blah-blah";
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            else
            {
                // Replace AssertFailedException with Xunit's exception or assertion
                Assert.True(false, "User root already exists. " +
                                  "Kick-out to prevent potential data loss situation " +
                                  "(i.e., the test attempts to delete the root at the end).");
            }
            TempFolder dir;
            using (dir = new TempFolder(root, name))
            {
                Debug.WriteLine($"Root: {dir.Root}");
                Debug.WriteLine($"FullName: {dir.FullName}");
                Assert.True(dir.Exists);
            }
            Assert.False(dir.Exists);
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