package crc64e58ce59a6d1a044d;


public class EditorsRenderer_2
	extends crc643f46942d9dd1fff9.ViewRenderer_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMeasure:(II)V:GetOnMeasure_IIHandler\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.EditorsRenderer`2, DevExpress.XamarinForms.Editors.Android", EditorsRenderer_2.class, __md_methods);
	}


	public EditorsRenderer_2 (android.content.Context p0)
	{
		super (p0);
		if (getClass () == EditorsRenderer_2.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.EditorsRenderer`2, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public EditorsRenderer_2 (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == EditorsRenderer_2.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.EditorsRenderer`2, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public EditorsRenderer_2 (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == EditorsRenderer_2.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.EditorsRenderer`2, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}


	public void onMeasure (int p0, int p1)
	{
		n_onMeasure (p0, p1);
	}

	private native void n_onMeasure (int p0, int p1);

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
