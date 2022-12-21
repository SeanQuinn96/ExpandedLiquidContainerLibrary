using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vintagestory.API.Common;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("liquidcontainerlibrary")]
[assembly: AssemblyDescription("")]
//[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]

[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2cb12963-5d53-4cb2-b23f-8bb9133e98ba")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.*")]

[assembly: ModInfo( "liquidcontainerlibrary", "liquidcontainerlibrary",
    Version = "1.0.0",
    Description = "Allows modders to give their liquid storage containers customisable spoilage/transition rates",
    Authors = new[] { "Sean_OCuin" })]

 [assembly: ModDependency("game")]
