package crc640a67887a4134e062;


public class CollectionViewOwnerWrapper
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.utils.popupmanagers.CollectionViewOwner
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getCollectionView:()Landroid/view/View;:GetGetCollectionViewHandler:DevExpress.Xamarin.Android.Utils.PopupManagers.ICollectionViewOwnerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_hasValue:()Z:GetHasValueHandler:DevExpress.Xamarin.Android.Utils.PopupManagers.ICollectionViewOwnerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_isDataSourceEmpty:()Z:GetIsDataSourceEmptyHandler:DevExpress.Xamarin.Android.Utils.PopupManagers.ICollectionViewOwnerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_getSelectedItemText:()Ljava/lang/CharSequence;:GetGetSelectedItemTextHandler:DevExpress.Xamarin.Android.Utils.PopupManagers.ICollectionViewOwnerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_clearValue:()V:GetClearValueHandler:DevExpress.Xamarin.Android.Utils.PopupManagers.ICollectionViewOwnerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_ensureSelectionVisible:()V:GetEnsureSelectionVisibleHandler:DevExpress.Xamarin.Android.Utils.PopupManagers.ICollectionViewOwnerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Wrappers.CollectionViewOwnerWrapper, DevExpress.XamarinForms.Editors.Android", CollectionViewOwnerWrapper.class, __md_methods);
	}


	public CollectionViewOwnerWrapper ()
	{
		super ();
		if (getClass () == CollectionViewOwnerWrapper.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Wrappers.CollectionViewOwnerWrapper, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public android.view.View getCollectionView ()
	{
		return n_getCollectionView ();
	}

	private native android.view.View n_getCollectionView ();


	public boolean hasValue ()
	{
		return n_hasValue ();
	}

	private native boolean n_hasValue ();


	public boolean isDataSourceEmpty ()
	{
		return n_isDataSourceEmpty ();
	}

	private native boolean n_isDataSourceEmpty ();


	public java.lang.CharSequence getSelectedItemText ()
	{
		return n_getSelectedItemText ();
	}

	private native java.lang.CharSequence n_getSelectedItemText ();


	public void clearValue ()
	{
		n_clearValue ();
	}

	private native void n_clearValue ();


	public void ensureSelectionVisible ()
	{
		n_ensureSelectionVisible ();
	}

	private native void n_ensureSelectionVisible ();

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
