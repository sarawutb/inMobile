package crc64e58ce59a6d1a044d;


public class DateTimeFormatter
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.DateTimeFormatter
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_format:(J)Ljava/lang/CharSequence;:GetFormat_JHandler:DevExpress.Xamarin.Android.Editors.IDateTimeFormatterInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.DateTimeFormatter, DevExpress.XamarinForms.Editors.Android", DateTimeFormatter.class, __md_methods);
	}


	public DateTimeFormatter ()
	{
		super ();
		if (getClass () == DateTimeFormatter.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DateTimeFormatter, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public java.lang.CharSequence format (long p0)
	{
		return n_format (p0);
	}

	private native java.lang.CharSequence n_format (long p0);

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
