package crc64e58ce59a6d1a044d;


public class DisplayTextProvider
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.DisplayTextFormatter
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_format:(Ljava/lang/CharSequence;Ljava/lang/CharSequence;)Ljava/lang/CharSequence;:GetFormat_Ljava_lang_CharSequence_Ljava_lang_CharSequence_Handler:DevExpress.Xamarin.Android.Editors.IDisplayTextFormatterInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.DisplayTextProvider, DevExpress.XamarinForms.Editors.Android", DisplayTextProvider.class, __md_methods);
	}


	public DisplayTextProvider ()
	{
		super ();
		if (getClass () == DisplayTextProvider.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DisplayTextProvider, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public java.lang.CharSequence format (java.lang.CharSequence p0, java.lang.CharSequence p1)
	{
		return n_format (p0, p1);
	}

	private native java.lang.CharSequence n_format (java.lang.CharSequence p0, java.lang.CharSequence p1);

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
