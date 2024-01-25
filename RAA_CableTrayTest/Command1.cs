#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

#endregion

namespace RAA_CableTrayTest
{
	[Transaction(TransactionMode.Manual)]
	public class Command1 : IExternalCommand
	{
		public Result Execute(
		  ExternalCommandData commandData,
		  ref string message,
		  ElementSet elements)
		{
			Document doc = commandData.Application.ActiveUIDocument.Document;
			UIDocument uiDoc = commandData.Application.ActiveUIDocument;

			try
			{
				if(uiDoc.Selection.GetElementIds().Count < 1)
				{
					TaskDialog.Show("Error", "Please select a cable tray.");
					return Result.Failed;
				}

				List<ElementId> elementIds = uiDoc.Selection.GetElementIds().ToList();

				// Start a transaction
				using (Transaction trans = new Transaction(doc, "Copy and Move Cable Tray"))
				{
					trans.Start();

					// Copy elements
					XYZ moveVector = new XYZ(0, 0, 2); // 10 units in Z-direction
					ElementTransformUtils.CopyElements(
						doc,
						elementIds,
						moveVector);

					trans.Commit();
				}

				return Result.Succeeded;
			}
			catch (Exception ex)
			{
				message = ex.Message;
				return Result.Failed;
			}
		}
	}
}
