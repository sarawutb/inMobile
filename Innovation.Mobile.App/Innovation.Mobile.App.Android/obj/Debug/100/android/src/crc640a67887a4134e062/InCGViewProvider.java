package crc640a67887a4134e062;


public class InCGViewProvider
	extends crc640a67887a4134e062.CGViewProvider_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Wrappers.InCGViewProvider, DevExpress.XamarinForms.Editors.Android", InCGViewProvider.class, __md_methods);
	}


	public InCGViewProvider ()
	{
		super ();
		if (getClass () == InCGViewProvider.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Wrappers.InCGViewProvider, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
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
