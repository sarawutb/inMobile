package crc640a67887a4134e062;


public class CGViewProvider_1
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.ChipItemViewProvider
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getItemCount:()I:GetGetItemCountHandler:DevExpress.Xamarin.Android.Editors.IChipItemViewProviderInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_getViewByIndex:(I)Landroid/view/View;:GetGetViewByIndex_IHandler:DevExpress.Xamarin.Android.Editors.IChipItemViewProviderInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_getViewTypeByIndex:(I)I:GetGetViewTypeByIndex_IHandler:DevExpress.Xamarin.Android.Editors.IChipItemViewProviderInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_recycleView:(Landroid/view/View;II)V:GetRecycleView_Landroid_view_View_IIHandler:DevExpress.Xamarin.Android.Editors.IChipItemViewProviderInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_updateView:(Landroid/view/View;I)V:GetUpdateView_Landroid_view_View_IHandler:DevExpress.Xamarin.Android.Editors.IChipItemViewProviderInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Wrappers.CGViewProvider`1, DevExpress.XamarinForms.Editors.Android", CGViewProvider_1.class, __md_methods);
	}


	public CGViewProvider_1 ()
	{
		super ();
		if (getClass () == CGViewProvider_1.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Wrappers.CGViewProvider`1, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public int getItemCount ()
	{
		return n_getItemCount ();
	}

	private native int n_getItemCount ();


	public android.view.View getViewByIndex (int p0)
	{
		return n_getViewByIndex (p0);
	}

	private native android.view.View n_getViewByIndex (int p0);


	public int getViewTypeByIndex (int p0)
	{
		return n_getViewTypeByIndex (p0);
	}

	private native int n_getViewTypeByIndex (int p0);


	public void recycleView (android.view.View p0, int p1, int p2)
	{
		n_recycleView (p0, p1, p2);
	}

	private native void n_recycleView (android.view.View p0, int p1, int p2);


	public void updateView (android.view.View p0, int p1)
	{
		n_updateView (p0, p1);
	}

	private native void n_updateView (android.view.View p0, int p1);

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
