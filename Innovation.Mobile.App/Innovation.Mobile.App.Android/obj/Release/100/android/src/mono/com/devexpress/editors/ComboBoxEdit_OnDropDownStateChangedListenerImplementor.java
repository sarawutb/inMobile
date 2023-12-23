package mono.com.devexpress.editors;


public class ComboBoxEdit_OnDropDownStateChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.ComboBoxEdit.OnDropDownStateChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDropDownStateChanged:(Lcom/devexpress/editors/ComboBoxEdit;Z)V:GetOnDropDownStateChanged_Lcom_devexpress_editors_ComboBoxEdit_ZHandler:DevExpress.Xamarin.Android.Editors.ComboBoxEdit/IOnDropDownStateChangedListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.Xamarin.Android.Editors.ComboBoxEdit+IOnDropDownStateChangedListenerImplementor, DevExpress.Xamarin.Android.Editors", ComboBoxEdit_OnDropDownStateChangedListenerImplementor.class, __md_methods);
	}


	public ComboBoxEdit_OnDropDownStateChangedListenerImplementor ()
	{
		super ();
		if (getClass () == ComboBoxEdit_OnDropDownStateChangedListenerImplementor.class) {
			mono.android.TypeManager.Activate ("DevExpress.Xamarin.Android.Editors.ComboBoxEdit+IOnDropDownStateChangedListenerImplementor, DevExpress.Xamarin.Android.Editors", "", this, new java.lang.Object[] {  });
		}
	}


	public void onDropDownStateChanged (com.devexpress.editors.ComboBoxEdit p0, boolean p1)
	{
		n_onDropDownStateChanged (p0, p1);
	}

	private native void n_onDropDownStateChanged (com.devexpress.editors.ComboBoxEdit p0, boolean p1);

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
