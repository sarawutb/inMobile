package crc645910afa5ac646ce1;


public class JObjectWrapper
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.TextEdit.JObjectWrapper, DevExpress.XamarinForms.Editors.Android", JObjectWrapper.class, __md_methods);
	}


	public JObjectWrapper ()
	{
		super ();
		if (getClass () == JObjectWrapper.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.TextEdit.JObjectWrapper, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
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
