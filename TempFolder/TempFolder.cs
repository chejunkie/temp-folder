﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ChEJunkie.Utilities.IO;

/// <summary>
/// Represents a temporary folder that is automatically cleaned up upon disposal.
/// </summary>
public class TempFolder : IDisposable
{
    private const string DefaultName = "";
    private const string DefaultRoot = "";
        { }
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TempFolder"/> class.
    /// </summary>
    public TempFolder() 
        : this(DefaultRoot, DefaultName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TempFolder"/> class
    /// with a specified root path and name.
    /// </summary>
    /// <param name="root">The root directory where the temporary folder should be created.</param>
    /// <param name="name">The name of the temporary folder.</param>
    public TempFolder(
        string root, 
        string name)
    {
        Root = DetermineRootPath(root);
        var validName = EnsureValidName(name);
        FullName = Path.Combine(Root, validName);
        Create();
    }
            FullName = Path.Combine(Root, name);

            Clean(Root);
            Create();
        }

    /// <summary>
    /// Gets a value indicating whether the temporary folder exists.
    /// </summary>
    public bool Exists => Directory.Exists(FullName);

    /// <summary>
    /// Gets the full path of the temporary folder.
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Gets the root directory for the temporary folder.
    /// </summary>
    public string Root { get; }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    /// <summary>
    /// Releases the unmanaged and optionally managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed 
    /// and unmanaged resources; false to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(
        bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Clean(FullName);
            }
                string[] subDirs = Directory.GetDirectories(dirPath);
                foreach (string dir in subDirs)
                {
                    ClearAttributes(dir);
                }
    private static void Clean(
        string path)
    {
        try
        {
            Directory.Delete(path, true);
        }
        catch (IOException)
        {
            ClearAttributes(path);
            DeleteDirectoryContents(path);
            Directory.Delete(path, true);
        }
    }

    private static void ClearAttributes(
        string path)
    {
        if (Directory.Exists(path))
        {
            File.SetAttributes(path, FileAttributes.Normal);
            {
                DirectoryInfo directory = new DirectoryInfo(dirPath);

                directory.EnumerateFiles()
                    .ToList().ForEach(f => f.Delete());

            foreach (var file in Directory.GetFiles(path))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }
        }
    }
                    Directory.Delete(dirPath, true);
                }
                catch (IOException) { }
            }
        }

    private static void DeleteDirectoryContents(
        string path)
    {
                file.Delete();
            }

            foreach (var dir in directory.EnumerateDirectories())
            {
                dir.Delete(true);
            }
        }
    }

    private static string EnsureValidName(
        string name)
    {
        var invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        var r = new Regex($"[{Regex.Escape(invalidChars)}]");
        name = r.Replace(name, "");

        return string.IsNullOrWhiteSpace(name) ? Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) : name;
    }
            }
            catch (IOException)
            {
            }
        }

    private static string GetCallerAssemblyName()
    {
        try
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            var callerAssemblies = new StackTrace().GetFrames()
                        .Where(x => x.GetMethod() != null && x.GetMethod()!.ReflectedType != null)
                        .Select(x => x.GetMethod()!.ReflectedType!.Assembly)
                        .Distinct()
                        .Where(x => x.GetReferencedAssemblies()
                            .Any(y => y.FullName == currentAssembly.FullName));

            return Path.GetFileNameWithoutExtension(callerAssemblies.Last().ManifestModule.Name);
        }
        catch
        {
            return Path.GetFileNameWithoutExtension(Assembly.GetCallingAssembly().ManifestModule.Name);
        }
    }

    private void Create()
    {
        if (Directory.Exists(FullName))
        {
            Clean(FullName);
        }
        else
    private static string DetermineRootPath(
        string root)
    {
        if (string.IsNullOrEmpty(root) || !Directory.Exists(root))
        {
            root = Path.GetTempPath();

            var callerName = GetCallerAssemblyName();
            root = Path.Combine(root, callerName);
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        return root;
    }
}