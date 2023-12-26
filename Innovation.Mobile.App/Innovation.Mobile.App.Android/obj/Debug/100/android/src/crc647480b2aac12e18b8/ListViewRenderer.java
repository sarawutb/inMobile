package crc647480b2aac12e18b8;


public class ListViewRenderer
	extends crc643f46942d9dd1fff9.ViewRenderer_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDetachedFromWindow:()V:GetOnDetachedFromWindowHandler\n" +
			"n_onAttachedToWindow:()V:GetOnAttachedToWindowHandler\n" +
			"n_onMeasure:(II)V:GetOnMeasure_IIHandler\n" +
			"n_forceLayout:()V:GetForceLayoutHandler\n" +
			"n_requestLayout:()V:GetRequestLayoutHandler\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.CollectionView.Android.ListViewRenderer, DevExpress.XamarinForms.CollectionView.Android", ListViewRenderer.class, __md_methods);
	}


	public ListViewRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ListViewRenderer.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.CollectionView.Android.ListViewRenderer, DevExpress.XamarinForms.CollectionView.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public ListViewRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ListViewRenderer.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.CollectionView.Android.ListViewRenderer, DevExpress.XamarinForms.CollectionView.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public ListViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ListViewRenderer.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.CollectionView.Android.ListViewRenderer, DevExpress.XamarinForms.CollectionView.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}


	public void onDetachedFromWindow ()
	{
		n_onDetachedFromWindow ();
	}

	private native void n_onDetachedFromWindow ();


	public void onAttachedToWindow ()
	{
		n_onAttachedToWindow ();
	}

	private native void n_onAttachedToWindow ();


	public void onMeasure (int p0, int p1)
	{
		n_onMeasure (p0, p1);
	}

	private native void n_onMeasure (int p0, int p1);


	public void forceLayout ()
	{
		n_forceLayout ();
	}

	private native void n_forceLayout ();


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
