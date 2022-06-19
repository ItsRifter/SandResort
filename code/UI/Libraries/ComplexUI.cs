using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ComplexUI
{
    public partial class SideTabUI : Panel
    {
        public Panel TabItems;
        public int activeTab = 1;
        public SideTabUI()
        {
            Log.Info( "render tabs" );
            TabItems = Add.Panel( "tabs tabsSide" );
        }

        public void AddTabItem(string Text, Action onClick)
        {
            Button btnItem = TabItems.Add.Button( Text, "tabItem" );
            btnItem.AddEventListener("onClick", () =>
            {
                onClick();
            });
        }
    }
}
