using System.Diagnostics;
using System.Reflection;

namespace Helpers.IO.Folders
{
    public class TempFolder : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="T:TempFolder.TempFolder" /> class.</summary>
        /// <param name="root">The root.</param>
        /// <param name="name">The name.</param>
        /// <remarks>The local temp (user) directory is used as the root, and the calling assembly as the folder name.</remarks>
        public TempFolder() : this("", "")
        { }

        /// <summary>Initializes a new instance of the <see cref="T:TempFolder.TempFolder" /> class.</summary>
        /// <param name="root">The path where the temp folder is created.</param>
        /// <param name="name">The name of the temp folder.</param>
        /// <remarks>Defaults will be used for any null, empty or invalid inputs.</remarks>
        public TempFolder(string root, string name)
        {
            if (string.IsNullOrEmpty(root) || !Directory.Exists(root))
            {
                root = Path.GetTempPath();
                string callerName;
                try
                {
                    var currentAssembly = Assembly.GetExecutingAssembly();
                    var callerAssemblies = new StackTrace().GetFrames()
                                .Select(x => x.GetMethod().ReflectedType.Assembly).Distinct()
                                .Where(x => x.GetReferencedAssemblies().Any(y => y.FullName == currentAssembly.FullName));
                    callerName = Path.GetFileNameWithoutExtension(callerAssemblies.Last().ManifestModule.Name);
                }
                catch (Exception)
                {
                    callerName = Path.GetFileNameWithoutExtension(Assembly.GetCallingAssembly().ManifestModule.Name);
                }
                root= Path.Combine(root, callerName);
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                name = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            }
            Root = root;
            FullName = Path.Combine(Root, name);

            Clean(Root);
            Create();
        }

        /// <summary>Gets a value indicating whether this <see cref="T:TempFolder.TempFolder" /> is exists.</summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.</value>
        public bool Exists => Directory.Exists(FullName);

        /// <summary>Gets full path of the directory.</summary>
        /// <value>The full name.</value>
        public string FullName { get; private set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="T:TempFolder.TempFolder" /> is disposed.</summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.</value>
        private bool Disposed { get; set; }

        /// <summary>Gets the root path of the temporary folder.</summary>
        /// <value>The root path.</value>
        public string Root { get; private set; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Clears the attributes of all files and subdirectories.</summary>
        /// <param name="path">The path.</param>
        private static void ClearAttributes(
            string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                File.SetAttributes(dirPath, FileAttributes.Normal);

                string[] subDirs = Directory.GetDirectories(dirPath);
                foreach (string dir in subDirs)
                {
                    ClearAttributes(dir);
                }

                string[] files = files = Directory.GetFiles(dirPath);
                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }
            }
        }

        /// <summary>Cleans the specified folder directory.</summary>
        /// <param name="fullName">The full name.</param>
        private void Clean(
            string fullName)
        {
            try
            {
                if (Directory.Exists(fullName))
                {
                    Directory.Delete(fullName, true);
                }
            }
            catch (IOException)
            {
                ClearAttributes(fullName);
                ClearDirectory(fullName);
            }
        }
        /// <summary>Clears the directory of all files and subdirectories.</summary>
        /// <param name="dirPath">The dir path.</param>
        /// <remarks>Deletes them.</remarks>
        private void ClearDirectory(
            string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                DirectoryInfo directory = new DirectoryInfo(dirPath);

                directory.EnumerateFiles()
                    .ToList().ForEach(f => f.Delete());

                directory.EnumerateDirectories()
                    .ToList().ForEach(d => d.Delete(true));

                try
                {
                    Directory.Delete(dirPath, true);
                }
                catch (IOException) { }
            }
        }

        /// <summary>Creates the temp folder for this instance.</summary>
        private void Create()
        {
            try
            {
                if (!Directory.Exists(FullName))
                {
                    Directory.CreateDirectory(FullName);
                }
            }
            catch (IOException)
            {
            }
        }

        /// <summary>
        /// Releases managed and unmanaged resources.
        /// </summary>
        /// <remarks>
        /// After disposing, no other object is referenced by it anymore.
        /// </remarks>
        private void Dispose(
            bool disposing)
        {
            if (!Disposed)
            {
                this.ReleaseUnmanagedResources();

                if (disposing)
                {
                    this.ReleaseManagedResources();
                }

                Disposed = true;
            }
        }

        /// <summary>Releases the managed resources.</summary>
        private void ReleaseManagedResources()
        {
            Clean(FullName);
        }

        /// <summary>Releases the unmanaged resources.</summary>
        private void ReleaseUnmanagedResources()
        {
        }

        /// <summary>
        /// Throws an exception if something is tried with an already disposed object.
        /// </summary>
        /// <remarks>
        /// All public methods should call this first.
        /// </remarks>
        private void ThrowIfDisposed()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
    }
}