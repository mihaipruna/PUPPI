PUPPICAD version 0.60
PUPPI version 0.3.1.0


-PUPPICAD is a free parametric geometry generator and Engineering visual development environment powered by the Parametric Universal Programming and Process Interface.PUPPI is available at http://visualprogramminglanguage.com
PUPPICAD displays a revolutionary 3D visual programming canvas which can also become a GUI for non-programmers. PUPPICAD also has a CAD visualization window from which the user can export 3D models to several file formats.
PUPPICAD was created by Programmatic Modeling Connection, LLC.

-PUPPICAD is in early access. Programmatic Modeling Connection, LLC disclaims all warranties. Use at own risk.

-With PUPPICAD you can build parametric geometry objects based on complex rules and observe the design through various stages at the same time.
PUPPICAD comes with almost 3000 different node types (visual programming modules) providing unparallel control for automating design and engineering tasks.

-PUPPICAD can easily be expanded in three ways:
	1) Users can save visual code snippets and import them as containers, which appear as a single node.(File Menu)
	2) PUPPICAD can generate visual programming modules directly from .NET dll libraries of precompiled code (class constructors, methods and enumerations are converted into PUPPIModule objects). 
	   The Configure Dynamic PUPPIModules tool is accessible from the File Menu.
	3) Developers with PUPPI licenses can create and distribute PUPPIModule dlls.These are loaded automatically into PUPPICAD if placed in the subfolder "PluginPUPPIModules".

Troubleshooting:

If script nodes don't run, you need to install the .NET Framework version 3.5 from here:
https://www.microsoft.com/en-us/download/details.aspx?id=21

Please report bugs or any feedback to us at: puppicad@pupi.co. It helps if you enable debug mode, then perform the same actions that are causing trouble,click the button with red text, then paste the debug log in the body of the email.

If the program crashes outright, it is probably because of an incompatible dll file that is being converted into PUPPIModules at runtime. PUPPICAD will clear all runtime module creation settings (the menu option to convert dll files to PUPPIModules at runtime is in the File menu). You can then restart PUPPICAD. The Parametric Universal Programming and Process Interface toolkit has advanced tools for creating PUPPIModules from existing .NET code which could work with more dll files. You can get PUPPI at visualprogramminglanguage.com

In case of a program crash at startup, you should send us the errorlog.txt file present in the same folder as the PUPPICAD executable.

