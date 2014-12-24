using Arx.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArxSoundSwitcher
{
    public class ArxSoundSwitcherWidget: WidgetBase
    {
        protected override string Identifier
        {
            get { return "flauschig.arxsoundswitcher"; }
        }

        protected override string Name
        {
            get { return "ArxSndSwitch"; }
        }
    }
}
