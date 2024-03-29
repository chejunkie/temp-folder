# Temporary Folder

[![NuGet version (ChEJunkie.TempFolder)](https://img.shields.io/nuget/v/ChEJunkie.TempFolder.svg?style=flat-square)](https://www.nuget.org/packages/ChEJunkie.TempFolder/)

Provides unique temporary folders that automatically delete after use (when the class is disposed). To ensure that the temporary folder is actually deleted on destroy, attributes are automatically cleared from contained files and subdirectories when necessary.

The default constructor places the temporary folder in the user temp directory under a folder with the name of the calling assembly. Use `Root` and `FullName` properties to view the automatic definitions. Since the temporary folder implements `IDisposable` it is easiest to implment with a using statement:

```csharp
[TestMethod]
public void Create_And_Destroy_Success()
{
    TempFolder dir;
    using (dir = new TempFolder())
    {
        // Root = <user temp directory>//<calling assembly name>
        // FullName = <root>//<randomly generated name>
        Debug.WriteLine($"Root: {dir.Root}");
        Debug.WriteLine($"FullName: {dir.FullName}");
        Assert.IsTrue(dir.Exists);
    }
    Debug.WriteLine($"Removed? {!dir.Exists}");
    Assert.IsFalse(dir.Exists);
}
```

If you choose, you can explicitaly define the root and temporary folder name in the constructor overload.
