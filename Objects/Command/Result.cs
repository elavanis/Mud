using Objects.Command.Interface;
using Objects.Global;
using Shared.TagWrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Command
{
    public class Result : IResult
    {
        [ExcludeFromCodeCoverage]
        public string ResultMessage { get; set; }

        [ExcludeFromCodeCoverage]
        public bool AllowAnotherCommand { get; set; }


        public Result(string message, bool allowAnotherCommand, TagType? tagType = TagType.Info)
        {
            AllowAnotherCommand = allowAnotherCommand;
            if (message != null)
            {
                if (tagType == null)
                {
                    ResultMessage = message;
                }
                else
                {
                    ResultMessage = GlobalReference.GlobalValues.TagWrapper.WrapInTag(message, (TagType)tagType);
                }
            }
        }
    }
}
