package crc647704af8f10fa7ac1;


public class AutoUpdateVersion
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Innovation.Mobile.App.Droid.Dependencies.AutoUpdateVersion, Innovation.Mobile.App.Android", AutoUpdateVersion.class, __md_methods);
	}


	public AutoUpdateVersion ()
	{
		super ();
		if (getClass () == AutoUpdateVersion.class) {
			mono.android.TypeManager.Activate ("Innovation.Mobile.App.Droid.Dependencies.AutoUpdateVersion, Innovation.Mobile.App.Android", "", this, new java.lang.Object[] {  });
		}
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
