Welcome To The Morph3D Character System!


********************************************
************** GETTING STARTED *************
********************************************

Step 1 —- If this is the first time that you've imported A Morph3D Package, you'll notice that there isn't any content in your folders. Don’t worry, all you need to do is find the “Packages” folder and inside of it, double click the “Shaders” unity package. Unity will then unpack and import your MCS content. 

When the import process finishes, double click the MCSMale, MCSFemale, MCSFemaleLite or MCSMaleLite package, and import your content. Now you are all set to start using the MCS System.

Step 2 —- Drag a Male or Female Character into a scene. Select the root game object of your character in your scene, if Step 1 was successful, you’ll find in the Editor pane the  ”M3D Character Manager" component. 

NOTE : If this component is missing or the MCS figure is not textured, reimport the MCS figure - you should now be ready for Step 3.

Step 3 —- Use the blendshape sliders to shape a character of your liking by toggling down the “Blendshapes” menu in the M3D Character Manager.

Step 4 —- Hair, Props, and Clothing are called “Costume Items" in the Morph3D Character System. To add a “Costume Item” to your figure, drag an MCS fbx prefab into the "Show Content Packs" slot found in the M3D Character Manager. You now have access to hiding and showing the Costume Item (hair, clothing and props) via the appropriate dropdown in the M3D Character Manager.


********************************************
*************** Updating MCS ***************
********************************************

Step 1 —- To update your current version of the MCS System begin by importing your package of choice. This will update your MCS scripts and will add the “Packages” folder.

Step 2 —- Double click the “Shaders.unitypackage” located inside the “Packages” folder. This will import the MCS custom shaders.

Step 3 —- Double click on the MCSMale, MCSFemale, MCSFemaleLite or MCSMaleLite package. This will overwrite your current MCS content with the latest version.

NOTE : If there are errors or if something in Step 1 —- 3 went wrong, you can update an alternate way.

	1 —- Start by deleting your Scripts, Resources and Vendor folders located inside the MORPH3D folder.
	
	2 —- Delete the StarterPacks, SharedTextures, Shaders and the Vendor folders located inside the Content folder.
	
	3 —- At this point, you should only have the Packages folder remaining. To continue, import again your MCSMale, MCSFemale, MCSFemaleLite or MCSMaleLite product ie from the Unity Asset Store or Morph3D Store. Proceed to follow the first step under Getting Started.
	
********************************************
******** Updating Add-On Products **********
********************************************

Step 1 —- Update your MCS add-on products by simply importing your chosen package into your project.

Step 2 —- If something goes awry, delete the Product's folder found in ContentPacks and perform a fresh import of that add-on product.


********************************************
****************** CREDITS *****************
********************************************

The "Vendor" contains shaders provided by the Unity Blacksmith project found here : https://www.assetstore.unity3d.com/en/#!/content/39941
If you haven't explored the source code for that project, you should, it's enlightening stuff.

Morph3D has had to make a few changes to those shaders and the GUIs for them so that our system can work as needed. Moving forward, we will be migrating into truly unique shaders inspired by the Volund shaders, but, for now, we've moved them into this vendor folder so credit can be given properly, and so the differences in them will not interfere with the originals, should you be using them.