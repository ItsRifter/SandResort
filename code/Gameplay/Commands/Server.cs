using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class SCGame
{
	[ConCmd.Server("sc_buyitem")]
	public static void BuyItem(string className)
	{
		var buyer = ConsoleSystem.Caller.Pawn as BasePawn;

		if ( buyer == null )
			return;

		if ( TypeLibrary.GetTypeByName( className ) == null )
			return;

		buyer.CreatePreviewProp( TypeLibrary.Create<PropBase>( className ) );
	}
}

