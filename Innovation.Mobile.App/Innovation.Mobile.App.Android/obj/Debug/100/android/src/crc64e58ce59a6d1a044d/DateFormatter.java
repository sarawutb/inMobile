package crc64e58ce59a6d1a044d;


public class DateFormatter
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.DateFormatter
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_format:(III)Ljava/lang/CharSequence;:GetFormat_IIIHandler:DevExpress.Xamarin.Android.Editors.IDateFormatterInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.DateFormatter, DevExpress.XamarinForms.Editors.Android", DateFormatter.class, __md_methods);
	}


	public DateFormatter ()
	{
		super ();
		if (getClass () == DateFormatter.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DateFormatter, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public java.lang.CharSequence format (int p0, int p1, int p2)
	{
		return n_format (p0, p1, p2);
	}

	private native java.lang.CharSequence n_format (int p0, int p1, int p2);

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
