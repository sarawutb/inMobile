package crc645910afa5ac646ce1;


public class TemplatedItemSourceAdapter
	extends crc645910afa5ac646ce1.ItemSourceAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getView:(ILandroid/view/View;Landroid/view/ViewGroup;)Landroid/view/View;:GetGetView_ILandroid_view_View_Landroid_view_ViewGroup_Handler\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.TextEdit.TemplatedItemSourceAdapter, DevExpress.XamarinForms.Editors.Android", TemplatedItemSourceAdapter.class, __md_methods);
	}


	public TemplatedItemSourceAdapter (android.content.Context p0)
	{
		super (p0);
		if (getClass () == TemplatedItemSourceAdapter.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.TextEdit.TemplatedItemSourceAdapter, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public android.view.View getView (int p0, android.view.View p1, android.view.ViewGroup p2)
	{
		return n_getView (p0, p1, p2);
	}

	private native android.view.View n_getView (int p0, android.view.View p1, android.view.ViewGroup p2);

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
