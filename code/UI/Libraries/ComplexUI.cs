using System;
using System.Collections.Generic;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ComplexUI
{
    public class SideTabUI : Panel
    {
        public List<Panel> TabItems;
        public int activeTab = 1;
        public int nextActiveTab = 1;
        public SideTabUI()
        {
            StyleSheet.Load("UI/Styles/libraries/ComplexUI.scss");

			TabItems = new List<Panel>();
			TabItems.Add( Add.Panel( "tabs tabsSide" ) );
		}

        public void AddTabItem(string Text, Action onClick)
        {
            var itemNumber = nextActiveTab;
            nextActiveTab++;

			Panel newTab = Add.Panel( "tabItem" );
			newTab.Add.Label( Text, "tabText" );
			newTab.Add.Panel( "lineOverlay" );
            
			if ( itemNumber == 1 )
            {
                newTab.SetClass("activeTab", true);
            }

            newTab.AddEventListener("onClick", () =>
            {
                TabItems[activeTab].SetClass("activeTab", false);
                activeTab = itemNumber;
                newTab.SetClass("activeTab", true);
                onClick();
            });

			TabItems.Add( newTab );
		}

		public void SetTabItem(int index, bool shouldEnable)
		{
			TabItems[index].SetClass( "activeTab", shouldEnable );

			/*if(shouldEnable)
				TabItems[index].Style.Set( "display: flex;" );
			else
				TabItems[index].Style.Set( "display: none;" );*/
		}
    }
}
