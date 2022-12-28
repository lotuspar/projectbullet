﻿using ProjectBullet.Core.Node;
using Sandbox.UI;

namespace ProjectBullet.UI.Workshop;

public partial class GraphNodeOut : Panel
{
	public GraphController.Connector Connector { get; set; }

	protected override void OnAfterTreeRender( bool firstTime )
	{
		base.OnAfterTreeRender( firstTime );

		if ( firstTime )
		{
			Connector.Element = this;
		}
	}

	public override void Delete( bool immediate = false )
	{
		Connector.Element = null;

		base.Delete( immediate );
	}

	protected override void OnRightClick( MousePanelEvent e )
	{
		base.OnRightClick( e );

		if ( Connector.IsConnected )
		{
			Connector.Disconnect();
		}
		
		e.StopPropagation();
	}

	public bool MakingLink { get; set; } = false;
	public bool IsConnected => Connector.IsConnected;
	public string RootClasses => $"{(IsConnected ? "connected" : "")} {(MakingLink ? "linking" : "")}";
}
