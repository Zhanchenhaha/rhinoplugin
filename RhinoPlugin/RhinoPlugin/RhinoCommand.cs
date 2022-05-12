using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;

namespace RhinoPlugin
{
    public class RhinoCommand : Command
    {
        public RhinoCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static RhinoCommand Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "RhinoCommand";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: start here modifying the behaviour of your command.
            // ---
            var view = doc.Views.ActiveView;
            if (null == view)
                return Result.Failure;

            // 命令选项提示语
            GetOption getOption = new GetOption();
            getOption.SetCommandPrompt("Select objectType");
            ObjectType[] objectTypes = (ObjectType[])Enum.GetValues(typeof(ObjectType));
            var options = getOption.AddOptionEnumSelectionList<ObjectType>("ObejctType", objectTypes, 0);
            getOption.Get();

            if (getOption.CommandResult() != Result.Success)
                return getOption.CommandResult();

            // 获取用户选项
            var option = getOption.Option();

            ObjectEnumeratorSettings objectEnumeratorSettings = new ObjectEnumeratorSettings();
            objectEnumeratorSettings.ObjectTypeFilter = objectTypes[option.Index];
            RhinoObject[] rhinoObjects = doc.Objects.FindByFilter(objectEnumeratorSettings);
            foreach (RhinoObject rhinoObject in rhinoObjects)
            {
                rhinoObject.Select(true);
            }


            doc.Views.Redraw();

            // ---
            return Result.Success;
        }
    }
}
