using System;
using System.Collections.Generic;
using System.Linq;
using Rainbow.Model;
using Rainbow.Storage;
using Rainbow.Storage.Yaml;
using RainbowCodeGeneration.Models;
using RainbowCodeGeneration.Rainbow;

namespace RainbowCodeGeneration
{
    public static class RainbowReader
    {
        private static readonly Guid TemplateGuid = new Guid("{AB86861A-6030-46C5-B394-E8F99E8B87DB}");
        private const string MasterDb = "master";

        public static IEnumerable<Template> GetTemplates(string physicalRootPath, string treeName, string treePath)
        {
            if (!System.IO.Directory.Exists(physicalRootPath))
                throw new InvalidOperationException($"Could not find the root path {physicalRootPath}");
            if (!System.IO.Directory.Exists($"{physicalRootPath}\\{treeName}"))
                throw new InvalidOperationException($"Could not find the tree with path {physicalRootPath}\\{treeName}");
            var ds = new SerializationFileSystemDataStore(physicalRootPath, false,
                    new TreeRootFactory(treeName, treePath, MasterDb),
                    new YamlSerializationFormatter(null, null));
            var templates = ds.GetMetadataByTemplateId(TemplateGuid, MasterDb);

            return templates.Select(tempalte => ds.GetById(tempalte.Id, MasterDb)).Select(item => new Template(item, GetTemplateFields(item)));
        }

        private static IEnumerable<IItemData> GetTemplateFields(IItemData template)
        {
            return template.GetChildren().SelectMany(templateSection => templateSection.GetChildren());
        }
    }
}
