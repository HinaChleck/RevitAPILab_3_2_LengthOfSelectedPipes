using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RevitAPILab_3_2_LengthOfSelectedPipes
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipeFilter(), "Выберите трубы"); //список ссылок на элементы
            var pipeList = new List<Pipe>(); //присвоение элементам списка самих элементов из списка ссылок выше
            double lengthParam = 0;

            foreach (var selectedElement in selectedElementRefList)
            {
                #region проверка на тип (не нужна с IFilter)
                //Element element = doc.GetElement(selectedElement);  ПРОВЕРКА НА ТИП. НЕ НУЖНА КОГДА РЕАЛИЗОВАН iFilter
                //if(element is Wall)
                //{
                //    Wall oWall= (Wall)element;
                //    wallList.Add(oWall);
                //}
                #endregion//

                Pipe oPipe = doc.GetElement(selectedElement) as Pipe;
                pipeList.Add(oPipe);
                lengthParam += oPipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
            }

            double length = UnitUtils.ConvertFromInternalUnits(lengthParam, DisplayUnitType.DUT_METERS);//преобразование из футов в метры
            TaskDialog.Show("Длина выбранных труб", $"Количество труб: {pipeList.Count}.\nОбщая длина: {length} м."); //вывод 

            return Result.Succeeded;
        }
    }
}
