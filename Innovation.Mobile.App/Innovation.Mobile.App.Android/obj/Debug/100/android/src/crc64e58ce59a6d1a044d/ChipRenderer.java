package crc64e58ce59a6d1a044d;


public class ChipRenderer
	extends crc64e58ce59a6d1a044d.EditorsRenderer_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_setBackgroundColor:(I)V:GetSetBackgroundColor_IHandler\n" +
			"n_requestLayout:()V:GetRequestLayoutHandler\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.ChipRenderer, DevExpress.XamarinForms.Editors.Android", ChipRenderer.class, __md_methods);
	}


	public ChipRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ChipRenderer.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.ChipRenderer, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public ChipRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ChipRenderer.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.ChipRenderer, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public ChipRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ChipRenderer.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.ChipRenderer, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}


	public void setBackgroundColor (int p0)
	{
		n_setBackgroundColor (p0);
	}

	private native void n_setBackgroundColor (int p0);


	public void requestLayout ()
	{
		n_requestLayout ();
	}

	private native void n_requestLayout ();

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
