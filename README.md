# RainbowCodeGeneration

[![NuGet](https://img.shields.io/nuget/v/RainbowCodeGeneration.svg?maxAge=259200)](https://www.nuget.org/packages/RainbowCodeGeneration)

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

* Ensure the assembly references at the top of the file resolve correctly (e.g. the path to your Sitecore.Kernel.dll). I have added an example based on NuGet packages however it is likely that you need to update the version numbers. The code generation is compatible with Unicorn 3 and 4 (Rainbow 1 and 2). 
* The `physicalFileStore` setting needs to point to where you store the Unicorn / Rainbow items for your project. The out of the box setting uses the relative paths that Habitat uses. 
* The `treeName` setting is the name of the sub-tree in Unicorn. This is the name of the folder for your Unicorn predicate to generate code for. 
* The `treePath` setting is the matching path in Sitecore. The "News" feature in Habitat would use "/sitecore/templates/Feature/News"

![Adjust Settings](http://www.heikofranz.info/wp-content/uploads/2016/07/RainbowCodeGeneration-Settings.png)

* For Sitecore 9 you also need to install the NuGet package for Microsoft.Extensions.DependencyInjection in version 1.0.0 and reference the DLLs. The configuration then looks as follows (for Sitecore 9 using Unicorn 4):

```
<#@ template debug="false" hostspecific="true" language="C#" #>
<#@ output extension=".cs" #>
<#@ assembly name="System.Core" #>
<# // NOTE - Reference your NuGet packages for Rainbow and RainbowCodeGeneration here #>
<#@ assembly name="$(SolutionDir)packages\Rainbow.Core.2.0.0\lib\net452\Rainbow.dll" #>
<#@ assembly name="$(SolutionDir)packages\Rainbow.Storage.Yaml.2.0.0\lib\net452\Rainbow.Storage.Yaml.dll" #>
<#@ assembly name="$(SolutionDir)packages\RainbowCodeGeneration.0.2.0\lib\net452\RainbowCodeGeneration.dll" #>
<# // NOTE - Reference your Sitecore.Kernel.dll and Sitecore.Logging.dll here #>
<#@ assembly name="$(SolutionDir)packages\Sitecore.Kernel.NoReferences.9.0.171002\lib\net462\Sitecore.Kernel.dll" #>
<#@ assembly name="$(SolutionDir)packages\Sitecore.Logging.NoReferences.9.0.171002\lib\net462\Sitecore.Logging.dll" #>
<#@ assembly name="$(SolutionDir)packages\Microsoft.Extensions.DependencyInjection.Abstractions.1.0.0\lib\netstandard1.0\Microsoft.Extensions.DependencyInjection.Abstractions.dll" #>
<#@ assembly name="$(SolutionDir)packages\Microsoft.Extensions.DependencyInjection.1.0.0\lib\netstandard1.1\Microsoft.Extensions.DependencyInjection.dll" #>
<# 
// CONFIGURATION
var physicalFileStore = @"D:\inetpub\wwwroot\sc900\Data\Unicorn"; // the path to your serialisation items
var treeName = "Feature.Sample\\Templates"; // the name of the configuration you want to code-generate 
var treePath = "/sitecore/templates/Sample"; // the matching path in Sitecore for the configuration
```

The code generation for your Sitecore templates will run when you save the T4 template. 

You can tweak the T4 template to your liking. The entire `IItemData` object from Rainbow for the template and its fields is available within the template. 

### Re-run code generation

When your Unicorn / Rainbow items update, you will have to re-run the code generation. In Visual Studio, right-click on the T4 template and select "Run Custom Tool" to re-run the code generation. 

![Adjust Settings](http://www.heikofranz.info/wp-content/uploads/2016/07/RainbowCodeGeneration-Example.png)

### Generate code for any item (not just templates)

Starting in version 0.3 it is possible to generate code based on any item. For an example T4 template, refer to [SitecoreKnownItems.tt](https://github.com/heikof/RainbowCodeGeneration/blob/develop/src/RainbowCodeGeneration.Test/SitecoreKnownItems.tt) in the test project of the repository. 

### Known issues

#### Re-serialisation fails if Visual Studio solution is open
Re-serialising an entire configuration fails if Visual Studio is open and a code generation is covering the configuration. Visual Studio appears to retain a lock on the folder and Unicorn cannot remove it. Close the solution and re-open it after you re-serialised the configuration. 
