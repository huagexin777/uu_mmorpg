This folder contains the RadControls for WinForms assemblies only - there is no installer. The controls should be installed in both Visual Studio Toolbox and GAC manually.

The internal build is not an add-on to the assemblies of an official RadControl for WinForms version. This means that you can use this internal build without any version of RadControls for WinForms installed. Please refer to the full list of steps that will help you to use the assemblies in your application. 

===============================================
INSTALLATION and PROJECT UPDATE INSTRUCTIONS
===============================================

1. Extract the assemblies from the archive in a folder. Make sure that you will not move or rename this folder later on.

2. Install the unzipped assemblies in the GAC. To do this, refer to the following knowledge base article: 
http://www.telerik.com/support/kb/winforms/general/adding-a-control-to-the-gac-global-assembly-cache-for-winforms.aspx
Make sure that the Telerik.WinForms.UI.Design.dll is registered in the GAC as well, since it is required for the design-time of RadControls for WinForms.

3. Install the unzipped assemblies in the Visual Studio Toolbox. To do this, refer to the following knowledge base article: 
http://www.telerik.com/support/kb/winforms/general/add-radcontrols-for-winforms-to-the-toolbox.aspx

4. Back up your project.

5. Update the references in your project. You may need to restart Visual Studio in order for this change to take effect (Visual Studio usually caches the assemblies).
When you update the references of your project to the new version, you may get “NoSuchPropertyRegistered” error. In order to overcome this error, refer to this help article: http://www.telerik.com/help/winforms/application_upgrade.html
