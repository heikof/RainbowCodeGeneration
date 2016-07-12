# RainbowCodeGeneration

[![NuGet](https://img.shields.io/nuget/v/RainbowCodeGeneration.svg?maxAge=2592000)](https://www.nuget.org/packages/RainbowCodeGeneration)

A simple set of utility classes and a sample T4 template that allow easy code generation for Sitecore templates from [Rainbow](https://github.com/kamsar/Rainbow) / [Unicorn](https://github.com/kamsar/Unicorn) serialized items. Currently, only the [YAML-based serialisation format](https://github.com/kamsar/Rainbow/tree/master/src/Rainbow.Storage.Yaml) is supported.

## Using the Rainbow / Unicorn code generation

You are probably already using Unicorn with the YAML serialisation format (you really should - it's awesome!). I am also assuming that you serialise your templates in a dedicated folder inside your solution, ideally in a modular fashion like in [Sitecore Habitat](https://github.com/Sitecore/Habitat). Using the Rainbow / Unicorn code generation is simple: 
* Install the nuget package 
* Configure the T4 template 
* Re-Run the code generation when Sitecore templates change 

### Installing the nuget package

Install the [RainbowCodeGeneration package](https://www.nuget.org/packages/RainbowCodeGeneration). This installs a set of simple utility classes and an example T4 template. The template will generate an empty `SitecoreTemplates` struct in your project.

![Empty Sitecore Template](http://www.heikofranz.info/wp-content/uploads/2016/07/RainbowCodeGeneration-Empty.png)

### Configuring the T4 template 

Adjust a few settings on the T4 template `SitecoreTemplates.tt` to make it run in your configuration. 

* Ensure the assembly references at the top of the file resolve correctly (especially the path to your Sitecore.Kernel.dll)
* The `physicalFileStore` setting needs to point to where you store the Unicorn / Rainbow items for your project. The out of the box setting uses the relative paths that Habitat uses. 
* The `treeName` setting is the name of the sub-tree in Unicorn. This is the name of the include on your predicate in Unicorn. The "News" feature in Habitat would use "Feature.News.Templates"
* The `treePath` setting is the matching path in Sitecore. The "News" feature in Habitat would use "/sitecore/templates/Feature/News"

![Adjust Settings](http://www.heikofranz.info/wp-content/uploads/2016/07/RainbowCodeGeneration-Settings.png)

The code generation for your Sitecore templates will run when you save the T4 template. 

You can tweak the T4 template to your liking. The entire `IItemData` object from Rainbow for the template and its fields is available within the template. 

### Re-run code generation

When your Unicorn / Rainbow items update, you will have to re-run the code generation. In Visual Studio, right-click on the T4 template and select "Run Custom Tool" to re-run the code generation. 

![Adjust Settings](http://www.heikofranz.info/wp-content/uploads/2016/07/RainbowCodeGeneration-Example.png)

### Known issues in 0.1.1

#### Visual Studio crashes when the underlying Unicorn items change
The code generation can make Visual Studio crash if the Sitecore.Logging.dll is not found. To fix this issue, add a reference to Sitecore.Logging.dll in the T4 template `SitecoreTemplates.tt` (in the same way that  Sitecore.Kernel.dll is referenced). This issue will be fixed in the next release. 

#### Re-serialisation fails if Visual Studio solution is open
Re-serialising an entire configuration fails if Visual Studio is open and a code generation is covering the configuration. Visual Studio appears to retain a lock on the folder and Unicorn cannot remove it. Close the solution and re-open it after you re-serialised the configuration. 
