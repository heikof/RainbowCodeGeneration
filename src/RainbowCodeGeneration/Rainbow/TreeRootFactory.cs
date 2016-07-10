using System.Collections.Generic;
using Rainbow.Storage;

namespace RainbowCodeGeneration.Rainbow
{
    class TreeRootFactory : ITreeRootFactory
    {
        private readonly string _name;
        private readonly string _path;
        private readonly string _databaseName;

        public TreeRootFactory(string name, string path, string databaseName)
        {
            _name = name;
            _path = path;
            _databaseName = databaseName;
        }

        public IEnumerable<TreeRoot> CreateTreeRoots()
        {
            yield return new TreeRoot(_name, _path, _databaseName);
        }
    }
}
