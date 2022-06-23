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
        public List<Panel> TabItems;
		int _defaultTab = 1;
        public int defaultTab
        {
            get { return _defaultTab; }
            set { _defaultTab = value; }
        }
        public int activeTab = 1;
        public int nextActiveTab = 1;
        public SideTabUI()
        {
            StyleSheet.Load("UI/Styles/libraries/ComplexUI.scss");

			TabItems = new List<Panel>();
			TabItems.Add( Add.Panel( "tabs tabsSide" ) );
		}

        public void AddTabItem(string Text, Action onClick, bool SwitchTabs = true)
        {
            var itemNumber = nextActiveTab;
            nextActiveTab++;

			Panel newTab = Add.Panel( "tabItem" );
			newTab.Add.Label( Text, "tabText" );
			newTab.Add.Panel( "lineOverlay" );
            
			if ( itemNumber == defaultTab )
            {
                newTab.SetClass("activeTab", true);
                //onClick();
            }

            newTab.AddEventListener("onClick", () =>
            {
                if ( SwitchTabs )
                {
                    TabItems[activeTab].SetClass("activeTab", false);
                    activeTab = itemNumber;
                    newTab.SetClass("activeTab", true);
                }
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
