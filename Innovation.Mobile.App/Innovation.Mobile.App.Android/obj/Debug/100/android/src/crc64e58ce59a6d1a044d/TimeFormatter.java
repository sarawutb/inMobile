package crc64e58ce59a6d1a044d;


public class TimeFormatter
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.TimeFormatter
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_format:(IIZ)Ljava/lang/CharSequence;:GetFormat_IIZHandler:DevExpress.Xamarin.Android.Editors.ITimeFormatterInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.TimeFormatter, DevExpress.XamarinForms.Editors.Android", TimeFormatter.class, __md_methods);
	}


	public TimeFormatter ()
	{
		super ();
		if (getClass () == TimeFormatter.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.TimeFormatter, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public java.lang.CharSequence format (int p0, int p1, boolean p2)
	{
		return n_format (p0, p1, p2);
	}

	private native java.lang.CharSequence n_format (int p0, int p1, boolean p2);

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
