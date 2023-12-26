package crc640a67887a4134e062;


public class AutoCompleateCollectionViewOwnerWrapper
	extends crc640a67887a4134e062.CollectionViewOwnerWrapper
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Wrappers.AutoCompleateCollectionViewOwnerWrapper, DevExpress.XamarinForms.Editors.Android", AutoCompleateCollectionViewOwnerWrapper.class, __md_methods);
	}


	public AutoCompleateCollectionViewOwnerWrapper ()
	{
		super ();
		if (getClass () == AutoCompleateCollectionViewOwnerWrapper.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Wrappers.AutoCompleateCollectionViewOwnerWrapper, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
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
