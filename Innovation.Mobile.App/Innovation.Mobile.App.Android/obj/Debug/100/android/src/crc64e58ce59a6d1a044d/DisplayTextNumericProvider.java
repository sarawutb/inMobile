package crc64e58ce59a6d1a044d;


public class DisplayTextNumericProvider
	extends crc64e58ce59a6d1a044d.DisplayTextProvider
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.DisplayTextNumericProvider, DevExpress.XamarinForms.Editors.Android", DisplayTextNumericProvider.class, __md_methods);
	}


	public DisplayTextNumericProvider ()
	{
		super ();
		if (getClass () == DisplayTextNumericProvider.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DisplayTextNumericProvider, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
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
